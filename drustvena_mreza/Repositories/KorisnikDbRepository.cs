using drustvena_mreza.Models;
using Microsoft.Data.Sqlite;

namespace drustvena_mreza.Repositories
{
    public class KorisnikDbRepository
    {
        private readonly string connectionString;
        public KorisnikDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<Korisnik> GetPaged(int page, int pageSize)
        {
            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(connectionString);
                sqliteConnection.Open();

                string sqliteQuery = "SELECT * FROM Users LIMIT @PageSize OFFSET @Offset";
                using SqliteCommand sqliteCommand = new SqliteCommand(sqliteQuery, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@PageSize", pageSize);
                sqliteCommand.Parameters.AddWithValue("@Offset", pageSize * (page - 1));

                using SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();

                List<Korisnik> AllKorisnik = new List<Korisnik>();

                while (sqliteDataReader.Read())
                {
                    int id = sqliteDataReader.GetInt32(0);
                    string korisnickoIme = sqliteDataReader.GetString(1);
                    string ime = sqliteDataReader.GetString(2);
                    string prezime = sqliteDataReader.GetString(3);
                    string datumRodjenja = sqliteDataReader.GetString(4);

                    Korisnik newKorisnik = new Korisnik(id, korisnickoIme, ime, prezime, datumRodjenja);

                    AllKorisnik.Add(newKorisnik);
                }

                return AllKorisnik;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }

        }


        public Korisnik GetById(int inputId)
        {
            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(connectionString);
                sqliteConnection.Open();

                string sqliteQuery = "SELECT * FROM Users WHERE Id=@Id";
                using SqliteCommand sqliteCommand = new SqliteCommand(sqliteQuery, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@Id", inputId);

                using SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();

                while (sqliteDataReader.Read())
                {
                    int id = sqliteDataReader.GetInt32(0);
                    string korisnickoIme = sqliteDataReader.GetString(1);
                    string ime = sqliteDataReader.GetString(2);
                    string prezime = sqliteDataReader.GetString(3);
                    string datumRodjenja = sqliteDataReader.GetString(4);

                    Korisnik newKorisnik = new Korisnik(id, korisnickoIme, ime, prezime, datumRodjenja);

                    return newKorisnik;
                }            
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
            return null;
        }


        public Korisnik Create(Korisnik korisnik)
        {
            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(connectionString);
                sqliteConnection.Open();

                string sqliteQuery = @"INSERT INTO Users (Username, Name, Surname, Birthday)
                                       VALUES (@KorisnickoIme, @Ime, @Prezime, @DatumRodjenja);
                                       SELECT last_insert_rowid();";
                                        
                using SqliteCommand sqliteCommand = new SqliteCommand(sqliteQuery, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@KorisnickoIme", korisnik.KorisnickoIme);
                sqliteCommand.Parameters.AddWithValue("@Ime", korisnik.Ime);
                sqliteCommand.Parameters.AddWithValue("@Prezime", korisnik.Prezime);
                sqliteCommand.Parameters.AddWithValue("@DatumRodjenja", korisnik.DatumRodjenja);

                int lastId = Convert.ToInt32(sqliteCommand.ExecuteScalar());

                korisnik.Id = lastId;

                return korisnik;

            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }

        }

        public Korisnik Update(int id, Korisnik korisnik)
        {
            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(connectionString);
                sqliteConnection.Open();

                string sqliteQuery = @"UPDATE Users
                                       SET Username=@KorisnickoIme, Name=@Ime, Surname=@Prezime, Birthday=@DatumRodjenja
                                       WHERE Id=@Id;";

                using SqliteCommand sqliteCommand = new SqliteCommand(sqliteQuery, sqliteConnection);

                sqliteCommand.Parameters.AddWithValue("@Id", id);

                sqliteCommand.Parameters.AddWithValue("@KorisnickoIme", korisnik.KorisnickoIme);
                sqliteCommand.Parameters.AddWithValue("@Ime", korisnik.Ime);
                sqliteCommand.Parameters.AddWithValue("@Prezime", korisnik.Prezime);
                sqliteCommand.Parameters.AddWithValue("@DatumRodjenja", korisnik.DatumRodjenja);

                korisnik.Id = id;

                int rowsAffected = sqliteCommand.ExecuteNonQuery();

                return korisnik;

            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }


        public void Delete(int id)
        {
            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(connectionString);
                sqliteConnection.Open();

                string sqliteQuery = @"
                                       DELETE FROM Users
                                       WHERE Id=@Id;";

                using SqliteCommand sqliteCommand = new SqliteCommand(sqliteQuery, sqliteConnection);

                sqliteCommand.Parameters.AddWithValue("@Id", id);

                int rowsAffected = sqliteCommand.ExecuteNonQuery();

                return;

            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }

        public int CountAll()
        {
            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(connectionString);
                sqliteConnection.Open();

                string sqliteQuery = "SELECT COUNT(*) FROM Users";
                using SqliteCommand sqliteCommand = new SqliteCommand(sqliteQuery, sqliteConnection);
                int totalCount = Convert.ToInt32(sqliteCommand.ExecuteScalar());

                return totalCount;

            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }
    }
}
