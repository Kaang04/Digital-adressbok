using System.Reflection.Metadata.Ecma335;
using System.Xml;

public struct AdressBok
{
    private Kontakt[] Kontakter;
    public string path;
    private object kontakter;
    const string KONTAKT_FORMAT = "| {0, -3} | {1, -25} | {2, -25} | {3, -25} | {4, -13} |";


    public AdressBok(string path)
    {
        Kontakter = Array.Empty<Kontakt>();
        this.path = path;
        Load();
    }


    public void Draw()
    {
        Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
        Console.WriteLine(KONTAKT_FORMAT, "ID", "Namn", "Telefonnummer", "Email", "Födelsedatum");
        Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

        for (int i = 0; i < Kontakter.Length; i++)
        {
            var kontakt = Kontakter[i];

            if(kontakt.DagarTillFödelsedag() == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if(kontakt.DagarTillFödelsedag() < 7 && kontakt.DagarTillFödelsedag() > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

      
            Console.WriteLine(KONTAKT_FORMAT,
                i + 1,
                kontakt.Efternamn + ", " + kontakt.Fornamn,
                kontakt.Telefonnumer,
                kontakt.Email,
                kontakt.Fodelsedag.ToString("yyyy-MM-dd"));

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
        }

    }

    public void Save()
    {
        if (Kontakter == null) { Kontakter = Array.Empty<Kontakt>(); }

        if (!File.Exists(path))
        {
            File.Create(path);
        }

        using (StreamWriter sw = new StreamWriter(path))
        {
            foreach (Kontakt kontakt in Kontakter)
            {
                sw.WriteLine(kontakt.Efternamn + ';' + kontakt.Fornamn + ';' + kontakt.Telefonnumer + ';' + kontakt.Email + ';' + kontakt.Fodelsedag.ToString("yyyy-MM-dd"));
            }
        }

    }

    public void Load()
    {
        if (!File.Exists(path)) ;

        var lines = File.ReadAllLines(path);
        Kontakter = new Kontakt[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            var data = lines[i].Split(';');
            if (data.Length == 5)
            {
                Kontakt kontakt = new Kontakt
                {
                    Efternamn = data[0],
                    Fornamn = data[1],
                    Telefonnumer = data[2],
                    Email = data[3],
                    Fodelsedag = DateTime.Parse(data[4])
                };

                Kontakter[i] = kontakt;

            }
        }
    }

    public void TaBortKontakt()
    {
        if (Kontakter.Length == 0)
        {
            Console.WriteLine("Det finns inga kontakter att ta bort ");
            return;
        }

        Draw();

        Console.WriteLine("Ange en kontakts ID nummer för att ta bort dem: ");
        if (int.TryParse(Console.ReadLine(), out int id) && id > 0 && id <= Kontakter.Length)
        {
            var nyKontakter = new Kontakt[Kontakter.Length - 1];

            int index = 0;
            for (int i = 0; i < Kontakter.Length; i++)
            {
                if (i != id - 1)
                {
                    nyKontakter[index] = Kontakter[i];
                    index++;
                }
            }

            Kontakter = nyKontakter;

            Save();

            Console.WriteLine("Kontakten har raderats");
        }

        else
        {
            Console.WriteLine("Ingen kontakt har raderats: FEL ID");
        }
    }

    public void LäggTillKontakt()
    {
        Kontakt kontakt = new Kontakt();

        while (true)
        {
            Console.WriteLine("Ange ett förnamn: ");

            string? name = Console.ReadLine();

            if (name == null || name == string.Empty)
            {
                Console.WriteLine("Input kan inte vara tomt");

                continue;
            }

            bool EjFörnamn = false;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == ' ') { continue; }

                if (!char.IsLetter(name[i]))
                {
                    EjFörnamn = true;
                    break;
                }

            }
            if (EjFörnamn)
            {
                Console.WriteLine("Kunde inte lägga till förnamn: FÅR BARA INNEHÅLLA BOKSTÄVER OCH MELLANSLAG");
                continue;
            }

            kontakt.Fornamn = name;

            break;
        }

        while (true)
        {
            Console.WriteLine("Ange ett efternamn: ");

            string? name = Console.ReadLine();

            if (name == null || name == string.Empty)
            {
                Console.WriteLine("Input kan inte vara tomt");

                continue;
            }

            bool EjEfternamn = false;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == '-' || name[i] == ' ') { continue; }

                if (!char.IsLetter(name[i]))
                {
                    EjEfternamn = true;
                    break;
                }

            }
            if (EjEfternamn)
            {
                Console.WriteLine("Kunde inte lägga till efternamn: EJ BOKSTÄVER");
                continue;
            }

