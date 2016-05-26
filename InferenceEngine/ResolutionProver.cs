using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Contains methods to run the Resolution Algorithm to determine inference. The
/// implementation has an error that is common to all applications for the
/// resolution algorithm: it will incorrectly infer properties if any of the
/// clauses create an empty clause.
/// </summary>
namespace InferenceEngine
{
    class ResolutionProver : ClauseParsing
    {
        //Stores the list of resolvents used to determine the empty clause.
        //Use the parent property to determine the chain.
        public Resolvant _resolvantChain = null;

        /// <summary>
        /// Uses the resolution algorithm to query the knowledge base.
        /// </summary>
        /// <param name="KB">The knowledge base as a string. Must be a single sentence.</param>
        /// <param name="query">The query as a string.</param>
        public bool Query(string KB, string query)
        {
            //Reset the chain from previous iterations.
            _resolvantChain = null;

            ConvertToCNF cnf = new ConvertToCNF();
            List<Resolvant> clauses = new List<Resolvant>();
            List<Resolvant> fullResolvents = new List<Resolvant>();

            //Construct the full sentence used in resolution algorithm
            string fullSentence = KB + "&-(" + query + ")";

            //Ensure the sentence is in CNF
            string CNF = cnf.ConvertCNF(fullSentence);

            //List the sentence as a list of disjunctions and add them to the initial
            //list of clauses.
            string[] clausesArray = GetListOfDisjunctions(CNF);
            foreach (string s in clausesArray)
            {
                clauses.Add(new Resolvant(null, null, s));
            }

            while (true)
            {
                //Resolve every pair of clauses.
                for (int i = 0; i < clauses.Count; i++)
                {
                    for (int j = 0; j < clauses.Count; j++)
                    {
                        List<Resolvant> subResolvents = new List<Resolvant>();
                        if (i != j)
                        {
                            Resolvant newResolvant = Resolve(clauses[i], clauses[j]);
                            subResolvents.Add(newResolvant);

                            //If a resovlent is empty, then the query is true. Reductio ad Absurdum.
                            if (newResolvant._clause.Equals(""))
                            {
                                _resolvantChain = newResolvant;
                                return true;
                            }

                            //Add the new resolvent to the list of full resolvents.
                            Union(fullResolvents, subResolvents);
                        }

                    }
                }

                //If no new resolvents were made, the query cannot be inferred. We have done all we can and
                //have not proven the query.
                if (ContainsAll(clauses, fullResolvents))
                    return false;

                //Add the new clauses to the list of clauses.
                Union(clauses, fullResolvents);
            }

        }

        /// <summary>
        /// Resolve two clauses clauseA and clauseB. It returns the resolvent
        /// in standard CNF. Can return the empty clause.
        /// </summary>
        /// <param name="clauseA">Clause a.</param>
        /// <param name="clauseB">Clause b.</param>
        /// <returns>The resolvent with parents set as clauseA and clauseB.</returns>
        private Resolvant Resolve(Resolvant clauseA, Resolvant clauseB)
        {
            //Create the new clause by conjoining the parent clauses.
            string newClause = clauseA._clause + "+" + clauseB._clause;

            //List the positive and negative literals in the new clause
            List<string> containsTrue = new List<string>();
            List<string> containsFalse = new List<string>();
            string tempString = "";

            for (int i = 0; i < newClause.Length; i++)
            {
                //Ignore the outside brackets
                if (!(newClause[i] == '(') && !(newClause[i] == ')'))
                {
                    //Since it is in CNF, all literals will be seperated by +
                    if (newClause[i] == '+')
                    {
                        if (!tempString.Equals(""))
                        {
                            //Once a literal is found, add it to the correct list if not already there
                            if (tempString[0] == '-')
                            {
                                if (!containsFalse.Contains(tempString.Substring(1)))
                                    containsFalse.Add(tempString.Substring(1));
                            }
                            else
                            {
                                if (!containsTrue.Contains(tempString))
                                    containsTrue.Add(tempString);
                            }
                        }
                        tempString = "";
                    }
                    else
                    {
                        tempString += newClause[i];
                    }
                }
            }

            //Same as above, repeated in the case of the final literal
            if (tempString != "")
            {
                if (!tempString.Equals(""))
                {
                    if (tempString[0] == '-')
                    {
                        if (!containsFalse.Contains(tempString.Substring(1)))
                            containsFalse.Add(tempString.Substring(1));
                    }
                    else
                    {
                        if (!containsTrue.Contains(tempString))
                            containsTrue.Add(tempString);
                    }
                }
            }

            //Sort the lists in alphabetical order, used to ensure equivilent clauses
            //are equal.
            containsFalse.Sort();
            containsTrue.Sort();

            tempString = "";

            //Remove literals where both the positive and negative form appear, as they are redundant.
            for (int i = 0; i < containsTrue.Count; i++)
            {
                if (containsFalse.Contains(containsTrue[i]))
                {
                    containsFalse.Remove(containsTrue[i]);
                    containsTrue.Remove(containsTrue[i]);
                    i--;
                }
            }

            newClause = "";

            //Write a new sentence containing only non-redundant literals
            //Since each literal is only listed once, it also removes redundancies internal to substrings
            //eg (a+b+b) becomes (a+b)
            foreach (string sT in containsTrue)
            {
                if (newClause == "")
                    newClause += sT;
                else
                    newClause += "+" + sT;
            }

            foreach (string sF in containsFalse)
            {
                if (newClause == "")
                    newClause += "-" + sF;
                else
                    newClause += "+-" + sF;
            }

            tempString = "";
            containsTrue.Clear();
            containsFalse.Clear();

            //Return a resolvent with parents as clauseA and clauseB, and the clause as what was determined.
            return new Resolvant(clauseA, clauseB, newClause);
        }

        /// <summary>
        /// Determines whether all of B is contained in A.
        /// </summary>
        /// <returns><c>true</c>, if all of B is in A, <c>false</c> otherwise.</returns>
        /// <param name="A">A the container list.</param>
        /// <param name="B">B the list to check.</param>
        private bool ContainsAll(List<Resolvant> A, List<Resolvant> B)
        {
            if (A.Count == 0)
                return false;

            foreach(Resolvant sB in B)
            {
                //If anything in B is not in A, return false
                if (!Contains(A, sB))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether a Resolvent in contained in the List A.
        /// </summary>
        /// <returns>True if B is in A, false otherwise</returns>
        /// <param name="A">A the list to check.</param>
        /// <param name="B">B the resolvent to test.</param>
        private bool Contains(List<Resolvant> A, Resolvant B)
        {
            //Look through the list A and try to find resolvent that contain B's clause.
            foreach (Resolvant sA in A)
            {
                if (sA._clause.Equals(B._clause))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Combines two lists of Resolvents in a union. That is, combines the lists
        /// such that there are no duplicates in the resulting list.
        /// </summary>
        /// <param name="A">A the first list.</param>
        /// <param name="B">B the second list.</param>
        private List<Resolvant> Union(List<Resolvant> A, List<Resolvant> B)
        {
            //Add each resolvent in B that is not contained in A.
            foreach (Resolvant sB in B)
            {
                if (!Contains(A, sB))
                    A.Add(sB);
            }

            return A;

        }

    }
}
