using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// This class implements a backwards chaining approach to evaluate entailment for horn clauses
    /// </summary>
    class AlternateBackChain
    {
        public List<string> _provenPremises = new List<string>(); 

        /// <summary>
        /// Creates a new instance of the Alternate Back Chain class
        /// </summary>
        public AlternateBackChain()
        {

        }

        /// <summary>
        /// Evaluates whether or not a given query is entailed by a given knowledge base
        /// </summary>
        /// <param name="knowledgeBase">the knowledge base in horn clause form</param>
        /// <param name="query">the term we are querying</param>
        /// <param name="agenda">a list of terms that have been examined</param>
        /// <returns>true if the query can be entailed from the knowledge base, false otherwise</returns>
        private bool BCProver(List<HornClause> knowledgeBase, string query, List<string> agenda)
        {
            bool provenFalse = false; //a flag set to true if we know that a given proposition cannot be proven from the horn clauses

            foreach (HornClause h in knowledgeBase)
            {
                if(h.conclusion == query)
                {
                    //need to reset provenFalse for every horn clause, otherwise we might miss premises that are proven true
                    provenFalse = false; 
                    foreach(string s in h.premise)
                    {
                        //only run BC Prover if not in the agenda - this prevents infinite loop issues
                        //e.g. a>b; b>a; would get stuck if we didn't check this
                        if (!agenda.Contains(s))
                        {
                            agenda.Add(s);
                            if (!BCProver(knowledgeBase, s, agenda))
                            {
                                //if we couldn't find anything then we this particular term is proven false
                                //doesn't mean the whole statement is false which is why provenFalse is used rather than return false;
                                provenFalse = true;
                            }
                        }
                        //if we reach the end of the premises, then this particular term is proven false
                        //can't return false since there may be another term where the query is a conclusion
                        else
                        {
                            provenFalse = true;
                        }
                    }
                    //if we get to the end of the horn clauses and we have a solution that works, 
                    //we can say that the query is entailed by the knowledge base
                    if (!provenFalse)
                    {
                        _provenPremises.Add(query);
                        return true;
                    }
                }
                //if we find the query as a premise where the conclusion is null, it is true
                else if (h.premise.Contains(query) && h.conclusion == null)
                {
                    _provenPremises.Add(query);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// An overloaded version of BCProver used as the function call from Program.cs when there is no agenda
        /// </summary>
        /// <param name="knowledgeBase">the knowledge base in horn clauses</param>
        /// <param name="query">the query to test</param>
        /// <returns>true is the query can be entailed from the knowledge base, false otherwise</returns>
        public bool BCProver(List<HornClause> knowledgeBase, string query)
        {
            return BCProver(knowledgeBase, query, new List<string>());
        }
    }
}
