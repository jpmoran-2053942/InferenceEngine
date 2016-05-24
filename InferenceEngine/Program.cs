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
            ConjunctiveNormalForm CNF = new ConjunctiveNormalForm();
            ConvertToCNF CNFConvert = new ConvertToCNF();
            ResolutionProver rP = new ResolutionProver();
            TruthTable tt = new TruthTable();
            BackwardChainingProver bCP = new BackwardChainingProver();
            string method = "";

            try
            {
                if (args.Length != 2)
                {
                    throw new Exception("Invalid number of arguments.");
                }

                method = args[0];
                reader.readFile(args[1]);

                if(method == "TT")
                {
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
                }
                else if(method == "BC")
                {
                    HornClauseReader BCHReader = new HornClauseReader();
                    if (bCP.BackwardChainCheck(BCHReader.GetHornClause(reader.GetKB()), reader.GetQuery()))
                    {
                        Console.Write("YES: ");
                        foreach (string s in bCP.ProvenPremises)
                        {
                            Console.Write(s + " ");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Unrecognised method");
                }

                /*HornClauseReader HReader = new HornClauseReader();
                ForwardChainProver FChain = new ForwardChainProver();
                Console.WriteLine("Forward Chain: " + FChain.ForwardChainEntails(HReader.GetHornClause(reader.GetKB()), reader.GetQuery()));

                Console.WriteLine(CNFConvert.ConvertCNF((reader.GetKB())));
                Console.WriteLine("Resolution Prover: " + rP.Query(CNFConvert.ConvertCNF((reader.GetKB())), reader.GetQuery()));*/

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}