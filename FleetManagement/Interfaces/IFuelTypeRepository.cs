using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Entities;

namespace FleetManagement.Business.Interfaces
{
    public interface IFuelTypeRepository
    {
        IReadOnlyList<FuelType> GetAllFuelTypes();
        void AddFuelTypeToFuelCard(int fuelTypeId, int fuelCardId);
        void RemoveFuelTypeFromFuelCard(int fuelTypeId, int fuelCardId);
        void AddFuelTypeToVehicle(int fuelTypeId, int vehicleId);
        void RemoveFuelTypeFromVehicle(int fuelTypeId, int vehicleId);

    }
}
