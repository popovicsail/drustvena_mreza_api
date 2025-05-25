using drustvena_mreza.Models;
using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace drustvena_mreza.Controllers
{
    [Route("api/grupa/{grupaId}/korisnik")]
    [ApiController]
    public class ClanstvaController : Controller
    {
        [HttpGet("members")]
        public ActionResult<List<Korisnik>> GetMembersById(int grupaId)
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

        [HttpGet("nonmembers")]
        public ActionResult<List<Korisnik>> GetNonMembersById(int grupaId)
        {
            if (!GrupaRepository.AllGrupa.ContainsKey(grupaId))
            {
                return NotFound();
            }

            List<Korisnik> allKorisnik = KorisnikRepository.AllKorisnik.Values.ToList();
            List<Korisnik> grupaKorisnik = KorisnikRepository.AllKorisnik.Values.ToList();

            foreach (Korisnik korisnik in allKorisnik)
            {
                foreach (Grupa grupa in korisnik.Grupa)
                {
                    if (grupa.Id == grupaId)
                    {
                        grupaKorisnik.Remove(korisnik);
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
            ClanstvaRepository.SaveClanstva();
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
            ClanstvaRepository.SaveClanstva();

            return NoContent();
        }


    }
}
