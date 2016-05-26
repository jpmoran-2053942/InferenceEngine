using System;
using System.Collections.Generic;

namespace InferenceEngine
{
    /// <summary>
    /// A class that holds strings to allow for a list of Nodes or Strings
    /// </summary>
    class HoldsString : NodeOrStringInterface
    {
        string _value;

        /// <summary>
        /// Creates a new instance of the Holds String class
        /// </summary>
        /// <param name="value"></param>
        public HoldsString(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Creates a leaf node for the string value
        /// </summary>
        /// <returns>a node with the same value as this</returns>
        public Node GetNode()
        {
            return new LeafNode(_value);
        }

        /// <summary>
        /// Checks for equality with a given string
        /// </summary>
        /// <param name="checkValue">The string to check against</param>
        /// <returns>true if equal, false otherwise</returns>
        public bool IsEqualTo(string checkValue)
        {
            return _value.Equals(checkValue);
        }

        /// <summary>
        /// An is this a node function from Node Or String Interface that identifies this object as a Holds String object
        /// </summary>
        /// <returns>false in all cases</returns>
        public bool IsANode()
        {
            return false;
        }

        /// <summary>
        /// Evaluates the validity of a propositional logic statement for a given model
        /// </summary>
        /// <param name="model">The model to evaluate with</param>
        /// <returns>Always throws an exception. This function should never be run on a Holds String object if binary tree conversion has been successful</returns>
        public bool Evaluate(List<string> model)
        {
            throw new Exception("Node not properly converted from string to leaf node");
        }
    }
}