using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Entities;

namespace FleetManagement.Business.Interfaces
{
    public interface IAddressRepository
    {
        bool AddressExists(Address address);
        void AddAddress(Address address);
        void UpdateAddress(Address address);
        void DeleteAddress(Address address);

    }
}
