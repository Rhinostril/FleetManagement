using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using FleetManagement.Business.Interfaces;
using FleetManagement.Business.Entities;

namespace FleetManagement.Data.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        
        private string connectionString = $"Data Source=fleetmanagserver.database.windows.net;Persist Security Info=True;User ID=fleetadmin;Password=***********";
        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
        public void AddAddress(Address address)
        {
            throw new NotImplementedException();
        }

        public bool AddressExists(Address address)
        {
            throw new NotImplementedException();
        }

        public bool AddressExists(int addressID)
        {
            throw new NotImplementedException();
        }

        public void DeleteAddress(Address address)
        {
            throw new NotImplementedException();
        }

        public void UpdateAddress(Address address)
        {
            throw new NotImplementedException();
        }






    }
}
