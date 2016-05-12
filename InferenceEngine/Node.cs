using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    abstract class Node : NodeOrStringInterface
    {
        public Node GetNode()
        {
            return this;
        }

        public bool IsEqualTo(string checkValue)
        {
            return false;
        }

        public bool IsANode()
        {
            return true;
        }

        public abstract bool Evaluate(Dictionary<string, bool> model);
    }
}