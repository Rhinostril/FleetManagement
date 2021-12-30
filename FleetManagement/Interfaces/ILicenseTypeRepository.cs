using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Entities;

namespace FleetManagement.Business.Interfaces
{
    public interface ILicenseTypeRepository
    {
        IReadOnlyList<LicenseType> GetAllLicenseTypes();
        void AddLicenseTypeToDriver(int licenseTypeId, int driverId);
        void RemoveLicenseTypeFromDriver(int licenseTypeId, int driverId);

    }
}
