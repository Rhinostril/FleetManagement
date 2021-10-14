using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Exceptions;

namespace FleetManagement.BusinessTests {
    public class AddressTests {

        [Fact]
        public void Test_AdressID_Valid() {

            int id = 1;
            Address adr = new Address();
            adr.SetAddressID(id);
            Assert.Equal(id, adr.AddressID);
        }
        [Fact]
        public void Test_AdressID_InValid1() {

            int id = 0;
            Address adr = new Address();
            Assert.Throws<AddressException>(()=>adr.SetAddressID(id));
        }

        [Fact]
        public void Test_AdressID_InValid2() {

            int id = -1;
            Address adr = new Address();
            Assert.Throws<AddressException>(() => adr.SetAddressID(id));
        }
        [Fact]
        public void Test_AdressStreet_Valid() {

            string street = "Tolhuislaan";
            Address adr = new Address();
            adr.SetStreet(street);
            Assert.Equal(street, adr.Street);
        }
        [Fact]
        public void Test_AdressStreet_InValid1() {

            string street = "";
            Address adr = new Address();
            Assert.Throws<AddressException>(() => adr.SetStreet(street));
        }
        [Fact]
        public void Test_AdressStreet_InValid2() {

            string street = null;
            Address adr = new Address();
            Assert.Throws<AddressException>(() => adr.SetStreet(street));
        }
        [Fact]
        public void Test_AdressHouseNr_Valid() {

            string HouseNr = "34";
            Address adr = new Address();
            adr.SetHouseNr(HouseNr);
            Assert.Equal(HouseNr, adr.HouseNr);
        }
        [Fact]
        public void Test_AdressHouseNr_InValid1() {

            string HouseNr = "";
            Address adr = new Address();
            Assert.Throws<AddressException>(() => adr.SetHouseNr(HouseNr));
        }
        [Fact]
        public void Test_AdressHouseNr_InValid2() {

            string HouseNr = null;
            Address adr = new Address();
            Assert.Throws<AddressException>(() => adr.SetHouseNr(HouseNr));
        }
        [Fact]
        public void Test_AdressPostalCode_Valid() {

            string PostalCode = "9000";
            Address adr = new Address();
            adr.SetPostalCode(PostalCode);
            Assert.Equal(PostalCode, adr.PostalCode);
        }
        [Fact]
        public void Test_AdressPostalCode_InValid1() {

            string PostalCode = "";
            Address adr = new Address();
            Assert.Throws<AddressException>(() => adr.SetPostalCode(PostalCode));
        }
        [Fact]
        public void Test_AdressPostalCode_InValid2() {

            string PostalCode = null;
            Address adr = new Address();
            Assert.Throws<AddressException>(() => adr.SetPostalCode(PostalCode));
        }
        [Fact]
        public void Test_AdressCity_Valid() {

            string City = "Ledebronx";
            Address adr = new Address();
            adr.SetCity(City);
            Assert.Equal(City, adr.City);
        }
        [Fact]
        public void Test_AdressCity_InValid1() {

            string City = "";
            Address adr = new Address();
            Assert.Throws<AddressException>(() => adr.SetCity(City));
        }
        [Fact]
        public void Test_AdressCity_InValid2() {

            string City = null;
            Address adr = new Address();
            Assert.Throws<AddressException>(() => adr.SetCity(City));
        }
    }
}
