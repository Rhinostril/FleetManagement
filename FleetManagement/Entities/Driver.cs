using FleetManagement.Business.Exceptions;
using FleetManagement.Business.Tools;
using System;
using System.Collections.Generic;

namespace FleetManagement.Business.Entities
{
    public class Driver {

        public int DriverID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Address Address { get; private set; }
        public string SecurityNumber { get; private set; }
        public List<string> DriversLicenceType { get; private set; }
        public Vehicle Vehicle { get; private set; }
        public FuelCard FuelCard { get; private set; }

        public Driver(string firstname, string lastname, DateTime dateofBirth, string securitynumber, List<string> licensetypes) {
            DriversLicenceType = new List<string>();
            SetFirstName(firstname);
            SetLastName(lastname);
            SetDateOfBirth(dateofBirth);
            SetDriversLicensetypes(licensetypes);
            SetSecurityNumber(securitynumber);
        }
        //maybe add another constructor with more options and use this as :base
        private void SetFirstName(string firstname) {
            FirstName = firstname;
        }
        private void SetLastName(string lastname) {
            LastName = lastname;
        }
        private void SetDateOfBirth(DateTime dateofbirth) {
            //possible check if Date is not past 150 years ago or so --> ask client
            DateOfBirth = dateofbirth;
        }
        public void SetSecurityNumber(string securtiynumber) {
            //this is public because this parameter can concievably still change after it is set
            bool validationSucceeded = SocialSecurityNumberChecker.checknormalSecurityNumber(securtiynumber);
            if(validationSucceeded) {
                SecurityNumber = securtiynumber;
            }
            else {
                throw new SecurityNumberException("The given social security number is invalid");
            }
        }

        private void SetDriversLicensetypes(List<string> licensetypes) {
            DriversLicenceType.Clear();
            DriversLicenceType.AddRange(licensetypes);
        }
        public void AddTypeToDriversLicense(List<string> licenseTypesToAdd) {
            foreach(string type in licenseTypesToAdd) {
                if(!DriversLicenceType.Contains(type)) {
                    //the list does not contain this type yet and it can be added
                    DriversLicenceType.Add(type);
                }//else it should be ignored
            }
        }

        public void SetAddress(Address address) {
            if(address != null)
            {
                Address = address;
            }
            else
            {
                throw new DriverException("Address cannot be null or empty");
            }

        }

        public void RemoveAddress(Address address) {
            if(Address == address) {
                Address = null;
            }
            else {
                throw new DriverException("Can't remove the targeted address because it is not the same as the given address");
            }
        }
        public void SetDriverID(int driverId) {
            if(driverId > 0)
            {
                DriverID = driverId;
            }
            else
            {
                throw new DriverException("Driver id needs to be greater than 0");
            }
        }
        public void SetVehicle(Vehicle vehicle) {
            if(vehicle != null)
            {
                Vehicle = vehicle;
            }
            else
            {
                throw new DriverException("Vehicle cannot be null or empty");
            }
        }

        public bool TryRemoveVehicle(Vehicle vehicle) {
            if(Vehicle == vehicle) {
                Vehicle = null;
                return true;
            }else {
                throw new DriverException("Vehicle cannot be removed because the driver does not drive this exact vehicle");
                //return false;
            }
        }
        public void SetFuelCard(FuelCard fuelCard) {
            if(fuelCard != null)
            {
                FuelCard = fuelCard;
            }
            else
            {
                throw new DriverException("Fuelcard cannot be empty or null");
            }
        }
        public void RemoveAnyFuelCard() {
            FuelCard = null;
        }
        public bool TryRemoveSpecificFuelCard(FuelCard fuelcard) {
            if(FuelCard != null && FuelCard == fuelcard) {
                FuelCard = null;
                return true;
            }else {
                throw new DriverException("Unable to remove this specific fuel card");
                //return false;
            }
        }


    }
}
