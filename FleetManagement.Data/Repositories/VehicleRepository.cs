using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
namespace FleetManagement.Data.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {

        private string connectionString = $"Data Source=tcp:fleetmanagserver.database.windows.net,1433;Initial Catalog=dboFleetmanagement;Persist Security Info=False;User ID=fleetadmin;Password=$qlpassw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public int? AddVehicle(Vehicle vehicle)
        {
            SqlConnection cn = GetConnection();
            string query = "INSERT INTO vehicle (brand,model,chasisNumber,licensePlate,vehicleType,color,doors)VALUES(@brand,@model,@chasisNumber,@licensePlate,@vehicleType,@color,@doors) SELECT SCOPE_IDENTITY();";
            using (SqlCommand cmd = cn.CreateCommand())
            {
                int? id = null;
                cn.Open();
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@brand", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@model", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@chasisNumber", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@licensePlate", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@vehicleType", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@color", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@doors", SqlDbType.NVarChar));

                    cmd.Parameters["@brand"].Value = vehicle.Brand;
                    cmd.Parameters["@model"].Value = vehicle.Model;
                    cmd.Parameters["@chasisNumber"].Value = vehicle.ChassisNumber;
                    cmd.Parameters["@licensePlate"].Value = vehicle.LicensePlate;
                    cmd.Parameters["@vehicleType"].Value = vehicle.VehicleType;
                    cmd.Parameters["@color"].Value = vehicle.Color;
                    cmd.Parameters["@doors"].Value = vehicle.Doors;

                    cmd.CommandText = query;
                    int n = Convert.ToInt32(cmd.ExecuteScalar());
                    id = n;

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
                finally
                {
                    cn.Close(); 
                }
                return id;
            }
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            SqlConnection cn = GetConnection();
            string query = "DELETE FROM vehicle WHERE vehicleId=@vehicleId";
            using (SqlCommand cmd = cn.CreateCommand())
            {
                cn.Open();
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@vehicleId", SqlDbType.Int));
                    cmd.Parameters["@vehicleId"].Value = vehicle.VehicleId;
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
                finally
                {
                    cn.Close();
                }
            }
        }
    

        public IReadOnlyList<Vehicle> GetAllVehicles()
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT * FROM Vehicle";
            List<Vehicle> vehicles = new List<Vehicle>();
            using(SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int vehicleId = (int)reader["vehicleId"];
                        string brand = (string)reader["brand"];
                        string model = (string)reader["model"];
                        string chassisNumber = (string)reader["chassisNumber"];
                        string licensePlate = (string)reader["licensePlate"];
                        string vehicleType = (string)reader["vehicleType"];
                        string color = (string)reader["color"];
                        int doors = (int)reader["doors"];
                        Vehicle vehicle = new Vehicle(vehicleId, brand, model, chassisNumber, licensePlate, new List<FuelType> { new FuelType("#") }, vehicleType, color, doors);
                        vehicles.Add(vehicle);
                    }
                    return vehicles.AsReadOnly();
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

        public IReadOnlyList<Vehicle> GetTop50Vehicles() {
            SqlConnection connection = GetConnection();
            string query = "SELECT TOP 50 * FROM Vehicle LEFT JOIN Driver ON [Driver].driverId=[Vehicle].driverId order by [Vehicle].vehicleId DESC ";
            List<Vehicle> vehicles = new List<Vehicle>();
            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int vehicleId = (int)reader["vehicleId"];
                        string brand = (string)reader["brand"];
                        string model = (string)reader["model"];
                        string chassisNumber = (string)reader["chassisNumber"];
                        string licensePlate = (string)reader["licensePlate"];
                        string vehicleType = (string)reader["vehicleType"];
                        string color = (string)reader["color"];
                        int doors = (int)reader["doors"];
                        int? driverid = null;
                        
                        if (!reader.IsDBNull("driverId")) {
                            int tempId = (int)reader.GetValue("driverId");
                            driverid = (int?)tempId;
                        }
                        if (driverid == null) {
                            Vehicle vehicle = new Vehicle(vehicleId, brand, model, chassisNumber, licensePlate, new List<FuelType> { new FuelType("#") }, vehicleType, color, doors);
                            vehicles.Add(vehicle);
                        } else {
                            DriverRepository driverRepo = new DriverRepository();
                            Driver driver = driverRepo.GetDriverById((int)driverid);
                            Vehicle vehicle = new Vehicle(vehicleId, brand, model, chassisNumber, licensePlate, new List<FuelType> { new FuelType("#") }, vehicleType, color, doors, driver);
                            vehicles.Add(vehicle);
                        }
                    }
                        
