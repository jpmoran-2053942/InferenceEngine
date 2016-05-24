using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class TruthTable
    {
        public TruthTable()
        {
        }

        public bool TruthTableEntails(NodeOrStringInterface rootNodeKB, NodeOrStringInterface rootNodeQuery)
        {
            List<string> symbols = LeafNode._allLeafNodes;
            return TruthTableCheckAll(rootNodeKB, rootNodeQuery, symbols, null);
        }

        public bool TruthTableCheckAll(NodeOrStringInterface rootNodeKB, NodeOrStringInterface rootNodeQuery, List<string> symbols, List<string> model)
        {
            //no symbols left means that every symbol has a value therefore we have a complete line of the truth table
            if (symbols.Count == 0)
            {
                if(rootNodeKB.Evaluate(model))
                {
                    //if knowldege base holds true and the model holds true then the knowledge base entails the model
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
                List<string> firstIsTrueModel;
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
                for(int i = 1; i < symbols.Count; i++)
                {
                    rest.Add(symbols[i]);
                }
                return TruthTableCheckAll(rootNodeKB, rootNodeQuery, rest, firstIsTrueModel) && TruthTableCheckAll(rootNodeKB, rootNodeQuery, rest, model);
            }
        }
    }
}
