using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class ANDNode : Node
    {
        private NodeOrStringInterface _leftChild;
        private NodeOrStringInterface _rightChild;

        public ANDNode(NodeOrStringInterface leftChild, NodeOrStringInterface rightChild)
        {
            _leftChild = leftChild;
            _rightChild = rightChild;
        }

        public NodeOrStringInterface LeftChild
        {
            get
            {
                return _leftChild;
            }
        }

        public NodeOrStringInterface RightChild
        {
            get
            {
                return _rightChild;
            }
        }
    }
}