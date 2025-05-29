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
        public ActionResult<Korisnik> Create([FromBody] Korisnik inputKorisnik)
        {
            if (string.IsNullOrWhiteSpace(inputKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(inputKorisnik.Ime) || string.IsNullOrWhiteSpace(inputKorisnik.Prezime) || string.IsNullOrWhiteSpace(inputKorisnik.DatumRodjenja.ToString()))
            {
                return BadRequest();
            }

            Korisnik newKorisnik = KorisnikDB.Create(inputKorisnik);

            return Ok(newKorisnik);
        }


        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik updateKorisnik)
        {
            if (string.IsNullOrWhiteSpace(updateKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(updateKorisnik.Ime) || string.IsNullOrWhiteSpace(updateKorisnik.Prezime) || string.IsNullOrWhiteSpace(updateKorisnik.DatumRodjenja.ToString()))
            {
                return BadRequest();
            }

            Korisnik updatedKorisnik = KorisnikDB.Update(id, updateKorisnik);


            return Ok(updatedKorisnik);
        }



        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            KorisnikDB.Delete(id);

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
