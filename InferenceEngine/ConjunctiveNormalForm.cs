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
                //Find the start of a bracket set
                if (convertedStringList[i].IsEqualTo("("))
                {
                    if (numberOfBrackets == 0)
                    {
                        leftBracketIndex = i;
                    }
                    //Track nested brackets
                    numberOfBrackets++;
                }
                //Find the end of a bracket set
                if (convertedStringList[i].IsEqualTo(")"))
                {
                    numberOfBrackets--;
                    if (numberOfBrackets == 0)
                    {
                        rightBracketIndex = i;
                    }
                }

                //Detect when a complete bracket set has been found
                if ((numberOfBrackets == 0) && (rightBracketIndex != 0))
                {
                    //Evaluate the bracket set as a new sentence by creating a sub-list of the bracket's contents and
                    //running a recursive function call on it (the recursive call is to handle nested brackets)
                    List<NodeOrStringInterface> bracketedList = new List<NodeOrStringInterface>();
                    for (int j = leftBracketIndex + 1; j < rightBracketIndex; j++)
                    {
                        bracketedList.Add(convertedStringList[j]);
                    }
                    //Remove the bracket sub sentence
                    for (int j = rightBracketIndex; j >= leftBracketIndex; j--)
                    {
                        convertedStringList.RemoveAt(j);
                    }
                    //Add the created sub-tree in the place of the bracketed sentence
                    convertedStringList.Insert(leftBracketIndex, CreateBinaryTree(bracketedList));
                    //Update the placement of i in the for loop to reflect the changes to the list
                    i -= rightBracketIndex - leftBracketIndex;
                    //Reset bracket search variables
                    leftBracketIndex = rightBracketIndex = 0;
                }
            }

            //check for negation
            for (int i = 0; i < convertedStringList.Count; i++)
            {
                if (convertedStringList[i].IsEqualTo("-"))
                {
                    //If a negation is found, create a negation node with the child as the literal following the negation
                    //Convert the literal to a node if a sub-tree hasn't already been created
                    if (!convertedStringList[i + 1].IsANode())
                    {
                        convertedStringList[i + 1] = convertedStringList[i + 1].GetNode();
                    }
                    //Add the new subtree in place of the negation string
                    NOTNode tempNode = new NOTNode(convertedStringList[i + 1]);
                    convertedStringList[i] = tempNode;
                    convertedStringList.Remove(tempNode.Child);
                    //Negation does not need to update the search position of i, as it only replace two string entries
                }
            }

            //check for AND
            for (int i = 0; i < convertedStringList.Count; i++)
            {
                if (convertedStringList[i].IsEqualTo("&"))
                {
                    //If an AND is found, create an AND node with the children as the literals following and preceeding the AND
                    //Convert the literals to nodes if a sub-tree hasn't already been created
                    if (!convertedStringList[i - 1].IsANode())
                    {
                        convertedStringList[i - 1] = convertedStringList[i - 1].GetNode();
                    }
                    if (!convertedStringList[i + 1].IsANode())
                    {
                        convertedStringList[i + 1] = convertedStringList[i + 1].GetNode();
                    }
                    //Add the new subtree in place of the AND string
                    ANDNode tempNode = new ANDNode(convertedStringList[i - 1], convertedStringList[i + 1]);
                    convertedStringList[i] = tempNode;
                    convertedStringList.Remove(tempNode.LeftChild);
                    convertedStringList.Remove(tempNode.RightChild);
                    //Update the placement of i in the for loop to reflect the changes to the list
                    i -= 1;
                }
            }

            //check for OR
            for (int i = 0; i < convertedStringList.Count; i++)
            {
                //If an AND is found, create an OR node with the children as the literals following and preceeding the OR
                //Convert the literals to nodes if a sub-tree hasn't already been created
                if (convertedStringList[i].IsEqualTo("+"))
                {
                    if (!convertedStringList[i - 1].IsANode())
                    {
                        convertedStringList[i - 1] = convertedStringList[i - 1].GetNode();
                    }
                    if (!convertedStringList[i + 1].IsANode())
                    {
                        convertedStringList[i + 1] = convertedStringList[i + 1].GetNode();
                    }
                    //Add the new subtree in place of the OR string
                    ORNode tempNode = new ORNode(convertedStringList[i - 1], convertedStringList[i + 1]);
                    convertedStringList[i] = tempNode;
                    convertedStringList.Remove(tempNode.LeftChild);
                    convertedStringList.Remove(tempNode.RightChild);
                    //Update the placement of i in the for loop to reflect the changes to the list
                    i -= 1;
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
                if (_specialCharacters.Contains(propositionalLogic[i]))
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