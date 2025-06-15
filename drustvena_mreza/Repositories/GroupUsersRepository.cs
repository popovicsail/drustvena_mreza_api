using drustvena_mreza.Utilities;
using Microsoft.Data.Sqlite;

namespace drustvena_mreza.Repositories
{
    public class GroupUsersRepository
    {
        private readonly string connectionString;
        public GroupUsersRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public void Add(int groupId, int userId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO GroupUsers (GroupId, UserId) 
                                 VALUES (@GroupId, @UserId);";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@GroupId", groupId);
                command.Parameters.AddWithValue("@UserId", userId);

                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                AllUtilities.HandleException(exception);
                throw;
            }
        }

        public void Remove(int groupId, int userId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"DELETE FROM GroupUsers 
                                 WHERE GroupId = @GroupId AND UserId = @UserId;";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@GroupId", groupId);
                command.Parameters.AddWithValue("@UserId", userId);

                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                AllUtilities.HandleException(exception);
                throw;
            }
        }
    }
}
