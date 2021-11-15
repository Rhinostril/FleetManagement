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
        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            SqlConnection cn = getConnection();
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
            SqlConnection cn = getConnection();
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
            throw new NotImplementedException();
        }

        public Vehicle GetVehicle(int vehicleId)
        {
            SqlConnection connection = getConnection();

            string query = "SELECT *" +
                           "FROM Vehicle" +
                           "LEFT JOIN Driver" +
                           "ON Vehicle.driverId = Driver.driverId " +
                           "WHERE vehicleId=@vehicleId";

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

                    Vehicle vehicle = new Vehicle(vehicleId, brand, model, chassisNr, licensePlate, new List<FuelType>(), vehicleType, color, doors);

                    reader.Close();
                    return vehicle;
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

        public Vehicle SearchVehicle(int? vehicleId, string brand, string model, string chassisNumber, string licensePlate, FuelType fuelType, string vehicleType, string color, int doors, Driver driver)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Vehicle> SearchVehicles(int? vehicleId, string brand, string model, string chassisNumber, string licensePlate, FuelType fuelType, string vehicleType, string color, int doors, Driver driver)
        {
            SqlConnection cn = getConnection();
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
            SqlConnection cn = getConnection();
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
            SqlConnection cn = getConnection();
            string query = "SELECT count(*) FROM vehicle WHERE vehicle=@vehicleId";
            using(SqlCommand cmd = cn.CreateCommand())
            {
                cn.Open();
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@vehicleId",SqlDbType.Int));
                    cmd.Parameters["@vehicleId"].Value = vehicle.VehicleId;
                    cmd.CommandText = query;
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
