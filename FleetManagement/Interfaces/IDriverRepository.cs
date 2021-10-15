using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;

namespace FleetManagement.Business.Interfaces
{
    public interface IDriverRepository
    {

        IReadOnlyList<Driver> GetAllDrivers();
        IReadOnlyList<Driver> SearchDrivers(int? id, string firstName, string lastName, DateTime dateOfBirth, Address address);
        Driver GetDriver(); //overbodig?
        Driver SearchDriver(int? id, string firstName, string lastName, DateTime dateOfBirth, Address address);
        bool DriverExists(int id);
        void AddDriver(Driver driver);
        void UpdateDriver(Driver driver);
        void DeleteDriver(Driver driver);

    }
}
