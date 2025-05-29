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
        private readonly GrupaDBRepository grupaDB;

        public GrupaController(GrupaDBRepository grupaDBRepository)
        {
            grupaDB = grupaDBRepository;
        }

        [HttpGet]
        public ActionResult GetPaged([FromQuery] int page, [FromQuery] int pageSize)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and PageSize must be higher than 0");
            }
            try
            {
                List<Grupa> allGrupa = grupaDB.GetPaged(page, pageSize);

                int totalCount = grupaDB.CountAll();

                Object result = new
                {
                    Data = allGrupa,
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
            Grupa grupa = grupaDB.GetById(inputId);

            return Ok(grupa);
        }


        [HttpPost]
        public ActionResult<Grupa> Create([FromBody] Grupa newGrupa)
        {
            if (string.IsNullOrWhiteSpace(newGrupa.Ime) || string.IsNullOrWhiteSpace(newGrupa.DatumOsnivanja.ToString()))
            {
                return BadRequest();
            }

            Grupa grupa = grupaDB.Create(newGrupa);

            return Ok(newGrupa);
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            grupaDB.Delete(id);

            return NoContent();
        }
    }
}
