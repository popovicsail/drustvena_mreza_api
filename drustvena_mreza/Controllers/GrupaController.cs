using drustvena_mreza.Models;
using drustvena_mreza.Repositories;
using drustvena_mreza.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace drustvena_mreza.Controllers
{
    [Route("api/grupa")]
    [ApiController]
    public class GrupaController : ControllerBase
    {
        GrupaDBRepository GrupaDB { get; set; } = new GrupaDBRepository();

        [HttpGet]
        public ActionResult<List<Grupa>> GetAll()
        {
            List<Grupa> grupeList = GrupaDB.GetAll();
            return grupeList;
        }


        [HttpGet("{inputId}")]
        public ActionResult<Korisnik> GetById(int inputId)
        {
            Grupa grupa = GrupaDB.GetById(inputId);

            return Ok(grupa);
        }


        [HttpPost]
        public ActionResult<Grupa> Create([FromBody] Grupa newGrupa)
        {
            if (string.IsNullOrWhiteSpace(newGrupa.Ime) || string.IsNullOrWhiteSpace(newGrupa.DatumOsnivanja.ToString()))
            {
                return BadRequest();
            }

            Grupa grupa = GrupaDB.Create(newGrupa);

            return Ok(newGrupa);
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            GrupaDB.Delete(id);

            return NoContent();
        }
    }
}
