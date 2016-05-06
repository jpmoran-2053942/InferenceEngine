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

        public NodeOrStringInterface CreateBinaryTree(List<NodeOrStringInterface> convertedStringList)
        {
           // List<NodeOrStringInterface> convertedStringList = ConvertToStringList(propositionalLogic);
            int leftBracketIndex = 0;
            int rightBracketIndex = 0;
            int numberOfBrackets = 0;

            //check brackets first
            for (int i = 0; i < convertedStringList.Count; i++)
            {
                if (convertedStringList[i].IsEqualTo("("))
                {
                    if (numberOfBrackets == 0)
                    {
                        leftBracketIndex = i;
                    }
                    numberOfBrackets++;
                }
                if (convertedStringList[i].IsEqualTo(")"))
                {
                    numberOfBrackets--;
                    if (numberOfBrackets == 0)
                    {
                        rightBracketIndex = i;
                    }
                }
            }
            if(rightBracketIndex != 0)
            {
                List<NodeOrStringInterface> bracketedList = new List<NodeOrStringInterface>();
                for (int i = leftBracketIndex + 1; i < rightBracketIndex; i++)
                {
                    bracketedList.Add(convertedStringList[i]);
                }
                for(int i = rightBracketIndex; i >= leftBracketIndex; i--)
                {
                    convertedStringList.RemoveAt(i);
                }
                convertedStringList.Add(CreateBinaryTree(bracketedList));
            }

            //check for negation
            for (int i = 0; i < convertedStringList.Count; i++)
            {
                if(convertedStringList[i].IsEqualTo("-"))
                {
                    if(!convertedStringList[i+1].IsANode())
                    {
                        convertedStringList[i + 1] = convertedStringList[i+1].GetNode();
                    }
                    NOTNode tempNode = new NOTNode(convertedStringList[i+1]);
                    convertedStringList[i] = tempNode;
                    convertedStringList.Remove(tempNode.Child);
                }
            }

            //check for AND
            for(int i = 0; i < convertedStringList.Count; i++)
            {
                if(convertedStringList[i].IsEqualTo("&"))
                {
                    if (!convertedStringList[i - 1].IsANode())
                    {
                        convertedStringList[i - 1] = convertedStringList[i - 1].GetNode();
                    }
                    if (!convertedStringList[i + 1].IsANode())
                    {
                        convertedStringList[i + 1] = convertedStringList[i + 1].GetNode();
                    }
                    ANDNode tempNode = new ANDNode(convertedStringList[i-1], convertedStringList[i+1]);
                    convertedStringList[i] = tempNode;
                    convertedStringList.Remove(tempNode.LeftChild);
                    convertedStringList.Remove(tempNode.RightChild);
                }
            }
            
            //check for OR
            for (int i = 0; i < convertedStringList.Count; i++)
            {
                if(convertedStringList[i].IsEqualTo("+"))
                {
                    if (!convertedStringList[i - 1].IsANode())
                    {
                        convertedStringList[i - 1] = convertedStringList[i - 1].GetNode();
                    }
                    if (!convertedStringList[i + 1].IsANode())
                    {
                        convertedStringList[i + 1] = convertedStringList[i + 1].GetNode();
                    }
                    ORNode tempNode = new ORNode(convertedStringList[i - 1], convertedStringList[i + 1]);
                    convertedStringList[i] = tempNode;
                    convertedStringList.Remove(tempNode.LeftChild);
                    convertedStringList.Remove(tempNode.RightChild);
                }
            }
            return convertedStringList[0];
        }

        public List<NodeOrStringInterface> ConvertToStringList(string propositionalLogic)
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
