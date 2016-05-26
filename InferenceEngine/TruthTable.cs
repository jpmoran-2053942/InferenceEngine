using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// This class implements a truth table approach to establish whether a query can be entailed from a given knowledge base
    /// </summary>
    class TruthTable
    {
        private int _numberOfOnesInTruthTable;

        /// <summary>
        /// Creates a new instance of the TruthTable class
        /// </summary>
        public TruthTable()
        {
            _numberOfOnesInTruthTable = 0;
        }

        /// <summary>
        /// Gets the Number of Ones in the Truth Table
        /// </summary>
        public int NumberOfOnesInTruthTable
        {
            get
            {
                return _numberOfOnesInTruthTable;
            }
        }

        /// <summary>
        /// Establishes whether a query can be entailed from a given knowledge base
        /// </summary>
        /// <param name="rootNodeKB">the root node in the knowledge base binary tree</param>
        /// <param name="rootNodeQuery">the root node in the query binary tree</param>
        /// <returns>true if the query can be entailed, false otherwise</returns>
        public bool TruthTableEntails(NodeOrStringInterface rootNodeKB, NodeOrStringInterface rootNodeQuery)
        {
            List<string> symbols = LeafNode._allLeafNodes;
            return TruthTableCheckAll(rootNodeKB, rootNodeQuery, symbols, null);
        }

        /// <summary>
        /// Checks all rows in the truth table to establish whether a query can be entailed from the knowledge base
        /// </summary>
        /// <param name="rootNodeKB">the root node of the knowledge base binary tree</param>
        /// <param name="rootNodeQuery">the root node of the query binary tree</param>
        /// <param name="symbols">all symbols in the truth table</param>
        /// <param name="model">the model used to evaluate truth</param>
        /// <returns>true if the query can be entailed, false otherwise</returns>
        public bool TruthTableCheckAll(NodeOrStringInterface rootNodeKB, NodeOrStringInterface rootNodeQuery, List<string> symbols, List<string> model)
        {
            //no symbols left means that every symbol has a value therefore we have a complete line of the truth table
            if (symbols.Count == 0)
            {
                if(rootNodeKB.Evaluate(model))
                {
                    //if knowldege base holds true and the model holds true then the knowledge base entails the model
                    _numberOfOnesInTruthTable++;
                    return rootNodeQuery.Evaluate(model);
                }
                else
                {
                    //if knowledge base is false, it will entail any query
                    return true;
                }
            }
            else
            {
                string first = symbols[0];
                List<string> firstIsTrueModel; //the model that will be passed in where that symbol is true (for false stays the same)
                if (model != null)
                {
                    firstIsTrueModel = new List<string>(model);
                    if (!firstIsTrueModel.Contains(first))
                    {
                        firstIsTrueModel.Add(first);
                    }
                }
                else
                {
                    firstIsTrueModel = new List<string>();
                    firstIsTrueModel.Add(first);
                }
                List<string> rest = new List<string>();
                //add all symbols but the first one to rest, this will be the next set of symbols
                for(int i = 1; i < symbols.Count; i++)
                {
                    rest.Add(symbols[i]);
                }
                return TruthTableCheckAll(rootNodeKB, rootNodeQuery, rest, firstIsTrueModel) && TruthTableCheckAll(rootNodeKB, rootNodeQuery, rest, model);
            }
        }
    }
}
