using System;
using System.Collections.Generic;

namespace InferenceEngine
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            FileReader reader = new FileReader();
            ConjunctiveNormalForm CNF = new ConjunctiveNormalForm();
            ConvertToCNF CNFConvert = new ConvertToCNF();

            reader.readFile("testcnfconvert.txt");
            Console.WriteLine(CNFConvert.ConvertCNF(reader.GetKB()));
            List<NodeOrStringInterface> convertedStringList = CNF.ConvertToStringList(reader.GetKBEntry(0));
            NodeOrStringInterface test = CNF.CreateBinaryTree(convertedStringList);

            Console.ReadLine();
        }
    }
}