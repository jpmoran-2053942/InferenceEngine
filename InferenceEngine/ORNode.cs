using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class ORNode : Node
    {
        private NodeOrStringInterface _leftChild;
        private NodeOrStringInterface _rightChild;

        public ORNode(NodeOrStringInterface leftChild, NodeOrStringInterface rightChild)
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