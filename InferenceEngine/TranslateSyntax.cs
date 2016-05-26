using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Provides functions to translate the syntax of sentences to the expected form for the program.
/// Specifically, converts the following:
///     NOT: ~ to -
///     AND: & to &
///     OR: \/ to +
///     IMPLY: => to >
///     BICONDITION: <=> to =
/// This is because it is easier to work with single character connectives than multiple characters.
/// </summary>

namespace InferenceEngine
{
    public class TranslateSyntax
    {
        /// <summary>
        /// Converts the syntax of a knowledge base to the expected form for the program.
        /// </summary>
        /// <param name="KB">The knowledge base as an array of strings.</param>
        /// <returns>The new knowledge base in expected form.</returns>
        protected string[] TranslateKB(string[] KB)
        {
            string[] TranslatedKB = new string[KB.Length];

            //Make a copy of the knowledge base.
            KB.CopyTo(TranslatedKB, 0);
            int i = 0;

            //Run the Translate function on each sentence in the knowledge base.
            foreach (string s in KB)
            {
                TranslatedKB[i] = Translate(s);
                i++;
            }

            //Return the new knowledge base.
            return TranslatedKB;
        }

        /// <summary>
        /// Translates a single logic sentence to the expected form for the program.
        /// </summary>
        /// <param name="sentence">A logic sentence as a string.</param>
        /// <returns>The new sentence.</returns>
        protected string Translate(string sentence)
        {
            //Copy the string into an array.
            char[] translated = new char[sentence.Length];
            sentence.CopyTo(0, translated, 0, sentence.Length);

            //Look for each connective and convert it to the expected form.
            for (int i = 0; i < translated.Length; i++)
            {
                switch (translated[i])
                {
                    case '~':
                        //~ to -
                        translated[i] = '-';
                        break;
                    case '&':
                        //Do nothing
                        break;
                    case '\\':
                        //\/ to +
                        //Since this connective is two characters, must also shift the characters that follow
                        //back one place.
                        translated[i] = '+';
                        translated = Shift(translated, i, 1);
                        break;
                    case '=':
                        //=> to >
                        //Also two characters.
                        if (translated[i + 1] == '>')
                        {
                            translated[i] = '>';
                            translated = Shift(translated, i, 1);
                        }
                        break;
                    case '<':
                        //<=> to =
                        //Three characters long, so must shift following characters two places back
                        translated[i] = '=';
                        translated = Shift(translated, i, 2);
                        break;
                    default:
                        //Do nothing
                        break;
                }
            }

            //Write the converted list to a string and return it.
            string temp = new string(translated);
            return temp;
        }

        /// <summary>
        /// Move characters following an index back count places. Removes the
        /// blank characters created. Replaces the characters shifted over.
        /// </summary>
        /// <param name="original">The character array to modify.</param>
        /// <param name="index">The index to modify from.</param>
        /// <param name="count">The number of places to shift back.</param>
        /// <returns>The new char array with shifted characters.</returns>
        private char[] Shift(char[] original, int index, int count)
        {
            //Create a new array of the shifted length. Not doing this results in blank spaces at the
            //end of the array.
            char[] newArray = new char[original.Length - count];

            for(int i = 0; i < newArray.Length; i++)
            {
                //For the indexes we want to shift...
                if (i > index)
                {
                    if (i + count < original.Length)
                        newArray[i] = original[i + count];
                    else
                        newArray[i] = ' ';
                }
                //For indexes before the shift point...
                else
                {
                    newArray[i] = original[i];
                }
            }
            return newArray;
        }

    }
}
