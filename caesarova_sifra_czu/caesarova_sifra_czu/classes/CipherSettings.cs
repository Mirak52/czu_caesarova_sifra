using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using caesarova_sifra_czu.models;
using caesarova_sifra_czu.repository;
using caesarova_sifra_czu.factory;

namespace caesarova_sifra_czu.classes
{
    public class CipherSettings
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string name { get; set; }
        public string hashphrase { get; set; }
        public string hash { get; set; }
        public int index { get; set; }

           //vytvoření nového záznamu v databázi
        public static void CreateNewSettings(CipherSettings cipherSettings)
        {
            cipherSettings.hash = CreateMd5Hash(cipherSettings.hashphrase);
            cipherSettings.index = CreateIndexByHash(cipherSettings.hash);
            DatabaseFactory.DatabaseCipherSettings.SaveItemAsync(cipherSettings);            
        }
        //vytvoření indexu pro šifrování z hashe
        //Ziskání čísel z hashe

        private static int CreateIndexByHash(string hash)
        {
            int index = 0;
            for (int i = 0; i < hash.Length; i++)
            {
                if (Char.IsDigit(hash[i]))
                {
                    if (Numbers.IsOdd(i))
                    {
                        index += hash[i];
                    }
                    else
                    {
                        index -= hash[i];
                    }
                }
            }
            if(index == 0)
            {
                //V případě, že index je 0 zkusí to znovu
                CreateIndexByHash(CreateMd5Hash(hash));
            }
            //kontrola posunití indexu, aby byl v určitém rozsahu
            if ((index > 0 && index > 26) || (index < 0 && index < -26))
            {
                int division = index / 26;
                if(division*26 == index)
                {
                    return CreateIndexByHash(CreateMd5Hash(hash));
                }
                if(division < 0)
                {
                    index = index - division * 26;
                }
                else
                {
                    index = index - division * 26;
                }
            }

            return index;
        }

        private static string CreateMd5Hash(string hashphrase)
        {
            //použítá funkce na vytvoření MD5 hashe, zdroj microsoft 
            using(MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(hashphrase));
                    // Create a new Stringbuilder to collect the bytes
                    // and create a string.
                    StringBuilder sBuilder = new StringBuilder();

                    // Loop through each byte of the hashed data 
                    // and format each one as a hexadecimal string.
                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }
                    return sBuilder.ToString();
                }
            }
    }
}
