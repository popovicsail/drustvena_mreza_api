using drustvena_mreza.Models;

namespace drustvena_mreza.Repositories
{
    public class ClanstvaRepository
    {
        private static string ClanstvaPath { get; set; } = "data/clanstva.csv";

        public ClanstvaRepository()
        {
            LoadClanstva();
        }
        public void LoadClanstva()
        {
            try
            {
                string[] clanstvaLinije = File.ReadAllLines(ClanstvaPath);

                foreach (string clanstvaLinija in clanstvaLinije)
                {
                    string[] clanstvaLinijaAll = clanstvaLinija.Split(",");
                    int korisnikId = int.Parse(clanstvaLinijaAll[0]);
                    int grupaId = int.Parse(clanstvaLinijaAll[1]);

                    // GrupaRepository.AllGrupa[grupaId].Korisnik.Add(KorisnikRepository.AllKorisnik[korisnikId]);
                    KorisnikRepository.AllKorisnik[korisnikId].Grupa.Add(GrupaRepository.AllGrupa[grupaId]);

                }
                Console.WriteLine("SUCCESS: LoadClanstva");
            }
            catch
            {
                Console.WriteLine("ERROR: LoadClanstva");
            }
        }

        public void SaveClanstva()
        {
            try
            {
                List<string> clanstvaLinije = new List<string>();

                foreach (Korisnik korisnik in KorisnikRepository.AllKorisnik.Values)
                {
                    foreach (Grupa grupa in korisnik.Grupa)
                    {
                        int korisnikId = korisnik.Id;
                        int grupaId = grupa.Id;
                        clanstvaLinije.Add($"{korisnikId},{grupaId}");
                    }
                }
                File.WriteAllLines(ClanstvaPath, clanstvaLinije);
                Console.WriteLine("SUCCESS: SaveClanstva");
            }
            catch
            {
                Console.WriteLine("ERROR: SaveClanstva");
            }
        }
    }
}
