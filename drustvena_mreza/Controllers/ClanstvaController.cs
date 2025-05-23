using drustvena_mreza.Models;
using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace drustvena_mreza.Controllers
{
    [Route("api/grupa/{grupaId}/korisnik")]
    [ApiController]
    public class ClanstvaController : Controller
    {
        private GrupaRepository grupaRepository = new GrupaRepository();
        private KorisnikRepository korisnikRepository = new KorisnikRepository();
        private ClanstvaRepository clanstvaRepository = new ClanstvaRepository();


        [HttpGet]
        public ActionResult<List<Korisnik>> GetById(int grupaId)
        {
            if (!GrupaRepository.AllGrupa.ContainsKey(grupaId))
            {
                return NotFound();
            }

            List<Korisnik> allKorisnik = KorisnikRepository.AllKorisnik.Values.ToList();
            List<Korisnik> grupaKorisnik = new List<Korisnik>();

            foreach (Korisnik korisnik in allKorisnik)
            {
                foreach (Grupa grupa in korisnik.Grupa)
                {
                    if (grupa.Id == grupaId)
                    {
                        grupaKorisnik.Add(korisnik);
                    }
                }
            }
            return Ok(grupaKorisnik);
        }

        [HttpPost("{korisnikId}")]
        public ActionResult AddKorisnikGrupa(int grupaId, int korisnikId)
        {
            if (!GrupaRepository.AllGrupa.ContainsKey(grupaId))
            {
                return NotFound();
            }

            if (!KorisnikRepository.AllKorisnik.ContainsKey(korisnikId))
            {
                return NotFound();
            }

            Korisnik korisnik = KorisnikRepository.AllKorisnik[korisnikId];
            Grupa grupa = GrupaRepository.AllGrupa[grupaId];

            foreach (Grupa g in korisnik.Grupa)
            {
                if (g.Id == grupaId)
                {
                    return BadRequest();
                }
            }

            korisnik.Grupa.Add(grupa);
            clanstvaRepository.SaveClanstva();
            return Ok(korisnik);

        }

        [HttpDelete("{korisnikId}")]
        public ActionResult RemoveKorisnikGrupa(int grupaId, int korisnikId)
        {
            if (!GrupaRepository.AllGrupa.ContainsKey(grupaId))
            {
                return NotFound();
            }

            if (!KorisnikRepository.AllKorisnik.ContainsKey(korisnikId))
            {
                return NotFound();
            }

            Korisnik korisnik = KorisnikRepository.AllKorisnik[korisnikId];
            Grupa grupa = GrupaRepository.AllGrupa[grupaId];

            bool korisnikJeClanGrupe = false;

            foreach (Grupa g in korisnik.Grupa)
            {
                if (g.Id == grupaId)
                {
                  korisnikJeClanGrupe=true; 
                }
            }
            if (!korisnikJeClanGrupe)
            {
                return BadRequest();
            }

            korisnik.Grupa.Remove(grupa);
            clanstvaRepository.SaveClanstva();

            return NoContent();
        }


    }
}
