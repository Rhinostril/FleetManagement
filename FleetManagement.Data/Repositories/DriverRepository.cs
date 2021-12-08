﻿using System;
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
            if (driver.Vehicle != null) {
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
            int? newdriverID = null;
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
            if (newdriverID != null) {
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
                        string firstname = reader.GetString("firstName");
                        //every driver should have a firstname and its best to use a column other than driverId for this one
                        //it does not matter that firstname isnt unique as we don't use the value but rather the fact that one exists as a response in this method
                        list.Add(firstname);

                    }
                    reader.Close();
                }
                catch (Exception e) {

                }
                finally
                {
                    connection.Close();
                }
            }
            if (list.Any()) {
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
        //public IReadOnlyList<Vehicle> SearchVehicles(int? vehicleId, string brand, string model, string chassisNumber, string licensePlate, string vehicleType, string color, int? doors) {

        //    List<Vehicle> vehicles = new List<Vehicle>();
        //    List<string> subquerylist = new List<string>();
        //    int numberofparams = 0;
        //    bool vehicleisNull = true;
        //    if (vehicleId != null) {
        //        vehicleisNull = false;
        //        numberofparams++;
        //        subquerylist.Add("vehicleId=@vehicleid");
        //    }
        //    bool brandisNull = true;
        //    if (!String.IsNullOrWhiteSpace(brand)) {
        //        brandisNull = false;
        //        if (numberofparams > 0) {
        //            subquerylist.Add(" AND ");
        //        }
        //        numberofparams++;
        //        subquerylist.Add("brand=@brand");
        //    }
        //    bool modelisNull = true;
        //    if (!String.IsNullOrWhiteSpace(model)) {
        //        modelisNull = false;
        //        if (numberofparams > 0) {
        //            subquerylist.Add(" AND ");
        //        }
        //        numberofparams++;
        //        subquerylist.Add("model=@model");
        //    }
        //    bool chasisNumberisNull = true;
        //    if (!String.IsNullOrWhiteSpace(chassisNumber)) {
        //        chasisNumberisNull = false;
        //        if (numberofparams > 0) {
        //            subquerylist.Add(" AND ");
        //        }
        //        numberofparams++;
        //        subquerylist.Add("chasisNumber=@chasisNumber");
        //    }
        //    bool licensePlateisNull = true;
        //    if (!String.IsNullOrWhiteSpace(licensePlate)) {
        //        licensePlateisNull = false;
        //        if (numberofparams > 0) {
        //            subquerylist.Add(" AND ");
        //        }
        //        numberofparams++;
        //        subquerylist.Add("licensePlate=@licensePlate");

        //    }
        //    bool vehicleTypeisNull = true;
        //    if (!String.IsNullOrWhiteSpace(vehicleType)) {
        //        vehicleTypeisNull = false;
        //        if (numberofparams > 0) {
        //            subquerylist.Add(" AND ");
        //        }
        //        numberofparams++;
        //        subquerylist.Add("vehicleType=@vehicleType");
        //    }
        //    bool colorisNull = true;
        //    if (!String.IsNullOrWhiteSpace(color)) {
        //        colorisNull = false;
        //        if (numberofparams > 0) {
        //            subquerylist.Add(" AND ");
        //        }
        //        numberofparams++;
        //        subquerylist.Add("color=@color");
        //    }
        //    bool doorsisNull = true;
        //    if (doors != null) {
        //        doorsisNull = false;
        //        if (numberofparams > 0) {
        //            subquerylist.Add(" AND ");
        //        }
        //        numberofparams++;
        //        subquerylist.Add("doors=@doors");
        //    }
        //    // if number of params is >1 you need comma separation

        //    string query = "";
        //    if (numberofparams <= 0) {
        //        //dont even query anything
        //        //maybe break here
        //    } else {
        //        query = $"SELECT * FROM vehicle WHERE {String.Join("", subquerylist)}";
        //    }
        //    SqlConnection cn = GetConnection();
        //    using (SqlCommand cmd = cn.CreateCommand()) {
        //        cn.Open();
        //        try {
        //            if (!vehicleisNull) {
        //                cmd.Parameters.Add(new SqlParameter("@vehicleid", SqlDbType.Int));
        //                cmd.Parameters["@vehicleid"].Value = vehicleId;
        //            }
        //            if (!brandisNull) {
        //                cmd.Parameters.Add(new SqlParameter("@brand", SqlDbType.NVarChar));
        //                cmd.Parameters["@brand"].Value = brand;
        //            }
        //            if (!modelisNull) {
        //                cmd.Parameters.Add(new SqlParameter("@model", SqlDbType.NVarChar));
        //                cmd.Parameters["@model"].Value = model;
        //            }
        //            if (!chasisNumberisNull) {
        //                cmd.Parameters.Add(new SqlParameter("@chasisNumber", SqlDbType.NVarChar));
        //                cmd.Parameters["@chasisNumber"].Value = chassisNumber;
        //            }
        //            if (!licensePlateisNull) {
        //                cmd.Parameters.Add(new SqlParameter("@licensePlate", SqlDbType.NVarChar));
        //                cmd.Parameters["@licensePlate"].Value = licensePlate;
        //            }
        //            if (!vehicleTypeisNull) {
        //                cmd.Parameters.Add(new SqlParameter("@vehicleType", SqlDbType.NVarChar));
        //                cmd.Parameters["@vehicleType"].Value = vehicleType;
        //            }
        //            if (!colorisNull) {
        //                cmd.Parameters.Add(new SqlParameter("@color", SqlDbType.NVarChar));
        //                cmd.Parameters["@color"].Value = color;
        //            }
        //            if (!doorsisNull) {
        //                cmd.Parameters.Add(new SqlParameter("@doors", SqlDbType.NVarChar));
        //                cmd.Parameters["@doors"].Value = doors;
        //            }

        //            cmd.CommandText = query;
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            while (reader.Read()) {
        //                int vehicleIdread = (int)reader["vehicleId"];
        //                string branddread = (string)reader["brand"];
        //                string modeldread = (string)reader["model"];
        //                string chassisNumberdread = (string)reader["chassisNumber"];
        //                string licensePlatedread = (string)reader["licensePlate"];
        //                string vehicleTypedread = (string)reader["vehicleType"];
        //                string colordread = (string)reader["color"];
        //                int doorsdread = (int)reader["doors"];
        //                Vehicle vehicle = new Vehicle(vehicleIdread, branddread, modeldread, chassisNumberdread, licensePlatedread, new List<FuelType> { new FuelType("#") }, vehicleTypedread, colordread, doorsdread);
        //                vehicles.Add(vehicle);
        //            }


        //        } catch (Exception ex) {

        //            throw new Exception(ex.Message);
        //        } finally {
        //            cn.Close();
        //        }
        //    }
        //    return vehicles.AsReadOnly();
        //}


        public IReadOnlyList<Driver> SearchDrivers(int? id, string lastName, string firstName, DateTime? dateOfBirth, string securtiyNumber, string street, string houseNR, string postalcode )
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            List<string> subquerylist = new List<string>();
            int numberofparams = 0;
            bool DriverIDisNull = true;
            if (id != null) {
                DriverIDisNull = false;
                numberofparams++;
                subquerylist.Add("driverid=@driverid");
            }
            bool lastNameisNull = true;
            if (!String.IsNullOrWhiteSpace(lastName)) {
                lastNameisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("lastName=@lastName");
            }
            bool firstNameisNull = true;
            if (!String.IsNullOrWhiteSpace(firstName)) {
                firstNameisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("firstName=@firstName");
            }
          
            bool dateOfBirthisNull = true;
            if (dateOfBirth != null) {
                dateOfBirthisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("dateOfBirth=@dateOfBirth");

            }
            bool securtiyNumberisNull = true;
            if (!String.IsNullOrWhiteSpace(securtiyNumber)) {
                securtiyNumberisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("securtiyNumber=@securtiyNumber");
            }
            bool streetisNull = true;
            if (!String.IsNullOrWhiteSpace(street)) {
                streetisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("street=@street");
            }
          
            bool houseNRisNull = true;
            if (!String.IsNullOrWhiteSpace(houseNR)) {
                houseNRisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("houseNR=@houseNR");
            }
            bool postalcodeisNull = true;
            if (!String.IsNullOrWhiteSpace(postalcode)) {
                postalcodeisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("postalCode=@postalCode");
            }
            // if all adres fields are not empty, search for that

            string query = "";
            if (numberofparams <= 0) {
                //dont even query anything
                //maybe break here
            } else {
                query = $"SELECT * FROM Driver WHERE {String.Join("", subquerylist)}";
            }

            SqlConnection connection = getConnection();

            string query = "SELECT * FROM Driver AS t1 JOIN Address AS t2 ON t1.addressId=t2.addressId " +
                           "WHERE firstName LIKE @firstName " +
                           "AND lastName LIKE @lastName ";

            using (SqlCommand command = connection.CreateCommand())
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
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    connection.Close();
                }
            }
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
            using (SqlCommand command = connection.CreateCommand())
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

        public bool DriverHasVehicle(int id)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM [Driver] WHERE driverId=@driverId AND vehicleId IS NOT NULL";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                    command.Parameters["@driverId"].Value = id;
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

        public bool DriverHasFuelCard(int id)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM [Driver] WHERE driverId=@driverId AND fuelcardId IS NOT NULL";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                    command.Parameters["@driverId"].Value = id;
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

        // *******************************************************************************************************
        // ********************************* UPDATE DRIVER WITH ADDRESS ******************************************
        // *******************************************************************************************************
        public void UpdateDriverWithAddress(Driver driver)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                UpdateAddress(driver.Address);
                UpdateDriver(driver);
                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void UpdateDriver(Driver driver)
        {
            SqlConnection connection = getConnection();
            string query = $"UPDATE [Driver] SET firstName=@firstName, lastName=@lastName, dateOfBirth=@dateOfBirth, securityNumber=@securityNumber, vehicleId=@vehicleId, fuelcardId=@fuelcardId WHERE driverId=@driverId";
            using(SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@dateOfBirth", SqlDbType.DateTime));
                    command.Parameters.Add(new SqlParameter("@securityNumber", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@vehicleId", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@fuelcardId", SqlDbType.Int));
                    command.Parameters["@driverId"].Value = driver.DriverID;
                    command.Parameters["@firstName"].Value = driver.FirstName;
                    command.Parameters["@lastName"].Value = driver.LastName;
                    command.Parameters["@dateOfBirth"].Value = driver.DateOfBirth;
                    command.Parameters["@securityNumber"].Value = driver.SecurityNumber;
                    if (driver.Vehicle != null)
                    {
                        command.Parameters["@vehicleId"].Value = driver.Vehicle.VehicleId;
                    }
                    else
                    {
                        command.Parameters["@vehicleId"].Value = DBNull.Value;
                    }
                    if(driver.FuelCard != null)
                    {
                        command.Parameters["@fuelcardId"].Value = driver.FuelCard.FuelCardId;
                    }
                    else
                    {
                        command.Parameters["@fuelcardId"].Value = DBNull.Value;
                    }
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
        private void UpdateAddress(Address address)
        {
            SqlConnection connection = getConnection();
            string query = $"UPDATE [Address] SET street=@street, houseNr=@houseNr, postalCode=@postalCode, city=@city, country=@country WHERE addressId=@addressId";
            using(SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@addressId", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@street", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@houseNr", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@postalCode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@city", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@country", SqlDbType.NVarChar));
                    command.Parameters["@addressId"].Value = address.AddressID;
                    command.Parameters["@street"].Value = address.Street;
                    command.Parameters["@houseNr"].Value = address.HouseNr;
                    command.Parameters["@postalCode"].Value = address.PostalCode;
                    command.Parameters["@city"].Value = address.City;
                    command.Parameters["@country"].Value = address.Country;
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

        // *******************************************************************************************************
        // *******************************************************************************************************
        // *******************************************************************************************************









    }
}
