using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Exceptions;

namespace FleetManagement.Business.Entities
{
    public class LicenseType
    {
        public int LicenseTypeId { get; private set; }
        public string LicenseName { get; private set; }

        public LicenseType(int licenseTypeId, string licenseName)
        {
            SetLicenseTypeId(licenseTypeId);
            SetLicenseName(licenseName);
        }

        public void SetLicenseTypeId(int id)
        {
            if(id > 0)
            {
                LicenseTypeId = id;
            }
            else
            {
                throw new LicenseTypeException("SetLicenseTypeId - id moet groter dan 0 zijn");
            }
        }

        public void SetLicenseName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                LicenseName = name;
            }
            else
            {
                throw new LicenseTypeException("SetLicenseName - naam mag niet leeg zijn");
            }
        }

    }
}
