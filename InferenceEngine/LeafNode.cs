using System.Collections.Generic;

namespace InferenceEngine
{
    /// <summary>
    /// This class generates leaf nodes used to evaluate the validity of a propositional logic statement
    /// </summary>
    class LeafNode: Node
    {
        string _value;
        public static List<string> _allLeafNodes = new List<string>();

        /// <summary>
        /// Creates a new instance of the leaf node class
        /// </summary>
        /// <param name="value">The value of the leaf node</param>
        public LeafNode(string value)
        {
            _value = value;
            //add all new leaf nodes to a list - _allLeafNodes
            //this is used by truth table to get all of the symbols 
            bool leafNodeInList = false;
            if (_allLeafNodes.Count != 0)
            {
                foreach (string s in _allLeafNodes)
                {
                    if (s == _value)
                    {
                        leafNodeInList = true;
                    }
                }
                if (leafNodeInList == false)
                {
                    _allLeafNodes.Add(_value);
                }
            }
            else
            {
                _allLeafNodes.Add(_value);
            }
        }

        /// <summary>
        /// Gets the value of the Leaf Node
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
        }

        /// <summary>
        /// Evaluates whether the leaf node is true or false for a given model
        /// </summary>
        /// <param name="model">The model used to evaluate the node</param>
        /// <returns></returns>
        public override bool Evaluate(List<string> model)
        {
            bool isTrue = false;
            if (model != null)
            {
                foreach (string s in model)
                {
                    if (s == _value)
                    {
                        isTrue = true;
                    }
                }
            }
            return isTrue;
        }
    }
}
