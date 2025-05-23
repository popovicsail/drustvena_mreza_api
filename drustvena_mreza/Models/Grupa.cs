namespace drustvena_mreza.Models
{
    public class Grupa
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public DateTime DatumOsnivanja { get; set; }
        // public List<Korisnik> Korisnik { get; set; } = new List<Korisnik>();

        public Grupa(int id, string ime, DateTime datumOsnivanja)
        {
            Id = id;
            Ime = ime;
            DatumOsnivanja = datumOsnivanja;
        }
        public string FormatZaSave()
        {
            return $"{Id},{Ime},{DatumOsnivanja}";
        }

    }

    //Prikaz datuma ima i vrijeme koje nastaje prilikom pravljenja objekta.
    //Malo sam istrazio i vidio da se moze rijesiti ovako
    public class GrupaDTO
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string DatumOsnivanja { get; set; }

        public GrupaDTO(Grupa grupa)
        {
            Id = grupa.Id;
            Ime = grupa.Ime;
            DatumOsnivanja = grupa.DatumOsnivanja.ToString("yyyy-MM-dd");
        }
    }
}
