using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurencyHire.Core
{
    public class Edge
    {
        public double Weight;
        public Node? Parent;
        public Node? Child;
        public double Rate { get; set; }
    }
}
