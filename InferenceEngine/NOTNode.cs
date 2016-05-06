using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class NOTNode: Node
    {
        private NodeOrStringInterface _child;

        public NOTNode(NodeOrStringInterface child)
        {
            _child = child;
        }

        public NodeOrStringInterface Child
        {
            get
            {
                return _child;
            }            
        }
    }
}
