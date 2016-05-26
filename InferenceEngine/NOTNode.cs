using System.Collections.Generic;


namespace InferenceEngine
{
    /// <summary>
    /// This class implements a NOT node for the binary tree within InferenceEngine
    /// </summary>
    class NOTNode : Node
    {
        private NodeOrStringInterface _child;

        /// <summary>
        /// Creates a new instance of the NOT node class
        /// </summary>
        /// <param name="child">the child node or string, the thing that will be NOTed</param>
        public NOTNode(NodeOrStringInterface child)
        {
            _child = child;
        }

        /// <summary>
        /// Gets the child of the not node, the literal that is NOTed.
        /// </summary>
        public NodeOrStringInterface Child
        {
            get
            {
                return _child;
            }
        }

        /// <summary>
        /// Evaluates whether the node is true or not under the given model
        /// </summary>
        /// <param name="model">the model used to evaluate the node</param>
        /// <returns>true if the node is true under the model, false otherwise</returns>
        public override bool Evaluate(List<string> model)
        {
            return !(_child.Evaluate(model));
        }
    }
}