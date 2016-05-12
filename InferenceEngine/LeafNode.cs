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

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            foreach(KeyValuePair<string, bool> entry in model)
            {
                if(entry.Key == _value)
                {
                    return entry.Value;
                }
            }
            throw new Exception("Key not found in dictionary");
        }
    }
}
