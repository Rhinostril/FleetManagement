using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            setFirstName(firstname);
            setLastName(lastname);
            setDateOfBirth(dateofBirth);
            setDriversLicensetypes(licensetypes);
            setSecurityNumber(securitynumber);
        }
        //maybe add another constructor with more options and use this as :base
        private void setFirstName(string firstname) {
            FirstName = firstname;
        }
        private void setLastName(string lastname) {
            LastName = lastname;
        }
        private void setDateOfBirth(DateTime dateofbirth) {
            //possible check if Date is not past 150 years or so --> ask client
            DateOfBirth = dateofbirth;
        }
        public void setSecurityNumber(string securtiynumber) {
            //this is public because this parameter can concievably still change after it is set
            if(validateSecurityNumber(securtiynumber)) {
                SecurityNumber = securtiynumber;
            }
            else {
                throw new DriverException("The given social security number is invalid");
            }

        }
        private bool validateSecurityNumber(string securitynumber) {
            //this method validates the inputted SN strictly, it is invalid until proven otherwise
            //security number that should pass this filter:
            //97.10.27-363.61
            bool validationsucceeded = false;
            if(securitynumber.Length == 15) {
                string DateSubString = securitynumber.Substring(0, 8); //Example: 97.10.27
                char[] DateArray = DateSubString.ToCharArray();
                if(Char.IsDigit(DateArray[0]) && Char.IsDigit(DateArray[1]) && DateArray[2] == '.' && Char.IsDigit(DateArray[3]) && Char.IsDigit(DateArray[4]) && DateArray[5] == '.' && Char.IsDigit(DateArray[6]) && Char.IsDigit(DateArray[7])) {
                    string[] DateSplitInto3 = DateSubString.Split('.');
                    int yearNumber = int.Parse(DateSplitInto3[0]);
                    int monthNumber = int.Parse(DateSplitInto3[1]);
                    bool monthIsBetweenZeroAndTwelve = false;
                    if(monthNumber >= 0 && monthNumber <= 12) {
                        monthIsBetweenZeroAndTwelve = true;
                        //this captures month 00 on purpose
                    }
                    if(monthIsBetweenZeroAndTwelve || (monthNumber>40 && monthNumber<=52) || (monthNumber>20 && monthNumber<=32)) {
                        int dayNumber = int.Parse(DateSplitInto3[2]);
                        bool zeromonthcheckpassed = true;
                        if(monthNumber == 0) {
                            //monthnumber was 0 so day needs to also be 0
                           if(dayNumber != 0) {
                                zeromonthcheckpassed = false;
                            }
                        } else {
                            if(dayNumber == 0) {
                                zeromonthcheckpassed = false;
                            }
                        }
                        if(DateTime.DaysInMonth(yearNumber, monthNumber) <= dayNumber && zeromonthcheckpassed) {




                        }//the given day number is not correct for the year and the month or it cant be 0 if the month isnt also 0
                    }//any month ranges that are not 0-12 or 20/40 are not allowed
                }//this fails because one of the date characters is incorrect, at this point 99.99.99 is allowed still
            }//this fails the test because string is not 15 characters long

            return validationsucceeded;
        }

        private void setDriversLicensetypes(List<string> licensetypes) {
            DriversLicenceType.Clear();
            DriversLicenceType.AddRange(licensetypes);
        }
        public void addTypeToDriversLicense(List<string> licenseTypesToAdd) {
            foreach(string type in licenseTypesToAdd) {
                if(!DriversLicenceType.Contains(type)) {
                    //the list does not contain this type yet and it can be added
                    DriversLicenceType.Add(type);
                }//else it should be ignored
            }
        }

        public void setAdress(Address address) {
            Address = address;
        }
        public void removeAdress(Address adress) {
            if(Address == adress) {
                Address = null;
            }
            else {
                throw new DriverException("Can't remove the targeted address because it is not the same as the given address");
            }
        }
        public void setDriverID(int driverid) {
            DriverID = driverid;
        }
        public void setVehicle(Vehicle vehicle) {
            Vehicle = vehicle;
        }

        public bool TryRemoveVehicle(Vehicle vehicle) {
            if(Vehicle == vehicle) {
                Vehicle = null;
                return true;
            }else {
                throw new DriverException("Vehicle cannot be removed because the driver does not drive this exact vehicle");
                return false;
            }
        }
        public void setFuelCard(FuelCard fuelcard) {
            FuelCard = fuelcard;
        }
        public void removeAnyFuelCard() {
            FuelCard = null;
        }
        public bool tryRemoveSpecificFuelCard(FuelCard fuelcard) {
            if(FuelCard != null && FuelCard == fuelcard) {
                FuelCard = null;
                return true;
            }else {
                throw new DriverException("Unable to remove this specific fuel card");
                return false;
            }
        }


    }
}
