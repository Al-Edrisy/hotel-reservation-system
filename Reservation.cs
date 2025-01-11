using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Hotel_System
{
    class Reservation
    {
        DBConnection conn = new DBConnection();

        // Get all reservations
        public DataTable GetAllReservations()
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM reservations", conn.GetConnection());
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();

            try
            {
                adapter.SelectCommand = command;
                adapter.Fill(table);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error fetching reservations: " + ex.Message);
            }

            return table;
        }

        // Make a new reservation
        public bool MakeReservation(int number, int client, DateTime dateIn, DateTime dateOut)
        {
            MySqlCommand command = new MySqlCommand();
            string queryInsert = "INSERT INTO `reservations`(`room_number`, `client_id`, `date_in`, `date_out`) VALUES (@number, @client, @dateIn, @dateOut)";
            command.CommandText = queryInsert;
            command.Connection = conn.GetConnection();

            command.Parameters.Add("@number", MySqlDbType.Int32).Value = number;
            command.Parameters.Add("@client", MySqlDbType.Int32).Value = client;
            command.Parameters.Add("@dateIn", MySqlDbType.Date).Value = dateIn;
            command.Parameters.Add("@dateOut", MySqlDbType.Date).Value = dateOut;

            try
            {
                conn.OpenConnection();
                if (command.ExecuteNonQuery() == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error making reservation: " + ex.Message);
                return false;
            }
            finally
            {
                conn.CloseConnection();
            }
        }

        // Edit an existing reservation
        public bool EditReservation(int id, int number, int client, DateTime dateIn, DateTime dateOut)
        {
            MySqlCommand command = new MySqlCommand();
            string queryUpdate = "UPDATE `reservations` SET `room_number`=@number, `client_id`=@client, `date_in`=@dateIn, `date_out`=@dateOut WHERE id=@id";
            command.CommandText = queryUpdate;
            command.Connection = conn.GetConnection();

            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@number", MySqlDbType.Int32).Value = number;
            command.Parameters.Add("@client", MySqlDbType.Int32).Value = client;
            command.Parameters.Add("@dateIn", MySqlDbType.Date).Value = dateIn;
            command.Parameters.Add("@dateOut", MySqlDbType.Date).Value = dateOut;

            try
            {
                // Debugging: print the query
                Console.WriteLine("Executing SQL: " + queryUpdate);
                conn.OpenConnection();
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    Console.WriteLine("Reservation updated successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine("No rows were updated. Make sure the 'id' exists in the database.");
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error editing reservation: " + ex.Message);
                return false;
            }
            finally
            {
                conn.CloseConnection();
            }
        }

        // Remove a reservation
        public bool RemoveReservation(int id)
        {
            MySqlCommand command = new MySqlCommand();
            string queryDelete = "DELETE FROM reservations WHERE id=@id";
            command.CommandText = queryDelete;
            command.Connection = conn.GetConnection();

            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            try
            {
                conn.OpenConnection();
                if (command.ExecuteNonQuery() == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error removing reservation: " + ex.Message);
                return false;
            }
            finally
            {
                conn.CloseConnection();
            }
        }
    }
}
