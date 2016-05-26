using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            FileReader reader = new FileReader();
            ConvertToCNF CNFConvert = new ConvertToCNF();
            string method = "";

            try
            {
                //Ensure the arugments are valid.
                if (args.Length != 2)
                {
                    throw new Exception("Invalid number of arguments.");
                }
                method = args[0];

                //Read the file. If there is an error, abort operation.
                if (!reader.readFile(args[1]))
                    return;

                //Determine which method to use
                switch (method)
                {
                    case "TT":
                        //Truth table method
                        TruthTable tt = new TruthTable();
                        ConjunctiveNormalForm CNF = new ConjunctiveNormalForm();

                        //Convert the knowledge base to CNF and put it in a list of strings
                        List<NodeOrStringInterface> stringListKB = CNF.ConvertToStringList(CNFConvert.ConvertCNF((reader.GetKB())));

                        //Create a binary tree from the string list
                        NodeOrStringInterface rootNodeKB = CNF.CreateBinaryTree(stringListKB);

                        //Evaluate the binary tree with the query
                        NodeOrStringInterface rootNodeQuery = new LeafNode(reader.GetQuery());

                        //Report the results including the number of true models
                        if(tt.TruthTableEntails(rootNodeKB, rootNodeQuery))
                        {
                            Console.Write("YES: " + tt.NumberOfOnesInTruthTable);
                        }
                        else
                        {
                            Console.Write("NO: " + tt.NumberOfOnesInTruthTable);
                        }
                        break;

                    case "BC":
                        //Backward chain method
                        AlternateBackChain bCP = new AlternateBackChain();
                        HornClauseReader BCHReader = new HornClauseReader();

                        //Convert the knowledge base into horn clause form
                        List<HornClause> hornClausesBC = BCHReader.GetHornClause(reader.GetKB());

                        //If the knowledge base can be converted to horn form
                        if (hornClausesBC != null)
                        {
                            //Use backward chaining to determine the query
                            if (bCP.BCProver(hornClausesBC, reader.GetQuery()))
                            {
                                Console.Write("YES: ");
                            }
                            else
                            {
                                Console.Write("NO: ");
                            }
                            //List the premises proven
                            foreach (string s in bCP._provenPremises)
                            {
                                Console.Write(s + " ");
                            }
                        }
                        break;

                    case "FC":
                        //Forward chain method
                        ForwardChainProver fCP = new ForwardChainProver();
                        HornClauseReader FCHReader = new HornClauseReader();

                        //Convert the knowledge base into horn clause form
                        List<HornClause> hornClausesFC = FCHReader.GetHornClause(reader.GetKB());

                        //If the knowledge base can be converted to horn form
                        if (hornClausesFC != null)
                        {
                            //Use backward chaining to determine the query
                            if (fCP.ForwardChainEntails(FCHReader.GetHornClause(reader.GetKB()), reader.GetQuery()))
                            {
                                Console.Write("YES: ");
                            }
                            else
                            {
                                Console.Write("NO: ");
                            }
                            //List the premises proven
                            foreach (string s in fCP.GetProvenPremise())
                            {
                                Console.Write(s + " ");
                            }
                        }
                        break;

                    case "R":
                        //Resolution algorithm
                        ResolutionProver rP = new ResolutionProver();

                        //Convert the knowldege base to CNF and query it using resolution algorithm
                        if (rP.Query(CNFConvert.ConvertCNF((reader.GetKB())), reader.GetQuery()))
                        {
                            Console.Write("YES: ");

                            //Print relevant clauses.
                            Stack<Resolvant> relevantClauses = new Stack<Resolvant>();
                            relevantClauses.Push(rP._resolvantChain);
                            while (relevantClauses.Count > 0)
                            {
                                Resolvant current = relevantClauses.Pop();
                                if (current._parentA != null)
                                {
                                    relevantClauses.Push(current._parentA);
                                    relevantClauses.Push(current._parentB);
                                }
                                Console.Write(current._clause + " ");
                            }
                        }
                        else
                        {
                            Console.Write("NO");
                        }
                        break;

                    default:
                        Console.Write("Unrecognised method");
                        break;
                }
            }

            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            Console.Write("\n");

        }

    }
}