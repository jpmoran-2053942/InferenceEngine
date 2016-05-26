using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Includes methods to get the horn clause form of knowledge bases.
/// </summary>
namespace InferenceEngine
{
    class HornClauseReader : ClauseParsing
    {
        /// <summary>
        /// Gets a list of HornClauses from a knowledge base. Returns null and writes a message
        /// if the knowledge base is not in horn clause form. Assumes there is only one implication
        /// per sentence.
        /// </summary>
        /// <param name="knowledgeBase">The knowledge base as an array of strings.</param>
        /// <returns>A list of HornClause. Returns null if not in horn form.</returns>
        public List<HornClause> GetHornClause(string[] knowledgeBase)
        {
            List<HornClause> returningList = new List<HornClause>();
            int connectiveIndex = 0;
            string tempPremise = "", tempConclusion = "";

            foreach (string s in knowledgeBase)
            {
                //Get the main connective of each sentence in the knowledge base.
                connectiveIndex = GetMainConnective(s);

                //If there isn't one, it is a truth statement.
                if (connectiveIndex == -1)
                {
                    returningList.Add(new HornClause(s));
                }

                //If it is an implication, it is in horn form.
                else if (s[connectiveIndex] == '>')
                {
                    //Split the sentence into before and after the implication. Before is the premise,
                    //after is the conclusion.
                    tempPremise = s.Split('>')[0];
                    tempConclusion = s.Split('>')[1];

                    returningList.Add(new HornClause(GetListOfDisjunctions(tempPremise), tempConclusion));
                }

                //If it is anything else, the sentence is not in horn form.
                else
                {
                    Console.WriteLine("Knowledge Base not in horn form.");
                    return null;
                }
            }

            return returningList;
        }

    }

}
