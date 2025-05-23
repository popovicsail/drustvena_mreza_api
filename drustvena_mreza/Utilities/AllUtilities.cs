using System.Globalization;

namespace drustvena_mreza.Utilities
{
    public static class AllUtilities
    {
        public static int ReturnIdMax(List<int> idAll)
        {
            for (int i = 1; i <= idAll.Count + 1; i++)
            {
                if (!idAll.Contains(i))
                {
                    return i;
                }
            }
            return -1;
        }

        public static int ReturnIdNew(List<int> idAll)
        {
            for (int i = 1; i <= idAll.Count + 1; i++)
            {
                if (!idAll.Contains(i))
                {
                    return i;
                }
            }
            return -1;
        }

        public static int InputCeoBroj(string objekat)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"{objekat}");

                    string ceoBroj = Console.ReadLine();

                    return int.Parse(ceoBroj);
                }
                catch (FormatException)
                {
                    Console.WriteLine("ERROR: Vrednost mora biti ceo broj.");
                }
            }
        }


        public static int InputPozitivanCeoBroj(string objekat)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"{objekat}");

                    int pozitivanCeoBroj = int.Parse(Console.ReadLine());

                    if (pozitivanCeoBroj < 1)
                    {
                        Console.WriteLine("ERROR: Vrednost mora biti ceo broj veći od 0");
                        continue;
                    }

                    return pozitivanCeoBroj;

                }
                catch (FormatException)
                {
                    Console.WriteLine("ERROR: Vrednost mora biti ceo broj veći od 0");
                }
            }
        }

        public static int InputCeoBrojUOpsegu(string objekat, int pocetakOpsega, int krajOpsega)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"{objekat}");

                    int ceoBrojUOpsegu = int.Parse(Console.ReadLine());

                    if (ceoBrojUOpsegu < pocetakOpsega || ceoBrojUOpsegu > krajOpsega)
                    {
                        Console.WriteLine("ERROR: Vrednost mora biti broj između " + pocetakOpsega + " i " + krajOpsega);
                        continue;
                    }

                    return ceoBrojUOpsegu;
                }
                catch (FormatException)
                {
                    Console.WriteLine("ERROR: Vrednost mora biti broj između " + pocetakOpsega + " i " + krajOpsega);
                }
            }
        }

        public static double InputDecimalanBroj(string objekat)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"{objekat}");

                    double decimalanBroj = double.Parse(Console.ReadLine());
                    return decimalanBroj;
                }
                catch (FormatException)
                {
                    Console.WriteLine("ERROR: Vrednost mora biti decimalan broj");
                }
            }
        }

        public static double InputPozitivanDecimalanBroj(string objekat)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"{objekat}");

                    double pozitivanDecimalanBroj = double.Parse(Console.ReadLine());

                    if (pozitivanDecimalanBroj < 1)
                    {
                        Console.WriteLine("ERROR: Vrednost mora biti pozitivan decimalan broj");
                        continue;
                    }

                    return pozitivanDecimalanBroj;
                }
                catch (FormatException)
                {
                    Console.WriteLine("ERROR: Vrednost mora biti decimalan broj");
                }
            }
        }

        public static bool InputBool(string objekat)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"{objekat}");

                    string izbor = Console.ReadLine().ToLower();

                    if (izbor == "da")
                    {
                        return true;
                    }
                    else if (izbor == "ne")
                    {
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Vrednost mora biti 'Da' ili 'Ne'");
                    }
                }
                catch
                {
                    Console.WriteLine("ERROR: Vrednost mora biti 'Da' ili 'Ne'");
                }
            }
        }

        public static string InputStringMinDuzine(string objekat, int length)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"{objekat}");

                    string stringMinDuzine = Console.ReadLine();

                    if (stringMinDuzine.Length < length)
                    {
                        Console.WriteLine($"ERROR: Vrednost mora minimum {length} karaktera duga");
                        continue;
                    }

                    return stringMinDuzine;
                }
                catch
                {
                    Console.WriteLine($"ERROR: Vrednost mora minimum {length} karaktera duga");
                }
            }
        }

        public static int InputIntSpecificDuzine(string objekat, int length)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"{objekat}");

                    string intSpecificDuzineString = Console.ReadLine();

                    if (intSpecificDuzineString.Length != length)
                    {
                        Console.WriteLine($"ERROR: Vrednost mora biti tačno {length} broja.");
                        continue;
                    }

                    if (!intSpecificDuzineString.All(char.IsDigit))
                    {
                        Console.WriteLine($"ERROR: Vrednost mora biti tačno {length} broja.");
                    }

                    return int.Parse(intSpecificDuzineString);
                }
                catch
                {
                    Console.WriteLine($"ERROR: Vrednost mora biti tačno {length} broja.");
                }
            }
        }

        public static string InputString(string objekat)
        {
            Console.WriteLine($"{objekat}");

            string stringUnos = Console.ReadLine();

            return stringUnos;
        }

        public static DateTime InputDatum(string objekat)
        {
            Console.WriteLine($"{objekat}");

            while (true)
            {
                try
                {
                    DateTime datum = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy.", CultureInfo.InvariantCulture);
                    return datum;
                }
                catch
                {
                    Console.WriteLine("ERROR: Vrednost mora biti u formatu dd.MM.yyyy.");
                    return DateTime.MinValue;
                }
            }
        }

        public static DateTime InputProsliDatum(string objekat)
        {
            Console.WriteLine($"{objekat}");

            while (true)
            {
                try
                {
                    DateTime buduciDatum = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy.", CultureInfo.InvariantCulture);

                    if (buduciDatum > DateTime.Now)
                    {
                        Console.WriteLine("ERROR: Vrednost mora biti vreme u prošlosti i u formatu dd.MM.yyyy.");
                        continue;
                    }

                    return buduciDatum;
                }
                catch
                {
                    Console.WriteLine("ERROR: Vrednost mora biti vreme u prošlosti i u formatu dd.MM.yyyy.");
                }
            }
        }

        public static DateTime InputBuduciDatum(string objekat)
        {
            Console.WriteLine($"{objekat}");

            while (true)
            {
                try
                {
                    DateTime buduciDatum = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy.", CultureInfo.InvariantCulture);

                    while (buduciDatum < DateTime.Now)
                    {
                        Console.WriteLine("ERROR: Vrednost mora biti vreme u budućnosti i u formatu dd.MM.yyyy.");
                        continue;
                    }

                    return buduciDatum;
                }
                catch
                {
                    Console.WriteLine("ERROR: Vrednost mora biti vreme u budućnosti i u formatu dd.MM.yyyy.");
                }
            }
        }
    }
}
