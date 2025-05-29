using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using drustvena_mreza.Models;
using drustvena_mreza.Utilities;
using Microsoft.Data.Sqlite;

namespace drustvena_mreza.Controllers
{
    [Route("api/korisnik")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        KorisnikDbRepository KorisnikDB { get; set; } = new KorisnikDbRepository();


        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            List<Korisnik> allKorisnik = KorisnikDB.GetAll();
            return allKorisnik;
        }


        [HttpGet("{inputId}")]
        public ActionResult<Korisnik> GetById(int inputId)
        {
            Korisnik korisnik = KorisnikDB.GetById(inputId);

            return Ok(korisnik);
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
            KorisnikRepository.SaveKorisnik();

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

            KorisnikRepository.SaveKorisnik();


            return Ok(korisnik);
        }



        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!KorisnikRepository.AllKorisnik.ContainsKey(id))
            {
                return NotFound();
            }

            KorisnikRepository.AllKorisnik.Remove(id);
            KorisnikRepository.SaveKorisnik();
            return NoContent();
        }


        [HttpPost("{id}/removeClanstvo")]
        public ActionResult<Korisnik> RemoveClanstvo([FromBody] ClanstvoDTO data)
        {
            Korisnik korisnik = KorisnikRepository.AllKorisnik[data.KorisnikId];
            Grupa grupa = GrupaRepository.AllGrupa[data.GrupaId];
            korisnik.Grupa.Remove(grupa);
            ClanstvaRepository.SaveClanstva();

            return Ok(korisnik);
        }

        [HttpPost("{id}/addClanstvo")]
        public ActionResult<Korisnik> AddClanstvo([FromBody] ClanstvoDTO data)
        {
            Korisnik korisnik = KorisnikRepository.AllKorisnik[data.KorisnikId];
            Grupa grupa = GrupaRepository.AllGrupa[data.GrupaId];
            korisnik.Grupa.Add(grupa);
            ClanstvaRepository.SaveClanstva();

            return Ok(korisnik);
        }
    }
}
