using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class ConjunctiveNormalForm
    {
        char[] _specialCharacters;

        public ConjunctiveNormalForm()
        {
            _specialCharacters = new char[] { '(', ')', '-', '&', '+' };
        }
        public Node CreateBinaryTree(string propositionalLogic)
        {
            List<NodeOrStringInterface> convertedStringList = ConvertToStringList(propositionalLogic);

            foreach (NodeOrStringInterface s in convertedStringList)
            {
                if (s.Equals("("))
                {

                }
            }

            return null;
        }

        private List<NodeOrStringInterface> ConvertToStringList(string propositionalLogic)
        {
            string tempString = "";
            List<NodeOrStringInterface> returningList = new List<NodeOrStringInterface>();

            for (int i = 0; i < propositionalLogic.Length; i++)
            {
                if(_specialCharacters.Contains(propositionalLogic[i]))
                {
                    if (!tempString.Equals(""))
                    {
                        returningList.Add(new HoldsString(tempString));
                    }
                    returningList.Add(new HoldsString(propositionalLogic[i].ToString()));
                    tempString = "";
                }
                else
                {
                    tempString += propositionalLogic[i];
                }
            }
            return returningList;
        }
    }
}
