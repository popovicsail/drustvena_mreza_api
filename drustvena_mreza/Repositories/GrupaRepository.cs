using drustvena_mreza.Models;

namespace drustvena_mreza.Repositories
{
    public static class GrupaRepository
    {
        private static string GrupaPath { get; set; } = "data/grupa.csv";
        public static Dictionary<int, Grupa> AllGrupa { get; set; } = new Dictionary<int, Grupa>();

        public static void LoadGrupa()
        {
            try
            {
                string[] grupaLinije = File.ReadAllLines(GrupaPath);

                foreach (string grupaLinija in grupaLinije)
                {
                    string[] grupaLinijaAll = grupaLinija.Split(",");

                    int newGrupaId = int.Parse(grupaLinijaAll[0]);
                    string newGrupaIme = grupaLinijaAll[1];
                    string newGrupaDatumOsnivanja = grupaLinijaAll[2];

                    Grupa newGrupa = new Grupa(newGrupaId, newGrupaIme, newGrupaDatumOsnivanja);

                    AllGrupa[newGrupaId] = newGrupa;
                }
                Console.WriteLine("SUCCESS: LoadGrupa");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("ERROR: LoadGrupa nije pronađen");
            }
            catch (FormatException)
            {
                Console.WriteLine("ERROR: LoadGrupa info nije u dobrom formatu");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("ERROR: LoadGrupa info nema dovoljno podataka");
            }
        }

        public static void SaveGrupa()
        {
            try
            {
                List<string> grupaLinije = new List<string>();

                foreach (Grupa grupa in AllGrupa.Values)
                {
                    grupaLinije.Add(grupa.FormatZaSave());
                }
                File.WriteAllLines(GrupaPath, grupaLinije);

                Console.WriteLine("SUCCESS: SaveGrupa");
            }
            catch
            {
                Console.WriteLine("ERROR: SaveGrupa");
            }
        }
    }
}
