using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace DatabazeKnih.V2; 
class Program
{
    public struct TKniha            // vytvarime strukturu a dekrarujeme promenne, ktere budeme pouzivat
    { 
        public string Nazev_knihy;
        public string Zanr_knihy;
        public string Jmeno_autora;
        public int datum_knihy;
        public int Cena_knihy;
    }

    static int Nacti(ref TKniha[] knihy)                           //vytvarime funkci, pro nacitani ze souboru csv 
    {
        int pocet = 0;                                                
        using (StreamReader s = new StreamReader(@"te.csv"))       //pouzijeme streamreader, aby se cetla informace a ukazujeme adresu souboru

        {
            string radek;                                        
            string[] pole = new string[5];                       //vytvarime pole, kde budou jednotlive elementy, ktere budou oddeleny carkami s teckami    
            while (s.EndOfStream != true)                  
            { 
                radek = s.ReadLine();                               
                pole = radek.Split(';');
                knihy[pocet].Nazev_knihy = pole[0];
                knihy[pocet].Zanr_knihy = pole[1];
                knihy[pocet].Jmeno_autora = pole[2];
                knihy[pocet].datum_knihy = int.Parse(pole[3]);
                knihy[pocet].Cena_knihy = int.Parse(pole[4]);
                pocet++;                                            // pak k poctu knih pridavame +1
            }
        }
        return pocet;     //vracime hodnotu funkce. 
    }

   
    static double Prumernacena(TKniha[] knihy, int pocet)         
    {
        double total = 0;                                          //deklarujeme promennou total pomoci double, aby vysledek nebyl jen cely
        for (int i = 0; i < pocet; i++)                            
        {
            total += knihy[i].Cena_knihy;                   // pomoci cyklu for pridavame k total cenu kazde knihy a pak vydelime to poctem knih
        }
        return total / pocet;
    }


    static void Nejstarsi(TKniha[] knihy, int pocet)
    {
        int index = 0;                                                  //pridavame pomocnou promennou index, abysme mohli porovnavat knihy
        for (int i = 1; i < pocet; i++)
        {
            if (knihy[i].datum_knihy < knihy[index].datum_knihy)                  
            {
                index = i;
            }

        }
        Console.WriteLine("Nejstarsi kniha je: {0}, rok: {1}: ", knihy[index].Nazev_knihy, knihy[index].datum_knihy);
    }
    static void Nejnovejsi(TKniha[] knihy, int pocet)
    {
        int index = 0;
        for (int i = 1; i < pocet; i++)
        {
            if (knihy[i].datum_knihy > knihy[index].datum_knihy)
            {
                index = i;
            }

        }
        Console.WriteLine("Nejnovejsi  kniha je: {0}, rok: {1}: ", knihy[index].Nazev_knihy, knihy[index].datum_knihy);
    }


    static void Ulozit(TKniha[] knihy, int pocet)
    {
        using (StreamWriter s = new StreamWriter(@"te.csv"))       //pomovi streamwriter davame pristup k souboru csv
        {
            for (int i = 0; i < pocet; i++)
            {
                s.WriteLine("{0}; {1}; {2}; {3}; {4}",
                    knihy[i].Nazev_knihy, knihy[i].Zanr_knihy, knihy[i].Jmeno_autora, knihy[i].datum_knihy, knihy[i].Cena_knihy); // ulozeni vseho do souboru
            }

            s.Flush();   // pomoci flush vyprazdnime buffer
        }
    }

    static void UlozitXML(TKniha[] knihy, int pocet) 
    {
        XmlWriterSettings set = new XmlWriterSettings();
        set.Indent = true;                                  // nastaveni
        using (XmlWriter xw = XmlWriter.Create(@"t.xml", set)) //ulozeni do xml
        {
            xw.WriteStartDocument(); // zacatek dokumentu
            xw.WriteStartElement("Knihy");  //zacatek elementu 
            for (int i = 0; i < pocet; i++)
            {
                xw.WriteStartElement("Knihy");
                xw.WriteAttributeString("Nazev", knihy[i].Nazev_knihy);            //pridavame atributy
                xw.WriteAttributeString("Zanr", knihy[i].Zanr_knihy);
                xw.WriteAttributeString("Jmeno", knihy[i].Jmeno_autora);
                xw.WriteAttributeString("Datum", knihy[i].datum_knihy.ToString());
                xw.WriteAttributeString("Cena", knihy[i].Cena_knihy.ToString());
                xw.WriteEndElement();  //konec elementu
            }
            xw.WriteEndElement();
            xw.WriteEndDocument();
            xw.Flush();

        }
    }


    static TKniha[] Smazat(TKniha[] knihy, int pocet, int i)
    {
        for (int y = i; y < pocet - 1; y++)
        {
            knihy[y] = knihy[y + 1];
        }
        return knihy;
    }

    static TKniha Pridat()
    {
        
        TKniha knihy;     
        Console.ForegroundColor = ConsoleColor.Gray;     //barvime console
        Console.Write("Zadejte nazev: ");          
        knihy.Nazev_knihy = Console.ReadLine();                    //zadavame informace o knize
        Console.Write("Zadejte zanr: ");
        knihy.Zanr_knihy = Console.ReadLine();
        Console.Write("Zadejte Jmeno a Příjmení Autora: ");
        knihy.Jmeno_autora = Console.ReadLine();
        Console.Write("Zadejte datum knihy: ");
        knihy.datum_knihy = int.Parse(Console.ReadLine()); 
        Console.Write("Zadejte cenu knihy kč: ");
        knihy.Cena_knihy = int.Parse(Console.ReadLine());
        Console.WriteLine("------------------");
        Console.ForegroundColor = ConsoleColor.Green;
        
    
    
        return knihy;  
    }

