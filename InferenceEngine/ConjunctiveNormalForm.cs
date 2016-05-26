using System;
using System.Collections.Generic;
using System.Linq;


namespace InferenceEngine
{
    /// <summary>
    /// This class is used to evaluate propositional logic statements
    /// </summary>
    class ConjunctiveNormalForm
    {
        char[] _specialCharacters;

        /// <summary>
        /// Creates a new instance of the Conjunctive Normal Form
        /// </summary>
        public ConjunctiveNormalForm()
        {
            _specialCharacters = new char[] { '(', ')', '-', '&', '+' };
        }

        /// <summary>
        /// Creates a binary tree for the purposes of evaluating logic statements
        /// Each operation and term in the equation becomes its own node
        /// </summary>
        /// <param name="convertedStringList">The list of all symbols and operators in the equation</param>
        /// <returns>Returns the root node of the generated binary tree</returns>
        public NodeOrStringInterface CreateBinaryTree(List<NodeOrStringInterface> convertedStringList)
        {
            int leftBracketIndex = 0;
            int rightBracketIndex = 0;
            int numberOfBrackets = 0;

            //check for brackets first
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

        /// <summary>
        /// Converts a propositional logic statement in string form to a list with each operator and literal from the statement
        /// in the correct order.
        /// </summary>
        /// <param name="propositionalLogic">The propositional logic statement to convert</param>
        /// <returns>A list containing all operators and literals.</returns>
        public List<NodeOrStringInterface> ConvertToStringList(string propositionalLogic)
        {
            string tempString = "";
            List<NodeOrStringInterface> returningList = new List<NodeOrStringInterface>();

            for (int i = 0; i < propositionalLogic.Length; i++)
            {
                //If we find a special character (operator) add the previous literal (may be multiple characters)
                //Then, add the special character 
                if (_specialCharacters.Contains(propositionalLogic[i]))
                {
                    //only add if there is a tempString - there may not be since two operators can appear in a row
                    //e.g. -(b+a) (the bracket follows directly after the negative)
                    if (!tempString.Equals(""))
                    {
                        returningList.Add(new HoldsString(tempString));
                    }
                    returningList.Add(new HoldsString(propositionalLogic[i].ToString()));
                    tempString = "";
                }
                else
                {
                    //if we haven't found a special character, we have found a literal (or part of it)
                    //store this until we find the next special character
                    tempString += propositionalLogic[i];
                }
            }
            //add the final term in the statement if there is one left (statement doesn't end with bracket)
            if(tempString != "")
            {
                returningList.Add(new HoldsString(tempString));
            }
            return returningList;
        }

        /// <summary>
        /// Validates a propositional logic statement for a given model
        /// </summary>
        /// <param name="propositionalLogic">the statement to validate</param>
        /// <param name="model">the model to evaluate with</param>
        /// <returns>true if the statement is true for the given model, false otherwise</returns>
        public bool EvaluateLogic(string propositionalLogic, List<string> model)
        {
            List<NodeOrStringInterface> convertedList = ConvertToStringList(propositionalLogic);
            bool result = false; 

            try
            {
                result = CreateBinaryTree(convertedList).Evaluate(model);
            }
            catch(Exception e)
            {
                //catches exception thrown from improperly converted binary tree - strings present when evaluating logic
                Console.WriteLine(e.Message);
            }
            return result;
        }
    }
}