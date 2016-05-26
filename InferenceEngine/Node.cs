using System.Collections.Generic;


namespace InferenceEngine
{
    /// <summary>
    /// This class is the base class for all nodes in the InferenceEngine
    /// </summary>
    abstract class Node : NodeOrStringInterface
    {
        /// <summary>
        /// Gets this
        /// </summary>
        /// <returns></returns>
        public Node GetNode()
        {
            return this;
        }

        /// <summary>
        /// Tests for equality with a given string
        /// </summary>
        /// <param name="checkValue">the string to check</param>
        /// <returns>will always return false since a node cannot be equal to a string</returns>
        public bool IsEqualTo(string checkValue)
        {
            return false;
        }

        /// <summary>
        /// Identifies this object as a type of node
        /// </summary>
        /// <returns>returns true in all cases</returns>
        public bool IsANode()
        {
            return true;
        }

        /// <summary>
        /// Abstract method for evaluating whether or not the node is true for a given model
        /// </summary>
        /// <param name="model">the model used to evaluate</param>
        /// <returns>true if the node is true in this model, false otherwise</returns>
        public abstract bool Evaluate(List<string> model);
    }
}