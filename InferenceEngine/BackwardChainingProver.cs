using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class BackwardChainingProver
    {
        private List<string> _provenPremises = new List<string>();

        public BackwardChainingProver()
        {
        }

        public List<string> ProvenPremises
        {
            get
            {
                return _provenPremises;
            }
        }

        public bool BackwardChainCheck(List<HornClause> hornClauses, string query)
        {
            List<string> agenda = new List<string>();
            List<string> checkedPremises = new List<string>();
            List<string> thingsToProve = new List<string>();
            return BackwardChainEntails(hornClauses, query, agenda, checkedPremises, thingsToProve);
        }

        public bool BackwardChainEntails(List<HornClause> hornClauses, string query, List<string> agenda, List<string> checkedPremises, List<string> thingsToProve)
        {
            bool inconclusive = true;

            if (hornClauses == null)
            {
                return false;
            }
            //go through horn clauses and find any terms where the query is a premise with no conclusion
            foreach(HornClause h in hornClauses)
            {
                if((h.premise.Contains(query)) && (h.conclusion == null))
                {
                    //this query proven therefore remove from thingsToProve
                    _provenPremises.Add(query);
                    int position = -1;
                    for (int i = 0; i < thingsToProve.Count; i++)
                    {
                        if (thingsToProve[i] == query)
                        {
                            position = i;
                        }
                    }
                    if (position >= 0)
                    {
                        thingsToProve.RemoveAt(position);
                    }
                    inconclusive = false;
                    //only want to return true if there is nothing else to prove true (no incomplete and terms)
                    if (thingsToProve.Count == 0)
                    {
                        return true;
                    }
                }
            }
            
            //go through horn clauses and find any terms where the query is a conclusion and add the premises to the agenda and checked list
            foreach(HornClause h in hornClauses)
            {
                if (h.conclusion != null)
                {
                    if (h.conclusion.Equals(query))
                    {
                        inconclusive = false;
                        foreach (string s in h.premise)
                        {
                            if (!checkedPremises.Contains(s))
                            {
                                agenda.Insert(0, s);
                            }
                            checkedPremises.Add(s);
                            //if there is an and term we need to prove both together
                            if (h.premise.Count > 1)
                            {
                                thingsToProve.Add(s);
                            }
                        }
                    }
                }
            }
            //if we can't prove one proposition in an and term, we can forget all of them
            if (inconclusive)
            {
                foreach(string s in thingsToProve)
                {
                    if(agenda.Contains(s))
                    {
                        agenda.Remove(s);
                    }
                }
                thingsToProve.Clear();
                _provenPremises.Clear();
            }

            //remove query from things to prove - its either true or we've just added "children" to things to prove
            int qPosition = -1;
            for (int i = 0; i < thingsToProve.Count; i++)
            {
                if (thingsToProve[i] == query)
                {
                    qPosition = i;
                }
            }
            if (qPosition >= 0)
            {
                thingsToProve.RemoveAt(qPosition);
                _provenPremises.Add(query);
            }

            //pop the agenda
            string nextQuery;
            if (agenda.Count > 0)
            {
                nextQuery = agenda[0];
                agenda.RemoveAt(0);
            }
            //if the agenda is empty, the knowledge base does not entail the query
            else
            {
                return false;
            }

            //run BackwardChainEntails on first term in the agenda
            return BackwardChainEntails(hornClauses, nextQuery, agenda, checkedPremises, thingsToProve);
        }
    }
}
