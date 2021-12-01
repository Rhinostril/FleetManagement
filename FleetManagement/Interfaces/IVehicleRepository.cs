using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Entities;

namespace FleetManagement.Business.Interfaces
{
    public interface IVehicleRepository
    {
        IReadOnlyList<Vehicle> GetAllVehicles();
        IReadOnlyList<Vehicle> GetTop50Vehicles();
        IReadOnlyList<Vehicle> GetVehiclesByAmount(int amount);
        IReadOnlyList<Vehicle> SearchVehicles(int? vehicleId, string brand, string model, string chassisNumber, string licensePlate, string vehicleType, string color, int? doors);
        Vehicle GetVehicle(int vehicleId);
        Vehicle SearchVehicle(int? vehicleId, string brand, string model, string chassisNumber, string licensePlate, string vehicleType, string color, int? doors);
        bool VehicleExists(Vehicle vehicle);
        bool VehicleHasDriver(Vehicle vehicle);
        void AddVehicle(Vehicle vehicle);
        void UpdateVehicle(Vehicle vehicle);
        void DeleteVehicle(Vehicle vehicle);


    }
}
