using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;
using System.Data.SqlClient;
using System.Linq;

namespace FleetManagement.Data
{
    public class DriverRepository : IDriverRepository
    {

        private string connectionString = $"Data Source=fleetmanagserver.database.windows.net;Persist Security Info=True;User ID=fleetadmin;Password=***********";

        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public void AddDriver(Driver driver)
        {
            SqlConnection connection = getConnection();
            string query = "INSERT INTO [Driver] (firstName,lastName,dateOfBirth,securityNumber,licenceType) Values (@firstName,@lastName,@dateOfBirth,@securityNumber,@licenceType)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try
                {                   
                    SqlParameter parDriverFirstName = new SqlParameter();
                    parDriverFirstName.ParameterName = "@firstName";
                    parDriverFirstName.SqlDbType = System.Data.SqlDbType.NVarChar;
                    command.Parameters.Add(parDriverFirstName);

                    SqlParameter parDriverLastName = new SqlParameter();
                    parDriverLastName.ParameterName = "@lastName";
                    parDriverLastName.SqlDbType = System.Data.SqlDbType.NVarChar;
                    command.Parameters.Add(parDriverLastName);

                    SqlParameter parDriverDateOfBirth = new SqlParameter();
                    parDriverDateOfBirth.ParameterName = "@dateOfBirth";
                    parDriverDateOfBirth.SqlDbType = System.Data.SqlDbType.DateTime;
                    command.Parameters.Add(parDriverDateOfBirth);

                    SqlParameter parDriverSecurityNr = new SqlParameter();
                    parDriverSecurityNr.ParameterName = "@securityNumber";
                    parDriverSecurityNr.SqlDbType = System.Data.SqlDbType.NVarChar;
                    command.Parameters.Add(parDriverSecurityNr);

                    SqlParameter parDriverlicenceType = new SqlParameter();
                    parDriverlicenceType.ParameterName = "@licenceType";
                    parDriverlicenceType.SqlDbType = System.Data.SqlDbType.NVarChar;
                    command.Parameters.Add(parDriverlicenceType);

                    command.Parameters["@firstName"].Value = driver.FirstName;
                    command.Parameters["@lastName"].Value = driver.LastName;
                    command.Parameters["@dateOfBirth"].Value = driver.DateOfBirth;
                    command.Parameters["@securityNumber"].Value = driver.SecurityNumber;

                    List<string> licensetypes = driver.DriversLicenceType;
                    string joinedTypes = String.Join('|',licensetypes);
                    command.Parameters["@licenceType"].Value = joinedTypes;
                    
                    connection.Open();
                    command.ExecuteNonQuery();
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
            //TODO if vehicle and address are not null, fetch their respective id's and update the driver with those id's
        }

        public void DeleteDriver(Driver driver)
        {
            SqlConnection connection = getConnection();

            string query = $"DELETE FROM [Driver] WHERE driverId=@ID;";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try
                {
                    SqlParameter pardriverid = new SqlParameter();
                    pardriverid.ParameterName = "@ID";
                    pardriverid.SqlDbType = System.Data.SqlDbType.Int;
                    command.Parameters.Add(pardriverid);
                    command.Parameters["@ID"].Value = driver.DriverID;

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool DriverExists(int id)
        {
            string query = $"SELECT * FROM [Driver] WHERE driverId=@ID;";
            var list = new List<object>();
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ID", id);
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string firstname  = reader.GetString(1);
                        //every driver should have a firstname and its best to use a column other than driverId for this one
                        list.Add(firstname);

                    }
                    reader.Close();
                }
                catch (Exception e){
                    
                }
                finally
                {
                    connection.Close();
                }
            }
            if (list.Any()){
                //there is at least one driver with this id in the system
                return true;
            }
            else
            {
                return false;
            }
        }

