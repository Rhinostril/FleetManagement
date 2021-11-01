using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;
using System.Data.SqlClient;

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

        public Vehicle GetVehicle()
        {
            throw new NotImplementedException();
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
