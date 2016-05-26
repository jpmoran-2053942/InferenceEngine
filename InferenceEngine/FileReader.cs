using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>

namespace InferenceEngine
{
    public class FileReader : TranslateSyntax
    {
        private string[] _knowledgeBase;
        private string _query = "";

        public FileReader()
        {
        }

        /// <summary>
        /// Reads the indicated file. File must have TELL as the first line, ASK as the third line, the knowledge base
        /// as the second line (delimiated by semicolons), and the query as the final line.
        /// </summary>
        /// <param name="file">The name of the file to read.</param>
        /// <returns>Returns false if the file cannot be read or the format is incorrect, true otherwise.</returns>
        public bool readFile(string file)
        {
            string fileContent;
            string[] tempKB;

            //Attempt to read the file.
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
                tempKB[i] = string.Join("", tempKB[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                if (tempKB[i] != "")
                {
                    _knowledgeBase[i] = tempKB[i];
                }
            }
            
            //Converts the connectives to the expected format for this program.
            _knowledgeBase = TranslateKB(_knowledgeBase);
            _query = Translate(fileArray[3]);

            //Now have _knowledgeBase as an array of strings with whitespace free strings and the query as a string in _query
            return true;
        }
        
        /// <summary>
        /// Returns the query. Must run ReadFile first.
        /// </summary>
        /// <returns>The query as a string. Null if ReadFile hasn't been run.</returns>
        public string GetQuery()
        {
            return _query;
        }

        /// <summary>
        /// Returns the knowledge base. Must run ReadFile first.
        /// </summary>
        /// <returns>The knowledge base as an array of string. Null if ReadFile hasn't been run.</returns>
        public string[] GetKB()
        {
            return _knowledgeBase;
        }

        /// <summary>
        /// Returns a particular knowledge base entry. Must run ReadFile first.
        /// </summary>
        /// <param name="num">Index of the entry.</param>
        /// <returns>The query as a string. Null if ReadFile hasn't been run or if the index is not in the array.</returns>
        public string GetKBEntry(int num)
        {
            if (num < _knowledgeBase.Length)
                return _knowledgeBase[num];

            return null;
        }

    }
}

