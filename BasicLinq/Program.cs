using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicLinq
{
    class Program
    {
        /// <summary>
        ///  Nějaké pomocné struktury, které později použijeme, všimněte si že C# je skutečně CASeSenSiTiVE.
        /// </summary>
        
        struct Auto
        {
            public string Značka;
            public string Barva;
            public int ID_Kategorie;

            public Auto(string značka, string barva, int id_kategorie)
            {
                Značka = značka; Barva = barva;
                ID_Kategorie = id_kategorie;
            }
        }

        struct KategorieAuta
        {
            public string Karoserie;
            public int PočetDveří;
            public int ID;

            public KategorieAuta(string karoserie, int početdveří, int id)
            {
                Karoserie = karoserie; PočetDveří = početdveří;
                ID = id;
            }

        }

        static void Main(string[] args)
        {
            Random Gen = new Random();
            
            // Vygeneruj 2 sady náhodných dat
            List<int> SadaA = GenerujData(Gen,20);
            List<int> SadaB = GenerujData(Gen,35);

            int random = Gen.Next(50);

            Console.WriteLine("Vybírám součty těch čísel z obou sad, která jsou menší než {0}", random);

            var MensiNez = from a in SadaA
                           from b in SadaB
                           where (a + b) < random
                           select a + b;

            foreach (int soucet in MensiNez)
            {
                Console.WriteLine("{0}", soucet);
            }

            // LinQ odkládá vyhodnocení výrazu až tehdy když je skutečně potřeba:

            for (int i = 0; i < 30; i++) // Změníme náhodně data
            {
                if (Gen.Next() % 3 == 0) continue;
                
                if (Gen.Next() % 2 == 0)
                    SadaA[Gen.Next(0, SadaA.Count - 1)] = Gen.Next(0,100);
                else SadaB[Gen.Next(0, SadaB.Count - 1)] = Gen.Next(0,100);
            }

            Console.WriteLine("Znovu s jinými čísly ale stejným query:");
            // Součty tady budou jiné, přestože používáme stejné query "MensiNez"
            foreach (int soucet in MensiNez)
            {
                Console.WriteLine("{0}", soucet);
            }

            Console.WriteLine("Vybírám stejná čísla v obou sadách");

            var ShodnaCisla = from a in SadaA
                              from b in SadaB
                              where a == b
                              select a;

            // Vypiš všechny shodné páry čísel a přeskoč první pár
            foreach (var shodnyPar in ShodnaCisla.Skip(1))
            {
                Console.WriteLine("{0}", shodnyPar);
            }

            // Vytvoříme si nějaké kategorie a auto které do těchto kategorií spadají
            KategorieAuta[] Kategorie = new KategorieAuta[5];
            Kategorie[0] = new KategorieAuta("Sedan", 3, 1);
            Kategorie[1] = new KategorieAuta("Sedan", 5, 2);
            Kategorie[2] = new KategorieAuta("Combi", 5, 3);
            Kategorie[3] = new KategorieAuta("SUV", 5, 4);
            Kategorie[4] = new KategorieAuta("Zemědělský stroj", 1, 5);

            // V garáži mám:
            Auto[] Auta = new Auto[11];
            Auta[0] = new Auto("Volkswagen", "modrá", 3);
            Auta[1] = new Auto("Audi", "bílá", 1);
            Auta[2] = new Auto("Audi", "modrá", 2);
            Auta[3] = new Auto("Volkswagen", "černá", 4);
            Auta[4] = new Auto("BMW", "černá", 1);
            Auta[5] = new Auto("BMW", "bílá", 2);
            Auta[6] = new Auto("Porsche", "stříbrná", 4);
            Auta[7] = new Auto("Volkswagen", "stříbrná", 4);
            Auta[8] = new Auto("Zetor", "červená", 5);
            Auta[9] = new Auto("Ferrari", "červená", 1);
            Auta[10] = new Auto("Škoda", "modrá", 3);

            // Vyber všechna červená vozidla
            var Červené = from auto in Auta
                          join kat in Kategorie on auto.ID_Kategorie equals kat.ID
                          where auto.Barva == "červená"
                          select new { auto.Značka, kat.Karoserie };

            Console.WriteLine("Všechna červená vozidla:");
            foreach (var vozidlo in Červené)
            {
                Console.WriteLine("{0} {1}", vozidlo.Značka, vozidlo.Karoserie);
            }


            // Vyber značku, barvu a počet dveří všech dostupných sedanů a seřaď abecedně vzestupně podle značky
            var Sedany = from auto in Auta
                         join kat in Kategorie on auto.ID_Kategorie equals kat.ID
                         where kat.Karoserie == "Sedan"
                         orderby auto.Značka ascending
                         select new { auto.Značka, auto.Barva, kat.PočetDveří };

            Console.WriteLine("Všechny dostupné sedany:");
            foreach (var sedan in Sedany)
            {
                Console.WriteLine("{0} {1} {2}", sedan.Značka, sedan.Barva, sedan.PočetDveří);
            }


            int CelkemDveříVGaráži = (from auto in Auta
                                      join kat in Kategorie on auto.ID_Kategorie equals kat.ID
                                      select kat.PočetDveří).Sum(); 
            // Toto už není odložené query, neboť jsem vynutil spočtení výsledku, takže kdybych později přišel o dveře tak se číslo už dál nezmění.

            Console.WriteLine("Všechna moje auta mají celekem {0} dveří", CelkemDveříVGaráži);

            Console.Read();
        }

        /// <summary>
        /// Metoda pro generování pseudonáhodných čísel od 0 do 100
        /// </summary>
        /// <param name="Generator">Generátor pseudonáhodných čísel</param>
        /// <param name="Pocet">Počet čísel k vygenerování</param>
        /// <returns>Pole pseudonáhodných čísel</returns>
        static List<int> GenerujData(Random Generator,int Pocet)
        {

            List<int> Ret = new List<int>();
            for (int i = 0; i < Pocet; i++)
                Ret.Add(Generator.Next(0,100));

            return Ret;
        }
    }
}
