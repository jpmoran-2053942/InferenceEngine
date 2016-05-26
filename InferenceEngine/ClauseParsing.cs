using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Contains methods to simplify and interact with clauses. Includes methods that 
/// </summary>
namespace InferenceEngine
{
    //List of connectives in order of precedence.
    enum Connectives
    {
        Bicondition,
        Imply,
        Or,
        And,
        Not,
        Literal,
        Nil
    };

    class ClauseParsing
    {
        /// <summary>
        /// Takes a CNF sentence as a string and rewrites it in a standard form. This involves,
        /// for each conjunction, removing repeated symbols and ordering them by having true
        /// symbols first, then by alphabetical order. It also removes conjunctions that include
        /// both the positive and negative of a symbol. It returns the new list of standard conjunctions
        /// as a string.
        /// </summary>
        /// <param name="sentence">A propositional logic sentence in CNF.</param>
        /// <returns>New sentence in standard form.</returns>
        protected string RemoveRedundancies(string sentence)
        {
            string tempString = "", convertedSentence = "";
            List<string> stringList = new List<string>();

            //Split the sentence by & and store the list in stringList
            for (int i = 0; i < sentence.Length; i++)
            {
                if (sentence[i] == '&')
                {
                    if (!tempString.Equals(""))
                    {
                        stringList.Add(tempString);
                    }
                    tempString = "";
                }
                else
                {
                    tempString += sentence[i];
                }
            }
            if (tempString != "")
            {
                stringList.Add(tempString);
            }

            List<string> containsTrue = new List<string>();
            List<string> containsFalse = new List<string>();
            tempString = "";

            //For each sub string, list the positive and negative literals
            foreach (string s in stringList)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    //Ignore the outside brackets
                    if (!(s[i] == '(') && !(s[i] == ')'))
                    {
                        //Since it is in CNF, all literals will be seperated by +
                        if (s[i] == '+')
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
                            tempString += s[i];
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
                
                //Sort the lists in alphabetical order, used to ensure equivilent clauses are the same string.
                containsFalse.Sort();
                containsTrue.Sort();

                tempString = "";

                bool redundant = false;

                //Determine if a sub string is redundant ie contains both the positive and negative of a literal
                foreach (string sT in containsTrue)
                {
                    foreach (string sF in containsFalse)
                        if (sT.Equals(sF))
                            redundant = true;
                }

                //If it is not redundant, write a new sentence containing only non-redundant literals
                //Since each literal is only listed once, it also removes redundancies internal to substrings
                //eg (a+b+b) becomes (a+b)
                if (!redundant)
                {
                    foreach (string sT in containsTrue)
                    {
                        if (tempString == "")
                            tempString += sT;
                        else
                            tempString += "+" + sT;
                    }

                    foreach (string sF in containsFalse)
                    {
                        if (tempString == "")
                            tempString += "-" + sF;
                        else
                            tempString += "+-" + sF;
                    }

                    if (convertedSentence == "")
                        convertedSentence += "(" + tempString + ")";
                    else
                        convertedSentence += "&(" + tempString + ")";
                }

                //If it is redundant, don't write the substring

                //Clear the lists for the next substring to check
                tempString = "";
                containsTrue.Clear();
                containsFalse.Clear();
                
                //Test the next substring
            }

            //Converted sentences is the new, standard sentence
            return convertedSentence;
        }

