using drustvena_mreza.Models;
using Microsoft.Data.Sqlite;

namespace drustvena_mreza.Repositories
{
    public class GrupaDBRepository
    {
        public List<Grupa> GetAll()
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

        public Grupa GetById(int inputId)
        {
            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=data/database.db");
                sqliteConnection.Open();

                string sqliteQuery = "SELECT * FROM Groups WHERE Id=@Id";
                using SqliteCommand sqliteCommand = new SqliteCommand(sqliteQuery, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@Id", inputId);

                using SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();

                while (sqliteDataReader.Read())
                {
                    int id = sqliteDataReader.GetInt32(0);
                    string ime = sqliteDataReader.GetString(1);
                    string datumOsnivanja = sqliteDataReader.GetString(2);

                    Grupa newGrupa = new Grupa(id, ime, datumOsnivanja);

                    return newGrupa;
                }
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


        public Grupa Create(Grupa grupaInput)
        {
            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=data/database.db");
                sqliteConnection.Open();

                string sqliteQuery = @"INSERT INTO Groups (Name, CreationDate)
                                       VALUES (@Name, @CreationDate);
                                       SELECT last_insert_rowid();";

                using SqliteCommand sqliteCommand = new SqliteCommand(sqliteQuery, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@Name", grupaInput.Ime);
                sqliteCommand.Parameters.AddWithValue("@CreationDate", grupaInput.DatumOsnivanja);

                int lastId = Convert.ToInt32(sqliteCommand.ExecuteScalar());

                grupaInput.Id = lastId;

                return grupaInput;

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

        public Grupa Update(int id, Grupa grupa)
        {
            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=data/database.db");
                sqliteConnection.Open();

                string sqliteQuery = @"UPDATE Groups
                                       SET Name=@Ime, CreationDate=@DatumOsnivanja
                                       WHERE Id=@Id;";

                using SqliteCommand sqliteCommand = new SqliteCommand(sqliteQuery, sqliteConnection);

                sqliteCommand.Parameters.AddWithValue("@Id", id);

                sqliteCommand.Parameters.AddWithValue("@Ime", grupa.Ime);
                sqliteCommand.Parameters.AddWithValue("@DatumOsnivanja", grupa.DatumOsnivanja);

                grupa.Id = id;

                int rowsAffected = sqliteCommand.ExecuteNonQuery();

                return grupa;

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


        public void Delete(int id)
        {
            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=data/database.db");
                sqliteConnection.Open();

                string sqliteQuery = @"
                                       DELETE FROM Groups
                                       WHERE Id=@Id;";

                using SqliteCommand sqliteCommand = new SqliteCommand(sqliteQuery, sqliteConnection);

                sqliteCommand.Parameters.AddWithValue("@Id", id);

                int rowsAffected = sqliteCommand.ExecuteNonQuery();

                return;

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

            return;
        }
    }
}
