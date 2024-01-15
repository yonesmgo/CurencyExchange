using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurencyHire.Core
{
    public class Node
    {
        public string Name;
        public Node current_node;
        public List<Edge> Edges = new List<Edge>();
        public Node(string Name)
        {
            this.Name = Name;
            current_node = this;
        }
        public Node AddEdge(Node child, double weight, double rate)
        {
            Edges.Add(new Edge()
            {
                Parent = current_node,
                Child = child,
                Weight = weight,
                Rate = rate
            });
            if (!child.Edges.Exists(a => a.Parent == child && a.Child == current_node))
            {
                child.AddEdge(current_node, weight, 1 / rate);
            }
            return current_node;
        }
    }
}
