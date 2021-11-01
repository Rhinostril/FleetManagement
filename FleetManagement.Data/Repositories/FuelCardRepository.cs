using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace FleetManagement.Data.Repositories
{
    public class FuelCardRepository : IFuelCardRepository
    {

        private string connectionString = $"Data Source=fleetmanagserver.database.windows.net;Persist Security Info=True;User ID=fleetadmin;Password=$qlpassw0rd";
        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }


        // SELECT

        

        public FuelCard GetFuelCard(int fuelCardId)
        {
            SqlConnection connection = getConnection();

            string query = "SELECT * FROM FuelCard WHERE fuelCardId=@fuelCardId";

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                SqlParameter paramId = new SqlParameter();
                paramId.ParameterName = "@fuelCardId";
                paramId.DbType = DbType.Int32;
                paramId.Value = fuelCardId;
                command.Parameters.Add(paramId);
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string cardNumber = (string)reader["cardNumber"];
                    DateTime validityDate = (DateTime)reader["validityDate"];
                    string pin = (string)reader["pin"];
                    FuelType fuelType = new FuelType((string)reader["fuelType"]);
                    bool isEnabled = (bool)reader["isEnabled"];
                    FuelCard fuelCard = new FuelCard(fuelCardId, cardNumber, validityDate, pin, fuelType, isEnabled);
                    reader.Close();
                    return fuelCard;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }    
        public IReadOnlyList<FuelCard> GetAllFuelCards()
        {
            SqlConnection connection = getConnection();
            
            List<FuelCard> fuelCards = new List<FuelCard>();

            string query = "SELECT * FROM Fuelcard";

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int fuelCardId = (int)reader["fuelCardId"];
                        string cardNumber = (string)reader["cardNumber"];
                        DateTime validityDate = (DateTime)reader["validityDate"];
                        string pin = (string)reader["pin"];
                        FuelType fuelType = new FuelType((string)reader["fuelType"]);
                        bool isEnabled = (bool)reader["isEnabled"];
                        fuelCards.Add(new FuelCard(fuelCardId, cardNumber, validityDate, pin, fuelType, isEnabled));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return fuelCards.AsReadOnly();
        }

        public FuelCard SearchFuelCard(int? fuelCardId, string cardNr, FuelType fuelType)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<FuelCard> SearchFuelCards(int? fuelCardId, string cardNr, FuelType fuelType)
        {
            SqlConnection connection = getConnection();

            List<FuelCard> fuelCards = new List<FuelCard>();

            string query = "SELECT * FROM FuelCard WHERE fuelCardId=@fuelCardId AND cardNumber=@cardNumber AND fuelType=@fuelType";

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@fuelCardId", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@cardNumber", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@fuelType", SqlDbType.NVarChar));

                    command.Parameters["@fuelCardId"].Value = fuelCardId;
                    command.Parameters["@cardNumber"].Value = cardNr;
                    command.Parameters["@fuelType"].Value = fuelType;
                    
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DateTime validityDate = (DateTime)reader["validityDate"];
                        string pin = (string)reader["pin"];
                        bool isEnabled = (bool)reader["isEnabled"];
                        fuelCards.Add(new FuelCard(cardNr, validityDate, pin, fuelType, isEnabled));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return fuelCards.AsReadOnly();
        }

        // INSERT UPDATE DELETE
        public void AddFuelCard(FuelCard fuelCard)
        {
            SqlConnection connection = getConnection();

            string query = "INSERT INTO Fuelcard (cardNumber, validityDate, pin, fuelType, isEnabled)" +
                           "VALUES (@cardNumber, @validityDate, @pin, @fuelType, @isEnabled)";

            using(SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    //command.Parameters.Add(new SqlParameter("@fuelCardId", SqlDbType.Int));       zonder id
                    command.Parameters.Add(new SqlParameter("@cardNumber", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@validityDate", SqlDbType.DateTime));
                    command.Parameters.Add(new SqlParameter("@pin", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@fuelType", SqlDbType.NVarChar));
                    //command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));         zonder driverId
                    command.Parameters.Add(new SqlParameter("@isEnabled", SqlDbType.Bit));

                    //command.Parameters["@fuelCardId"].Value = fuelCard.FuelCardId;                zonder id
                    command.Parameters["@cardNumber"].Value = fuelCard.CardNumber;
                    command.Parameters["@validityDate"].Value = fuelCard.ValidityDate;
                    command.Parameters["@pin"].Value = fuelCard.Pin;
                    command.Parameters["@fuelType"].Value = fuelCard.FuelType.FuelName;
                    //command.Parameters["@driverId"].Value = fuelCard.Driver.DriverID;             zonder driverId
                    command.Parameters["@isEnabled"].Value = fuelCard.IsEnabled;

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                    
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void UpdateFuelCard(FuelCard fuelCard)
        {
            SqlConnection connection = getConnection();

            string query = "UPDATE Fuelcard" +
                           "SET cardNumber=@cardNumber, validityDate=@validityDate, pin=@pin, fuelType=@fuelType, driverId=@driverId, isEnabled=@isEnabled" +
                           "WHERE fuelCardId=@fuelCardId";

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@cardNumber", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@validityDate", SqlDbType.DateTime));
                    command.Parameters.Add(new SqlParameter("@pin", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@fuelType", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@isEnabled", SqlDbType.Bit));

                    command.Parameters["@cardNumber"].Value = fuelCard.CardNumber;
                    command.Parameters["@validityDate"].Value = fuelCard.ValidityDate;
                    command.Parameters["@pin"].Value = fuelCard.Pin;
                    command.Parameters["@fuelType"].Value = fuelCard.FuelType;
                    command.Parameters["@driverId"].Value = fuelCard.Driver.DriverID;
                    command.Parameters["@isEnabled"].Value = fuelCard.IsEnabled;

                    command.CommandText = query;
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void DeleteFuelCard(FuelCard fuelCard)
        {
            SqlConnection connection = getConnection();

            string query = "DELETE FROM Fuelcard WHERE fuelCardId=@fuelCardId";
            
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@fuelCardId", SqlDbType.Int));
                    command.Parameters["@fuelCardId"].Value = fuelCard.FuelCardId;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        // EXISTS
        public bool FuelCardExists(FuelCard fuelCard)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM FuelCard WHERE fuelCardId=@fuelCardId OR cardNumber=@cardNumber";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@fuelCardId", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@cardNumber", SqlDbType.NVarChar));

                    command.Parameters["@fuelCardId"].Value = fuelCard.FuelCardId;
                    command.Parameters["@cardNumber"].Value = fuelCard.CardNumber;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }



        //END OF REPOSITORY
    }
}
