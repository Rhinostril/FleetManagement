using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace FleetManagement.Data.Repositories
{
    public class FuelCardRepository : IFuelCardRepository
    {
        
       private string connectionString = $"Data Source=tcp:fleetmanagserver.database.windows.net,1433;Initial Catalog=dboFleetmanagement;Persist Security Info=False;User ID=fleetadmin;Password=$qlpassw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

       private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        // SELECT
        public FuelCard GetFuelCard(int fuelCardId)
        {
            SqlConnection connection = getConnection();

            string query = "SELECT * FROM FuelType AS t1 JOIN FuelCardFuelType AS t2 ON t1.fuelTypeId=t2.fuelTypeId " +
                           "JOIN FuelCard AS t3 ON t2.fuelCardId=t3.fuelCardId LEFT JOIN Driver AS t4 on t3.driverId=t4.driverId " +
                           "WHERE t3.fuelCardId=@fuelCardId";

            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();

                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@fuelCardId", SqlDbType.Int));
                    command.Parameters["@fuelCardId"].Value = fuelCardId;

                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    string cardNumber = (string)reader["cardNumber"];
                    DateTime validityDate = (DateTime)reader["validityDate"];
                    string pin = (string)reader["pin"];
                    bool isEnabled = (bool)reader["isEnabled"];

                    List<FuelType> fuelTypes = new List<FuelType>();
                    string fuelName = (string)reader["name"];
                    int fuelTypeId = (int)reader["fuelTypeId"];
                    fuelTypes.Add(new FuelType(fuelTypeId, fuelName));

                    FuelCard fuelCard = new FuelCard(fuelCardId, cardNumber, validityDate, pin, fuelTypes, isEnabled);

                    if (!reader.IsDBNull(8))
                    {
                        int driverid = (int)reader["driverId"];
                        string firstName = (string)reader["firstName"];
                        string lastName = (string)reader["lastName"];
                        DateTime date = (DateTime)reader["dateOfBirth"];
                        string secnr = (string)reader["securityNumber"];
                        Driver driver = new Driver(firstName, lastName, date, secnr);
                        driver.SetDriverID(driverid);
                        fuelCard.SetDriver(driver);
                    }

                    while (reader.Read())
                    {
                        fuelName = (string)reader["name"];
                        fuelTypeId = (int)reader["fuelTypeId"];
                        fuelTypes.Add(new FuelType(fuelTypeId, fuelName));
                        fuelCard.SetFuelTypes(fuelTypes);
                    }

                    reader.Close();
                    return fuelCard;
                }
                catch (Exception ex)
                {

                    Console.WriteLine();
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

            string query = "SELECT * FROM Fuelcard";

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    List<FuelCard> fuelCards = new List<FuelCard>();
                    while (reader.Read())
                    {
                        int fuelCardId = (int)reader["fuelCardId"];
                        string cardNumber = (string)reader["cardNumber"];
                        DateTime validityDate = (DateTime)reader["validityDate"];
                        string pin = (string)reader["pin"];
                        bool isEnabled = (bool)reader["isEnabled"];
                        fuelCards.Add(new FuelCard(fuelCardId, cardNumber, validityDate, pin, isEnabled));
                    }
                    return fuelCards.AsReadOnly();
                }
                catch (Exception ex) {
                    Console.WriteLine();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IReadOnlyList<FuelCard> GetTop50FuelCards() {
            SqlConnection connection = getConnection();
            List<FuelCard> fuelCards = new List<FuelCard>();
            string query = "SELECT TOP (50) Fuelcard.fuelCardId, Fuelcard.cardNumber, Fuelcard.validityDate, Fuelcard.pin, Fuelcard.driverId, Fuelcard.isEnabled, Driver.firstName, Driver.lastName FROM [Fuelcard] LEFT JOIN [Driver] ON Fuelcard.driverId=Driver.driverId ORDER BY Fuelcard.fuelCardId DESC; ";

            using (SqlCommand command = connection.CreateCommand()) {
                command.CommandText = query;
                connection.Open();
                try {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int fuelCardId = (int)reader["fuelCardId"];
                        string cardNumber = (string)reader["cardNumber"];
                        DateTime validityDate = (DateTime)reader["validityDate"];
                        string pin = (string)reader["pin"];
                        if(pin.Length != 4) {
                            int id = fuelCardId;
                            Console.WriteLine();
                        }
                        bool isEnabled = (bool)reader["isEnabled"];
                        FuelCard fuelCard = new FuelCard(fuelCardId, cardNumber, validityDate, pin, isEnabled);
                        if(!reader.IsDBNull(4))
                        {
                            int driverid = (int)reader["driverId"];
                            string firstName = (string)reader["firstName"];
                            string lastName = (string)reader["lastName"];
                             Driver d = new Driver(firstName, lastName);
                            d.SetDriverID(driverid);
                            fuelCard.SetDriver(d);
                        }
                        fuelCards.Add(fuelCard);
                    }
                } catch (Exception ex) {

                    Console.WriteLine();
                    throw new Exception(ex.Message);
                } finally {
                    connection.Close();
                }
            }
            return fuelCards.AsReadOnly();
        }

        public IReadOnlyList<FuelCard> GetFuelCardsByAmount(int amount) {
            SqlConnection connection = getConnection();
            List<FuelCard> fuelCards = new List<FuelCard>();
            string query = "SELECT TOP @amount * FROM Fuelcard ORDER BY fuelCardId DESC; ";

            using (SqlCommand command = connection.CreateCommand()) {
                command.CommandText = query;
                command.Parameters.AddWithValue("@amount", amount);
                connection.Open();
                try {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int fuelCardId = (int)reader["fuelCardId"];
                        string cardNumber = (string)reader["cardNumber"];
                        DateTime validityDate = (DateTime)reader["validityDate"];
                        string pin = (string)reader["pin"];
                        FuelType fuelType = new FuelType((string)reader["fuelType"]);
                        bool isEnabled = (bool)reader["isEnabled"];
                        fuelCards.Add(new FuelCard(fuelCardId, cardNumber, validityDate, pin, new List<FuelType>(), isEnabled));
                    }
                } catch (Exception ex) {

                    Console.WriteLine();
                    throw new Exception(ex.Message);
                } finally {
                    connection.Close();
                }
            }
            return fuelCards.AsReadOnly();
        }

       

        public IReadOnlyList<FuelCard> SearchFuelCards(string cardNr, DateTime? validityDate)
        {
            SqlConnection connection = getConnection();

            List<FuelCard> fuelCards = new List<FuelCard>();

            string query = "SELECT * FROM FuelCard WHERE cardNumber LIKE @cardNumber";

            if(validityDate != null)
            {
                query += " AND validityDate = @validityDate";
            }

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@cardNumber", SqlDbType.NVarChar));
                    command.Parameters["@cardNumber"].Value = cardNr + "%";
                    if(validityDate != null)
                    {
                        command.Parameters.Add(new SqlParameter("@validityDate", SqlDbType.DateTime));
                        command.Parameters["@validityDate"].Value = validityDate;
                    }
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int fuelCardId = (int)reader["fuelCardId"];
                        string cardNumber = (string)reader["cardNumber"];
                        DateTime valDate = (DateTime)reader["validityDate"];
                        string pin = (string)reader["pin"];
                        bool isEnabled = (bool)reader["isEnabled"];
                        fuelCards.Add(new FuelCard(fuelCardId, cardNumber, valDate, pin, isEnabled));
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
        public int? AddFuelCard(FuelCard fuelCard)
        {
            SqlConnection connection = getConnection();

            string query = "INSERT INTO Fuelcard (cardNumber, validityDate, pin, isEnabled)" +
                           "VALUES (@cardNumber, @validityDate, @pin, @isEnabled)" +
                           " SELECT SCOPE_IDENTITY();";

            using(SqlCommand command = connection.CreateCommand())
            {
                int? id = null;
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@cardNumber", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@validityDate", SqlDbType.DateTime));
                    command.Parameters.Add(new SqlParameter("@pin", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@isEnabled", SqlDbType.Bit));

                    command.Parameters["@cardNumber"].Value = fuelCard.CardNumber;
                    command.Parameters["@validityDate"].Value = fuelCard.ValidityDate;
                    command.Parameters["@pin"].Value = fuelCard.Pin;
                    command.Parameters["@isEnabled"].Value = fuelCard.IsEnabled;

                    command.CommandText = query;
                    int n = Convert.ToInt32(command.ExecuteScalar());
                    id = n;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return id;
            }
        }
        
        

        // *************************************************************************************************
        // ********************** DELETE FUELCARD & REMOVE CONNECTION WITH DRIVER **************************
        // *************************************************************************************************

        public void DeleteFuelCardTransaction(FuelCard fuelCard)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try {

                DeleteFuelCardFromForeignTables(fuelCard, transaction);
                DeleteFuelCard(fuelCard, transaction);
                if (fuelCard.Driver != null)
                {
                    RemoveDriverConnection(fuelCard);
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void DeleteFuelCardFromForeignTables(FuelCard fuelCard, SqlTransaction transaction) {
            SqlConnection connection = getConnection();

            string query = "DELETE FROM FuelCardFuelType WHERE fuelCardId=@fuelCardId";

            using (SqlCommand command = connection.CreateCommand()) {
                connection.Open();
                try {
                    command.Transaction = transaction;
                    command.Parameters.Add(new SqlParameter("@fuelCardId", SqlDbType.Int));
                    command.Parameters["@fuelCardId"].Value = fuelCard.FuelCardId;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new Exception(ex.Message);
                } finally {
                    connection.Close();
                }
            }
        }
        private void DeleteFuelCard(FuelCard fuelCard, SqlTransaction transaction)
        {
            SqlConnection connection = getConnection();

            string query = "DELETE FROM Fuelcard WHERE fuelCardId=@fuelCardId";

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Transaction = transaction;
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
        private void RemoveDriverConnection(FuelCard fuelCard)
        {
            SqlConnection connection = getConnection();
            string query = $"UPDATE Driver SET fuelcardId=NULL WHERE driverId=@driverId";
            using(SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                    command.Parameters["@driverId"].Value = fuelCard.Driver.DriverID;
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
        public void RemoveDriverConnectionByDriverId(int driverid) {
            SqlConnection connection = getConnection();
            string query = $"UPDATE Driver SET fuelcardId=@nullvalue WHERE driverId=@driverId";
            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@nullvalue", DBNull.Value);
                    //command.Parameters.Add(new SqlParameter("@nullvalue", SqlDbType.Int));
                    //command.Parameters["@nullvalue"].Value =0;
                    command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                    command.Parameters["@driverId"].Value = driverid;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new Exception(ex.Message);
                } finally {
                    connection.Close();
                }
            }
        }

        // *************************************************************************************************
        // ********************** UPDATE FUELCARD & UPDATE CONNECTION WITH DRIVER **************************
        // *************************************************************************************************

        public void UpdateFuelCardTransaction(FuelCard fuelCard)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                UpdateFuelCard(fuelCard, transaction);
                if(fuelCard.Driver != null)
                {
                    UpdateDriverConnection(fuelCard, transaction);
                }
                else
                {
                    //RemoveDriverConnection(fuelCard);
                    //since the fuelcard object allready removed the driver it is impossible to retrieve the driver id after the fact.
                    // this has been resolved with the use of a removedriverById (driverid) method that acts befor a driver was removed.
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void UpdateFuelCard(FuelCard fuelCard, SqlTransaction transaction)
        {
            if(fuelCard.Driver != null) {
                SqlConnection connection = getConnection();

                string query = "UPDATE Fuelcard " +
                               "SET cardNumber=@cardNumber, validityDate=@validityDate, pin=@pin, driverId=@driverId, isEnabled=@isEnabled " +
                               "WHERE fuelCardId=@fuelCardId";

                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    try {
                        command.Transaction = transaction;
                        command.Parameters.Add(new SqlParameter("@cardNumber", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@validityDate", SqlDbType.DateTime));
                        command.Parameters.Add(new SqlParameter("@pin", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@isEnabled", SqlDbType.Bit));
                        command.Parameters.Add(new SqlParameter("@fuelCardId", SqlDbType.Int));

                        command.Parameters["@cardNumber"].Value = fuelCard.CardNumber;
                        command.Parameters["@validityDate"].Value = fuelCard.ValidityDate;
                        command.Parameters["@pin"].Value = fuelCard.Pin;
                        command.Parameters["@isEnabled"].Value = fuelCard.IsEnabled;
                        command.Parameters["@fuelCardId"].Value = fuelCard.FuelCardId;
                        command.Parameters["@driverId"].Value = fuelCard.Driver.DriverID;
                     
                        command.CommandText = query;
                        command.ExecuteNonQuery();

                    } catch (Exception ex) {
                        throw new Exception(ex.Message);
                    } finally {
                        connection.Close();
                    }
                }

            } else {
                SqlConnection connection = getConnection();

                string query = "UPDATE Fuelcard " +
                               "SET cardNumber=@cardNumber, validityDate=@validityDate, pin=@pin, driverId=@driverId, isEnabled=@isEnabled " +
                               "WHERE fuelCardId=@fuelCardId";

                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    try {
                        command.Parameters.Add(new SqlParameter("@cardNumber", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@validityDate", SqlDbType.DateTime));
                        command.Parameters.Add(new SqlParameter("@pin", SqlDbType.Int));
                        
                        command.Parameters.Add(new SqlParameter("@isEnabled", SqlDbType.Bit));
                        command.Parameters.Add(new SqlParameter("@fuelCardId", SqlDbType.Int));

                        command.Parameters["@cardNumber"].Value = fuelCard.CardNumber;
                        command.Parameters["@validityDate"].Value = fuelCard.ValidityDate;
                        command.Parameters["@pin"].Value = fuelCard.Pin;
                        command.Parameters["@isEnabled"].Value = fuelCard.IsEnabled;
                        command.Parameters["@fuelCardId"].Value = fuelCard.FuelCardId;
                        command.Parameters.AddWithValue("@driverId", DBNull.Value);

                        command.CommandText = query;
                        command.ExecuteNonQuery();

                    } catch (Exception ex) {
                        throw new Exception(ex.Message);
                    } finally {
                        connection.Close();
                    }
                }

            }
        }
        private void UpdateDriverConnection(FuelCard fuelCard, SqlTransaction transaction)
        {
            SqlConnection connection = getConnection();
            string query = $"UPDATE DRIVER SET fuelcardId=@fuelcardId WHERE driverId=@driverId";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.Transaction = transaction;
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@fuelcardId", SqlDbType.Int));
                    command.Parameters["@fuelcardId"].Value = fuelCard.FuelCardId;
                    command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                    command.Parameters["@driverId"].Value = fuelCard.Driver.DriverID;
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


        // *************************************************************************************************
        // *************************************************************************************************
        // *************************************************************************************************

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

        public bool FuelCardHasDriver(FuelCard fuelCard)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM [FuelCard] WHERE fuelCardId=@fuelCardId AND driverId IS NOT NULL";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@fuelCardId", SqlDbType.Int));
                    command.Parameters["@fuelCardId"].Value = fuelCard.FuelCardId;
                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
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

        public FuelCard SearchFuelCard(int? fuelCardId, string cardNr, FuelType fuelType) {
            throw new NotImplementedException();
        }

       




        //END OF REPOSITORY
    }
}
