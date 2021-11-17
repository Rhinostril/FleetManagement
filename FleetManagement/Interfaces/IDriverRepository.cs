using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;

namespace FleetManagement.Business.Interfaces
{
    public interface IDriverRepository {

        IReadOnlyList<Driver> GetAllDrivers();
        IReadOnlyList<Driver> GetTop50Drivers();
        IReadOnlyList<Driver> GetDriversByAmount(int amount);
        IReadOnlyList<Driver> SearchDrivers(int? id, string firstName, string lastName, DateTime dateOfBirth, Address address);
        Driver GetDriverById(int id);
        Driver SearchDriver(int? id, string firstName, string lastName, DateTime dateOfBirth, Address address);
        bool DriverExists(int id);
        void AddDriver(Driver driver);
        void UpdateDriver(Driver driver);
        void DeleteDriver(Driver driver);
        bool DoesSecurityNumberExist(string securityNumber);

    }
}
