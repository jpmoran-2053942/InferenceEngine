using System.Collections.Generic;

namespace InferenceEngine
{
    /// <summary>
    /// This class implements an OR node for the binary tree within InferenceEngine
    /// </summary>
    class ORNode : Node
    {
        private NodeOrStringInterface _leftChild;
        private NodeOrStringInterface _rightChild;

        /// <summary>
        /// Creates a new instance of the OR node class
        /// </summary>
        /// <param name="leftChild">the left child of the AND node</param>
        /// <param name="rightChild">the right child of the AND node</param>
        public ORNode(NodeOrStringInterface leftChild, NodeOrStringInterface rightChild)
        {
            _leftChild = leftChild;
            _rightChild = rightChild;
        }

        /// <summary>
        /// Gets the left child
        /// </summary>
        public NodeOrStringInterface LeftChild
        {
            get
            {
                return _leftChild;
            }
        }

        /// <summary>
        /// Gets the right child
        /// </summary>
        public NodeOrStringInterface RightChild
        {
            get
            {
                return _rightChild;
            }
        }

        /// <summary>
        /// Evaluates whether the node is true or not under the given model
        /// </summary>
        /// <param name="model">the model used to evaluate the node</param>
        /// <returns>true if the node is true under the model, false otherwise</returns>
        public override bool Evaluate(List<string> model)
        {
            return _leftChild.Evaluate(model) || _rightChild.Evaluate(model);
        }
    }
}