using System;
using System.Collections.Generic;

//http://stackoverflow.com/questions/17568067/how-to-parse-a-boolean-expression-and-load-it-into-a-class

namespace InferenceEngine
{
    public class FileReader : TranslateSyntax
    {
        private string[] _knowledgeBase;
        private string _query = "";

        public FileReader()
        {
        }

        //Set to private after debugging
        public bool readFile(string file)
        {
            string fileContent;
            string[] tempKB;

            try
            {

                fileContent = System.IO.File.ReadAllText(@file);
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("File not found.");

                return false;
            }

            //Deliminate the content and clean up the resulting array.
            string[] fileArray = fileContent.Split('\n');

            for (int i = 0; i < fileArray.Length; i++)
            {
                fileArray[i] = fileArray[i].Trim();
            }

            //Determine whether the file format is correct.
            if (!(fileArray[0].Equals("TELL")) || !(fileArray[2].Equals("ASK")) || (fileArray.Length != 4))
            {
                Console.WriteLine("File format incorrect.");

                return false;
            }

            //Split the knowledge base into a list of strings
            tempKB = fileArray[1].Split(';');
            if (tempKB[tempKB.Length - 1].Equals(""))
                _knowledgeBase = new string[tempKB.Length - 1];
            else
                _knowledgeBase = new string[tempKB.Length];
            for (int i = 0; i < tempKB.Length; i++)
            {
                //https://msdn.microsoft.com/en-us/library/tabh47cf(v=vs.110).aspx
                //https://msdn.microsoft.com/en-us/library/57a79xd0(v=vs.110).aspx
                tempKB[i] = string.Join("", tempKB[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                if (tempKB[i] != "")
                {
                    _knowledgeBase[i] = tempKB[i];
                }
            }
            
            _knowledgeBase = TranslateKB(_knowledgeBase);

            _query = Translate(fileArray[3]);

            //Now have _knowledgeBase as an array of strings with whitespace free strings and the query as a string in _query

            return true;
        }

        public HornClause[] GetHornClauseSet(string file)
        {
            return null;
        }

        public string[] GetPropositionalLogicSet(string file)
        {
            return null;
        }

        public string GetCNF(string file)
        {
            return null;
        }
        
        public string GetQuery()
        {
            return _query;
        }

        public string[] GetKB()
        {
            return _knowledgeBase;
        }

        public string GetKBEntry(int num)
        {
            return _knowledgeBase[num];
        }


        /// <summary>
        /// Attempts to read the specified file. Stores the puzzle as an int array. The
        /// file format is:
        /// First Line: [mxn]; 
        /// Second Line: *initial configuration; 
        /// Third Line: *end state; 
        /// where m is the column and n is the row
        /// Use "GetPuzzle" to return the puzzle.
        /// </summary>
        /// <returns><c>true</c>, if the file was read successfully, <c>false</c> if the
        /// file does not exist or there is an issue with the file format.</returns>
        /// <param name="file">File.</param>


    }
}

