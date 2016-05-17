using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    //Need to translate:
    //NOT: ~ = -
    //AND: & = &
    //OR: \/ = +
    //IMPLY: => = >
    //BICONDITION: <=> = =

    public class TranslateSyntax
    {
        protected string[] TranslateKB(string[] KB)
        {
            string[] TranslatedKB = new string[KB.Length];
            KB.CopyTo(TranslatedKB, 0);
            int i = 0;

            foreach (string s in KB)
            {
                TranslatedKB[i] = Translate(s);
                i++;
            }

            return TranslatedKB;
        }

        protected string Translate(string sentence)
        {
            char[] translated = new char[sentence.Length];
            sentence.CopyTo(0, translated, 0, sentence.Length);

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
                        translated[i] = '+';
                        translated = Shift(translated, i, 1);
                        break;
                    case '=':
                        //=> to >
                        if (translated[i + 1] == '>')
                        {
                            translated[i] = '>';
                            translated = Shift(translated, i, 1);
                        }
                        break;
                    case '<':
                        //<=> to =
                        translated[i] = '=';
                        translated = Shift(translated, i, 2);
                        break;
                    default:
                        //Do nothing
                        break;
                }
            }

            string test = new string(translated);

            return test;
        }

        private char[] Shift(char[] original, int index, int count)
        {
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
