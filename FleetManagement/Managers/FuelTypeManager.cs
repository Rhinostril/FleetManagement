using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;

namespace FleetManagement.Business.Managers
{
    public class FuelTypeManager
    {
        private IFuelTypeRepository repo;

        public FuelTypeManager(IFuelTypeRepository repo)
        {
            this.repo = repo;
        }

        public IReadOnlyList<FuelType> GetAllFuelTypes()
        {
            try
            {
                return repo.GetAllFuelTypes();
            }
            catch(Exception ex)
            {
                throw new Exception("FuelTypeManager - GetAllFuelTypes()", ex);
            }
        }

        public void AddFuelTypeToFuelCard(int fuelTypeId, int fuelCardId)
        {
            try
            {
                repo.AddFuelTypeToFuelCard(fuelTypeId, fuelCardId);
            }
            catch (Exception ex)
            {
                throw new Exception("FuelTypeManager - AddFuelTypeToFuelCard()", ex);
            }
        }

        public void RemoveFuelTypeFromFuelCard(int fuelTypeId, int fuelCardId)
        {
            try
            {
                repo.RemoveFuelTypeFromFuelCard(fuelTypeId, fuelCardId);
            }
            catch (Exception ex)
            {
                throw new Exception("FuelTypeManager - RemoveFuelTypeFromFuelCard()", ex);
            }
        }

        public void AddFuelTypeToVehicle(int fuelTypeId, int vehicleId)
        {
            try
            {
                repo.AddFuelTypeToVehicle(fuelTypeId, vehicleId);
            }
            catch (Exception ex)
            {
                throw new Exception("FuelTypeManager - AddFuelTypeToVehicle()", ex);
            }
        }
        public void RemoveFuelTypeFromVehicle(int fuelTypeId, int vehicleId)
        {
            try
            {
                repo.RemoveFuelTypeFromVehicle(fuelTypeId, vehicleId);
            }
            catch (Exception ex)
            {
                throw new Exception("FuelTypeManager - RemoveFuelTypeToVehicle()", ex);
            }
        }


    }
}
