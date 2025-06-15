using drustvena_mreza.Models;
using drustvena_mreza.Utilities;
using Microsoft.Data.Sqlite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace drustvena_mreza.Repositories
{
    public class PostRepository
    {
        private readonly string connectionString;
        public PostRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<Post> GetAll()
        {
            List<Post> allPost = new List<Post>();
            try
            {

                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"SELECT p.Id, p.Content, p.DateOfPublishing,
                                 u.Id AS UserId, u.Username, u.FirstName AS FirstName, u.LastName AS LastName, u.DateOfBirth
                                 FROM Posts p
                                 LEFT JOIN Users u ON p.UserId = u.Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();

                Post post = null;

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string content = Convert.ToString(reader["Content"]);
                    DateTime dateOfPublishing = DateTime.ParseExact(reader["DateOfPublishing"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    post = new Post(id, content, dateOfPublishing);

                    allPost.Add(post);

                    if (reader["UserId"] != DBNull.Value)
                    {
                        int userId = Convert.ToInt32(reader["Id"]);
                        string username = reader["Username"].ToString();
                        string firstName = reader["FirstName"].ToString();
                        string lastName = reader["LastName"].ToString();
                        DateTime dateOfBirth = DateTime.ParseExact(reader["DateOfBirth"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        User user = new User(userId, username, firstName, lastName, dateOfBirth);

                        post.Author = user;
                    }
                }
                return allPost;
            }

            catch (Exception exception)
            {
                AllUtilities.HandleException(exception);
                throw;
            }
        }

        public Post Create(Post post)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO Posts (UserId, Content, DateOfPublishing) 
                                 VALUES (@UserId, @Content, @DateOfPublishing);
                                 SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", post.Author.Id);
                command.Parameters.AddWithValue("@Content", post.Content);
                command.Parameters.AddWithValue("@Date", post.DateOfPublishing.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

                post.Id = Convert.ToInt32(command.ExecuteScalar());

                return post;
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

                string query = "DELETE FROM Posts WHERE Id = @Id";
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