        /// <summary>
        /// Returns the index of the main connective in the sentence.
        /// Returns -1 if the sentence is a literal.
        /// </summary>
        /// <param name="sentence">A proper logic sentence.</param>
        /// <returns>-1 if the sentence is a literal, otherwise the index of the main connective</returns>
        protected int GetMainConnective(string sentence)
        {
            int index = 0, bracketNesting = 0, indexOfMain = -1, maxAllowNesting = 0, highestNesting = 0;
            Connectives mainConnective = Connectives.Nil;

            //Search through the string for a logic operator
            do
            {
                index = 0;
                foreach (char c in sentence)
                {
                    switch (c)
                    {
                        //If a bracket is found and no other operator is found, look inside this bracket
                        //by setting the next loop to ignore the first encountered bracket
                        case '(':
                            bracketNesting++;
                            if (bracketNesting > highestNesting)
                                highestNesting = bracketNesting;
                            break;
                        case ')':
                            bracketNesting--;
                            break;
                        case '-':
                        case '&':
                        case '+':
                        case '>':
                        case '=':
                            //Only look for connectives if they are in the outer most bracket nesting
                            if (bracketNesting == maxAllowNesting)
                            {
                                //Only note the location if the precedence is lower than the current
                                if (getConnectivePrecedence(c) < mainConnective)
                                {
                                    indexOfMain = index;
                                    mainConnective = getConnectivePrecedence(c);
                                }
                            }

                            break;
                        default:
                            break;
                    }

                    index++;
                }

                //If no main connective was found, look inside the outermost bracket layer
                if (indexOfMain == -1)
                    maxAllowNesting++;
            } while ((indexOfMain == -1) && (maxAllowNesting <= highestNesting));

            //Returns -1 if no main connective found ie it is a literal.
            return indexOfMain;
        }

        /// <summary>
        /// Determines the precedence of an operator.
        /// </summary>
        /// <param name="c">The character to test.</param>
        /// <returns>The equivalent "Connectives" enumeration</returns>
        protected Connectives getConnectivePrecedence(char c)
        {
            switch (c)
            {
                case '-':
                    return Connectives.Not;
                case '&':
                    return Connectives.And;
                case '+':
                    return Connectives.Or;
                case '>':
                    return Connectives.Imply;
                case '=':
                    return Connectives.Bicondition;
            }

            return Connectives.Literal;
        }

        /// <summary>
        /// Takes a sentence and removes any unpaired brackets.
        /// </summary>
        /// <param name="sentence">A logic sentence.</param>
        /// <returns>The new sentence with unpaired brackets removed.</returns>
        protected string RemoveUnpairedBrackets(string sentence)
        {
            int bracketNesting = 0;

            //Look through the sentence and count the number of left and right brackets.
            //The resulting bracketNesting is positive if there are more left than right,
            //or negative if more right than left. It is zero if there are no outstanding
            //brackets.
            foreach (char c in sentence)
            {
                switch (c)
                {
                    case '(':
                        bracketNesting++;
                        break;
                    case ')':
                        bracketNesting--;
                        break;
                    default:
                        break;
                }
            }

            List<char> newSentence = new List<char>();

            //There are no unpaired brackets.
            if (bracketNesting == 0)
                return sentence;

            //More left than right
            //Remove left brackets from the left side of the sentence
            if (bracketNesting > 0)
            {
                for (int i = 0; i < sentence.Length; i++)
                {
                    if ((bracketNesting == 0) || (sentence[i] != '('))
                        newSentence.Add(sentence[i]);
                    else
                        bracketNesting--;
                }
            }

            //More right than left
            //Remove right brackets from the right side of the sentence
            if (bracketNesting < 0)
            {
                for (int i = sentence.Length - 1; i >= 0; i--)
                {
                    if ((bracketNesting == 0) || (sentence[i] != ')'))
                        newSentence.Insert(0, sentence[i]);
                    else
                        bracketNesting++;
                }
            }

            return CharListToString(newSentence);
        }

        /// <summary>
        /// Expects to take the argument as a sentence in CNF.
        /// It will split the string with & as delimiator and remove
        /// any gratuitous brackets from the substrings.
        /// </summary>
        /// <param name="sentence">A sentence in CNF.</param>
        /// <returns>List of disjunctions in the sentence</returns>
        protected string[] GetListOfDisjunctions(string sentence)
        {
            string[] list = sentence.Split('&');
            string[] bracketsRemoved = new string[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                foreach (char c in list[i])
                {
                    if ((c != '(') && (c != ')'))
                        bracketsRemoved[i] += c;
                }
            }

            return bracketsRemoved;
        }

        /// <summary>
        /// Converts a char list to a string.
        /// </summary>
        /// <param name="charList">A list of characters</param>
        /// <returns>The concatenated string</returns>
        protected string CharListToString(List<char> charList)
        {
            string final = "";

            foreach (char c in charList)
                final += c;

            return final;
        }

    }
}
