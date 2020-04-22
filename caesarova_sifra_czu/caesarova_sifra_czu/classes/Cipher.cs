using System;
using System.Collections.Generic;
using System.Text;

namespace caesarova_sifra_czu.classes
{
    public class Cipher
    {
        public string text  { get; set; }
        public string cryptedText  { get; set; }
        public string path { get; set; }

        //šifrování/dešifrování textu
        public static string TextEncryption(string text, int index, bool encryption = false)
        {
            char[] buffer = text.ToCharArray();
            //zda se jedná o šifrování
            if (encryption)
            {
                buffer = text.ToLower().ToCharArray();
            }
            for (int i = 0; i < buffer.Length; i++)
            {

                char letter = buffer[i];

                letter = ChechCzechLetter(letter);
                //posunutí znaku o index
                if (encryption)
                {
                    letter = (char)(letter + index);
                  
                }
                else
                {
                    letter = (char)(letter - index);
                   
                }
                
                buffer[i] = letter;
            }
            return new string(buffer);
        }
        //Zbavení českých znaků
        private static char ChechCzechLetter(char letter)
        {
            switch (letter)
            {
                case 'ě':
                    return 'e';
                case 'š':
                    return 's';
                case 'č':
                    return 'c';
                case 'ř':
                    return 'r';
                case 'ž':
                    return 'z';
                case 'ý':
                    return 'y';
                case 'á':
                    return 'a';
                case 'í':
                    return 'i';
                case 'é':
                    return 'e';
                case 'ť':
                    return 't';
                default:
                    return letter;
            }
        }
    }
}