                    return vehicles.AsReadOnly();
                } catch (Exception ex) {
                    throw new Exception(ex.Message);
                } finally {
                    connection.Close();
                }
            }
        }

        public IReadOnlyList<Vehicle> GetVehiclesByAmount(int amount) {
            SqlConnection connection = GetConnection();
            string query = "SELECT TOP @amount * FROM Vehicle";
            List<Vehicle> vehicles = new List<Vehicle>();
            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    command.Parameters.AddWithValue("@amount",amount);
                    command.CommandText = query;
                    connection.Open();
                   
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        int vehicleId = (int)reader["vehicleId"];
                        string brand = (string)reader["brand"];
                        string model = (string)reader["model"];
                        string chassisNumber = (string)reader["chassisNumber"];
                        string licensePlate = (string)reader["licensePlate"];
                        string vehicleType = (string)reader["vehicleType"];
                        string color = (string)reader["color"];
                        int doors = (int)reader["doors"];
                        Vehicle vehicle = new Vehicle(vehicleId, brand, model, chassisNumber, licensePlate, new List<FuelType> { new FuelType("#") }, vehicleType, color, doors);
                        vehicles.Add(vehicle);
                    }
                    return vehicles.AsReadOnly();
                } catch (Exception ex) {
                    throw new Exception(ex.Message);
                } finally {
                    connection.Close();
                }
            }
        }

        public Vehicle GetVehicle(int vehicleId)
        {
            SqlConnection connection = GetConnection();

            string query = "SELECT * FROM FuelType AS t1 JOIN VehicleFuelType AS t2 ON t1.fuelTypeId=t2.fuelTypeId JOIN Vehicle AS t3 ON t2.vehicleId=t3.vehicleId LEFT JOIN Driver AS t4 ON t3.driverId=t4.driverId WHERE t2.vehicleId=@vehicleId";

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                SqlParameter paramId = new SqlParameter();
                paramId.ParameterName = "@vehicleId";
                paramId.DbType = DbType.Int32;
                paramId.Value = vehicleId;
                command.Parameters.Add(paramId);
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    
                    reader.Read();

                    string brand = (string)reader["brand"];
                    string model = (string)reader["model"];
                    string chassisNr = (string)reader["chassisNumber"];
                    string licensePlate = (string)reader["licensePlate"];
                    string vehicleType = (string)reader["vehicleType"];
                    string color = (string)reader["color"];
                    int doors = (int)reader["doors"];

                    List<FuelType> fuelTypes = new List<FuelType>();
                    FuelType fuelType = new FuelType((string)reader["name"]);
                    fuelTypes.Add(fuelType);

                    Vehicle vehicle = new Vehicle(vehicleId, brand, model, chassisNr, licensePlate, fuelTypes, vehicleType, color, doors); // Init Vehicle met FuelType

                    Driver driver = null;

                    if (!reader.IsDBNull(12)) // Heeft Vehicle een driver ?
                    {
                        driver = new Driver((string)reader["firstName"], (string)reader["lastName"]);
                        vehicle.SetDriver(driver);
                    }

                    while (reader.Read())
                    {
                        fuelType = new FuelType((string)reader["name"]);
                        fuelTypes.Add(fuelType);
                    }

                    vehicle.SetFuelTypes(fuelTypes); // Assign FuelTypes List

                    reader.Close();
                    return vehicle;
                }
                catch (Exception ex)
                {
                    throw new Exception("VehicleRepository - GetVehicle(int vehicleId)", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public Vehicle SearchVehicle(int? vehicleId, string brand, string model, string chassisNumber, string licensePlate, string vehicleType, string color, int? doors) {
            List<Vehicle> vehicles = new List<Vehicle>();
            List<string> subquerylist = new List<string>();
            int numberofparams = 0;
            bool vehicleisNull = true;
            if (vehicleId != null) {
                vehicleisNull = false;
                numberofparams++;
                subquerylist.Add("vehicleId=@vehicleid");
            }
            bool brandisNull = true;
            if (!String.IsNullOrWhiteSpace(brand)) {
                brandisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("brand=@brand");
            }
            bool modelisNull = true;
            if (!String.IsNullOrWhiteSpace(model)) {
                modelisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("model=@model");
            }
            bool chasisNumberisNull = true;
            if (!String.IsNullOrWhiteSpace(chassisNumber)) {
                chasisNumberisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("chasisNumber=@chasisNumber");
            }
            bool licensePlateisNull = true;
            if (!String.IsNullOrWhiteSpace(licensePlate)) {
                licensePlateisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("licensePlate=@licensePlate");

            }
            bool vehicleTypeisNull = true;
            if (!String.IsNullOrWhiteSpace(vehicleType)) {
                vehicleTypeisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("vehicleType=@vehicleType");
            }
            bool colorisNull = true;
            if (!String.IsNullOrWhiteSpace(color)) {
                colorisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("color=@color");
            }
            bool doorsisNull = true;
            if (doors != null) {
                doorsisNull = false;
                if (numberofparams > 0) {
                    subquerylist.Add(" AND ");
                }
                numberofparams++;
                subquerylist.Add("doors=@doors");
            }
            // if number of params is >1 you need comma separation

            string query = "";
            if (numberofparams <= 0) {
                //dont even query anything
                //maybe break here
            }else{
                query= $"SELECT * FROM vehicle LEFT JOIN Driver ON [Driver].driverId=[Vehicle].driverId  WHERE {String.Join("",subquerylist)}";
            }
            SqlConnection cn = GetConnection();
            using (SqlCommand cmd = cn.CreateCommand()) {
                cn.Open();
                try {
                    if (!vehicleisNull) {
                        cmd.Parameters.Add(new SqlParameter("@vehicleid", SqlDbType.Int));
                        cmd.Parameters["@vehicleid"].Value = vehicleId;
                    }
                    if (!brandisNull) {
                        cmd.Parameters.Add(new SqlParameter("@brand", SqlDbType.NVarChar));
                        cmd.Parameters["@brand"].Value = brand;
                    }
                    if (!modelisNull) {
                        cmd.Parameters.Add(new SqlParameter("@model", SqlDbType.NVarChar));
                        cmd.Parameters["@model"].Value = model;
                    }
                    if (!chasisNumberisNull) {
                        cmd.Parameters.Add(new SqlParameter("@chasisNumber", SqlDbType.NVarChar));
                        cmd.Parameters["@chasisNumber"].Value = chassisNumber;
                    }
                    if (!licensePlateisNull) {
                        cmd.Parameters.Add(new SqlParameter("@licensePlate", SqlDbType.NVarChar));
                        cmd.Parameters["@licensePlate"].Value = licensePlate;
                    }
                    if (!vehicleTypeisNull) {
                        cmd.Parameters.Add(new SqlParameter("@vehicleType", SqlDbType.NVarChar));
                        cmd.Parameters["@vehicleType"].Value = vehicleType;
                    }
                    if (!colorisNull) {
                        cmd.Parameters.Add(new SqlParameter("@color", SqlDbType.NVarChar));
                        cmd.Parameters["@color"].Value = color;
                    }
                    if (!doorsisNull) {
                        cmd.Parameters.Add(new SqlParameter("@doors", SqlDbType.NVarChar));
                        cmd.Parameters["@doors"].Value = doors;
                    }

                    cmd.CommandText = query;
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                
                    int vehicleIdread = (int)reader["vehicleId"];
                    string branddread = (string)reader["brand"];
                    string modeldread = (string)reader["model"];
                    string chassisNumberdread = (string)reader["chassisNumber"];
                    string licensePlatedread = (string)reader["licensePlate"];
                    string vehicleTypedread = (string)reader["vehicleType"];
                    string colordread = (string)reader["color"];
                    int doorsdread = (int)reader["doors"];
                    int? driverid = null;
                    if (!reader.IsDBNull("driverId")) {
                        int tempId = (int)reader.GetValue("driverId");
                        driverid = (int?)tempId;
                    }
                    if (driverid == null) {
                        Vehicle vehicle = new Vehicle(vehicleIdread, branddread, modeldread, chassisNumberdread, licensePlatedread, new List<FuelType> { new FuelType("#") }, vehicleTypedread, colordread, doorsdread);
                        vehicles.Add(vehicle);
                    } else {
                        DriverRepository driverRepo = new DriverRepository();
                        Driver driver = driverRepo.GetDriverById((int)driverid);
                        Vehicle vehicle = new Vehicle(vehicleIdread, branddread, modeldread, chassisNumberdread, licensePlatedread, new List<FuelType> { new FuelType("#") }, vehicleTypedread, colordread, doorsdread, driver);
                        vehicles.Add(vehicle);
                    }
                    
                } catch (Exception ex) {

                    throw new Exception(ex.Message);
                } finally {
                    cn.Close();
                }
            }
            return vehicles.FirstOrDefault();
        }

    

        public IReadOnlyList<Vehicle> SearchVehicles(int? vehicleId, string brand, string model, string chassisNumber, string licensePlate, string vehicleType, string color, int? doors)
        {
        
        List<Vehicle> vehicles = new List<Vehicle>();
        List<string> subquerylist = new List<string>();
        int numberofparams = 0;
        bool vehicleisNull = true;
        if (vehicleId != null) {
            vehicleisNull = false;
            numberofparams++;
            subquerylist.Add("vehicleId=@vehicleid");
        }
        bool brandisNull = true;
        if (!String.IsNullOrWhiteSpace(brand)) {
            brandisNull = false;
            if (numberofparams > 0) {
                subquerylist.Add(" AND ");
            }
            numberofparams++;
            subquerylist.Add("brand=@brand");
        }
        bool modelisNull = true;
        if (!String.IsNullOrWhiteSpace(model)) {
            modelisNull = false;
            if (numberofparams > 0) {
                subquerylist.Add(" AND ");
            }
            numberofparams++;
            subquerylist.Add("model=@model");
        }
        bool chasisNumberisNull = true;
        if (!String.IsNullOrWhiteSpace(chassisNumber)) {
            chasisNumberisNull = false;
            if (numberofparams > 0) {
                subquerylist.Add(" AND ");
            }
            numberofparams++;
            subquerylist.Add("chasisNumber=@chasisNumber");
        }
        bool licensePlateisNull = true;
        if (!String.IsNullOrWhiteSpace(licensePlate)) {
            licensePlateisNull = false;
            if (numberofparams > 0) {
                subquerylist.Add(" AND ");
            }
            numberofparams++;
            subquerylist.Add("licensePlate=@licensePlate");

        }
        bool vehicleTypeisNull = true;
        if (!String.IsNullOrWhiteSpace(vehicleType)) {
            vehicleTypeisNull = false;
            if (numberofparams > 0) {
                subquerylist.Add(" AND ");
            }
            numberofparams++;
            subquerylist.Add("vehicleType=@vehicleType");
        }
        bool colorisNull = true;
        if (!String.IsNullOrWhiteSpace(color)) {
            colorisNull = false;
            if (numberofparams > 0) {
                subquerylist.Add(" AND ");
            }
            numberofparams++;
            subquerylist.Add("color=@color");
        }
        bool doorsisNull = true;
        if (doors != null) {
            doorsisNull = false;
            if (numberofparams > 0) {
                subquerylist.Add(" AND ");
            }
            numberofparams++;
            subquerylist.Add("doors=@doors");
        }
        // if number of params is >1 you need comma separation

        string query = "";
        if (numberofparams <= 0) {
            //dont even query anything
            //maybe break here
        } else {
            query = $"SELECT * FROM vehicle LEFT JOIN Driver ON [Driver].driverId=[Vehicle].driverId  WHERE {String.Join("", subquerylist)}";
        }
        SqlConnection cn = GetConnection();
        using (SqlCommand cmd = cn.CreateCommand()) {
            cn.Open();
            try {
                if (!vehicleisNull) {
                    cmd.Parameters.Add(new SqlParameter("@vehicleid", SqlDbType.Int));
                    cmd.Parameters["@vehicleid"].Value = vehicleId;
                }
                if (!brandisNull) {
                    cmd.Parameters.Add(new SqlParameter("@brand", SqlDbType.NVarChar));
                    cmd.Parameters["@brand"].Value = brand;
                }
                if (!modelisNull) {
                    cmd.Parameters.Add(new SqlParameter("@model", SqlDbType.NVarChar));
                    cmd.Parameters["@model"].Value = model;
                }
                if (!chasisNumberisNull) {
                    cmd.Parameters.Add(new SqlParameter("@chasisNumber", SqlDbType.NVarChar));
                    cmd.Parameters["@chasisNumber"].Value = chassisNumber;
                }
                if (!licensePlateisNull) {
                    cmd.Parameters.Add(new SqlParameter("@licensePlate", SqlDbType.NVarChar));
                    cmd.Parameters["@licensePlate"].Value = licensePlate;
                }
                if (!vehicleTypeisNull) {
                    cmd.Parameters.Add(new SqlParameter("@vehicleType", SqlDbType.NVarChar));
                    cmd.Parameters["@vehicleType"].Value = vehicleType;
                }
                if (!colorisNull) {
                    cmd.Parameters.Add(new SqlParameter("@color", SqlDbType.NVarChar));
                    cmd.Parameters["@color"].Value = color;
                }
                if (!doorsisNull) {
                    cmd.Parameters.Add(new SqlParameter("@doors", SqlDbType.NVarChar));
                    cmd.Parameters["@doors"].Value = doors;
                }

                cmd.CommandText = query;
                SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) {
                        int vehicleIdread = (int)reader["vehicleId"];
                        string branddread = (string)reader["brand"];
                        string modeldread = (string)reader["model"];
                        string chassisNumberdread = (string)reader["chassisNumber"];
                        string licensePlatedread = (string)reader["licensePlate"];
                        string vehicleTypedread = (string)reader["vehicleType"];
                        string colordread = (string)reader["color"];
                        int doorsdread = (int)reader["doors"];
                        int? driverid = null;
                        if (!reader.IsDBNull("driverId")) {
                            int tempId = (int)reader.GetValue("driverId");
                            driverid = (int?)tempId;
                        }
                        if (driverid == null) {
                            Vehicle vehicle = new Vehicle(vehicleIdread, branddread, modeldread, chassisNumberdread, licensePlatedread, new List<FuelType> { new FuelType("#") }, vehicleTypedread, colordread, doorsdread);
                            vehicles.Add(vehicle);
                        } else {
                            DriverRepository driverRepo = new DriverRepository();
                            Driver driver = driverRepo.GetDriverById((int)driverid);
                            Vehicle vehicle = new Vehicle(vehicleIdread, branddread, modeldread, chassisNumberdread, licensePlatedread, new List<FuelType> { new FuelType("#") }, vehicleTypedread, colordread, doorsdread, driver);
                            vehicles.Add(vehicle);
                        }
                    }

              
            } catch (Exception ex) {

                throw new Exception(ex.Message);
            } finally {
                cn.Close();
            }
        }
        return vehicles.AsReadOnly();
        }

        

        public bool VehicleExists(Vehicle vehicle)
        {
            SqlConnection cn = GetConnection();
            string query = "SELECT count(*) FROM vehicle WHERE vehicleId=@vehicleId OR chassisNumber=@chassisNumber OR licensePlate=@licensePlate";
            using(SqlCommand cmd = cn.CreateCommand())
            {
                cn.Open();
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@vehicleId",SqlDbType.Int));
                    cmd.Parameters["@vehicleId"].Value = vehicle.VehicleId;
                    cmd.Parameters.Add(new SqlParameter("@chassisNumber", SqlDbType.NVarChar));
                    cmd.Parameters["@chassisNumber"].Value = vehicle.ChassisNumber;
                    cmd.Parameters.Add(new SqlParameter("@licensePlate", SqlDbType.NVarChar));
                    cmd.Parameters["@licensePlate"].Value = vehicle.LicensePlate;
                    cmd.CommandText = query;
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cn.Close();
                }
            }
        }

        public bool VehicleHasDriver(Vehicle vehicle) {
            SqlConnection cn = GetConnection();
            string query = "SELECT count(*) FROM vehicle WHERE vehicleId=@vehicleId AND driverId IS NOT NULL";
            using (SqlCommand cmd = cn.CreateCommand()) {
                cn.Open();
                try {
                    cmd.Parameters.Add(new SqlParameter("@vehicleId", SqlDbType.Int));
                    cmd.Parameters["@vehicleId"].Value = vehicle.VehicleId;
                    cmd.CommandText = query;
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new Exception(ex.Message);
                } finally {
                    cn.Close();
                }
            }
        }

        // ********************************************************************
        // **************** UPDATE VEHICLE WITH TRANSACTION *******************
        // ********************************************************************
        public void RemoveDriverIdFromVehicle(Vehicle vehicle) {
            if(vehicle.Driver != null) {
                SqlConnection connection = GetConnection();
                string query = $"UPDATE [Vehicle] SET [Vehicle].driverId=@driverId WHERE vehicleId=@vehicleId";
                using (SqlCommand command = connection.CreateCommand()) {
                    try {
                        connection.Open();
                        command.Parameters.AddWithValue("driverId", DBNull.Value);
                        
                        command.Parameters.Add(new SqlParameter("@vehicleId", SqlDbType.Int));
                        command.Parameters["@vehicleId"].Value = vehicle.VehicleId;

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


        public void UpdateVehicleWithDriver(Vehicle vehicle)
        {
            SqlConnection connection = GetConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                UpdateVehicle(vehicle);
                if (vehicle.Driver != null)
                {
                    ConnectVehicleToDriver(vehicle);
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
        public void UpdateVehicle(Vehicle vehicle)
        {
            bool vehicleHasDriver = false;
            int? driverID = null;
            if(vehicle.Driver != null) {
                vehicleHasDriver = true;
                driverID = vehicle.Driver.DriverID;
            }
            SqlConnection cn = GetConnection();

            string query = "";
            if (!vehicleHasDriver) {
                query = "UPDATE vehicle" +
                " SET brand=@brand,model=@model,chassisNumber=@chasisNumber,licensePlate=@licensePlate,vehicleType=@vehicleType,color=@color,doors=@doors"
                + " WHERE vehicleId=@vehicleId";

                using (SqlCommand cmd = cn.CreateCommand()) {
                    cn.Open();
                    try {
                        cmd.Parameters.Add(new SqlParameter("@brand", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@model", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@chasisNumber", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@licensePlate", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@vehicleType", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@color", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@doors", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@vehicleId", SqlDbType.Int));

                        cmd.Parameters["@brand"].Value = vehicle.Brand;
                        cmd.Parameters["@model"].Value = vehicle.Model;
                        cmd.Parameters["@chasisNumber"].Value = vehicle.ChassisNumber;
                        cmd.Parameters["@licensePlate"].Value = vehicle.LicensePlate;
                        cmd.Parameters["@vehicleType"].Value = vehicle.VehicleType;
                        cmd.Parameters["@color"].Value = vehicle.Color;
                        cmd.Parameters["@doors"].Value = vehicle.Doors;
                        cmd.Parameters["@vehicleId"].Value = vehicle.VehicleId;


                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                    } catch (Exception ex) {

                        throw new Exception(ex.Message);
                    } finally {
                        cn.Close();
                    }
                }
            } else {
                query = "UPDATE vehicle" +
                " SET brand=@brand,model=@model,chassisNumber=@chasisNumber,licensePlate=@licensePlate,vehicleType=@vehicleType,color=@color,doors=@doors ,driverId=@driverid"
                + " WHERE vehicleId=@vehicleId";

                using (SqlCommand cmd = cn.CreateCommand()) {
                    cn.Open();
                    try {
                        cmd.Parameters.Add(new SqlParameter("@brand", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@model", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@chasisNumber", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@licensePlate", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@vehicleType", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@color", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@doors", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@vehicleId", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("@driverid", SqlDbType.Int));

                        cmd.Parameters["@brand"].Value = vehicle.Brand;
                        cmd.Parameters["@model"].Value = vehicle.Model;
                        cmd.Parameters["@chasisNumber"].Value = vehicle.ChassisNumber;
                        cmd.Parameters["@licensePlate"].Value = vehicle.LicensePlate;
                        cmd.Parameters["@vehicleType"].Value = vehicle.VehicleType;
                        cmd.Parameters["@color"].Value = vehicle.Color;
                        cmd.Parameters["@doors"].Value = vehicle.Doors;
                        cmd.Parameters["@vehicleId"].Value = vehicle.VehicleId;
                        cmd.Parameters["@driverid"].Value = (int)driverID;

                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                    } catch (Exception ex) {

                        throw new Exception(ex.Message);
                    } finally {
                        cn.Close();
                    }
                }
            }
            

        }
        private void ConnectVehicleToDriver(Vehicle vehicle)
        {
            SqlConnection connection = GetConnection();
            string query = $"UPDATE [Driver] SET vehicleId=@vehicleId WHERE driverId=@driverId";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@vehicleId", SqlDbType.Int));

                    command.Parameters["@driverId"].Value = vehicle.Driver.DriverID;
                    command.Parameters["@vehicleId"].Value = vehicle.VehicleId;

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

        public void DeleteVehicleFuelRecord(Vehicle vehicle) {
            SqlConnection cn = GetConnection();
            string query = "DELETE FROM VehicleFuelType WHERE vehicleId=@vehicleId";
            using (SqlCommand cmd = cn.CreateCommand()) {
                cn.Open();
                try {
                    cmd.Parameters.Add(new SqlParameter("@vehicleId", SqlDbType.Int));
                    cmd.Parameters["@vehicleId"].Value = vehicle.VehicleId;
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();

                } catch (Exception ex) {

                    throw new Exception(ex.Message);
                } finally {
                    cn.Close();
                }
            }
        }


        // ********************************************************************
        // ********************************************************************
        // ********************************************************************






    }
}
