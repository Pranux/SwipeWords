using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class DatabaseContext
    {
        private string ConnectionString { get; set; }

        public DatabaseContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public int CalculateOffset(string command, MySqlConnection conn, double exclusionPercentage)
        {
            MySqlCommand countCmd = new MySqlCommand(command, conn);
            var totalWords = Convert.ToInt32(countCmd.ExecuteScalar());
            var offset = (int)(totalWords * exclusionPercentage);
            return offset;
        }

        public List<string> AddWordsToList(string query, MySqlConnection conn, int offset, int amount)
        {
            List<string> selectedWords = new List<string>();

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@offset", offset);
            cmd.Parameters.AddWithValue("@amount", amount);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    selectedWords.Add(reader.GetString("word"));
                }
            }
            return selectedWords;
        }
        
        public List<string> GetSelectedCorrectWords(int amount = 5, double exclusionPercentage = 0.30)
        {
            List<string> selectedWords = new List<string>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                
                // Query to select words excluding the top percentage by frequency
                var query = @"
                    SELECT word 
                    FROM CorrectWords
                    WHERE frequency < (
                        SELECT MAX(frequency) 
                        FROM (
                            SELECT frequency
                            FROM CorrectWords
                            ORDER BY frequency DESC
                            LIMIT @offset
                        ) AS topPercent
                    )
                    ORDER BY RAND()
                    LIMIT @amount";

                var offset = CalculateOffset("SELECT COUNT(*) FROM CorrectWords", conn, exclusionPercentage);

                selectedWords = AddWordsToList(query, conn, offset, amount);
            }
            return selectedWords;
        }
        
        public List<string> GetSelectedIncorrectWords(int amount = 5, double exclusionPercentage = 0.30)
        {
            List<string> selectedWords = new List<string>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                
                // Query to select words excluding the top percentage by frequency
                var query = @"
                    SELECT word 
                    FROM IncorrectWords
                    WHERE frequency < (
                        SELECT MAX(frequency) 
                        FROM (
                            SELECT frequency
                            FROM IncorrectWords
                            ORDER BY frequency DESC
                            LIMIT @offset
                        ) AS topPercent
                    )
                    ORDER BY RAND()
                    LIMIT @amount";

                var offset = CalculateOffset("SELECT COUNT(*) FROM IncorrectWords", conn, exclusionPercentage);

                selectedWords = AddWordsToList(query, conn, offset, amount);
            }
            return selectedWords;
        }
    }
}