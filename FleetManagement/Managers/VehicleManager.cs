using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;

namespace FleetManagement.Business.Managers
{
    public class VehicleManager {

        private IVehicleRepository repo;

        public VehicleManager(IVehicleRepository repo) {
            this.repo = repo;
        }

        public void AddVehicle(Vehicle vehicle) {
            try {
                if (!repo.VehicleExists(vehicle)) {
                    repo.AddVehicle(vehicle);
                } else {
                    throw new VehicleManagerException("VehicleManager - AddVehicle - Vehicle already added");
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateVehicle(Vehicle vehicle) {
            try {
                // Bestaat vehicle met properties al ?


            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            try {
                if (repo.VehicleExists(vehicle))
                {
                    if (!repo.VehicleHasDriver(vehicle))
                    {
                        repo.DeleteVehicle(vehicle);
                    }
                    else
                    {
                        throw new VehicleManagerException("VehicleManager - DeleteVehicle - Vehicle still has a driver");
                    }
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

        public IReadOnlyList<Vehicle> GetAllVehicles() {
            try {
                return repo.GetAllVehicles();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public IReadOnlyList<Vehicle> GetLatestVehicles() {
            try {
                return repo.GetTop50Vehicles();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public IReadOnlyList<Vehicle> Searchvehicles(int? vehicleId, string brand, string model, string chassisNumber, string licensePlate, string vehicleType, string color, int? doors) {
            try {
                return repo.SearchVehicles(vehicleId, brand, model, chassisNumber, licensePlate, vehicleType, color, doors);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Vehicle GetVehicle(int vehicleId)
        {
            try
            {
                return repo.GetVehicle(vehicleId);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