    static void vypisKnihy(TKniha[] knihy, int i)
    {
        Console.WriteLine("Název knihy: {0};\tŽánr: {1};\tJméno autora: {2};\tDatum: {3};\tCena kč: {4}",        
            knihy[i].Nazev_knihy, knihy[i].Zanr_knihy, knihy[i].Jmeno_autora, knihy[i].datum_knihy, knihy[i].Cena_knihy);
    }
    static void Hlavicka(string text = "")   //universalni funkce pro hlavicku menu, kterou muzeme vsude zadavat 
    {
        Console.Write("Databaze Knih v 1.0");
        if (text != "") 
        {
            Console.WriteLine(" - {0}", text);
            for (int i = 0; i < text.Length + 20; i++) ;    
            Console.Write("-");
        }
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        TKniha[] knihy = new TKniha[30]; //vytvoreni pole Tkniha
        int pocet = 0, i;    
        char volba = 'k';
        do
        {

            Console.Clear();             
            Console.ForegroundColor = ConsoleColor.Yellow; //menu console
            Hlavicka("Menu");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("...............");
            Console.WriteLine("Nacti knihy CSV[n]");            
            Console.WriteLine("Pridat knihu [p]");
            Console.WriteLine("Smazat knihu [s]");
            Console.WriteLine("Smazat vse [f]");
            Console.WriteLine("Vypsat knihu [v]");
            Console.WriteLine("Vypsat knihy [w]");
            Console.WriteLine("Vypsat prumernou cenu [d]");
            Console.WriteLine("Vypsat nejstarší knihu [r]");           
            Console.WriteLine("Vypsat nejnovější knihu [j]");
            Console.WriteLine("Ulozit do souboru CSV[u]");
            Console.WriteLine("Ulozit do souboru XML[x]");
            Console.WriteLine("Konec [k]");
            Console.WriteLine(".............");
            Console.Write("Zadejte volbu: ");
            volba = char.ToLower(Console.ReadKey().KeyChar);  //cteme znak
            Console.Clear();
            switch (volba)     //pomoci selekci switch volame funkce a  vybirame, co program ma delat
            {

                case 'n':
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Hlavicka("Nacitani CSV");
                    pocet = Nacti(ref knihy);
                    Console.WriteLine("-------------------");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Kniha byla nactena");
                    Console.ReadKey();
                    break;

                case 'x':

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Hlavicka("Ulozeni XML");
                    UlozitXML(knihy, pocet);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Ulozeno XML");
                    Console.ReadKey();
                    break;

               
                case 'd':
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Hlavicka("Prumerna cena knihy");
                    Console.WriteLine("Prumerna cena knihy je {0} kč", Prumernacena(knihy,pocet));
                    


                    break;
                case 'r':
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Hlavicka("Vypsani nejstarsi knihy");
                    Nejstarsi(knihy, pocet);
                    break;
      
                case 'j':
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Hlavicka("Vypsani nejnovejsi knihy");
                    Nejnovejsi(knihy, pocet);

                    break;


                case 'p':
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Hlavicka("Pridani Knih");
                    knihy[pocet] = Pridat();
                    pocet++;
                    break;

                case 'u':
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Hlavicka("Ulozeni CSV");
                    Ulozit(knihy, pocet);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Ulozeno CSV");
                    Console.ReadKey();
                    break;

                case 's':
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Hlavicka("Smazani Knih");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("Zadejte index knihy: ");
                    i = int.Parse(Console.ReadLine());
                    knihy = Smazat(knihy, pocet, i);
                    pocet = pocet - 1;
                    Console.WriteLine("-------------------");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Kniha byla smazana");
                    break;

                case 'v':
                    Console.Clear();
                    Hlavicka("Vypsani knihy");
                    Console.WriteLine("------------------------");
                    Console.Write("Zadejte index: ");
                    i = int.Parse(Console.ReadLine());
                    vypisKnihy(knihy, i);
                    Console.ReadKey();
                    break;

                case 'w':
                    Hlavicka("Vypsani knih");
                    Console.WriteLine("------------------------"); //opakujeme  funkci vypis knihy, aby se vypsaly vsechny knihy
                    for (i = 0; i < pocet; i++)
                    {
                        vypisKnihy(knihy, i);
                    }
                    Console.WriteLine("------------------------");
                    Console.WriteLine("Pocet knih celkem je: {0}", pocet);

                    break;

                case 'f':
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Hlavicka("Smazani vsech Knih");
                    for (i = 0; i <= pocet; i++)
                    {
                        Smazat(knihy, pocet, i);    //to stejne se smazanim
                        pocet--;
                    }
                    Console.WriteLine("-------------------");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Knihy byly smazany");
                    break;


                case 'k':
                    Console.WriteLine("Konec Programu");
                    break;

                default:
                    Console.WriteLine("Chybna volba");
                    break;

            }
            Console.ReadKey();

        }
        while (volba != 'k'); //vykonava se pokud neni zadana K

    }
}


