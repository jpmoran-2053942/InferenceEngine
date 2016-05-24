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
                if (args.Length != 2)
                {
                    throw new Exception("Invalid number of arguments.");
                }

                method = args[0];
                reader.readFile(args[1]);

                switch (method)
                {
                    case "TT":
                        TruthTable tt = new TruthTable();
                        ConjunctiveNormalForm CNF = new ConjunctiveNormalForm();
                        List<NodeOrStringInterface> stringListKB = CNF.ConvertToStringList(CNFConvert.ConvertCNF((reader.GetKB())));
                        NodeOrStringInterface rootNodeKB = CNF.CreateBinaryTree(stringListKB);
                        NodeOrStringInterface rootNodeQuery = new LeafNode(reader.GetQuery());
                        if(tt.TruthTableEntails(rootNodeKB, rootNodeQuery))
                        {
                            Console.WriteLine("YES: " + tt.NumberOfOnesInTruthTable);
                        }
                        else
                        {
                            Console.WriteLine("NO: " + tt.NumberOfOnesInTruthTable);
                        }
                        break;
                    case "BC":
                        BackwardChainingProver bCP = new BackwardChainingProver();
                        HornClauseReader BCHReader = new HornClauseReader();
                        if (bCP.BackwardChainCheck(BCHReader.GetHornClause(reader.GetKB()), reader.GetQuery()))
                        {
                            Console.Write("YES: ");
                        }
                        else
                        {
                            Console.Write("NO: ");
                        }
                        foreach (string s in bCP.ProvenPremises)
                        {
                            Console.Write(s + " ");
                        }
                        break;
                    case "FC":
                        ForwardChainProver fCP = new ForwardChainProver();
                        HornClauseReader FCHReader = new HornClauseReader();
                        if (fCP.ForwardChainEntails(FCHReader.GetHornClause(reader.GetKB()), reader.GetQuery()))
                        {
                            Console.Write("YES: ");
                        }
                        else
                        {
                            Console.Write("NO: ");
                        }
                        foreach (string s in fCP.GetProvenPremise())
                        {
                            Console.Write(s + " ");
                        }
                        break;
                    case "R":
                        ResolutionProver rP = new ResolutionProver();
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
                        Console.WriteLine("Unrecognised method");
                        break;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}