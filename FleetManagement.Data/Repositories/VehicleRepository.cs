using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace FleetManagement.Data
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
            throw new NotImplementedException();
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public bool VehicleExists(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

    }
}
