using drustvena_mreza.Models;
using drustvena_mreza.Repositories;
using drustvena_mreza.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace drustvena_mreza.Controllers
{
    [Route("api/grupa")]
    [ApiController]
    public class GrupaController : ControllerBase
    {

        private GrupaRepository grupaRepository = new GrupaRepository();


        [HttpGet]
        public ActionResult<List<GrupaDTO>> GetAll()   //U Grupa.cs sam napravio klasu (GrupaDTO) da mogu konvertovati vrijeme u string i prikazati u dobrom formatu
        {
            List<GrupaDTO> grupeList = GrupaRepository.AllGrupa.Values.Select(grupa => new GrupaDTO (grupa)).ToList();
            return grupeList;
        }

        [HttpPost]
        public ActionResult<Grupa> Create([FromBody] Grupa newGrupa)
        {
            if (string.IsNullOrWhiteSpace(newGrupa.Ime) || string.IsNullOrWhiteSpace(newGrupa.DatumOsnivanja.ToString()))
            {
                return BadRequest();
            }
            if (GrupaRepository.AllGrupa.ContainsKey(newGrupa.Id))
            {
                return BadRequest();
            }
            GrupaRepository.AllGrupa[newGrupa.Id] = newGrupa;
            grupaRepository.SaveGrupa();
            return Ok(newGrupa);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!GrupaRepository.AllGrupa.ContainsKey(id))
            {
                return NotFound();
            }

            GrupaRepository.AllGrupa.Remove(id);
            grupaRepository.SaveGrupa();
            return NoContent();
        }


    }
}
