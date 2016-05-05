using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class LeafNode: Node
    {
        string _value;

        public LeafNode(string value)
        {
            _value = value;
        }
    }
}
