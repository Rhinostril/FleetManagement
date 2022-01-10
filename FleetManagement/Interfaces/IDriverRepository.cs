using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;

namespace FleetManagement.Business.Interfaces
{
    public interface IDriverRepository {

        IReadOnlyList<Driver> GetAllDrivers();
        IReadOnlyList<Driver> GetTop50Drivers();
        IReadOnlyList<Driver> GetDrivers(int? amount);
        IReadOnlyList<Driver> GetDriversByAmount(int amount);
        IReadOnlyList<Driver> SearchDrivers(int? id, string lastName, string firstName, DateTime? dateOfBirth, string securtiyNumber, string street, string houseNR, string postalcode);
        Driver GetDriverById(int id);
        Driver SearchDriver(int? id, string firstName, string lastName, DateTime dateOfBirth, Address address);
        bool DriverExists(int id);
        bool DriverHasVehicle(int id);
        bool DriverHasFuelCard(int id);
        void AddDriverWithAddress(Driver driver);
        void UpdateDriverWithAddress(Driver driver);
        void DeleteDriverWithAddress(Driver driver);
        bool DoesSecurityNumberExist(string securityNumber);
        void RemoveVehicleIdFromDriver(Driver driver);



    }
}
