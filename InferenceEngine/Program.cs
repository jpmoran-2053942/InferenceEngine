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

            reader.readFile("testcomplex.txt");
            List<NodeOrStringInterface> convertedStringList = CNF.ConvertToStringList(reader.GetKBEntry(0));
            NodeOrStringInterface test = CNF.CreateBinaryTree(convertedStringList);

            Console.ReadLine();
        }
    }
}