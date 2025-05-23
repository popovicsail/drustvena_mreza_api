namespace drustvena_mreza.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public List<Grupa> Grupa { get; set; } = new List<Grupa>();

        public Korisnik(int id, string korisnickoIme, string ime, string prezime, DateTime datumRodjenja)
        {
            Id = id;
            KorisnickoIme = korisnickoIme;
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
        }
        public string FormatZaSave()
        {
            return $"{Id},{KorisnickoIme},{Ime},{Prezime},{DatumRodjenja}";
        }
    }
}
