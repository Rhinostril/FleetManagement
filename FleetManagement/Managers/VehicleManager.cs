using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;

namespace FleetManagement.Business.Managers
{
    public class VehicleManager
    {

        private IVehicleRepository repo;

        public VehicleManager(IVehicleRepository repo)
        {
            this.repo = repo;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            try
            {
                if (!repo.VehicleExists(vehicle))
                {
                    repo.AddVehicle(vehicle);
                }
                else
                {
                    throw new VehicleManagerException("VehicleManager - AddVehicle - Vehicle already added");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            try
            {
                // Bestaat vehicle met properties al ?


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            try
            {
                if (repo.VehicleExists(vehicle))
                {
                    repo.DeleteVehicle(vehicle);
                }
                else
                {
                    throw new VehicleManagerException("VehicleManager - DeleteVehicle - Vehicle already deleted");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IReadOnlyList<Vehicle> GetAllVehicles()
        {
            try
            {
                return repo.GetAllVehicles();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
