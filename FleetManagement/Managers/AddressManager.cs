using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;
using FleetManagement.Business.ManagerExceptions;

namespace FleetManagement.Business.Managers
{
    public class AddressManager : IAddressRepository
    {
        private IAddressRepository repo;
        public AddressManager(IAddressRepository repo)
        {
            this.repo = repo;
        }

        public void AddAdress(Address address)
        {
            try
            {
                if (!repo.AddressExists(address.AddressID))
                {
                    repo.AddAddress(address);
                }
                else
                {
                    throw new AddressManagerException("Address - AddAddress - Address already added.");
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public void DeleteAddress(Address address)
        {
            try
            {
                if (repo.AddressExists(address.AddressID))
                {
                    repo.DeleteAddress(address);
                }
                else
                {
                    throw new AddressManagerException("Address - AddAddress - Address already deleted.");
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public void UpdateAddress(Address address)
        {
            try
            {

            }
            catch (Exception e)
            {

                throw new Exception (e.Message);
            }
        }
       

        bool IAddressRepository.AddressExists(Address address)
        {
            throw new NotImplementedException();
        }

        void IAddressRepository.AddAddress(Address address)
        {
            throw new NotImplementedException();
        }

        public bool AddressExists(int addressID)
        {
            throw new NotImplementedException();
        }
    }
}
