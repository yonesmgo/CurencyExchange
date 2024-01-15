namespace CurencyHire.Model.Entites
{
    public class Graph
    {
        public  Node? Root;
        public List<Node> AllNodes = new List<Node>();
        public Node CreateRoot(string name)
        {
            Root = CreateNode(name);
            return Root;
        }
        public Node CreateNode(string name)
        {
            var n = new Node(name);
            AllNodes.Add(n);
            return n;
        }
        public double?[,] CreateAdjMatrix()
        {
            double?[,] adj = new double?[AllNodes.Count, AllNodes.Count];
            for (int i = 0; i < AllNodes.Count; i++)
            {
                Node node1 = AllNodes[i];
                for (int j = 0; j < AllNodes.Count; j++)
                {
                    Node node2 = AllNodes[j];
                    var edge = node1.Edges.FirstOrDefault(a => a.Child == node2);
                    if (edge != null)
                    {
                        adj[i, j] = edge.Weight;
                    }
                    else
                    {
                        adj[i, j] = 0;
                    }
                }
            }
            return adj;
        }
        public double?[,] CreateRateMatrix()
        {
            double?[,] rate = new double?[AllNodes.Count, AllNodes.Count];
            for (int i = 0; i < AllNodes.Count; i++)
            {
                Node node1 = AllNodes[i];
                for (int j = 0; j < AllNodes.Count; j++)
                {
                    Node node2 = AllNodes[j];
                    var edge = node1.Edges.FirstOrDefault(a => a.Child == node2);
                    if (edge != null)
                    {
                        rate[i, j] = edge.Rate;
                    }
                    else
                    {
                        rate[i, j] = 0;
                    }
                }
            }
            return rate;
        }
        public int miniDist(int[] distance, bool[] tset)
        {
            int minimum = int.MaxValue;
            int index = 0;
            for (int k = 0; k < distance.Length; k++)
            {
                if (!tset[k] && distance[k] <= minimum)
                {
                    minimum = distance[k];
                    index = k;
                }
            }
            return index;
        }
        public List<int> Dijkstar(double?[,] graph, int src, int dest)
        {
            int length = graph.GetLength(0);
            int[] distance = new int[length];
            bool[] used = new bool[length];
            int[] prev = new int[length];

            for (int i = 0; i < length; i++)
            {
                distance[i] = int.MaxValue;
                used[i] = false;
                prev[i] = -1;
            }
            distance[src] = 0;

            for (int k = 0; k < length - 1; k++)
            {
                int minNode = miniDist(distance, used);
                used[minNode] = true;
                for (int i = 0; i < length; i++)
                {
                    if (graph[minNode, i] > 0)
                    {
                        int shortestToMinNode = distance[minNode];
                        int? distanceToNextNode = (int?)graph[minNode, i];
                        int? totalDistance = shortestToMinNode + distanceToNextNode;
                        if (totalDistance < distance[i])
                        {
                            distance[i] = (int)totalDistance;
                            prev[i] = minNode;
                        }
                    }
                }
            }
            if (distance[dest] == int.MaxValue)
            {
                return new List<int>();
            }
            var path = new LinkedList<int>();
            int currentNode = dest;
            while (currentNode != -1)
            {
                path.AddFirst(currentNode);
                currentNode = prev[currentNode];
            }
            return path.ToList();
        }
        //calc new rate and path
        public bool PrintPath(ref double?[,] graph, string[] labels, string src, string dest, ref double?[,] rateGraph, out double outRate)
        {
            int source = Array.IndexOf(labels, src);
            int destination = Array.IndexOf(labels, dest);
            var paths = Dijkstar(graph, source, destination);

            if (paths.Count > 0)
            {
                int? path_length = 0;
                double? rate_total = 1;
                for (int i = 0; i < paths.Count - 1; i++)
                {
                    int? length = (int?)graph[paths[i], paths[i + 1]];
                    path_length += length;

                    double? rate = (double?)rateGraph[paths[i], paths[i + 1]];
                    rate_total *= rate;
                }
                outRate = (double)rate_total;
                return true;
            }
            else
            {
                Console.WriteLine("No Path");
                outRate = (double)0;
                return false;
            }
        }
    }
}
