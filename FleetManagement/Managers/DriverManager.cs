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
                repo.UpdateDriverWithAddress(driver);

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
                    if (!repo.DriverHasFuelCard(driver.DriverID))
                    {
                        if (!repo.DriverHasVehicle(driver.DriverID))
                        {
                            repo.DeleteDriverWithAddress(driver);
                        }
                        else
                        {
                            throw new ManagerException("DriverManager - DeleteDriver - Driver still has a vehicle");
                        }
                    }
                    else
                    {
                        throw new ManagerException("DriverManager - DeleteDriver - Driver still has a fuelcard");
                    }
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

        public IReadOnlyList<Driver> SearchDrivers(int? id, string lastName, string firstName, DateTime? dateOfBirth, string securtiyNumber, string street, string houseNR, string postalcode)
        {
            try
            {
                return repo.SearchDrivers(id, lastName, firstName, dateOfBirth, securtiyNumber, street, houseNR, postalcode);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