        public IReadOnlyList<Driver> GetAllDrivers()
        {
            string query = $"SELECT * FROM [Driver];";
            List<Driver> driverlist = new List<Driver>();
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection)) {
                connection.Open();
                try {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int driverid = (int)reader.GetValue(0);
                        string firstname = reader.GetString(1);
                        string lastname = reader.GetString(2);
                        DateTime dateofbirth = (DateTime)reader.GetValue(3);
                        int? addressId = (int?)reader.GetValue(4); //nullable int
                        string securityNumber = reader.GetString(5);
                        int? vehicleid = (int?)reader.GetValue(6);
                        int? fuelcardid = (int?)reader.GetValue(7);
                        string licenseTypes = reader.GetString(8);
                        List<string> SplitLicenseTypes = licenseTypes.Split('|').ToList();

                        Driver D = new Driver(firstname, lastname, dateofbirth, securityNumber, SplitLicenseTypes);
                        if(addressId != null) {
                            //TODO get address from id and set it on driver using driver.setaddress(Adr)
                        }
                        if (vehicleid != null) {
                            //TODO get Vehicle from id and set it on driver using driver.setVehicle(Vhcl)
                        }
                        if (fuelcardid != null) {
                            //TODO get fuelcard from id and set it on driver using driver.setfuelcard(fuelCrd)
                        }

                        driverlist.Add(D);

                    }
                    reader.Close();
                } catch (Exception e) {

                } finally {
                    connection.Close();
                }
            }
            return driverlist.AsReadOnly();
        }


        public Driver SearchDriver(int? id, string firstName, string lastName, DateTime dateOfBirth, Address address)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Driver> SearchDrivers(int? id, string firstName, string lastName, DateTime dateOfBirth, Address address)
        {
            throw new NotImplementedException();
        }

        public void UpdateDriver(Driver driver)
        {
            throw new NotImplementedException();
        }

        public Driver GetDriverById(int id) {
            string query = $"SELECT * FROM [Driver] WHERE driverId=@ID;";
            List<Driver> driverlist = new List<Driver>();
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection)) {
                command.Parameters.AddWithValue("@ID", id);
                connection.Open();
                try {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int driverid = (int)reader.GetValue(0);
                        string firstname = reader.GetString(1);
                        string lastname = reader.GetString(2);
                        DateTime dateofbirth = (DateTime)reader.GetValue(3);
                        int? addressId = (int?)reader.GetValue(4); //nullable int
                        string securityNumber = reader.GetString(5);
                        int? vehicleid = (int?)reader.GetValue(6);
                        int? fuelcardid = (int?)reader.GetValue(7);
                        string licenseTypes = reader.GetString(8);
                        List<string> SplitLicenseTypes = licenseTypes.Split('|').ToList();

                        Driver D = new Driver(firstname, lastname, dateofbirth, securityNumber, SplitLicenseTypes);
                        if (addressId != null) {
                            //TODO get address from id and set it on driver using driver.setaddress(Adr)
                        }
                        if (vehicleid != null) {
                            //TODO get Vehicle from id and set it on driver using driver.setVehicle(Vhcl)
                        }
                        if (fuelcardid != null) {
                            //TODO get fuelcard from id and set it on driver using driver.setfuelcard(fuelCrd)
                        }

                        driverlist.Add(D);

                    }
                    reader.Close();
                } catch (Exception e) {

                } finally {
                    connection.Close();
                }
            }
            return driverlist.First();
        }

        public bool DoesSecurityNumberExist(string securityNumber) {
            string query = $"SELECT * FROM [Driver] WHERE securityNumber=@securityNumber;";
            List<int> driverIDlist = new List<int>();
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection)) {

                SqlParameter parDriverSecurityNr = new SqlParameter();
                parDriverSecurityNr.ParameterName = "@securityNumber";
                parDriverSecurityNr.SqlDbType = System.Data.SqlDbType.NVarChar;
                command.Parameters.Add(parDriverSecurityNr);
                command.Parameters["@securityNumber"].Value = securityNumber;

                connection.Open();
                try {
                    //this reads all the protenial drivers with a given security number
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int driverid = (int)reader.GetValue(0);
                        driverIDlist.Add(driverid);
                    }
                    reader.Close();
                } catch (Exception e) {

                } finally {
                    connection.Close();
                }
            }

            if (driverIDlist.Any()) {
                //beware the driver cannot be added because this security number allready exists
                return true;
            } else {
                return false;
            }
        }
    }
}
