using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class ConvertToCNF
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

        char[] _specialCharacters;

        public ConvertToCNF()
        {
            _specialCharacters = new char[] { '(', ')', '-', '&', '+' };
        }

        public string ConvertCNF(string[] _knowledgebase)
        {
            string fullSentence = "";

            //Make a copy of the knowledge base to convert each part.
            string[] convertedKnowledgeBase = new string[_knowledgebase.Length];
            _knowledgebase.CopyTo(convertedKnowledgeBase, 0);

            for (int i = 0; i < convertedKnowledgeBase.Length; i++)
            {
                convertedKnowledgeBase[i] = ConvertSingle(convertedKnowledgeBase[i]);

                //Combine each converted sentence into a single sentence
                if (convertedKnowledgeBase[i] != "")
                {
                    if (fullSentence == "")
                        fullSentence += convertedKnowledgeBase[i];
                    else
                        fullSentence += "&" + convertedKnowledgeBase[i];
                }
            }
            
            return RemoveRedundancies(fullSentence);
        }

        private string RemoveRedundancies(string sentence)
        {
            string tempString = "", convertedSentence = "";
            List<string> stringList = new List<string>();

            //Split the sentence by &
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
            if(tempString != "")
            {
                stringList.Add(tempString);
            }

            //For each sub string, list the positive and negative literals
            List<string> containsTrue = new List<string>();
            List<string> containsFalse = new List<string>();
            tempString = "";

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

                tempString = "";
                containsTrue.Clear();
                containsFalse.Clear();
            }

            return convertedSentence;
        }

        private string ConvertSingle(string sentence)
        {
            int indexOfConnective = GetMainConnective(sentence);
            string P = "", Q = "";
            
            //If there is no main connective, the sentence is a literal.
            if (indexOfConnective == -1)
                return sentence;

            switch (sentence[indexOfConnective])
            {
                case '&':
                    //Convert each sentence that makes up the main connective.
                    P = ConvertSingle(RemoveUnpairedBrackets(sentence.Substring(0, indexOfConnective)));
                    Q = ConvertSingle(RemoveUnpairedBrackets(sentence.Substring(indexOfConnective + 1)));

                    //The CNF of a disjunction of two CNF sentences is the disjunction of the two sentences.
                    return P + "&" + Q;
                case '+':
                    P = ConvertSingle(RemoveUnpairedBrackets(sentence.Substring(0, indexOfConnective)));
                    Q = ConvertSingle(RemoveUnpairedBrackets(sentence.Substring(indexOfConnective + 1)));

                    string[] disjunctP = GetListOfDisjunctions(P);
                    string[] disjunctQ = GetListOfDisjunctions(Q);

                    string newSentence = "";

                    //The CNF of a conjunction of two CNF sentences is the disjunction of the conjunction each
                    //combination of disjunctions.
                    foreach (string sP in disjunctP)
                    {
                        foreach (string sQ in disjunctQ)
                        {
                            if (newSentence == "")
                                newSentence += "(" + sP + "+" + sQ + ")";
                            else
                                newSentence += "&(" + sP + "+" + sQ + ")";
                        }
                    }

                    return newSentence;
                case '-':
                    string O = ConvertSingle(RemoveUnpairedBrackets(sentence.Substring(indexOfConnective + 1)));
                    int mainConnectiveO = GetMainConnective(O);

                    //If O is a literal, return not O
                    if (mainConnectiveO == -1)
                        return "-" + O;

                    //If there is a main connective, it is either +, & or -
                    if (mainConnectiveO > 0)
                        P = ConvertSingle(RemoveUnpairedBrackets(O.Substring(0, mainConnectiveO)));
                    Q = ConvertSingle(RemoveUnpairedBrackets(O.Substring(mainConnectiveO + 1)));

                    switch (O[mainConnectiveO])
                    {
                        //DeMorgan's Law
                        case '&':
                            return ConvertSingle("-(" + P + ")+-(" + Q + ")");
                        //DeMorgan's Law
                        case '+':
                            return ConvertSingle("-(" + P + ")&-(" + Q + ")");
                        //Double negation
                        case '-':
                            return Q;
                        default:
                            break;
                    }

                    break;
                case '>':
                    P = ConvertSingle(RemoveUnpairedBrackets(sentence.Substring(0, indexOfConnective)));
                    Q = ConvertSingle(RemoveUnpairedBrackets(sentence.Substring(indexOfConnective + 1)));

                    //The CNF of an implication of two CNF sentences is the conjunction as so
                    return ConvertSingle("-(" + P + ")+" + Q);
                case '=':
                    P = ConvertSingle(RemoveUnpairedBrackets(sentence.Substring(0, indexOfConnective)));
                    Q = ConvertSingle(RemoveUnpairedBrackets(sentence.Substring(indexOfConnective + 1)));

                    //The CNF of an equivalence of two CNF sentences is the conjunction as so
                    return ConvertSingle("(" + P + "&" + Q + ")+(-(" + P + ")&-(" + Q + "))");
                default:
                    break;
            }

            return null;
        }

        /// <summary>
        /// Returns the index of the main connective in the sentence.
        /// Returns -1 if the sentence is a literal.
        /// </summary>
        /// <param name="sentence">A sentence.</param>
        /// <returns></returns>
        private int GetMainConnective(string sentence)
        {
            int index = 0, bracketNesting = 0, indexOfMain = -1, maxAllowNesting = 0, highestNesting = 0;
            Connectives mainConnective = Connectives.Nil;

            do
            {
                index = 0;
                foreach (char c in sentence)
                {
                    switch (c)
                    {
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

        private Connectives getConnectivePrecedence(char c)
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

        private string RemoveUnpairedBrackets (string sentence)
        {
            int bracketNesting = 0;

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

            if (bracketNesting < 0)
            {
                for (int i = sentence.Length-1; i >= 0; i--)
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
        /// <param name="sentence"></param>
        /// <returns></returns>
        private string[] GetListOfDisjunctions (string sentence)
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

        private string CharListToString (List<char> charList)
        {
            string final = "";

            foreach (char c in charList)
                final += c;

            return final;
        }

    }
}
