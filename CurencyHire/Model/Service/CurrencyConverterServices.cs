using CurencyHire.Core.Interface;
using CurencyHire.Model.Entites;
namespace CurencyHire.Core.Service
{
    public class CurrencyConverterServices : ICurrencyConverter
    {
        public static IEnumerable<Tuple<string, string, double>> lstExchangeList;
        public static Graph graph;
        public Task ClearConfiguration()
        {
            lstExchangeList = null;
            graph = null;
            return Task.CompletedTask;
        }
        public async Task<double> Convert(string fromCurrency, string toCurrency, double amount)
        {
            //DO exchange
            string[] labels = graph.AllNodes.Select(s => s.Name).ToArray();
            double?[,] adj = graph.CreateAdjMatrix();
            double?[,] rateADJ = graph.CreateRateMatrix();
            double outRate;
            double resulrAmount = 0;
            if (graph.PrintPath(ref adj, labels, fromCurrency, toCurrency, ref rateADJ, out outRate))
            {
                var fromNode = graph.AllNodes.Where(a => a.Name.Equals(fromCurrency)).FirstOrDefault();
                var toNode = graph.AllNodes.Where(a => a.Name.Equals(toCurrency)).FirstOrDefault();
                fromNode.AddEdge(toNode, 1, outRate);
                resulrAmount = amount * outRate;
            }
            return await Task.FromResult(resulrAmount);
        }
        public Task UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            int row = 0;
            if (graph == null)
            {
                graph = new Graph();
                //Initial Node
                if (lstExchangeList is null)
                {
                    lstExchangeList = conversionRates.Select(arr => Tuple.Create(arr.Item1, arr.Item2, arr.Item3));
                }
                var root = lstExchangeList.FirstOrDefault().Item1.ToString();
                var t = (lstExchangeList.Select(x => x.Item1).Distinct().ToList());
                var t2 = (lstExchangeList.Select(x => x.Item2).Distinct().ToList());
                t2.AddRange(t);
                t2 = t2.Distinct().ToList();
                List<Node> lstNode = ((t2.Select(x => new Node(x)).Distinct())).ToList();
                foreach (var item in lstNode)
                {
                    if (row == 0)
                    {
                        graph.CreateRoot(item.Name);
                    }
                    else
                    {
                        graph.CreateNode(item.Name);
                    }
                    row++;
                }
            }
            else
            {
                var tempList = lstExchangeList.Select(a=> new DbTuple
                {
                    from = a.Item1,
                    to = a.Item2,
                    rate = a.Item3
                }).ToList();
                foreach (var item in conversionRates)
                {
                    bool isVisitedItem = false;
                    foreach (var exchangeItem in tempList)
                    {
                        if ((item.Item1.Equals(exchangeItem.from) && item.Item2.Equals(exchangeItem.to)))
                        {
                            exchangeItem.rate = item.Item3;
                            isVisitedItem = true;
                        }
                        else if ((item.Item1.Equals(exchangeItem.to) && item.Item2.Equals(exchangeItem.from)))
                        {
                            exchangeItem.rate = (1 / item.Item3);
                            isVisitedItem = true;
                        }
                    }
                    if (!isVisitedItem)
                    {
                        tempList.Add(new DbTuple
                        {
                            from = item.Item1,
                            to = item.Item2,
                            rate = item.Item3
                        });

                        if (!graph.AllNodes.Any(i => i.Name.Equals(item.Item1)))
                        {
                            graph.CreateNode(item.Item1);
                        }
                        if (!graph.AllNodes.Any(i => i.Name.Equals(item.Item2)))
                        {
                            graph.CreateNode(item.Item2);
                        }
                    }
                }
                lstExchangeList = tempList.Select(arr => Tuple.Create(arr.from, arr.to, arr.rate));
            }
            //Initial Edge
            foreach (var item in graph.AllNodes)
            {
                var lstToCurrency = lstExchangeList.Where(i => i.Item1.Equals(item.Name.ToString())).ToList();
                if (lstToCurrency.Any())
                {
                    foreach (var item3 in lstToCurrency)
                    {
                        var _node = graph.AllNodes.Where(i => i.Name.Equals(item3.Item2)).FirstOrDefault();

                        item.AddEdge(_node, 1, item3.Item3);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
