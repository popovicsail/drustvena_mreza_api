using drustvena_mreza.Models;
using System.Globalization;
using Microsoft.Data.Sqlite;
using drustvena_mreza.Utilities;

namespace drustvena_mreza.Repositories
{
    public class GroupRepository
    {
        private readonly string connectionString;
        public GroupRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<Group> GetPaged(int page, int pageSize)
        {
            List<Group> allGroup = new List<Group>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Groups LIMIT @PageSize OFFSET @Offset";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@Offset", pageSize * (page - 1));

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string ime = reader.GetString(1);
                    DateTime datumOsnivanja = DateTime.ParseExact(reader["DateOfCreation"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    Group newGrupa = new Group(id, ime, datumOsnivanja);
                    allGroup.Add(newGrupa);
                }

                return allGroup;
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

                string query = "SELECT COUNT(*) FROM Groups";
                using SqliteCommand command = new SqliteCommand(query, connection);

                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception exception)
            {
                AllUtilities.HandleException(exception);
                throw;
            }
        }

        public Group GetById(int id)
        {
            Group group = null;

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"SELECT g.Id AS GroupId, g.Name, g.DateOfCreation, 
                                 u.Id AS UserId, u.Username, u.FirstName AS FirstName, u.LastName AS LastName, u.DateOfBirth
                                 FROM Groups g
                                 LEFT JOIN GroupUsers gu ON g.Id = gu.GroupId
                                 LEFT JOIN Users u ON gu.UserId = u.Id
                                 WHERE GroupId = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (group == null)
                    {
                        int newId = id;
                        string ime = reader["FirstName"].ToString();
                        DateTime datumOsnivanja = DateTime.ParseExact(reader["DateOfCreation"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        group = new Group(newId, ime, datumOsnivanja);
                    }

                    if (reader["UserId"] != DBNull.Value)
                    {
                        int newId = Convert.ToInt32(reader["UserId"]);
                        string username = reader["Username"].ToString();
                        string firstName = reader["FirstName"].ToString();
                        string lastName = reader["LastName"].ToString();
                        DateTime dateOfBirth = DateTime.ParseExact(reader["DateOfBirth"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        User user = new User(newId, username, firstName, lastName, dateOfBirth);

                        group.GroupUsers.Add(user);
                    }
                }

                return group;
            }
            catch (Exception exception)
            {
                AllUtilities.HandleException(exception);
                throw;
            }
        }

        public Group Create(Group group)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO Groups (Name, DateOfCreation) 
                                 VALUES (@Name, @DateOfCreation);
                                 SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Name", group.Name);
                command.Parameters.AddWithValue("@DateOfCreation", group.DateOfCreation.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

                group.Id = Convert.ToInt32(command.ExecuteScalar());

                return group;
            }
            catch (Exception exception)
            {
                AllUtilities.HandleException(exception);
                throw;
            }
        }


        public Group Update(Group group)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"UPDATE Groups 
                                 SET Name = @Name, DateOfCreation = @DateOfCreation
                                 WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Name", group.Name);
                command.Parameters.AddWithValue("@DateOfCreation", group.DateOfCreation.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@Id", group.Id);

                int affectedRows = command.ExecuteNonQuery();
                return affectedRows > 0 ? group : null;
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

                string query = "DELETE FROM Groups WHERE Id = @Id";
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
