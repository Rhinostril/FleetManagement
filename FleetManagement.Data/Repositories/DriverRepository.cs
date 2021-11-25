using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;
using System.Data.SqlClient;
using System.Linq;
using System.Data;

namespace FleetManagement.Data.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private Dictionary<int, string> Alldriverlicensetypes = new Dictionary<int, string>();
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
        public Dictionary<int, string> GetDriversLicensePairs() {
            Dictionary<int, string> driverlicensetypes = new Dictionary<int, string>();
            string licenseQuery = "SELECT * FROM [LicenseType]";
            SqlConnection licenseconnection = getConnection();
            using (SqlCommand command = new SqlCommand(licenseQuery, licenseconnection)) {

                licenseconnection.Open();
                try {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int typeID = (int)reader.GetValue("licenseTypeId");
                        string TypeText = (string)reader.GetValue("name");
                        driverlicensetypes.Add(typeID, TypeText);
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
                    //if a driver has no license type this will simply not loop once
                    //inserts a driverid and typeid into the DriversLicenceType table that can be used elsewhere
                    if (Alldriverlicensetypes.ContainsValue(licensetype)) {
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
                            int key = Alldriverlicensetypes.FirstOrDefault(x => x.Value == licensetype).Key;
                            command.Parameters["@driverid"].Value = key;

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
                        string firstname  = reader.GetString("firstName");
                        //every driver should have a firstname and its best to use a column other than driverId for this one
                        //it does not matter that firstname isnt unique as we don't use the value but rather the fact that one exists as a response in this method
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
            string query = $"SELECT * FROM [Driver] LEFT JOIN [Vehicle] ON Vehicle.vehicleId = Driver.vehicleId LEFT JOIN [Address] ON Address.addressId = Driver.addressId  ;"; 
            List<Driver> driverlist = new List<Driver>();
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection)) {
                connection.Open();
                try {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int driverid = (int)reader.GetValue("driverId");
                        string firstname = reader.GetString("firstName");
                        string lastname = reader.GetString("lastName");
                        DateTime dateofbirth = (DateTime)reader.GetValue("dateOfBirth");
                        int? addressId = null;
                        if (!reader.IsDBNull("addressId")) {
                            addressId = (int?)reader.GetValue("addressId");
                        }
                        string securityNumber = reader.GetString("securityNumber");
                        int? vehicleid = null;
                        if (!reader.IsDBNull("vehicleId")) {
                            vehicleid = (int?)reader.GetValue("vehicleId");
                        }
                        int? fuelcardid = null;
                        if (!reader.IsDBNull("fuelcardId")) {
                            fuelcardid = (int?)reader.GetValue("fuelcardId");
                        }
                        List<string> LicenseTypes = GetLicensesByDriverID(driverid);

                        Driver D = new Driver(firstname, lastname, dateofbirth, securityNumber);
                        D.SetDriverID(driverid);
                        if (addressId != null) {
                            string street = (string)reader.GetValue("street");
                            string houseNr = (string)reader.GetValue("houseNr");
                            string postalCode = (string)reader.GetValue("postalCode");
                            string city = (string)reader.GetValue("city");
                            string country = (string)reader.GetValue("country");
                            Address a = new Address((int)addressId, street, houseNr, postalCode, city, country);
                            D.SetAddress(a);
                        }
                        if (vehicleid != null) {
                            string brand = (string)reader.GetValue("brand");
                            string model = (string)reader.GetValue("model");
                            string chassisNr = (string)reader.GetValue("chassisNumber");
                            string licensePlate = (string)reader.GetValue("licensePlate");
                            string vehicleType = (string)reader.GetValue("vehicleType");
                            string color = (string)reader.GetValue("color");
                            int doors = (int)reader.GetValue("doors");
                            List<FuelType> vehiclefueltypes = GetvehcileFueltypes((int)vehicleid);
                            Vehicle v = new Vehicle(brand, model, chassisNr, licensePlate, vehiclefueltypes, vehicleType, color, doors);
                            D.SetVehicle(v);
                        }
                        if (fuelcardid != null) {
                            string cardNumber = (string)reader.GetValue("cardNumber");
                            DateTime validityDate = (DateTime)reader.GetValue("validityDate");
                            string pin = (string)reader.GetValue("pin");
                            List<FuelType> fuelcardFueltypes = GetfuelcardFueltypes((int)fuelcardid);
                            bool isEnabled = (bool)reader.GetValue("isEnabled");
                            FuelCard fc = new FuelCard(cardNumber, validityDate, pin, fuelcardFueltypes, isEnabled);
                            D.SetFuelCard(fc);
                        }
                        if (LicenseTypes.Any()) {
                            D.SetDriversLicensetypes(LicenseTypes);
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
        public IReadOnlyList<Driver> GetTop50Drivers() {
            string query = $"SELECT TOP 50 * FROM [Driver] LEFT JOIN [Vehicle] ON Vehicle.vehicleId = Driver.vehicleId LEFT JOIN [Address] ON Address.addressId = Driver.addressId ORDER BY Driver.driverid DESC ;";
            List<Driver> driverlist = new List<Driver>();
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection)) {
                connection.Open();
                try {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int driverid = (int)reader.GetValue("driverId");
                        string firstname = reader.GetString("firstName");
                        string lastname = reader.GetString("lastName");
                        DateTime dateofbirth = (DateTime)reader.GetValue("dateOfBirth");
                        int? addressId = null;
                        if (!reader.IsDBNull("addressId")) {
                            addressId = (int?)reader.GetValue("addressId");
                        }
                        string securityNumber = reader.GetString("securityNumber");
                        int? vehicleid = null;
                        if (!reader.IsDBNull("vehicleId")) {
                            vehicleid = (int?)reader.GetValue("vehicleId");
                        }
                        int? fuelcardid = null;
                        if (!reader.IsDBNull("fuelcardId")) {
                            fuelcardid = (int?)reader.GetValue("fuelcardId");
                        }
                        List<string> LicenseTypes = GetLicensesByDriverID(driverid);

                        Driver D = new Driver(firstname, lastname, dateofbirth, securityNumber);
                        D.SetDriverID(driverid);
                        if (addressId != null) {
                            string street = (string)reader.GetValue("street");
                            string houseNr = (string)reader.GetValue("houseNr");
                            string postalCode = (string)reader.GetValue("postalCode");
                            string city = (string)reader.GetValue("city");
                            string country = (string)reader.GetValue("country");
                            Address a = new Address((int)addressId, street, houseNr, postalCode, city, country);
                            D.SetAddress(a);
                        }
                        if (vehicleid != null) {
                            string brand = (string)reader.GetValue("brand");
                            string model = (string)reader.GetValue("model");
                            string chassisNr = (string)reader.GetValue("chassisNumber");
                            string licensePlate = (string)reader.GetValue("licensePlate");
                            string vehicleType = (string)reader.GetValue("vehicleType");
                            string color = (string)reader.GetValue("color");
                            int doors = (int)reader.GetValue("doors");
                            List<FuelType> vehiclefueltypes = GetvehcileFueltypes((int)vehicleid);
                            Vehicle v = new Vehicle(brand, model, chassisNr, licensePlate, vehiclefueltypes, vehicleType, color, doors);
                            D.SetVehicle(v);
                        }
                        if (fuelcardid != null) {
                            string cardNumber = (string)reader.GetValue("cardNumber");
                            DateTime validityDate = (DateTime)reader.GetValue("validityDate");
                            string pin = (string)reader.GetValue("pin");
                            List<FuelType> fuelcardFueltypes = GetfuelcardFueltypes((int)fuelcardid);
                            bool isEnabled = (bool)reader.GetValue("isEnabled");
                            FuelCard fc = new FuelCard(cardNumber, validityDate, pin, fuelcardFueltypes, isEnabled);
                            D.SetFuelCard(fc);
                        }
                        if (LicenseTypes.Any()) {
                            D.SetDriversLicensetypes(LicenseTypes);
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

        public IReadOnlyList<Driver> GetDriversByAmount(int amount) {
            string query = $"SELECT TOP @amount * FROM [Driver] LEFT JOIN [Vehicle] ON Vehicle.vehicleId = Driver.vehicleId LEFT JOIN [Address] ON Address.addressId = Driver.addressId ORDER BY Driver.driverid DESC ;";
            List<Driver> driverlist = new List<Driver>();
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection)) {
                command.Parameters.AddWithValue("@amount", amount);
                connection.Open();
                try {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int driverid = (int)reader.GetValue("Driver.driverId");
                        string firstname = reader.GetString("firstName");
                        string lastname = reader.GetString("lastName");
                        DateTime dateofbirth = (DateTime)reader.GetValue("dateOfBirth");
                        int? addressId = (int?)reader.GetValue("Driver.addressId"); //nullable int
                        string securityNumber = reader.GetString("securityNumber");
                        int? vehicleid = (int?)reader.GetValue("Driver.vehicleId");
                        int? fuelcardid = (int?)reader.GetValue("Driver.fuelcardId");
                        List<string> LicenseTypes = GetLicensesByDriverID(driverid);

                        Driver D = new Driver(firstname, lastname, dateofbirth, securityNumber);
                        if (addressId != null) {
                            string street = (string)reader.GetValue("street");
                            string houseNr = (string)reader.GetValue("houseNr");
                            string postalCode = (string)reader.GetValue("postalCode");
                            string city = (string)reader.GetValue("city");
                            string country = (string)reader.GetValue("country");
                            Address a = new Address((int)addressId, street, houseNr, postalCode, city, country);
                            D.SetAddress(a);
                        }
                        if (vehicleid != null) {
                            string brand = (string)reader.GetValue("brand");
                            string model = (string)reader.GetValue("model");
                            string chassisNr = (string)reader.GetValue("chassisNumber");
                            string licensePlate = (string)reader.GetValue("licensePlate");
                            string vehicleType = (string)reader.GetValue("vehicleType");
                            string color = (string)reader.GetValue("color");
                            int doors = (int)reader.GetValue("doors");
                            List<FuelType> vehiclefueltypes = GetvehcileFueltypes((int)vehicleid);
                            Vehicle v = new Vehicle(brand, model, chassisNr, licensePlate, vehiclefueltypes, vehicleType, color, doors);
                            D.SetVehicle(v);
                        }
                        if (fuelcardid != null) {
                            string cardNumber = (string)reader.GetValue("cardNumber");
                            DateTime validityDate = (DateTime)reader.GetValue("validityDate");
                            string pin = (string)reader.GetValue("pin");
                            List<FuelType> fuelcardFueltypes = GetfuelcardFueltypes((int)fuelcardid);
                            bool isEnabled = (bool)reader.GetValue("isEnabled");
                            FuelCard fc = new FuelCard(cardNumber, validityDate, pin, fuelcardFueltypes, isEnabled);
                            D.SetFuelCard(fc);
                        }
                        if (LicenseTypes.Any()) {
                            D.SetDriversLicensetypes(LicenseTypes);
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

        public List<FuelType> GetfuelcardFueltypes(int fuelcardid) {
            List<FuelType> fuelTypes = new List<FuelType>();
            string query = "SELECT (fuelCardId,[FuelCardFuelType].fuelTypeId,name) FROM [FuelCardFuelType] LEFT JOIN [FuelType] ON [FuelCardFuelType].fuelTypeId = [FuelType].fuelTypeId where fuelCardId=@ID ";
            SqlConnection fuelcardtypeconnection = getConnection();
            using (SqlCommand command = new SqlCommand(query, fuelcardtypeconnection)) {
                fuelcardtypeconnection.Open();
                try {
                    command.Parameters.AddWithValue("@ID", fuelcardid);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {

                        int fueltypeid = (int)reader.GetValue("fuelTypeId");
                        string name = (string)reader.GetValue("name");
                        fuelTypes.Add(new FuelType(fueltypeid, name));
                    }
                    reader.Close();

                } catch (Exception ex) {

                } finally {
                    fuelcardtypeconnection.Close();
                }

            }
            return fuelTypes;
        }

        public List<string> GetLicensesByDriverID(int Driverid) {
            List<string> types = new List<string>();
            string query = "SELECT * FROM [DriverLicenseType] where driverId=@ID";
            SqlConnection licenseconnection = getConnection();
            using (SqlCommand command = new SqlCommand(query, licenseconnection)) {
                licenseconnection.Open();
                try {
                    command.Parameters.AddWithValue("@ID", Driverid);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int licenseTypeId = (int)reader.GetValue("licenseTypeId");
                        if (Alldriverlicensetypes.ContainsKey(licenseTypeId)) {
                            types.Add(Alldriverlicensetypes[licenseTypeId]);
                        }
                    }
                    reader.Close();

                } catch (Exception ex) {

                } finally {
                    licenseconnection.Close();
                }
            }
            return types;
        }
        public List<FuelType> GetvehcileFueltypes(int vehicleid) {
            List<FuelType> fuelTypes = new List<FuelType>();
            string query = "SELECT (vehicleId,[VehicleFuelType].fuelTypeId,name) FROM [VehicleFuelType] LEFT JOIN [FuelType] ON [VehicleFuelType].fuelTypeId = [FuelType].fuelTypeId where vehicleId=@ID ";
            SqlConnection vehiclefueltypeconnection = getConnection();
            using (SqlCommand command = new SqlCommand(query, vehiclefueltypeconnection)) {
                vehiclefueltypeconnection.Open();
                try {
                    command.Parameters.AddWithValue("@ID", vehicleid);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {

                        int fueltypeid = (int)reader.GetValue("fuelTypeId");
                        string name = (string)reader.GetValue("name");
                        fuelTypes.Add(new FuelType(fueltypeid, name));
                    }
                    reader.Close();

                } catch (Exception ex) {

                } finally {
                    vehiclefueltypeconnection.Close();
                }

            }
            return fuelTypes;
        }
        public void GetAddressByID(int addressID) {
           
        }
        public Driver SearchDriver(int? id, string firstName, string lastName, DateTime dateOfBirth, Address address)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Driver> SearchDrivers(int? id, string firstName, string lastName, DateTime dateOfBirth, Address address)
        {
            SqlConnection connection = getConnection();

            string query = "SELECT * FROM Driver AS t1 JOIN Address AS t2 ON t1.addressId=t2.addressId " +
                           "WHERE firstName LIKE @firstName " +
                           "AND lastName LIKE @lastName ";

            using(SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();

                    command.CommandText = query;

                    command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar));

                    command.Parameters["@firstName"].Value = firstName + "%";
                    command.Parameters["@lastName"].Value = lastName + "%";

                    SqlDataReader reader = command.ExecuteReader();
                    List<Driver> drivers = new List<Driver>();
                    while (reader.Read())
                    {
                        int driverId = (int)reader["driverId"];
                        string fName = (string)reader["firstName"];
                        string lName = (string)reader["lastName"];
                        DateTime dob = (DateTime)reader["dateOfBirth"];
                        string securityNumber = (string)reader["securityNumber"];
                        Driver driver = new Driver(driverId, fName, lName, dob, securityNumber);
                        drivers.Add(driver);
                    }
                    return drivers.AsReadOnly();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void UpdateDriver(Driver driver)
        {

            //TODO UPDATE METHOD WITH UNCERTAIN FIELDS
            throw new NotImplementedException();
        }

        public Driver GetDriverById(int id) {
            string query = $"SELECT * FROM [Driver] LEFT JOIN [Vehicle] ON Vehicle.vehicleId = Driver.vehicleId LEFT JOIN [Address] ON Address.addressId = Driver.addressId where [Driver].driverId=@ID;";
            List<Driver> driverlist = new List<Driver>();
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection)) {
                connection.Open();
                command.Parameters.AddWithValue("@ID", id);
                try {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int driverid = (int)reader.GetValue("Driver.driverId");
                        string firstname = reader.GetString("firstName");
                        string lastname = reader.GetString("lastName");
                        DateTime dateofbirth = (DateTime)reader.GetValue("dateOfBirth");
                        int? addressId = (int?)reader.GetValue("Driver.addressId"); //nullable int
                        string securityNumber = reader.GetString("securityNumber");
                        int? vehicleid = (int?)reader.GetValue("Driver.vehicleId");
                        int? fuelcardid = (int?)reader.GetValue("Driver.fuelcardId");
                        List<string> LicenseTypes = GetLicensesByDriverID(driverid);

                        Driver D = new Driver(firstname, lastname, dateofbirth, securityNumber);
                        D.SetDriverID(driverid);
                        if (addressId != null) {
                            string street = (string)reader.GetValue("street");
                            string houseNr = (string)reader.GetValue("houseNr");
                            string postalCode = (string)reader.GetValue("postalCode");
                            string city = (string)reader.GetValue("city");
                            string country = (string)reader.GetValue("country");
                            Address a = new Address((int)addressId, street, houseNr, postalCode, city, country);
                            D.SetAddress(a);
                        }
                        if (vehicleid != null) {
                            string brand = (string)reader.GetValue("brand");
                            string model = (string)reader.GetValue("model");
                            string chassisNr = (string)reader.GetValue("chassisNumber");
                            string licensePlate = (string)reader.GetValue("licensePlate");
                            string vehicleType = (string)reader.GetValue("vehicleType");
                            string color = (string)reader.GetValue("color");
                            int doors = (int)reader.GetValue("doors");
                            List<FuelType> vehiclefueltypes = GetvehcileFueltypes((int)vehicleid);
                            Vehicle v = new Vehicle(brand, model, chassisNr, licensePlate, vehiclefueltypes, vehicleType, color, doors);
                            D.SetVehicle(v);
                        }
                        if (fuelcardid != null) {
                            string cardNumber = (string)reader.GetValue("cardNumber");
                            DateTime validityDate = (DateTime)reader.GetValue("validityDate");
                            string pin = (string)reader.GetValue("pin");
                            List<FuelType> fuelcardFueltypes = GetfuelcardFueltypes((int)fuelcardid);
                            bool isEnabled = (bool)reader.GetValue("isEnabled");
                            FuelCard fc = new FuelCard(cardNumber, validityDate, pin, fuelcardFueltypes, isEnabled);
                            D.SetFuelCard(fc);
                        }
                        if (LicenseTypes.Any()) {
                            D.SetDriversLicensetypes(LicenseTypes);
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



        public IReadOnlyList<Driver> GetDrivers(int? amount)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM Driver";
            if (amount != null)
            {
                query = $"SELECT TOP {amount} FROM Driver";
            }
            using(SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader();
                    List<Driver> drivers = new List<Driver>();
                    while (reader.Read())
                    {
                        int driverId = (int)reader["driverId"];
                        string firstName = (string)reader["firstName"];
                        string lastName = (string)reader["lastName"];
                        DateTime dateOfBirth = (DateTime)reader["dateOfBirth"];
                        string securityNumber = (string)reader["securityNumber"];
                        Driver driver = new Driver(firstName, lastName, dateOfBirth, securityNumber);
                        drivers.Add(driver);
                    }
                    return drivers.AsReadOnly();
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

       
    }
}
