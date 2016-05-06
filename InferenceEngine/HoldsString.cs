using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class HoldsString : NodeOrStringInterface
    {
        string _value;

        public HoldsString(string value)
        {
            _value = value;
        }

        public Node GetNode()
        {
            return new LeafNode(_value);
        }

        public bool IsEqualTo(string checkValue)
        {
            return _value.Equals(checkValue);
        }

        public bool IsANode()
        {
            return false;
        }
    }
}