using System.Xml.Linq;

namespace CurencyHire.Model.Entites
{
    public class Edge
    {
        public double Weight;
        public Node? Parent;
        public Node? Child;
        public double Rate { get; set; }
    }
}
