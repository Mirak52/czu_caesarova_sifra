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
        //první spuštěná funkce
        static void Main(string[] args)
        {
            //pozdrav...
            Console.WriteLine("Vítejte v aplikaci pro šifrovanou komunikaci");
            //zavolání funkce RunConsole bez návratové hodnoty
            RunConsole();

        }
        //základ aplikace, umožnuje chod meníčka, 
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
        //vypíše meníčko pro uživatele
        //statická void metoda
        static void setChoiceMenuText()
        {
            Console.WriteLine("Vyberte z několika možností");
            Console.WriteLine("Uložit nový profil pro šifru: 1");
            Console.WriteLine("Načíst text pro dešifrování ze souboru .txt: 2");
            Console.WriteLine("Vytvořit vlastní šifrovaný text: 3");
            Console.WriteLine("Vypsat záznamy z databáze: 4");
        }

        //Reakce na vstup uživatele 
        static void makeResponseForChoice()
        {
            Console.Write("Zadejte vstup: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                //Reakce na vstup uživatele, vytváření nového profilu
                case "1":
                    Console.WriteLine(Environment.NewLine +"Nyní vytváříte nový profil");
                    //vytvoření nového objektu
                    CipherSettings cipherSettings = new CipherSettings();
                    Console.Write("Zadejte název profilu: ");
                    cipherSettings.name = Console.ReadLine();
                    //SQL select na databázi zdali nebyl vytvořen učet se stejným jménom
                    var cipherSettingsResult = DatabaseFactory.DatabaseCipherSettings.SelectByName(cipherSettings.name).Result;
                    //počet záznamků, pokud není nula spustí konzoli znovu
                    if (cipherSettingsResult.Count() != 0)
                    {
                        Console.WriteLine("účet s tímto jménem byl již vytvořen");
                        RunConsole();
                    }

                    Console.Write("Zadejte frázi pro generování šifry: ");
                    cipherSettings.hashphrase = Console.ReadLine();
                    //funkce na uložení nového nastavení
                    CipherSettings.CreateNewSettings(cipherSettings);
                    Console.WriteLine("Úspěšně vytvořen záznam"+Environment.NewLine);
                    break;
                    //Dešifrování záznamu
                case "2":

                    Cipher cipher = new Cipher();
                    Console.Write("Zadejte název profilu pro dešifrování záznamu: ");
                    cipherSettings = new CipherSettings();
                    cipherSettings.name = Console.ReadLine();
                    //sql select, musí nalézt profil
                     cipherSettingsResult = DatabaseFactory.DatabaseCipherSettings.SelectByName(cipherSettings.name).Result;
                    if (cipherSettingsResult.Count() == 0)
                    {
                        Console.WriteLine("Nebyl nalezen zadaný profil, prosím nejprve ho vytvořte");
                        RunConsole();
                    }

                    Console.Write("Zadejte cestu k souboru: ");
                    cipher.path = Console.ReadLine();
                    //kontrola zadaného vstupu, zdali soubor existuje
                    CheckFilePath(cipher.path);
                    cipher.cryptedText = File.ReadAllText(cipher.path);
                    //dešifrování textu
                    cipher.text = Cipher.TextEncryption(cipher.cryptedText, cipherSettingsResult[0].index);
                    Console.WriteLine("Dešifrovaný text: {0}", cipher.text);
                    break;
                    //šifrování zadaného textu
                case "3":
                    Console.Write("Zadejte název profilu pro šifrování záznamu: ");
                    cipherSettings = new CipherSettings();
                    cipherSettings.name = Console.ReadLine();
                    //sql select profilu pro dešifrování
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
                    //šifrování textu
                    cipher.cryptedText = Cipher.TextEncryption(cipher.text, cipherSettingsResult[0].index, true);
                    if (decision)
                    {
                        //uloží soubor s aktualním časem do hlavní složky
                        Console.WriteLine("Soubor uložen zde:" + System.AppDomain.CurrentDomain.BaseDirectory  +DateTime.Now.ToString("dd-MM-yyyy_H-mm-ss") +".txt");
                        File.WriteAllText(@System.AppDomain.CurrentDomain.BaseDirectory  +DateTime.Now.ToString("dd-MM-yyyy_H-mm-ss") +".txt", cipher.cryptedText);
                    }
                    else
                    {
                        Console.Write("Šifrovaný text: ");
                        Console.WriteLine(cipher.cryptedText);
                    }
                    break;
                case "4":
                    //vypsání šifrovacích nastavení
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
            //kontrola existence souboru
            if (!File.Exists(path))            
            {
                Console.WriteLine("Zadaná cesta k souboru neexistuje");
                Console.Write("Zadejte znovu cestu: ");
                CheckFilePath(Console.ReadLine());
            }        
        }
        //recursivní funkce pro získání rozhodnutí od uživatele
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
            }   
        }

    }

}
