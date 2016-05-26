using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Includes methods to determine whether the knowledge base entails a query. Also
/// tracks the list of premises proven in this process.
/// </summary>
namespace InferenceEngine
{
    class ForwardChainProver
    {
        private List<string> _provenPremise = new List<string>();

        /// <summary>
        /// Determines whether the knowledge base entails the query. Returns true if so,
        /// false otherwise.
        /// </summary>
        /// <returns><c>true</c>, if the knowledge base entails the query, <c>false</c> otherwise.</returns>
        /// <param name="hornClauses">The knowledge base as a list of HornClauses.</param>
        /// <param name="query">The query as a string.</param>
        public bool ForwardChainEntails(List<HornClause> hornClauses, string query)
        {
            //Clear any proven premises from previous executions.
            _provenPremise.Clear();

            //If there is no knowledge base, nothing is entailed.
            if (hornClauses == null)
                return false;

            //Count is the number of unique premises in a horn clause. If we find all the premises of a clause,
            //the conclusion can be proven.
            Dictionary<HornClause, int> count = new Dictionary<HornClause, int>();
            //A list of each symbol that indicates whether it has been inferred.
            Dictionary<string, bool> inferred = new Dictionary<string, bool>();
            //The agenda of symbols that have been proven and can be used to prove other clauses.
            Stack<string> agenda = new Stack<string>();

            //Determine the number of symbols in the knowledge base.
            foreach(HornClause h in hornClauses)
            {
                //For every clause, check every symbol in h's premise and add new ones to the inferred list
                foreach (string p in h.premise)
                    if (!inferred.ContainsKey(p))
                        //All symbols are initially uninferred.
                        inferred.Add(p, false);

                //If there is no conclusion, the premise is true, add it to the agenda
                if (h.conclusion == null)
                {
                    //There should only be one premise in a true clause
                    agenda.Push(h.premise[0]);
                }
                //If there is a conclusion, the premises must be proven before the conclusion is inferred
                else
                {
                    //Add each non-truth clause to the count list
                    count.Add(h, h.premise.Count);
                }
            }

            //As long as there are new symbols in the agenda, see what can be proven
            while (agenda.Count > 0)
            {
                string p = agenda.Pop();

                //Check if we've proven the query
                if (p.Equals(query))
                    return true;

                //If the current agenda item isn't inferred, infer it now
                if (inferred.ContainsKey(p))
                {
                    if (inferred[p] == false)
                    {
                        inferred[p] = true;
                        _provenPremise.Add(p);

                        //Only do these steps if we haven't already inferred the symbol
                        //Find other clauses that contain the symbol in the premise that are not truth-clauses
                        foreach (HornClause h in hornClauses)
                        {
                            if ((h.premise.Contains(p)) && (h.conclusion != null))
                            {
                                //If it does, reduce the number of premise symbolst that need to be found
                                count[h]--;
                                if (count[h] == 0)
                                    //If all premise symbols are found, add the conclusion to the agenda
                                    agenda.Push(h.conclusion);
                            }
                        }
                    }

                }
            }

            //If we run out of agenda, we can't infer the query
            return false;
        }

        /// <summary>
        /// Gets the list of proven premises. Returns an empty list if nothings has
        /// been proven.
        /// </summary>
        /// <returns>The plist of proven premises.</returns>
        public List<string> GetProvenPremise()
        {
            return _provenPremise;
        }
    }
}
