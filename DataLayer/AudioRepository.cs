using MySql.Data.MySqlClient;

namespace AudioProj.DataLayer
{
    public class AudioRepository
    {
        private readonly string connectionString = "Server=localhost;Database=AudioDB;User=root;Password=Pa$$word;port=3306;";
        public void SaveToDatabase(byte[] audioData)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "INSERT INTO audiotable (AudioData, RecordedAt) VALUES (@AudioData, @RecordedAt)";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AudioData", audioData);
            command.Parameters.AddWithValue("@RecordedAt", DateTime.Now);
            command.ExecuteNonQuery();
        }

        public byte[] LoadLatestAudioFromDatabase()
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            // ✅ FIXED: MySQL does not support "TOP 1" — use "LIMIT 1"
            string sql = "SELECT AudioData FROM audiotable ORDER BY RecordedAt DESC LIMIT 1";
            using var command = new MySqlCommand(sql, connection);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return (byte[])reader["AudioData"];
            }

            throw new Exception("No audio found in the database.");
        }

        public List<(int Id, DateTime RecordedAt)> GetAllAudioEntries()
        {
            var entries = new List<(int, DateTime)>();

            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "SELECT Id, RecordedAt FROM audiotable ORDER BY RecordedAt DESC";
            using var command = new MySqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32("Id");
                DateTime recordedAt = reader.GetDateTime("RecordedAt");
                entries.Add((id, recordedAt));
            }

            return entries;
        }

        public byte[] LoadAudioById(int id)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "SELECT AudioData FROM audiotable WHERE Id = @Id";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return (byte[])reader["AudioData"];
            }

            throw new Exception($"Audio with Id={id} not found.");
        }


    }
}