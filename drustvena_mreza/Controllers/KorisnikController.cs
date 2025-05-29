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
        private readonly KorisnikDbRepository korisnikDB;

        public KorisnikController(KorisnikDbRepository korisnikDbRepository)
        {
            korisnikDB = korisnikDbRepository;
        }


        [HttpGet]
        public ActionResult GetPaged ([FromQuery] int page, [FromQuery] int pageSize)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and PageSize must be higher than 0");
            }
            try
            {
                List<Korisnik> allKorisnik = korisnikDB.GetPaged(page, pageSize);

                int totalCount = korisnikDB.CountAll();
                Object result = new
                {
                    Data = allKorisnik,
                    TotalCount = totalCount
                };
                return Ok(result);
            }
            catch
            {
                return Problem("ERROR: An error occured.");
            }
        }


        [HttpGet("{inputId}")]
        public ActionResult<Korisnik> GetById(int inputId)
        {
            Korisnik korisnik = korisnikDB.GetById(inputId);

            return Ok(korisnik);
        }


        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik inputKorisnik)
        {
            if (string.IsNullOrWhiteSpace(inputKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(inputKorisnik.Ime) || string.IsNullOrWhiteSpace(inputKorisnik.Prezime) || string.IsNullOrWhiteSpace(inputKorisnik.DatumRodjenja.ToString()))
            {
                return BadRequest();
            }

            Korisnik newKorisnik = korisnikDB.Create(inputKorisnik);

            return Ok(newKorisnik);
        }


        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik updateKorisnik)
        {
            if (string.IsNullOrWhiteSpace(updateKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(updateKorisnik.Ime) || string.IsNullOrWhiteSpace(updateKorisnik.Prezime) || string.IsNullOrWhiteSpace(updateKorisnik.DatumRodjenja.ToString()))
            {
                return BadRequest();
            }

            Korisnik updatedKorisnik = korisnikDB.Update(id, updateKorisnik);


            return Ok(updatedKorisnik);
        }



        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            korisnikDB.Delete(id);

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
