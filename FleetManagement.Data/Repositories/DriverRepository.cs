using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;
using System.Data.SqlClient;
using System.Linq;
using System.Data;

namespace FleetManagement.Data
{
    public class DriverRepository : IDriverRepository
    {
        private Dictionary<string, int> Alldriverlicensetypes = new Dictionary<string, int>();
        private string connectionString = $"Data Source=tcp:fleetmanagserver.database.windows.net,1433;Initial Catalog=dboFleetmanagement;Persist Security Info=False;User ID=fleetadmin;Password=$qlpassw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public DriverRepository() {
            //this sets the driverslicensetype dict so that it can be used by insert commands
            Alldriverlicensetypes = GetDriversLicensePairs();
        }
        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
        public Dictionary<string, int> GetDriversLicensePairs() {
            Dictionary<string, int> driverlicensetypes = new Dictionary<string, int>();
            string licenseQuery = "SELECT * FROM [LicenseType]";
            SqlConnection licenseconnection = getConnection();
            using (SqlCommand command = new SqlCommand(licenseQuery, licenseconnection)) {

                licenseconnection.Open();
                try {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int typeID = (int)reader.GetValue("licenseTypeId");
                        string TypeText = (string)reader.GetValue("name");
                        driverlicensetypes.Add(TypeText, typeID);
                    }                    
                    reader.Close();
                } catch (Exception e) {

                } finally {
                    licenseconnection.Close();
                }
            }
            return driverlicensetypes;

        }
        public void AddDriver(Driver driver)
        {
            int? vehicleid = null; //dont set this as 0 because if the read fails, the id should not be set to 0
            int? fuelcardid = null;
            int? addressId = null;
            if (driver.FuelCard != null) {
                // get fuelcard Id from fuelcard
                //cardnr and validityDate are not null and together form robust enough unique identifyer
                string FuelcardIDQuery = "SELECT * FROM [FuelCard] where cardNumber=@cardnr AND validityDate=@date";

                SqlConnection Fuelcardconnection = getConnection();
                using (SqlCommand command = new SqlCommand(FuelcardIDQuery, Fuelcardconnection)) {
                    command.Parameters.AddWithValue("@cardnr", driver.FuelCard.CardNumber);
                    command.Parameters.AddWithValue("@date", driver.FuelCard.ValidityDate);
                    Fuelcardconnection.Open();
                    try {
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        fuelcardid = (int)reader.GetValue("fuelCardId");
                        reader.Close();
                    } catch (Exception e) {

                    } finally {
                        Fuelcardconnection.Close();
                    }
                }
            }
            if(driver.Vehicle != null) {
                //get vehicleId from vehicle table
                //chassis brand and model are not null and together form robust enough unique identifyer
                string vehicleIDQuery = "SELECT * FROM [Vehicle] where chassisNumber=@chassisnr AND model=@model AND brand=@brand ";
                
                SqlConnection vehicleconnection = getConnection();
                using (SqlCommand command = new SqlCommand(vehicleIDQuery, vehicleconnection)) {
                    command.Parameters.AddWithValue("@chassisnr", driver.Vehicle.ChassisNumber);
                    command.Parameters.AddWithValue("@model", driver.Vehicle.Model);
                    command.Parameters.AddWithValue("@brand", driver.Vehicle.Brand);
                    vehicleconnection.Open();
                    try {
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        vehicleid = (int)reader.GetValue("vehicleId");
                        reader.Close();
                    } catch (Exception e) {

                    } finally {
                        vehicleconnection.Close();
                    }
                }
            }
            if (driver.Address != null) {
                //get addressid from address table
                //All param's in Address are not nullable so why not use them all for our query <- todo take extra care because of empty constructor! currently address can be non-null and empty
                string AddressIDQuery = "SELECT * FROM [Address] where street=@street AND houseNr=@houseNr AND postalCode=@postalCode AND city=@city  AND country=@country";

                SqlConnection Adressconnection = getConnection();
                using (SqlCommand command = new SqlCommand(AddressIDQuery, Adressconnection)) {
                    command.Parameters.AddWithValue("@street", driver.Address.Street);
                    command.Parameters.AddWithValue("@houseNr", driver.Address.HouseNr);
                    command.Parameters.AddWithValue("@postalCode", driver.Address.PostalCode);
                    command.Parameters.AddWithValue("@city", driver.Address.City);
                    command.Parameters.AddWithValue("@country", driver.Address.Country);
                    Adressconnection.Open();
                    try {
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        addressId = (int)reader.GetValue("addressId");
                        reader.Close();
                    } catch (Exception e) {

                    } finally {
                        Adressconnection.Close();
                    }
                }
            }

            string query = "INSERT INTO [Driver] (firstName,lastName,dateOfBirth,addressId,securityNumber,vehicleid,fuelcardid) Values (@firstName,@lastName,@dateOfBirth,@addressId,@securityNumber,@vehicleid,@fuelcardid);SELECT CAST(scope_identity() AS int)";
            int? newdriverID= null;
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection)) {
                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try {
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

                    SqlParameter parAddressId = new SqlParameter();
                    parAddressId.ParameterName = "@addressId";
                    parAddressId.SqlDbType = System.Data.SqlDbType.Int;
                    command.Parameters.Add(parAddressId);

                    SqlParameter parDriverSecurityNr = new SqlParameter();
                    parDriverSecurityNr.ParameterName = "@securityNumber";
                    parDriverSecurityNr.SqlDbType = System.Data.SqlDbType.NVarChar;
                    command.Parameters.Add(parDriverSecurityNr);

                    SqlParameter parvehicleId = new SqlParameter();
                    parvehicleId.ParameterName = "@vehicleid";
                    parvehicleId.SqlDbType = System.Data.SqlDbType.Int;
                    command.Parameters.Add(parvehicleId);

                    SqlParameter parFuelCardID = new SqlParameter();
                    parFuelCardID.ParameterName = "@fuelcardid";
                    parFuelCardID.SqlDbType = System.Data.SqlDbType.Int;
                    command.Parameters.Add(parFuelCardID);

                    command.Parameters["@firstName"].Value = driver.FirstName;
                    command.Parameters["@lastName"].Value = driver.LastName;
                    command.Parameters["@dateOfBirth"].Value = driver.DateOfBirth;
                    command.Parameters["@securityNumber"].Value = driver.SecurityNumber;

                    command.Parameters["@addressId"].Value = addressId;
                    command.Parameters["@vehicleid"].Value = vehicleid; //TODO test if this works
                    command.Parameters["@fuelcardid"].Value = fuelcardid;

                    connection.Open();
                    newdriverID = (int)command.ExecuteScalar();
                    transaction.Commit();

                } catch (Exception ex) {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                } finally {
                    connection.Close();
                }
            }
            if(newdriverID != null) {
                foreach (string licensetype in driver.DriversLicenceType) {
                    //inserts a driverid and typeid into the DriversLicenceType table that can be used elsewhere
                    if (Alldriverlicensetypes.ContainsKey(licensetype)) {
                        string insertLicenseQuery = "INSERT INTO [DriverLicenseType] (driverId,licenseTypeId) Values (@driverid,@licenseID)";
                        SqlConnection licenseconnection = getConnection();
                        using (SqlCommand command = new SqlCommand(insertLicenseQuery, licenseconnection)) {
                            SqlTransaction transaction = connection.BeginTransaction();
                            command.Transaction = transaction;
                            try {

                                SqlParameter pardriverId = new SqlParameter();
                            pardriverId.ParameterName = "@driverid";
                            pardriverId.SqlDbType = System.Data.SqlDbType.Int;
                            command.Parameters.Add(pardriverId);
                            command.Parameters["@driverid"].Value = newdriverID;

                            SqlParameter parlicenseID = new SqlParameter();
                            parlicenseID.ParameterName = "@licenseID";
                            parlicenseID.SqlDbType = System.Data.SqlDbType.Int;
                            command.Parameters.Add(parlicenseID);
                            command.Parameters["@driverid"].Value = Alldriverlicensetypes[licensetype];

                            connection.Open();
                            command.ExecuteNonQuery();
                            transaction.Commit();
                        } catch (Exception ex) {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        } finally {
                            connection.Close();
                        }
                    }
                    } else {
                        //trying to add a type that is not in the database => ASK what to do here?
                    }
                }
            }//if the previous insert failed, the id will be null so dont add anything

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
                        List<string> SplitLicenseTypes = licenseTypes.Split('|').ToList(); //Feedback?

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