            kontakt.Efternamn = name;

            break;
        }

        while (true)
        {
            Console.WriteLine("Ange ett telefonnummer (XXX-XXX XX XX): ");

            string? name = Console.ReadLine();

            if (name == null || name == string.Empty)
            {
                Console.WriteLine("Input kan inte vara tomt");

                continue;
            }

            bool EjNummer = false;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == '-' || name[i] == ' ') { continue; }

                if (!int.TryParse(name[i].ToString(), out int result))
                {
                    EjNummer = true;
                    break;
                }

            }

            if(EjNummer)
            {
                Console.WriteLine("Kunde inte lägga till telefonnummer: EJ SIFFROR");
                continue;
            }     


            kontakt.Telefonnumer = name;

            break;
        }

        while (true)
        {
            Console.WriteLine("Ange ett email: "); 

            string? name = Console.ReadLine();

            if (name == null || name == string.Empty)
            {
                Console.WriteLine("Input kan inte vara tomt");

                continue;
            }

            int LetterCount = 0;
            int DigitCount = 0;
            int atSymbolCount = 0;
            bool FöreSymbol = true;

            foreach (char ch in name)
            {
                if (ch == '@')
                {
                    atSymbolCount++;
                    FöreSymbol = false;
                    continue;
                }

                if (FöreSymbol)
                {
                    if (char.IsLetter(ch)) 
                        LetterCount++;
                    else if (char.IsDigit(ch))
                        DigitCount++;
                }
            }

            if (LetterCount < 5)
            {
                Console.WriteLine("Email måste innehålla minst fem bokstäver");
                continue;
            }

            if (DigitCount < 2)
            {
                Console.WriteLine("Email måste innehålla minst två siffror");
                continue;
            }

            if (atSymbolCount != 1)
            {
                Console.WriteLine("Email måste innehålla exakt en @");
                continue;
            }


            kontakt.Email = name;

            break;
        }

        while (true)
        {
            Console.WriteLine("Ange ett födelsedatum (yyyy-mm-dd): ");

            string? input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input) && DateTime.TryParse(input, out DateTime fodelsedag))
            {
                if (fodelsedag > DateTime.Today)
                {
                    Console.WriteLine("Födelsedatumet kan inte vara i framtiden");
                    continue ;
                }

                kontakt.Fodelsedag = fodelsedag;

                break;
            }

            Console.WriteLine("Felaktigt format, försök igen");
        }

        Array.Resize(ref Kontakter, Kontakter.Length + 1);

        Kontakter[Kontakter.Length - 1] = kontakt;

        Save();
    }

    public void RedigeraKontakter() 
    {
       while (true)
        {
            if (Kontakter.Length == 0)
            {
                Console.WriteLine("Det finns inga kontakter att redigera");
                return;
            }

            Console.WriteLine("Ange ett ID som du vill redigera: ");
            string? resultat  = Console.ReadLine();

             if (int.TryParse(resultat, out int id) && id > 0 && id <= Kontakter.Length)
            {
                var kontakt = Kontakter[id - 1];

                Console.WriteLine("Vad vill du redigera?");
                Console.WriteLine("1. Förnamn");
                Console.WriteLine("2. Efternamn");
                Console.WriteLine("3. Telefonnummer");
                Console.WriteLine("4. Email");
                Console.WriteLine("5. Födelsedatum");
                
                string? val = Console.ReadLine();

                switch (val)
                {
                    case "1":
                        while (true)
                        {
                            Console.WriteLine("Ange ett förnamn: ");

                            string? name = Console.ReadLine();

                            if (name == null || name == string.Empty)
                            {
                                Console.WriteLine("Input kan inte vara tomt");

                                continue;
                            }

                            bool EjFörnamn = false;
                            for (int i = 0; i < name.Length; i++)
                            {
                                if (name[i] == ' ') { continue; }

                                if (!char.IsLetter(name[i]))
                                {
                                    EjFörnamn = true;
                                    break;
                                }

                            }
                            if (EjFörnamn)
                            {
                                Console.WriteLine("Kunde inte lägga till förnamn: FÅR BARA INNEHÅLLA BOKSTÄVER OCH MELLANSLAG");
                                continue;
                            }

                            kontakt.Fornamn = name;

                            break;
                        }
                        break;

                    case "2":
                        while (true)
                        {
                            Console.WriteLine("Ange ett efternamn: ");

                            string? name = Console.ReadLine();

                            if (name == null || name == string.Empty)
                            {
                                Console.WriteLine("Input kan inte vara tomt");

                                continue;
                            }

                            bool EjEfternamn = false;
                            for (int i = 0; i < name.Length; i++)
                            {
                                if (name[i] == '-' || name[i] == ' ') { continue; }

                                if (!char.IsLetter(name[i]))
                                {
                                    EjEfternamn = true;
                                    break;
                                }

                            }
                            if (EjEfternamn)
                            {
                                Console.WriteLine("Kunde inte lägga till efternamn: EJ BOKSTÄVER");
                                continue;
                            }

                            kontakt.Efternamn = name;

                            break;
                        }
                        break;

                    case "3":
                        while (true)
                        {
                            Console.WriteLine("Ange ett telefonnummer (XXX-XXX XX XX): ");

                            string? name = Console.ReadLine();

                            if (name == null || name == string.Empty)
                            {
                                Console.WriteLine("Input kan inte vara tomt");

                                continue;
                            }

                            bool EjNummer = false;
                            for (int i = 0; i < name.Length; i++)
                            {
                                if (name[i] == '-' || name[i] == ' ') { continue; }

                                if (!int.TryParse(name[i].ToString(), out int result))
                                {
                                    EjNummer = true;
                                    break;
                                }

                            }

                            if (EjNummer)
                            {
                                Console.WriteLine("Kunde inte lägga till telefonnummer: EJ SIFFROR");
                                continue;
                            }


                            kontakt.Telefonnumer = name;

                            break;
                        }
                        break;

                    case "4":
                        while (true)
                        {
                            Console.WriteLine("Ange ett email: ");

                            string? name = Console.ReadLine();

                            if (name == null || name == string.Empty)
                            {
                                Console.WriteLine("Input kan inte vara tomt");

                                continue;
                            }

                            int LetterCount = 0;
                            int DigitCount = 0;
                            int atSymbolCount = 0;
                            bool FöreSymbol = true;

                            foreach (char ch in name)
                            {
                                if (ch == '@')
                                {
                                    atSymbolCount++;
                                    FöreSymbol = false;
                                    continue;
                                }

                                if (FöreSymbol)
                                {
                                    if (char.IsLetter(ch))
                                        LetterCount++;
                                    else if (char.IsDigit(ch))
                                        DigitCount++;
                                }
                            }

                            if (LetterCount < 5)
                            {
                                Console.WriteLine("Email måste innehålla minst fem bokstäver");
                                continue;
                            }

                            if (DigitCount < 2)
                            {
                                Console.WriteLine("Email måste innehålla minst två siffror");
                                continue;
                            }

                            if (atSymbolCount != 1)
                            {
                                Console.WriteLine("Email måste innehålla exakt en @");
                                continue;
                            }


                            kontakt.Email = name;

                            break;
                        }
                        break;

                    case "5":
                        while (true)
                        {
                            Console.WriteLine("Ange ett födelsedatum (yyyy-mm-dd): ");

                            string? input = Console.ReadLine();

                            if (!string.IsNullOrEmpty(input) && DateTime.TryParse(input, out DateTime fodelsedag))
                            {
                                if (fodelsedag > DateTime.Today)
                                {
                                    Console.WriteLine("Födelsedatumet kan inte vara i framtiden");
                                    continue;
                                }

                                kontakt.Fodelsedag = fodelsedag;

                                break;
                            }

                            Console.WriteLine("Felaktigt format, försök igen");
                        }
                        break;
                }

                Kontakter[id - 1] = kontakt;

                Save();

                Console.WriteLine("Kontakten har upptaderats");
                break;

            }
             else
            {
                Console.WriteLine("Kontakt kunde inte redigeras: ID FINNS EJ");
                continue;
            }

            }

        }
}



    public struct Kontakt
    {
        public string Fornamn;
        public string Efternamn;
        public string Telefonnumer;
        public string Email;
        public DateTime Fodelsedag;

        public int DagarTillFödelsedag()
        {
            DateTime a = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime b = new DateTime(DateTime.Now.Year, Fodelsedag.Month, Fodelsedag.Day);
            return (b - a).Days; 

        }

    }