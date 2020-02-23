using System;
using System.IO;
using System.Linq;
using System.Text;
using caesarova_sifra_czu.classes;
using caesarova_sifra_czu.factory;
using caesarova_sifra_czu.repository;
using caesarova_sifra_czu.sqlite;

namespace caesarova_sifra_czu
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Vítejte v aplikaci pro šifrovanou komunikaci");
            RunConsole();
           
        }
        static void RunConsole()
        {
            bool run = true;
            while (run)
            {
                setChoiceMenuText();
                makeResponseForChoice();
                Console.Write("Přejete si pokračovat?´Y/N: ");
                run = DecisionMaker(Console.ReadLine());
            }
            Environment.Exit(0);
        }
        static void setChoiceMenuText()
        {
            Console.WriteLine("Vyberte z několika možností");
            Console.WriteLine("Uložit nový profil pro šifru: 1");
            Console.WriteLine("Načíst text pro dešifrování  ze souboru .txt: 2");
            Console.WriteLine("Vytvořit vlastní šifrovaný text: 3");
            Console.WriteLine("Vypsat záznamy z databáze: 4");
        }
        static void makeResponseForChoice()
        {
            Console.Write("Zadejte vstup: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine(Environment.NewLine +"Nyní vytváříte nový profil");

                    CipherSettings cipherSettings = new CipherSettings();
                    Console.Write("Zadejte název profilu: ");
                    cipherSettings.name = Console.ReadLine();
                    var cipherSettingsResult = DatabaseFactory.DatabaseCipherSettings.SelectByName(cipherSettings.name).Result;
                    if (cipherSettingsResult.Count() != 0)
                    {
                        Console.WriteLine("účet s tímto jménem byl již vytvořen");
                        RunConsole();
                    }

                    Console.Write("Zadejte frázi pro generování šifry: ");
                    cipherSettings.hashphrase = Console.ReadLine();
                    
                    CipherSettings.CreateNewSettings(cipherSettings);
                    Console.WriteLine("Úspěšně vytvořen záznam"+Environment.NewLine);
                    break;
                case "2":

                    Cipher cipher = new Cipher();
                    Console.Write("Zadejte název profilu pro dešifrování záznamu: ");
                    cipherSettings = new CipherSettings();
                    cipherSettings.name = Console.ReadLine();
                     cipherSettingsResult = DatabaseFactory.DatabaseCipherSettings.SelectByName(cipherSettings.name).Result;
                    if (cipherSettingsResult.Count() == 0)
                    {
                        Console.WriteLine("Nebyl nalezen zadaný profil, prosím nejprve ho vytvořte");
                        RunConsole();
                    }

                    Console.Write("Zadejte cestu k souboru: ");
                    cipher.path = Console.ReadLine();
                    CheckFilePath(cipher.path);
                    cipher.cryptedText = File.ReadAllText(cipher.path);
                    cipher.text = Cipher.TextEncryption(cipher.cryptedText, cipherSettingsResult[0].index);

                    Console.WriteLine("Dešifrovaný text: {0}", cipher.text);


                    break;
                case "3":
                    Console.Write("Zadejte název profilu pro šifrování záznamu: ");
                    cipherSettings = new CipherSettings();
                    cipherSettings.name = Console.ReadLine();
                    cipherSettingsResult = DatabaseFactory.DatabaseCipherSettings.SelectByName(cipherSettings.name).Result;
                    if (cipherSettingsResult.Count() == 0)
                    {
                        Console.WriteLine("Nebyl nalezen zadaný profil, prosím nejprve ho vytvořte");
                        RunConsole();
                    }
                    Console.Write("Zadejte šifrovaný text: ");
                    cipher = new Cipher();
                    cipher.text = Console.ReadLine();
                    Console.Write("Přejete si uložit text do souboru? Y/N : ");

                    bool decision = DecisionMaker(Console.ReadLine());
                    cipher.cryptedText = Cipher.TextEncryption(cipher.text, cipherSettingsResult[0].index, true);
                    if (decision)
                    {
                        Console.WriteLine("Soubor uložen zde: C:/Users/Public/Documents/šifra_" + DateTime.Now.ToString("dd-MM-yyyy_H-mm-ss")+".txt");
                        File.WriteAllText(@"C:\Users\Public\Documents\šifra_"+DateTime.Now.ToString("dd-MM-yyyy_H-mm-ss") +".txt", cipher.cryptedText);
                    }
                    else
                    {
                        Console.Write("Šifrovaný text: ");
                        Console.WriteLine(cipher.cryptedText);
                    }
                    break;
                case "4":
                    cipherSettingsResult = DatabaseFactory.DatabaseCipherSettings.Select().Result;
                    for (int i =0; i < cipherSettingsResult.Count; i++)
                    {
                        Console.WriteLine("Jméno: {0}, hashovací fráze: {1}, hash: {2}, index {3}", 
                            cipherSettingsResult[i].name, 
                            cipherSettingsResult[i].hashphrase, 
                            cipherSettingsResult[i].hash, 
                            cipherSettingsResult[i].index);
                    }
                    break;

                default:
                    Console.WriteLine("Nezvolil jste správně");
                    Console.WriteLine();
                    Main(null);
                    break;
            }
        }


        private static void CheckFilePath(string path)
        {
            if (!File.Exists(path))            
            {
                Console.WriteLine("Zadaná cesta k souboru neexistuje");
                Console.Write("Zadejte znovu cestu: ");
                CheckFilePath(Console.ReadLine());
            }        
        }

        private static bool DecisionMaker(string response)
        {
            response = response.ToLower();
            switch (response)
            {
                case "y":
                    return true;
                case "n":
                    return false;
                default:
                    Console.WriteLine("Zadejte pouze Y/N");
                    return DecisionMaker(Console.ReadLine());
                    break;
            }
        }

    }

}
