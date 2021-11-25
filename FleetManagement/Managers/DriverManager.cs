using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;

namespace FleetManagement.Business.Managers
{
    public class DriverManager
    {

        private IDriverRepository repo; // readonly ??

        public DriverManager(IDriverRepository repo)
        {
            this.repo = repo;
        }

        public void AddDriver(Driver driver)
        {
            try
            {
                if (!repo.DriverExists(driver.DriverID))
                {
                    repo.AddDriver(driver);
                }
                else
                {
                    throw new ManagerException("DriverManager - AddDriver - Driver already added");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateDriver(Driver driver)
        {
            try
            {
                // Bestaat driver met zelfde properties al?

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Driver GetDriverByID(int driverID) {
            try {

                Driver diver = repo.GetDriverById(driverID);
                return diver;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public IReadOnlyList<Driver> GetLatestDrivers() {
            try {

                return repo.GetTop50Drivers();

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public IReadOnlyList<Driver> GetAllDrivers() {
            try {

                return repo.GetDrivers(25);

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }


        public void DeleteDriver(Driver driver) {
            try
            {
                if (repo.DriverExists(driver.DriverID))
                {
                    repo.DeleteDriver(driver);
                }
                else
                {
                    throw new ManagerException("DriverManager - DeleteDriver - Driver already deleted");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IReadOnlyList<Driver> SearchDrivers(int? driverId, string firstName, string lastName, DateTime dateOfBirth, Address address)
        {
            try
            {
                return repo.SearchDrivers(driverId, firstName, lastName, dateOfBirth, address);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
