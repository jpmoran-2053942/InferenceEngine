using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class HornClauseReader : ClauseParsing
    {
        public List<HornClause> GetHornClause(string[] knowledgeBase)
        {
            List<HornClause> returningList = new List<HornClause>();
            int connectiveIndex = 0;
            string tempPremise = "", tempConclusion = "";

            foreach (string s in knowledgeBase)
            {
                connectiveIndex = GetMainConnective(s);
                if (connectiveIndex == -1)
                {
                    returningList.Add(new HornClause(s));
                }
                else if (s[connectiveIndex] == '>')
                {
                    tempPremise = s.Split('>')[0];
                    tempConclusion = s.Split('>')[1];

                    returningList.Add(new HornClause(GetListOfDisjunctions(tempPremise), tempConclusion));
                }
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
