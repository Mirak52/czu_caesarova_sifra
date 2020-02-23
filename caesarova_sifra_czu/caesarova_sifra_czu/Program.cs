using System;

namespace caesarova_sifra_czu
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Vítejte v aplikaci pro šifrovanou komunikaci");
            bool run = true;
            while (run)
            {
                setChoiceMenu();



            }
        }
        static void setChoiceMenu()
        {
            Console.WriteLine("Vyberte z několika možností");
            Console.WriteLine("Uložit nový profil pro šifru: 1");
            Console.WriteLine("Načíst text ze souboru .txt: 2");
            Console.WriteLine("Vytvořit vlastní šifru z konzole: 3");
            Console.WriteLine("Vytvořit vlastní šifru ze souboru: 4");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                default:
                    Console.WriteLine("Nezvolil jste správně");
                    Console.WriteLine();
                    Main(null);
                    break;
            }
        }
    }
}
