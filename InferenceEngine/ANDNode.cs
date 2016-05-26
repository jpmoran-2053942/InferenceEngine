using System.Collections.Generic;


namespace InferenceEngine
{
    /// <summary>
    /// This class is an AND node used to evaluate propositional logic statements
    /// </summary>
    class ANDNode : Node
    {
        private NodeOrStringInterface _leftChild;
        private NodeOrStringInterface _rightChild;

        /// <summary>
        /// Creates a new instance of the AND node class
        /// </summary>
        /// <param name="leftChild">The left side of the AND equation</param>
        /// <param name="rightChild">The right side of the AND equation</param>
        public ANDNode(NodeOrStringInterface leftChild, NodeOrStringInterface rightChild)
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
        /// Evaluates the value of the AND node
        /// </summary>
        /// <param name="model">The model being tested by the truth table</param>
        /// <returns>true if the AND statement is true, flase otherwise</returns>
        public override bool Evaluate(List<string> model)
        {
            return _leftChild.Evaluate(model) && _rightChild.Evaluate(model);
        }
    }
}