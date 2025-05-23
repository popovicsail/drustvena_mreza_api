using drustvena_mreza.Models;

namespace drustvena_mreza.Repositories
{
    public class KorisnikRepository
    {
        private string KorisnikPath { get; } = "data/korisnik.csv";
        public static Dictionary<int, Korisnik> AllKorisnik { get; set; } = new Dictionary<int, Korisnik>();
        
        public KorisnikRepository()
        {
            LoadKorisnik();
        }

        private void LoadKorisnik()
        {
            try
            {
                string[] korisnikLinije = File.ReadAllLines(KorisnikPath);

                foreach (string korisnikLinija in korisnikLinije)
                {
                    string[] korisnikLinijaAll = korisnikLinija.Split(",");

                    int newKorisnikId = int.Parse(korisnikLinijaAll[0]);
                    string newKorisnikKorisnickoIme = korisnikLinijaAll[1];
                    string newKorisnikIme = korisnikLinijaAll[2];
                    string newKorisnikPrezime = korisnikLinijaAll[3];
                    DateTime newKorisnikDatumRodjenja = DateTime.Parse(korisnikLinijaAll[4]);

                    Korisnik newKorisnik = new Korisnik(newKorisnikId, newKorisnikKorisnickoIme, newKorisnikIme, newKorisnikPrezime, newKorisnikDatumRodjenja);

                    AllKorisnik[newKorisnikId] = newKorisnik;
                }
                Console.WriteLine("SUCCESS: LoadKorisnik");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("ERROR: LoadKorisnik nije pronađen");
            }
            catch (FormatException)
            {
                Console.WriteLine("ERROR: LoadKorisnik info nije u dobrom formatu");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("ERROR: LoadKorisnik info nema dovoljno podataka");
            }
        }

        public void SaveKorisnik()
        {
            try
            {
                List<string> korisnikLinije = new List<string>();

                foreach (Korisnik korisnik in AllKorisnik.Values)
                {
                    korisnikLinije.Add(korisnik.FormatZaSave());
                }
                File.WriteAllLines(KorisnikPath, korisnikLinije);

                Console.WriteLine("SUCCESS: SaveKorisnik");
            }
            catch
            {
                Console.WriteLine("ERROR: SaveKorisnik");
            }
        }
    }
}
