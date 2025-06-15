using drustvena_mreza.Models;
using System.Globalization;
using Microsoft.Data.Sqlite;
using drustvena_mreza.Utilities;

namespace drustvena_mreza.Repositories
{
    public class UserRepository
    {
        private readonly string connectionString;
        public UserRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<User> GetPaged(int page, int pageSize)
        {
            List<User> allUser = new List<User>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Users LIMIT @PageSize OFFSET @Offset";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@Offset", pageSize * (page - 1));

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string username = reader["Username"].ToString();
                    string firstName = reader["FirstName"].ToString();
                    string lastName = reader["LastName"].ToString();
                    DateTime dateOfBirth = DateTime.ParseExact(reader["DateOfBirth"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    User user = new User(id, username, firstName, lastName, dateOfBirth);

                    allUser.Add(user);

                }
                return allUser;
            }

            catch (Exception exception)
            {
                AllUtilities.HandleException(exception);
                throw;
            }
        }

        public List<User> GetAll()
        {
            List<User> allUser = new List<User>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Users";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string username = reader["Username"].ToString();
                    string firstName = reader["FirstName"].ToString();
                    string lastName = reader["LastName"].ToString();
                    DateTime dateOfBirth = DateTime.ParseExact(reader["DateOfBirth"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    User user = new User(id, username, firstName, lastName, dateOfBirth);

                    allUser.Add(user);

                }
                return allUser;
            }

            catch (Exception exception)
            {
                AllUtilities.HandleException(exception);
                throw;
            }
        }

        public int CountAll()
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT COUNT(*) FROM Users";
                using SqliteCommand command = new SqliteCommand(query, connection);

                return Convert.ToInt32(command.ExecuteScalar());
            }

            catch (Exception exception)
            {
                AllUtilities.HandleException(exception);
                throw;
            }
        }

        public User GetById(int id)
        {
            User user = null;

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"SELECT Id, Username, FirstName, LastName, DateOfBirth
                             FROM Users
                             WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using SqliteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int newId = id;
                    string username = reader["Username"].ToString();
                    string firstName = reader["FirstName"].ToString();
                    string lastName = reader["LastName"].ToString();
                    DateTime dateOfBirth = DateTime.ParseExact(reader["DateOfBirth"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    user = new User(id, username, firstName, lastName, dateOfBirth);

                }

                return user;
            }

            catch (Exception exception)
            {
                AllUtilities.HandleException(exception);
                throw;
            }
        }


        public User Create(User user)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO Users (Username, FirstName, LastName, DateOfBirth) 
                             VALUES (@Username, @FirstName, @LastName, @DateOfBirth);
                             SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

                user.Id = Convert.ToInt32(command.ExecuteScalar());

                return user;
            }

            catch (Exception exception)
            {
                AllUtilities.HandleException(exception);
                throw;
            }
        }

        public User Update(User user)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"UPDATE Users 
                             SET Username = @Username, FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth
                             WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@Id", user.Id);


                int affectedRows = command.ExecuteNonQuery();
                return affectedRows > 0 ? user : null;
            }

            catch (Exception exception)
            {
                AllUtilities.HandleException(exception);
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Users WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }

            catch (Exception exception)
            {
                AllUtilities.HandleException(exception);
                throw;
            }
        }
    }

}
