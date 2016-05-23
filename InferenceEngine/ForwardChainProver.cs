using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class ForwardChainProver
    {
        public bool ForwardChainEntails(List<HornClause> hornClauses, string query)
        {
            if (hornClauses == null)
                return false;

            Dictionary<HornClause, int> count = new Dictionary<HornClause, int>();
            Dictionary<string, bool> inferred = new Dictionary<string, bool>();
            Stack<string> agenda = new Stack<string>();

            foreach(HornClause h in hornClauses)
            {
                //For every clause, check every symbol in h's premise and add new ones to the inferred list
                foreach (string p in h.premise)
                    if (!inferred.ContainsKey(p))
                        inferred.Add(p, false);

                //If there is no conclusion, the premise is true, add it to the agenda
                if (h.conclusion == null)
                {
                    //There should only be one premise in a true clause
                    agenda.Push(h.premise[0]);
                }
                else
                {
                    //Add each non-truth clause to the count list
                    count.Add(h, h.premise.Count);
                }
            }

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
    }
}
