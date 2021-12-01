using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;
using System.Data.SqlClient;
using System.Data;

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

        public void AddVehicle(Vehicle vehicle)
        {
            SqlConnection cn = GetConnection();
            string query = "INSERT INTO vehicle (brand,model,chasisNumber,licensePlate,vehicleType,color,doors)VALUES(@brand,@model,@chasisNumber,@licensePlate,@vehicleType,@color,@doors)";
            using (SqlCommand cmd = cn.CreateCommand())
            {
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
            string query = "SELECT TOP 50 * FROM Vehicle";
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

        public Vehicle SearchVehicle(int? vehicleId, string brand, string model, string chassisNumber, string licensePlate, FuelType fuelType, string vehicleType, string color, int doors, Driver driver)
        {
            




        }

        public IReadOnlyList<Vehicle> SearchVehicles(int? vehicleId, string brand, string model, string chassisNumber, string licensePlate, FuelType fuelType, string vehicleType, string color, int doors, Driver driver)
        {
            //this only works if all params are filled out which often isnt the case
            SqlConnection cn = GetConnection();
            List<Vehicle> vehicles = new List<Vehicle>();
            string query = "SELECT * FROM vehicle WHERE brand=@brand,model=@model,chasisNumber=@chasisNumber,licensePlate=@licensePlate,vehicleType=@vehicleType,color=@color,doors=@doors";
             using (SqlCommand cmd = cn.CreateCommand())
            {
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

                    cmd.Parameters["@brand"].Value = brand;
                    cmd.Parameters["@model"].Value = model;
                    cmd.Parameters["@chasisNumber"].Value = chassisNumber;
                    cmd.Parameters["@licensePlate"].Value = licensePlate;
                    cmd.Parameters["@vehicleType"].Value = vehicleType;
                    cmd.Parameters["@color"].Value = color;
                    cmd.Parameters["@doors"].Value = doors;


                    cmd.CommandText = query;
                    SqlDataReader reader = cmd.ExecuteReader();
                    //while (reader.Read())
                    //{
                    //    string brand = (string)["brand"];
                    //    string model = (string)reader["model"];
                    //    vehicles.Add(new Vehicle(brand,model,chassisNumber,licensePlate,new List<FuelType>(),vehicleType,color,doors));
                    //}
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
            return vehicles.AsReadOnly();
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            SqlConnection cn = GetConnection();
            string query = "UPDATE vehicle" +
                "SET brand=@brand,model=@model,chasisNumber=@chasisNumber,licensePlate=@licensePlate,vehicleType=@vehicleType,color=@color,doors=@doors"
                + "WHERE vehicleId=@vehicleId";

            using (SqlCommand cmd = cn.CreateCommand())
            {
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

        public bool VehicleExists(Vehicle vehicle)
        {
            SqlConnection cn = GetConnection();
            string query = "SELECT count(*) FROM vehicle WHERE vehicle=@vehicleId OR chassisNumber=@chassisNumber OR licensePlate=@licensePlate";
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

    }
}
