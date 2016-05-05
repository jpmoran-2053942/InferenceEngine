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
            List<string> convertedStringList = ConvertToStringList(propositionalLogic);


            return null;
        }

        private List<string> ConvertToStringList(string propositionalLogic)
        {
            string tempString = "";
            List<string> returningList = new List<string>();

            for (int i = 0; i < propositionalLogic.Length; i++)
            {
                if(_specialCharacters.Contains(propositionalLogic[i]))
                {
                    if (!tempString.Equals(""))
                    {
                        returningList.Add(tempString);
                    }
                    returningList.Add(propositionalLogic[i].ToString());
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
