using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// This class serves as an interface allowing both Nodes and Strings to be in the binary tree in the InferenceEngine
    /// </summary>
    interface NodeOrStringInterface
    {
        /// <summary>
        /// Gets the node
        /// </summary>
        /// <returns>The node</returns>
        Node GetNode();

        /// <summary>
        /// Checks for equality with a given string
        /// </summary>
        /// <param name="checkValue">the string to check</param>
        /// <returns>true if equal, false otherwise</returns>
        bool IsEqualTo(string checkValue);

        /// <summary>
        /// Identifies whether the given object is a node (as opposed to a string)
        /// </summary>
        /// <returns>true if a node, false if a string</returns>
        bool IsANode();

        /// <summary>
        /// Evaluates whether a given node is true under the given model
        /// </summary>
        /// <param name="model">the model used to evaluate</param>
        /// <returns>true if the node is true in the model, false otherwise</returns>
        bool Evaluate(List<string> model);
    }
}