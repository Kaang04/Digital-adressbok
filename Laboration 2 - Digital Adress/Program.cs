const string FILE_NAME = "Digital adressbok.csv";

AdressBok Adressbok = new AdressBok(FILE_NAME);

while (true)
{

    Console.WriteLine("Kontaktlistameny: ");
    Console.WriteLine("1. Lägg till kontakt");
    Console.WriteLine("2. Visa kontakter");
    Console.WriteLine("3. Redigera kontakter");
    Console.WriteLine("4. Ta bort kontakt");
    Console.WriteLine("5. Avsluta");
    Console.WriteLine("Ange ditt val:");

    string? input = Console.ReadLine();
    if (!int.TryParse(input, out int value))
    {
        continue;
    }

    if (value > 5 || value < 1)
    {
        continue;
    }

    if (value == 1)
    {
        Adressbok.LäggTillKontakt();
    }

    else if (value == 2)
    {
        Adressbok.Draw();     
    }

    else if (value == 3)
    {
        Adressbok.Draw();
        Adressbok.RedigeraKontakter();
    }

    else if (value == 4)
    {
        Adressbok.TaBortKontakt();
    }

    else if (value == 5)
    {
        break;
    }

    Adressbok.Save();

}