using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class ConvertToCNF : ClauseParsing
    {
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

        public string ConvertCNF(string _knowledgebase)
        {
            string[] dummyArray = { _knowledgebase};
            return ConvertCNF(dummyArray);
        }

        protected string ConvertSingle(string sentence)
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
        
    }
}
