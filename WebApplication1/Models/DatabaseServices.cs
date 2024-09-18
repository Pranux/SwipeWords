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
        
        public List<string> GetSelectedCorrectWords(int amount = 5, double exclusionPercentage = 0.30)
        {
            List<string> selectedWords = new List<string>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                
                // Query to select words excluding the top percentage by frequency
                string query = @"
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

                // Get the total count of words
                MySqlCommand countCmd = new MySqlCommand("SELECT COUNT(*) FROM CorrectWords", conn);
                int totalWords = Convert.ToInt32(countCmd.ExecuteScalar());
                int offset = (int)(totalWords * exclusionPercentage);

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
                string query = @"
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

                // Get the total count of words
                MySqlCommand countCmd = new MySqlCommand("SELECT COUNT(*) FROM IncorrectWords", conn);
                int totalWords = Convert.ToInt32(countCmd.ExecuteScalar());
                int offset = (int)(totalWords * exclusionPercentage);

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
            }
            return selectedWords;
        }
    }
}