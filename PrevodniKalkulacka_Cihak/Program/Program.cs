using System.Linq.Expressions;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;

string vysledekFinal = "";
string cisloStart = "";
string doSoustavy = "";
string zeSoustavy = "";

//Prázdnej texťák -> nabídka přidání jména-> ulozi se do textaku a bude pokazde vitat jmenem.
//1. nabídka, v jaké číselné soustavě chceme číslo zadat (Z jaké soustavy bude program převádět)(A-D).
//2. nabídka, do jaké číselné soustavy chceme číslo převést (Do jaké soustavy bude program převádět)(A-D).
//3. nabídka, program převede číslo ze zvolené soustay do zvolené soustavy a nabídne uložení všech údajů z příkladu do CSV souboru (ANO/NE).

//start
Vitej();
while (true)
{
    Console.WriteLine("(^o^)    NOVÝ PŘÍKLAD    (^o^)");    
    MetodaVyber(Ze() + Do());
    vysledekFinal = "";
    cisloStart = "";
    doSoustavy = "";
    zeSoustavy = "";
}//konec
void NajdiSoubor()//Metoda, která pokud neexistuje soubor, ze kterho by mohla číst, tak ho vytvoří.
{
    string fileName = "Config_Udaje.txt";
    string path = @"Data\\";
    string file = Path.Combine(path, fileName);
    File.AppendAllText(file, null);
}
string[] ReadData() ////Metoda, která vrací pole stringů, keré načte ze souboru.
{
    string[] lines = null;
    string nazevSouboru = "Config_Udaje.txt";
    string cesta = Path.Combine(Environment.CurrentDirectory, @"Data\\");
    string pathB = Path.Combine(cesta, nazevSouboru);
    lines = File.ReadAllLines(pathB);
    return lines;

}

void WriteData(string jmeno, string prijmeni) //Metoda, která přijme dvě proměnné typu string a zapíše obě do souboru.
{
    string[] JmenoPrijmeni = { jmeno, prijmeni };
    string fileName = "Config_Udaje.txt";
    string path = Path.Combine(Environment.CurrentDirectory, @"Data\\");
    string pathB = Path.Combine(path, fileName);
    File.WriteAllLines(pathB, JmenoPrijmeni);
}
void Vitej() //Metoda, která se supustí hned po spuštění porgramu a přivítá uživatele jménem.
//Případně pokud jmeéno nebylo zadáno dříve, tak nabídne zadání nového jména, které se následně uloží do souboru
{
    NajdiSoubor();//v pripade ze neexistuje, tak se vytvori
    string[] Udaje = ReadData();
    if (Udaje.Length == 0)
    {
        Console.WriteLine("Zadej své jméno: \n");
        string jmeno = Console.ReadLine();
        Console.WriteLine("Zadej své přijmení: \n");
        string prijmeni = Console.ReadLine();
        WriteData(jmeno, prijmeni);

    }
    else
    {
        Console.Write("Vítej uživateli: ");
        foreach (string udaj in Udaje)
        {
            Console.Write(udaj + " ");
        }
        Console.WriteLine();
    }

}
string Ze()//Metoda pro výběr Z jaké soustavy chceme číslo převádět.

{
    while (!ADCheck(zeSoustavy))
    {
        Console.WriteLine("\nZ 2  soustavy\t[A]\n" + "Z 10 soustavy\t[B]\n" + "Z 16 soustavy\t[C]\n" + "Z BCD kodu\t[D]\n" + "Konec programu\t[K]\n");

        zeSoustavy = Console.ReadLine();
        if (zeSoustavy == "K")//podmimnka pro ukonceni programu
        {
            Environment.Exit(0);
        }
    }
        return zeSoustavy;
    
    
}

string Do()//Metoda pro výběr DO jaké soustavy chceme číslo převádět.
{
    while (!ADCheck(doSoustavy))
    {


        Console.WriteLine("Do 2 soustavy\t[A]\n" + "Do 10 soustavy\t[B]\n" + "Do 16 soustavy\t[C]\n" + "Do BCD kodu\t[D]\n");
        doSoustavy = Console.ReadLine();  
    }
    return doSoustavy;
}

