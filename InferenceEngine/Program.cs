using System;

namespace InferenceEngine
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            FileReader reader = new FileReader();
            ConjunctiveNormalForm CNF = new ConjunctiveNormalForm();

            reader.readFile("test2.txt");
            CNF.CreateBinaryTree(reader.GetKBEntry(0));

            Console.ReadLine();
        }
    }
}
