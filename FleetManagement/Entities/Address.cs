using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Exceptions;

namespace FleetManagement.Business.Entities {
    public class Address {
        public int AddressID { get; private set; }
        public string Street { get; private set; }
        public string HouseNr { get; private set; }
        public string PostalCode { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }


        public void SetAddressID(int ID) {
            if(ID > 0) {
                AddressID = ID;
            }else {
                throw new AddressException("AddressID cannot be 0 or negative");
            }
            
        }
        public void SetStreet(string street) {
            if(!String.IsNullOrEmpty(street)) {
                Street = street;
            }
            else {
                throw new AddressException("Address street cannot be empty or missing");
            }

        }
        public void SetHouseNr(string houseNr) {
           
            if(!String.IsNullOrEmpty(houseNr)) {
                HouseNr = houseNr;
            }
            else {
                throw new AddressException("Address housenumber cannot be empty or missing");
            }

        }
        public void SetPostalCode(string postalcode) {
            

            if(!String.IsNullOrEmpty(postalcode)) {
                PostalCode = postalcode;
            }
            else {
                throw new AddressException("Address postalcode cannot be empty or missing");
            }
        }
        public void SetCity(string city) {

            if(!String.IsNullOrEmpty(city)) {
                City = city;
            }
            else {
                throw new AddressException("Address city cannot be empty or missing");
            }
           
        }
        public void SetCountry(string country) {
           
            if(!String.IsNullOrEmpty(country)) {
                Country = country;
            }
            else {
                throw new AddressException("Address city cannot be empty or missing");
            }
        }

    }
}