string zadaniCisel()//Metoda pro zadání čísla, které chceme převést.
{
    Console.WriteLine("Zadej číslo, které chceš převést:\n");
    cisloStart = Console.ReadLine();
    return cisloStart;
}

void vypisVysledek(string cislo)//Metoda, která vypíše výseldek a následně spustí nabídku pro uložení příkladu do souboru
{
    if (cislo == null || cislo == "0" || cislo == "")
    {

    }
    else
    {
        Console.WriteLine("___________________\nVýsledek je: " + cislo + "\n___________________");
        VolbaUlozeni(cisloStart, vysledekFinal, doSoustavy);
    }
}

void VolbaUlozeni(string cislo, string vysledek, string doSoustavy)//Metoda, která prijme proměnne z prikladu, ktere nasledne umoznuje ulozit do csv souboru.
{
    
    Console.WriteLine("Přejete si příklad uložit do CSV souboru?\nAno\t[A]\nNe\t[N]");
    string odpoved = Console.ReadLine();
    if (odpoved == "A")
    {
        switch (doSoustavy)
        {
            case "A":
                doSoustavy = "Do Dvojkove";
                break;
            case "B"://do 10
                doSoustavy = "Do Desitkove";
                break;
            case "C"://do 16
                doSoustavy = "Do Sestnactkove";
                break;
            case "D":// do bcd
                doSoustavy = "Do BCD kodu";
                break;

        }
        switch (zeSoustavy)
        {
            case "A":
                zeSoustavy = "Z Dvojkove";
                break;
            case "B"://do 10
                zeSoustavy = "Z Desitkove";
                break;
            case "C"://do 16
                zeSoustavy = "Z Sestnactkove";
                break;
            case "D":// do bcd
                zeSoustavy = "Z BCD kodu";
                break;

        }

        string fileName = "Vysledky.csv";
        string path = @"Data\\";
        string file = Path.Combine(path, fileName);

        String separator = ";";
        StringBuilder output = new StringBuilder();

        string[] headings = { "Ze Soustavy", "Cislo", "Vysledek", "Do Soustavy" };//nadpisy
        output.AppendLine(string.Join(separator, headings));



        String[] newLine = { zeSoustavy, cisloStart, vysledekFinal, doSoustavy };//údaje
        output.AppendLine(string.Join(separator, newLine));



        try
        {
            File.AppendAllText(file, output.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Data nemohla byt zapsana do CSV souboru.");
            return;
        }
        Console.WriteLine("Data byly zapsány do souboru: Vysledky.csv");

    }
    else
    {
        Console.WriteLine("Přejete si vysledek poslat na server?\nAno\t[A]\nNe\t[N]");
        odpoved = Console.ReadLine();
        if (odpoved == "A")
        { 
            Thread Server = new Thread(ServerStart);
            Server.Start();
            Console.WriteLine("Připojování");
            ClientStart();
            Server.Join(); //čeká než se ukončí hlavní vlákno.

            

        }
        

    }

}

void ClientStart()//metoda ktera spusti klienta, který se bude napojovat na již spusteny server na portě 8080
{
    // Nastavení IP adresy a portu serveru
    const string IP_ADDRESS = "localhost";
    const int PORT = 8080;

    // Vytvoření připojení k serveru
    TcpClient client = new TcpClient(IP_ADDRESS, PORT);

    // Zaslání dat na server
    byte[] buffer = Encoding.ASCII.GetBytes(vysledekFinal);
    client.GetStream().Write(buffer, 0, buffer.Length);
    Console.WriteLine($"Klient odeslal: {vysledekFinal}");

    // Přijmutí odpovědi od serveru
    buffer = new byte[client.ReceiveBufferSize];
    int bytesRead = client.GetStream().Read(buffer, 0, client.ReceiveBufferSize);
    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
    Console.WriteLine($"Klient přijal: {response}");

    // Ukončení spojení s serverem

    client.Close();
}
void ServerStart()//metoda která spustí server na portu 8080
{
    // Nastavení portu
    const int PORT = 8080;

    // Vytvoření a spuštění serveru
    TcpListener listener = new TcpListener(IPAddress.Any, PORT);
    listener.Start();//poslouchá a čeká na připojení klienta
    Console.WriteLine($"Server běží na portu {PORT}");

    while (true)
    {
        // Čekání na klienta
        TcpClient client = listener.AcceptTcpClient();
        Console.WriteLine("Klient připojen");

        // Přijmutí dat od klienta
        byte[] buffer = new byte[client.ReceiveBufferSize];
        int bytesRead = client.GetStream().Read(buffer, 0, client.ReceiveBufferSize);
        string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine($"Server přijal: {data}");

        // Zpracování dat a odeslání odpovědi
        string response = data + "-> Servrem overeno!";
        byte[] responseBuffer = Encoding.ASCII.GetBytes(response);
        client.GetStream().Write(responseBuffer, 0, responseBuffer.Length);
        Console.WriteLine($"Server Odeslal: {response}");

        // Ukončení spojení s klientem
        client.Close();
        Console.WriteLine("Klient odpojen");
    }
}







void MetodaVyber(string vyber)//Metoda přijíma parametr typu string. V metodě se string porovnává ve switchi a podle vstupu spustí určitý case.
{
    string desitkova = "";
    string dvojkova = "";
    //Dva,deset,sestnact,bcd
    switch (vyber)
    {
        case "AB"://2->10
            cisloStart = zadaniCisel();
            vysledekFinal = ZDvaDoDeset(cisloStart).ToString();
            vypisVysledek(vysledekFinal);
            break;
        case "AC"://2->16//
            cisloStart = zadaniCisel();
            vysledekFinal = ZDvaDoSestnact(cisloStart);
            vypisVysledek(vysledekFinal);
            break;
        case "AD"://2->bcd//
            cisloStart = zadaniCisel();
            desitkova = ZDvaDoDeset(cisloStart).ToString();
            vysledekFinal = ZDesetDoBCD(desitkova);
            vypisVysledek(vysledekFinal);
            break;
        case "BA"://10->2
            cisloStart = zadaniCisel();
            vysledekFinal = ZDesetDoDva(cisloStart);
            vypisVysledek(vysledekFinal);
            break;
        case "BC"://10->16
            cisloStart = zadaniCisel();
            dvojkova = ZDesetDoDva(cisloStart);
            vysledekFinal = ZDvaDoSestnact(dvojkova);
            vypisVysledek(vysledekFinal);
            break;
        case "BD"://10->BCD
            cisloStart = zadaniCisel();
            vysledekFinal = ZDesetDoBCD(cisloStart);
            vypisVysledek(vysledekFinal);
            break;
        case "CA":////16->2
            cisloStart = zadaniCisel();
            vysledekFinal = ZSestnactDoDva(cisloStart);
            vypisVysledek(vysledekFinal);
            break;
        case "CB":////16->10 ---- udělat!!!
            cisloStart = zadaniCisel();
            dvojkova = ZSestnactDoDva(cisloStart);
            vysledekFinal = ZDvaDoDeset(dvojkova).ToString();
            vypisVysledek(vysledekFinal);
            break;
        case "CD"://16->BCD
            cisloStart = zadaniCisel();
            dvojkova = ZSestnactDoDva(cisloStart);
            desitkova = ZDvaDoDeset(dvojkova).ToString();
            vysledekFinal = ZDesetDoBCD(desitkova);

            vypisVysledek(vysledekFinal);
            break;
        case "DA"://BCD->2//
            cisloStart = zadaniCisel();
            desitkova = ZBCDDoDeset(cisloStart).ToString();
            vysledekFinal = ZDesetDoDva(desitkova);
            vypisVysledek(vysledekFinal);
            break;
        case "DB"://BCD->10
            cisloStart = zadaniCisel();
            vysledekFinal = ZBCDDoDeset(cisloStart);
            vypisVysledek(vysledekFinal);
            break;
        case "DC"://BCD->16
            cisloStart = zadaniCisel();
            desitkova = ZBCDDoDeset(cisloStart).ToString();
            dvojkova = ZDesetDoDva(desitkova);
            vysledekFinal = ZDvaDoSestnact(dvojkova);
            vypisVysledek(vysledekFinal);
            break;



    }



}



bool DvaCheck(string vstup)//kontroluje jestli uzivatel zadal pouze cisla 1 a 0 a zadna pismena, stejny check plati i pro BCD

{
    Regex regex = new Regex("^[0-1]+$");//pouze čísla pouze 1 a 0
    if (regex.IsMatch(vstup))//hlídá zadaná čísla
    {
        return true;
    }
    else
    {
        Console.WriteLine("___________________\nŠPATNÝ FORMÁT\n___________________");
        return false;
    }
}

bool DesetCheck(string vstup)//kontroluje jestli uzivatel nezadal pismena
{
    Regex regex = new Regex("^[0-9]+$");
    if (regex.IsMatch(vstup))//hlídá zadaná čísla
    {
        return true;
    }
    else
    {
        Console.WriteLine("___________________\nŠPATNÝ FORMÁT\n___________________");
        return false;
    }
}

bool ADCheck(string vstup)
{
    Regex azpattern = new Regex("[A-D]+");
    if (azpattern.IsMatch(vstup)||vstup == "K")//hlídá zadaná čísla
    {
        return true;
    }
    else
    {
        return false;
    }

}

bool SestnactCheck(string vstup)//kontroluje jestli pismena A-f a cisla 0-9
{
    Regex azpattern = new Regex("[A-F]+");
    Regex numbers = new Regex("^[0-9]+$");
    if (numbers.IsMatch(vstup) && azpattern.IsMatch(vstup))//hlídá zadaná čísla
    {
        return true;
    }
    else
    {
        Console.WriteLine("___________________\nŠPATNÝ FORMÁT\n___________________");
        return false;
    }

}
double ZDvaDoDeset(string sub)//Metoda pro převod z dvojkove soustavy do desitkove
{
    double vysledek = 0;
    double mocnina = 0.0;
    if (DvaCheck(sub))
    {//kontrola formatu

        int x = Convert.ToInt32(sub);
        List<int> listX = GetIntArray(x);
        listX.Reverse();
        foreach (var y in listX)
        {
            if (y == 1)
            {
                vysledek = vysledek + (1 * Math.Pow(2, mocnina));
            }
            else
            {
                vysledek = vysledek + (0 * Math.Pow(2, mocnina));
            }
            mocnina++;




        }
    }
    return vysledek;
}

string ZDesetDoDva(string sub)//metoda převádí čísla z desítkové soustavy do dvojkové
{
    string vysledek = "";
    if (DesetCheck(sub))
    {

        int cislo = Convert.ToInt32(sub);
        List<int> list = new List<int>();

        while (cislo > 0)
        {
            if ((cislo % 2) == 1)
            {
                list.Add(1);
                cislo = (cislo - 1);
            }
            else
            {
                list.Add(0);
            }
            cislo = cislo / 2;
        }
        list.Reverse();

        foreach (int x in list)
        {
            vysledek = vysledek + x.ToString();
        }

    }
    return vysledek;
}

List<int> GetIntArray(int num)//stackowerflow rozdělí celé číslo do jednotlivých digits
{
    List<int> listOfInts = new List<int>();
    while (num > 0)
    {
        listOfInts.Add(num % 10);
        num = num / 10;
    }
    listOfInts.Reverse();
    return listOfInts;
}
string ZBCDDoDeset(string vstup)//Metoda převádí z BCD kodu do desítkové soustavy
{
    vysledekFinal = "";
    if (DvaCheck(vstup))
    {
        while (vstup.Length % 4 != 0)
        {
            vstup = "0" + vstup;
        }

        int chunkSize = 4;
        int stringLength = vstup.Length;
        for (int i = 0; i < stringLength; i += chunkSize)//rozdeli na ctverice
        {
            string ctverice = vstup.Substring(i, chunkSize);
            vysledekFinal = vysledekFinal + ZDvaDoDeset(ctverice);
        }

    }
    return vysledekFinal;
}
string ZDesetDoBCD(string vstup)//metoda převádí čísla z desítkové soustavy do BCD kodu
{
    vysledekFinal = "";
    if (DesetCheck(vstup))
    {
        foreach (char cis in vstup)
        {
            string x = ZDesetDoDva(cis.ToString());
            Console.WriteLine(x.Length);
            while (x.Length < 4)
            {

                x = "0" + x;
            }
            vysledekFinal = vysledekFinal + x;
        }
    }
    return vysledekFinal;

}
string ZSestnactDoDva(string vstup)//metoda převádí čísla z šestnáctkoé soustavy do dvojkové
{
    string vysledek = "";
    if (SestnactCheck(vstup))
    {
        string promenna = "";
        foreach (char cis in vstup)
        {
            string vysS = "";
            switch (cis.ToString())
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    vysS = cis.ToString();
                    break;

                case "A":
                    vysS = "10";

                    break;
                case "B":
                    vysS = "11";

                    break;
                case "C":
                    vysS = "12";

                    break;
                case "D":
                    vysS = "13";

                    break;
                case "E":
                    vysS = "14";

                    break;
                case "F":
                    vysS = "15";

                    break;

            }
            Console.WriteLine(vysledek);
            vysledek = vysledek + ZDesetDoDva(vysS);

        }

    }
    return vysledek;

}
string ZDvaDoSestnact(string vstup)//metoda převádí čísla z dvojkové soustavy do šestnáctkové
{
    string vysledek = "";
    if (DvaCheck(vstup))
    {
        List<int> poleVysledku = new List<int>();
        List<int> poleBinar = new List<int>();
        int prevod;
        int chunkSize = 1;
        int stringLength = vstup.Length;
        for (int i = 0; i < stringLength; i += chunkSize)//rozdeli na ctverice
        {
            string ctverice = vstup.Substring(i, chunkSize);
            prevod = Convert.ToInt32(ctverice);
            poleBinar.Add(prevod);
        }

        int y = 0;

        int nasobek = 0;

        poleBinar.Reverse();
        while (poleBinar.Count % 4 != 0)
        {
            poleBinar.Add(0);
        }
        poleBinar.Reverse();
        int pozice = 0;
        int promenna = 0;

        for (int i = 0; i < poleBinar.Count; i++)
        {
            //správně

            if (i == 3 + pozice)//hlida pozice v poli
            {
                nasobek = 1;
            }
            if (i == 2 + pozice)
            {
                nasobek = 2;
            }
            if (i == 1 + pozice)
            {
                nasobek = 4;
            }
            if (i == 0 + pozice)
            {
                nasobek = 8;
            }

            poleBinar[i] = poleBinar[i] * nasobek;

            /*else
             {
                 poleBinar[i] = ;
             }*/
            promenna++;
            if (promenna == 4)//hlida aby cislo bylo rozdeleno po 4
            {

                pozice = pozice + 4;
                promenna = 0;

            }

        }
        for (int i = 0; i < poleBinar.Count; i = i + 4)
        {

            poleVysledku.Add((poleBinar[i] + poleBinar[i + 1] + poleBinar[i + 2] + poleBinar[i + 3]));
        }

        foreach (int vys in poleVysledku)
        {
            string vysS = "";
            switch (vys)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    vysS = Convert.ToString(vys);
                    break;

                case 10:
                    vysS = "A";

                    break;
                case 11:
                    vysS = "B";

                    break;
                case 12:
                    vysS = "C";

                    break;
                case 13:
                    vysS = "D";

                    break;
                case 14:
                    vysS = "E";

                    break;
                case 15:
                    vysS = "F";

                    break;

            }
            vysledek = vysledek + vysS;
        }

    }
    return vysledek;

}

//Prázdnej texťák -> nabídka přidání jména-> ulozi se do textaku a bude pokazde vitat jmenem.
//1. nabídka, v jaké číselné soustavě chceme číslo zadat (Z jaké soustavy bude program převádět)(A-D).
//2. nabídka, do jaké číselné soustavy chceme číslo převést (Do jaké soustavy bude program převádět)(A-D).
//3. nabídka, program převede číslo ze zvolené soustay do zvolené soustavy a nabídne uložení všech údajů z příkladu do CSV souboru (ANO/NE).






