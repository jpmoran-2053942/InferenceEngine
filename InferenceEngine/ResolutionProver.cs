﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class ResolutionProver : ClauseParsing
    {
        public bool Query(string KB, string query)
        {
            ConvertToCNF cnf = new ConvertToCNF();
            string fullSentence = KB + "&-(" + query + ")";
            string CNF = cnf.ConvertCNF(fullSentence);
            string[] clausesArray = GetListOfDisjunctions(CNF);
            List<string> clauses = new List<string>();
            List<string> fullResolvents = new List<string>();

            clauses.AddRange(clausesArray);

            while (true)
            {
                for (int i = 0; i < clauses.Count; i++)
                {
                    for (int j = 0; j < clauses.Count; j++)
                    {
                        List<string> subResolvents = new List<string>();
                        if (i != j)
                        {
                            subResolvents.Add(Resolve(clauses[i], clauses[j]));

                            //If a resovlent is empty, then the query is true.
                            if (subResolvents.Contains(""))
                                return true;

                            Union(fullResolvents, subResolvents);
                        }

                    }
                }

                if (ContainsAll(clauses, fullResolvents))
                    return false;

                Union(clauses, fullResolvents);
            }
        }

        private string Resolve(string clauseA, string clauseB)
        {
            string newClause = clauseA + "+" + clauseB;

            //For each sub string, list the positive and negative literals
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

            //Determine if a sub string is redundant ie contains both the positive and negative of a literal
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

            //If it is not redundant, write a new sentence containing only non-redundant literals
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

            return newClause;
        }

        private bool ContainsAll(List<string> A, List<string> B)
        {
            if (A.Count == 0)
                return false;

            foreach(string sB in B)
            {
                bool contains = false;
                foreach (string sA in A)
                {
                    if (sA.Equals(sB))
                        contains = true;
                }

                if (!contains)
                    return false;
            }

            return true;
        }

        private List<string> Union(List<string> A, List<string> B)
        {
            foreach (string s in B)
                if (!A.Contains(s))
                    A.Add(s);

            return A;

        }

    }
}