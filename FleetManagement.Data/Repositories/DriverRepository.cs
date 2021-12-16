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
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
        public Dictionary<int, string> GetDriversLicensePairs() {
            Dictionary<int, string> driverlicensetypes = new Dictionary<int, string>();
            string licenseQuery = "SELECT * FROM [LicenseType]";
            SqlConnection licenseconnection = GetConnection();
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

        public bool DriverExists(int id)
        {
            string query = $"SELECT * FROM [Driver] WHERE driverId=@ID;";
            var list = new List<object>();
            SqlConnection connection = GetConnection();
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
            SqlConnection connection = GetConnection();
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
            SqlConnection connection = GetConnection();
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
            SqlConnection connection = GetConnection();
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
            SqlConnection fuelcardtypeconnection = GetConnection();
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
            SqlConnection licenseconnection = GetConnection();
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
            SqlConnection vehiclefueltypeconnection = GetConnection();
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

        public IReadOnlyList<Driver> SearchDrivers(int? id, string lastName, string firstName, DateTime? dateOfBirth, string securtiyNumber, string street, string houseNR, string postalcode )
        {
            List<Driver> driverlist = new List<Driver>();
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
                query = $"SELECT * FROM Driver LEFT JOIN [Address] ON Address.addressId = Driver.addressId  WHERE {String.Join("", subquerylist)}";
            }

            SqlConnection connection = GetConnection();

            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    if (!DriverIDisNull) {
                        cmd.Parameters.Add(new SqlParameter("@driverid", SqlDbType.Int));
                        cmd.Parameters["@driverid"].Value = id;
                    }
                    if (!lastNameisNull) {
                        cmd.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar));
                        cmd.Parameters["@lastName"].Value = lastName;
                    }
                    if (!firstNameisNull) {
                        cmd.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar));
                        cmd.Parameters["@firstName"].Value = firstName;
                    }
                    if (!dateOfBirthisNull) {
                        cmd.Parameters.Add(new SqlParameter("@dateOfBirth", SqlDbType.DateTime));
                        cmd.Parameters["@dateOfBirth"].Value = dateOfBirth;
                    }
                    if (!securtiyNumberisNull) {
                        cmd.Parameters.Add(new SqlParameter("@securtiyNumber", SqlDbType.NVarChar));
                        cmd.Parameters["@securtiyNumber"].Value = securtiyNumber;
                    }
                    if (!streetisNull) {
                        cmd.Parameters.Add(new SqlParameter("@street", SqlDbType.NVarChar));
                        cmd.Parameters["@street"].Value = street;
                    }
                    if (!houseNRisNull) {
                        cmd.Parameters.Add(new SqlParameter("@houseNR", SqlDbType.NVarChar));
                        cmd.Parameters["@houseNR"].Value = houseNR;
                    }
                    if (!postalcodeisNull) {
                        cmd.Parameters.Add(new SqlParameter("@postalCode", SqlDbType.NVarChar));
                        cmd.Parameters["@postalCode"].Value = postalcode;
                    }
                    connection.Open();

                    cmd.CommandText = query;

                    

                    SqlDataReader reader = cmd.ExecuteReader();
                   
                    while (reader.Read())
                    {
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
                            string streetValue = (string)reader.GetValue("street");
                            string houseNr = (string)reader.GetValue("houseNr");
                            string postalCode = (string)reader.GetValue("postalCode");
                            string city = (string)reader.GetValue("city");
                            string country = (string)reader.GetValue("country");
                            Address a = new Address((int)addressId, streetValue, houseNr, postalCode, city, country);
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
                   
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    connection.Close();
                }
                return driverlist.AsReadOnly();
            }
            
        }
        public Driver GetDriverById(int id) {
            string query = $"SELECT * FROM [Driver] LEFT JOIN [Vehicle] ON Vehicle.vehicleId = Driver.vehicleId LEFT JOIN [Address] ON Address.addressId = Driver.addressId where [Driver].driverId=@ID;";
            List<Driver> driverlist = new List<Driver>();
            SqlConnection connection = GetConnection();
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
            SqlConnection connection = GetConnection();
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
            SqlConnection connection = GetConnection();
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
            SqlConnection connection = GetConnection();
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
            SqlConnection connection = GetConnection();
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

        // *********************************************************************************
        // ***************** INSERT INTO DRIVER & ADDRESS TRANSACTION **********************
        // *********************************************************************************
        public void AddDriverWithAddress(Driver driver)
        {
            SqlConnection connection = GetConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                int addressId = AddAddress(driver.Address);
                AddDriver(driver, addressId);
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
        private void AddDriver(Driver driver, int addressId)
        {
            SqlConnection connection = GetConnection();
            string query = $"INSERT INTO [Driver] (firstName, lastName, dateOfBirth, addressId, securityNumber) VALUES (@firstName, @lastName, @dateOfBirth, @addressId, @securityNumber)";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar));
                    command.Parameters["@firstName"].Value = driver.FirstName;
                    command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar));
                    command.Parameters["@lastName"].Value = driver.LastName;
                    command.Parameters.Add(new SqlParameter("@dateOfBirth", SqlDbType.DateTime));
                    command.Parameters["@dateOfBirth"].Value = driver.DateOfBirth;
                    command.Parameters.Add(new SqlParameter("@addressId", SqlDbType.Int));
                    command.Parameters["@addressId"].Value = addressId;
                    command.Parameters.Add(new SqlParameter("@securityNumber", SqlDbType.NVarChar));
                    command.Parameters["@securityNumber"].Value = driver.SecurityNumber;
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
        private int AddAddress(Address address)
        {
            SqlConnection connection = GetConnection();
            string query = $"INSERT INTO [Address] (street, houseNr, postalCode, city, country) OUTPUT INSERTED.addressId VALUES (@street, @houseNr, @postalCode, @city, @country)";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@street", SqlDbType.NVarChar));
                    command.Parameters["@street"].Value = address.Street;
                    command.Parameters.Add(new SqlParameter("@houseNr", SqlDbType.NVarChar));
                    command.Parameters["@houseNr"].Value = address.HouseNr;
                    command.Parameters.Add(new SqlParameter("@postalCode", SqlDbType.NVarChar));
                    command.Parameters["@postalCode"].Value = address.PostalCode;
                    command.Parameters.Add(new SqlParameter("@city", SqlDbType.NVarChar));
                    command.Parameters["@city"].Value = address.City;
                    command.Parameters.Add(new SqlParameter("@country", SqlDbType.NVarChar));
                    command.Parameters["@country"].Value = address.Country;
                    int addressId = (int)command.ExecuteScalar();
                    return addressId;
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
        // ********************************* UPDATE DRIVER WITH TRANSACTION **************************************
        // *******************************************************************************************************
        public void UpdateDriverWithAddress(Driver driver)
        {
            SqlConnection connection = GetConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                UpdateAddress(driver.Address);
                UpdateDriver(driver);
                if(driver.Vehicle != null)
                {
                    ConnectVehicleToDriver(driver);
                }
                if(driver.FuelCard != null)
                {
                    ConnectFuelCardToDriver(driver);
                }
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
            SqlConnection connection = GetConnection();
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
            SqlConnection connection = GetConnection();
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
        private void ConnectVehicleToDriver(Driver driver)
        {
            SqlConnection connection = GetConnection();
            string query = $"UPDATE [Vehicle] SET driverId=@driverId WHERE vehicleId=@vehicleId";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@vehicleId", SqlDbType.Int));

                    command.Parameters["@driverId"].Value = driver.DriverID;
                    command.Parameters["@vehicleId"].Value = driver.Vehicle.VehicleId;

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
        private void ConnectFuelCardToDriver(Driver driver)
        {
            SqlConnection connection = GetConnection();
            string query = $"UPDATE [Fuelcard] SET driverId=@driverId WHERE fuelCardId=@fuelCardId";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@fuelCardId", SqlDbType.Int));

                    command.Parameters["@driverId"].Value = driver.DriverID;
                    command.Parameters["@fuelCardId"].Value = driver.FuelCard.FuelCardId;

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

        // *******************************************************************************************************
        // ***************************** DELETE DRIVER & ADDRESS WITH TRANSACTION ********************************
        // *******************************************************************************************************

        public void DeleteDriverWithAddress(Driver driver)
        {
            SqlConnection connection = GetConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                DeleteDriver(driver);
                DeleteAddress(driver.Address);
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
        private void DeleteAddress(Address address)
        {
            SqlConnection connection = GetConnection();
            string query = $"DELETE FROM [Address] WHERE addressId=@addressId";
            using(SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@addressId", SqlDbType.Int));
                    command.Parameters["@addressId"].Value = address.AddressID;
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
        private void DeleteDriver(Driver driver)
        {
            SqlConnection connection = GetConnection();
            string query = $"DELETE FROM [Driver] WHERE driverId=@driverId";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                    command.Parameters["@driverId"].Value = driver.DriverID;
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

        // *******************************************************************************************************
        // *******************************************************************************************************
        // *******************************************************************************************************






    }
}
