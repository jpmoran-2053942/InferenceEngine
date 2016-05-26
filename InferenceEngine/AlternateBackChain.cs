using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class AlternateBackChain
    {
        public List<string> _provenPremises = new List<string>();

        public AlternateBackChain()
        {

        }

        private bool BCProver(List<HornClause> knowledgeBase, string query, List<string> agenda)
        {
            bool provenFalse = false; 

            foreach (HornClause h in knowledgeBase)
            {
                if(h.conclusion == query)
                {
                    provenFalse = false;
                    foreach(string s in h.premise)
                    {
                        if (!agenda.Contains(s))
                        {
                            agenda.Add(s);
                            if (!BCProver(knowledgeBase, s, agenda))
                            {
                                provenFalse = true;
                            }
                        }
                        else
                        {
                            provenFalse = true;
                        }
                    }
                    if (!provenFalse)
                    {
                        _provenPremises.Add(query);
                        return true;
                    }
                }
                else if (h.premise.Contains(query) && h.conclusion == null)
                {
                    _provenPremises.Add(query);
                    return true;
                }
            }
            return false;
        }

        public bool BCProver(List<HornClause> knowledgeBase, string query)
        {
            return BCProver(knowledgeBase, query, new List<string>());
        }
    }
}
