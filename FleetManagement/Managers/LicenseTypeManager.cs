using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;

namespace FleetManagement.Business.Managers
{
    public class LicenseTypeManager
    {

        private ILicenseTypeRepository repo;

        public LicenseTypeManager(ILicenseTypeRepository repo)
        {
            this.repo = repo;
        }

        public IReadOnlyList<LicenseType> GetAllLicenseTypes()
        {
            try
            {
                return repo.GetAllLicenseTypes();
            }
            catch(Exception ex)
            {
                throw new Exception("LicenseTypeManager - GetAllLicenseTypes()", ex);
            }
        }

        public void AddLicenseTypeToDriver(int licenseTypeId, int driverId)
        {
            try
            {
                repo.AddLicenseTypeToDriver(licenseTypeId, driverId);
            }
            catch (Exception ex)
            {
                throw new Exception("FuelTypeManager - AddFuelTypeToFuelCard()", ex);
            }
        }

        public void RemoveLicenseTypeFromDriver(int licenseTypeId, int driverId)
        {
            try
            {
                repo.RemoveLicenseTypeFromDriver(licenseTypeId, driverId);
            }
            catch (Exception ex)
            {
                throw new Exception("FuelTypeManager - RemoveFuelTypeFromFuelCard()", ex);
            }
        }


    }
}
