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

        [HttpGet]
        public ActionResult<List<Grupa>> GetAll()   //U Grupa.cs sam napravio klasu (GrupaDTO) da mogu konvertovati vrijeme u string i prikazati u dobrom formatu
        {
            List<Grupa> grupeList = GetAllGrupaFromDatabase();
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
            GrupaRepository.SaveGrupa();
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
            GrupaRepository.SaveGrupa();
            return NoContent();
        }

        private List<Grupa> GetAllGrupaFromDatabase()
        {
            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=data/database.db");
                sqliteConnection.Open();

                string sqliteQuery = "SELECT * FROM Groups";
                using SqliteCommand sqliteCommand = new SqliteCommand(sqliteQuery, sqliteConnection);

                using SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();

                List<Grupa> AllGrupa = new List<Grupa>();

                while (sqliteDataReader.Read())
                {
                    int grupaId = sqliteDataReader.GetInt32(0);
                    string grupaIme = sqliteDataReader.GetString(1);
                    string grupaDatumOsnivanja = sqliteDataReader.GetString(2);

                    Grupa newGrupa = new Grupa(grupaId, grupaIme, grupaDatumOsnivanja);

                    AllGrupa.Add(newGrupa);
                }

                return AllGrupa;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

            return null;
        }

    }
}
