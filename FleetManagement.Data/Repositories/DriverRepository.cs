using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;
using System.Data.SqlClient;

namespace FleetManagement.Data
{
    public class DriverRepository : IDriverRepository
    {

        private string connectionString = $"Data Source=fleetmanagserver.database.windows.net;Persist Security Info=True;User ID=fleetadmin;Password=***********";

        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public void AddDriver(Driver driver)
        {
            throw new NotImplementedException();
        }

        public void DeleteDriver(Driver driver)
        {
            throw new NotImplementedException();
        }

        public bool DriverExists(int id)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Driver> GetAllDrivers()
        {
            throw new NotImplementedException();
        }

        public Driver GetDriver()
        {
            throw new NotImplementedException();
        }

        public Driver SearchDriver(int? id, string firstName, string lastName, DateTime dateOfBirth, Address address)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Driver> SearchDrivers(int? id, string firstName, string lastName, DateTime dateOfBirth, Address address)
        {
            throw new NotImplementedException();
        }

        public void UpdateDriver(Driver driver)
        {
            throw new NotImplementedException();
        }
    }
}
