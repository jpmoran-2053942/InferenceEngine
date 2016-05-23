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

            reader.readFile("testmultibracket.txt");

            Console.WriteLine(CNFConvert.ConvertCNF((reader.GetKB())));
            Console.WriteLine(rP.Query(CNFConvert.ConvertCNF((reader.GetKB())), reader.GetQuery()));

            List<NodeOrStringInterface> stringListKB = CNF.ConvertToStringList(CNFConvert.ConvertCNF((reader.GetKB())));
            NodeOrStringInterface rootNodeKB = CNF.CreateBinaryTree(stringListKB);
            NodeOrStringInterface rootNodeQuery = new LeafNode(reader.GetQuery());
            Console.WriteLine("\nTruth table solution: " + tt.TruthTableEntails(rootNodeKB, rootNodeQuery));


            /*List<NodeOrStringInterface> convertedStringList = CNF.ConvertToStringList(CNFConvert.ConvertCNF(reader.GetKB()));
            NodeOrStringInterface test = CNF.CreateBinaryTree(convertedStringList);*/

            Console.ReadLine();
        }
    }
}