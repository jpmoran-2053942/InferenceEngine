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

            reader.readFile("test2.txt");
            List<NodeOrStringInterface> convertedStringList = CNF.ConvertToStringList(reader.GetKBEntry(0));
            CNF.CreateBinaryTree(convertedStringList);

            Console.ReadLine();
        }
    }
}