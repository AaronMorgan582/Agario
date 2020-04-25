using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Database
{
    public class AgarioDatabase
    {

        /// <summary>
        /// The information necessary for the program to connect to the Database
        /// </summary>
        public readonly string connection_string;

        /// <summary>
        /// Upon construction of this static class, build the connection string
        /// </summary>
        public AgarioDatabase()
        {
            var builder = new ConfigurationBuilder();

            builder.AddUserSecrets<AgarioDatabase>();
            IConfigurationRoot Configuration = builder.Build();
            var SelectedSecrets = Configuration.GetSection("AgarioDBSecrets");

            connection_string = new SqlConnectionStringBuilder()
            {
                DataSource = SelectedSecrets["ServerURL"],
                InitialCatalog = SelectedSecrets["DBName"],
                UserID = SelectedSecrets["UserName"],
                Password = SelectedSecrets["DBPassword"],
                ConnectTimeout = 15
            }.ConnectionString;
        }


        /// <summary>
        ///  Test several connections and print the output to the console
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
        }
        public DataSet Get_HighScores()
        {
            return Build_DataSet(connection_string, "HighScores", "LargestMass");
        }

        /// <summary>
        /// Getter for the TimeInFirst Data
        /// </summary>
        /// <returns></returns>
        public DataSet Get_First_Times()
        {
            return Build_DataSet(connection_string, "TimeInFirst", "TotalTime");
        }

        public DataSet Get_Player_Data(string player_name)
        {
            return Build_DataSet(connection_string, $"{player_name}Data", "Mass");
        }

        public DataSet Insert_HighScore_Data(string player_name, float mass, int rank, string time_alive)
        {
            string table_name = player_name + "Data";
            DataSet my_data_set = new DataSet();
            Console.WriteLine(time_alive);
            try
            {
                using (SqlConnection con = new SqlConnection(connection_string))
                {
                    con.Open();
                    string sql_command = @$"IF NOT EXISTS (SELECT*FROM HighScores WHERE PlayerName = '{player_name}')
                                                BEGIN
                                                INSERT INTO HighScores (PlayerName, LargestMass, LongestTimeAlive, HighestRank)
                                                VALUES ('{player_name}', {mass}, '{time_alive}', {rank})
                                                END
                                            ELSE
                                                BEGIN
                                                DECLARE @HighestRecordedScore float;
                                                SELECT @HighestRecordedScore = Max(Mass) FROM {table_name}
                                                UPDATE HighScores SET LargestMass = @HighestRecordedScore WHERE PlayerName = '{player_name}'
                                                UPDATE HighScores SET LongestTimeAlive = '{time_alive}' WHERE PlayerName = '{player_name}'
                                                UPDATE HighScores SET HighestRank = {rank} WHERE PlayerName = '{player_name}'
                                                END";
                    SqlDataAdapter my_sql_data_adapter = new SqlDataAdapter(sql_command, con);

                    my_sql_data_adapter.Fill(my_data_set, $"{table_name}");
                }
            }
            catch (SqlException exception)
            {
                Console.WriteLine($"Error in SQL connection:\n   - {exception.Message}");
            }

            return my_data_set;
        }


        public DataSet Insert_Player_Data(string player_name, float mass, int rank, string time_alive)
        {
            string table_name = player_name + "Data";
            DataSet my_data_set = new DataSet();
            Console.WriteLine(time_alive);
            try
            {
                using (SqlConnection con = new SqlConnection(connection_string))
                {
                    con.Open();
                    string sql_command = @$"IF OBJECT_ID('{table_name}') IS NULL
                                                BEGIN
                                                CREATE TABLE {table_name} (
                                                PlayerName varchar(50) NOT NULL,
                                                Mass float NOT NULL,
                                                Rank int NOT NULL,
                                                TimePlayed varchar(50) NOT NULL,
	                                            GameSession int identity(1,1) NOT NULL
                                                )   
                                                INSERT INTO {table_name} (PlayerName, Mass, Rank, TimePlayed)
                                                VALUES
                                                ('{player_name}', {mass}, {rank}, '{time_alive}')
                                                END
                                            ELSE
                                                BEGIN
                                                INSERT INTO {table_name} (PlayerName, Mass, Rank, TimePlayed)
                                                VALUES
                                                ('{player_name}', {mass}, {rank}, '{time_alive}')
                                                END;
                                                ";
                    SqlDataAdapter my_sql_data_adapter = new SqlDataAdapter(sql_command, con);

                    my_sql_data_adapter.Fill(my_data_set, $"{table_name}");
                }
            }
            catch (SqlException exception)
            {
                Console.WriteLine($"Error in SQL connection:\n   - {exception.Message}");
            }

            return my_data_set;
        }

        private static DataSet Build_DataSet(string connection_string, string table_name, string column_name)
        {
            DataSet my_data_set = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connection_string))
                {
                    con.Open();

                    string sql_command = $"SELECT * FROM {table_name} ORDER BY {column_name} DESC";
                    SqlDataAdapter my_sql_data_adapter = new SqlDataAdapter(sql_command, con);

                    my_sql_data_adapter.Fill(my_data_set, $"{table_name}");
                }
            }
            catch (SqlException exception)
            {
                Console.WriteLine($"Error in SQL connection:\n   - {exception.Message}");
            }
            return my_data_set;
        }
    }

}