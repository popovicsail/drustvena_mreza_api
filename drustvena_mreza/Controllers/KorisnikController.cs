using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using drustvena_mreza.Models;
using drustvena_mreza.Utilities;

namespace drustvena_mreza.Controllers
{
    [Route("api/korisnik")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private GrupaRepository grupaRepository = new GrupaRepository();
        private KorisnikRepository korisnikRepository = new KorisnikRepository();
        private ClanstvaRepository clanstvaRepository = new ClanstvaRepository();


        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            List<Korisnik> allKorisnik = KorisnikRepository.AllKorisnik.Values.ToList();
            return allKorisnik;
        }


        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            if (!KorisnikRepository.AllKorisnik.ContainsKey(id))
            {
                return NotFound();
            }
            return Ok(KorisnikRepository.AllKorisnik[id]);
        }


        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik newKorisnik)
        {
            if (string.IsNullOrWhiteSpace(newKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(newKorisnik.Ime) || string.IsNullOrWhiteSpace(newKorisnik.Prezime) || string.IsNullOrWhiteSpace(newKorisnik.DatumRodjenja.ToString()))
            {
                return BadRequest();
            }

            newKorisnik.Id = AllUtilities.ReturnIdNew(KorisnikRepository.AllKorisnik.Keys.ToList());
            KorisnikRepository.AllKorisnik[newKorisnik.Id] = newKorisnik;
            korisnikRepository.SaveKorisnik();

            return Ok(newKorisnik);
        }


        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik updateKorisnik)
        {
            if (string.IsNullOrWhiteSpace(updateKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(updateKorisnik.Ime) || string.IsNullOrWhiteSpace(updateKorisnik.Prezime) || string.IsNullOrWhiteSpace(updateKorisnik.DatumRodjenja.ToString()))
            {
                return BadRequest();
            }

            if (!KorisnikRepository.AllKorisnik.ContainsKey(id))
            {
                return NotFound();
            }

            Korisnik korisnik = KorisnikRepository.AllKorisnik[id];
            korisnik.KorisnickoIme = updateKorisnik.KorisnickoIme;
            korisnik.Ime = updateKorisnik.Ime;
            korisnik.Prezime = updateKorisnik.Prezime;
            korisnik.DatumRodjenja = updateKorisnik.DatumRodjenja;

            korisnikRepository.SaveKorisnik();


            return Ok(korisnik);
        }
    }

}
