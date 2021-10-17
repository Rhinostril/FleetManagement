using FleetManagement.Business.Exceptions;
using FleetManagement.Business.Tools;
using System;
using System.Collections.Generic;
using System.Linq; 

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

            try {
                DriversLicenceType = new List<string>();
                SetFirstName(firstname);
                SetLastName(lastname);
                SetDateOfBirth(dateofBirth);
                SetDriversLicensetypes(licensetypes);
                SetSecurityNumber(securitynumber);
            }catch(Exception ex) {
                throw new Exception(ex.Message);
            }

        }
        //maybe add another constructor with more options and use this as :base
        private void SetFirstName(string firstname) {
            string trimmed = firstname.Trim();
            if(String.IsNullOrEmpty(trimmed)) {
                throw new DriverException("Cannot set an empty first name");
            }
            else {
                FirstName = firstname;
            }
           
        }
        private void SetLastName(string lastname) {
            string trimmed = lastname.Trim();
            if(String.IsNullOrEmpty(trimmed)) {
                throw new DriverException("Cannot set an empty last name");
            }
            else {
                LastName = lastname;
            }
            
        }
        private void SetDateOfBirth(DateTime dateofbirth) {
            TimeSpan offset = DateTime.Now - dateofbirth;
            if(offset.TotalDays <= 0) {
                throw new DriverException("Date of birth cannot be in the future");
            }else if(offset.TotalDays >= 36524) {
                throw new DriverException("Date of birth cannot be more than 100 years in the past");
            }
            else {
                DateOfBirth = dateofbirth;
            }
         
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

        private void SetDriversLicensetypes(List<string> licensetypes) { //feedback leerkrach

            //ASK client: Do we check if this list contains any licenses?
            //Usecase where we might not need license : self driving cars

            //bool iscollectionempty = licensetypes.Any();
            //if(iscollectionempty) {
            //    throw new DriverException("driver must have at least 1 driverslicense");
            //}
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
            else { 
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
        public void SetVehicle(Vehicle newvehicle) {
            if(newvehicle != null) {
                if(this.Vehicle != newvehicle) {
                    //case where the new vehicle isnt the same as the current one
                    if(this.Vehicle.HasDriver(this)) {
                        this.Vehicle.RemoveDriver(); //if the previous vehicle still has this driver, remove it
                    }
                    if(!newvehicle.HasDriver(this)) {
                        newvehicle.RemoveDriver();
                        newvehicle.SetDriver(this);
                    }
                    this.Vehicle = newvehicle;
                }
                else {//case where we try to set the exact same vehicle
                    throw new VehicleException("Driver - Setvehicle:  vehicle not new");
                }
            }
            else {
                throw new DriverException("Driver - Setvehicle: new vehicle is null");
            }     
            
        }
        public void RemoveVehicle() {
            Vehicle = null;
        }
        public bool HasVehicle(Vehicle vehicle) {
            if(Vehicle != null) {
                if(this.Vehicle == vehicle) {
                    return true;
                }
                else { 
                    return false;
                }
            }
            else {
                return false;
            }
        }
        
        public bool TryRemoveVehicle(Vehicle vehicle) {
            if(Vehicle == vehicle) {
                Vehicle = null;
                return true;
            }else {
                throw new DriverException("Vehicle cannot be removed because the driver does not drive this exact vehicle");
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
        public void RemoveFuelCard() {
            FuelCard = null;
        }
        public bool HasFuelCard(FuelCard fuelcard) {
            if(FuelCard != null) {
                if(this.FuelCard == fuelcard) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }

    }
}
