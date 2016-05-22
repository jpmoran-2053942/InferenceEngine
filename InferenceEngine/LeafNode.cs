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
        public static List<string> _allLeafNodes = new List<string>();

        public LeafNode(string value)
        {
            _value = value;
            bool leafNodeInList = false;
            if (_allLeafNodes.Count != 0)
            {
                foreach (string s in _allLeafNodes)
                {
                    if (s == _value)
                    {
                        leafNodeInList = true;
                    }
                }
                if (leafNodeInList == false)
                {
                    _allLeafNodes.Add(_value);
                }
            }
            else
            {
                _allLeafNodes.Add(_value);
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }

        public override bool Evaluate(List<string> model)
        {
            bool isTrue = false;
            if (model != null)
            {
                foreach (string s in model)
                {
                    if (s == _value)
                    {
                        isTrue = true;
                    }
                }
            }
            return isTrue;
        }
    }
}
