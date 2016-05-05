using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class HoldsString: NodeOrStringInterface
    {
        string _value;

        public Node GetNode()
        {
            return new LeafNode(_value);
        }
    }
}
