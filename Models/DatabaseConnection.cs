using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace empoweraidgroup.Models
{
    public class DatabaseConnection
    {
        private readonly string _connectionString = "Server=localhost;Database=empoweraid_group2;User ID=root;Password=jedy12345;";

        public MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(_connectionString);
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Database connection error: " + ex.Message);
            }
            return conn;
        }

        public void Disconnect(MySqlConnection conn)
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}
