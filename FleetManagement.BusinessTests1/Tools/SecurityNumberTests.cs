
using FleetManagement.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Tools;
using Xunit;

namespace FleetManagement.Business.Tools.Tests {

    public class SecurityNumberTests {
        private Dictionary<int, string> ValidtestSecurityNumbers;
        private Dictionary<int, string> InValidtestSecurityNumbers;
        public SecurityNumberTests() {
           
            ValidtestSecurityNumbers = new Dictionary<int, string>();
            ValidtestSecurityNumbers.Add(1, "97.10.27-363.61"); 
            ValidtestSecurityNumbers.Add(2, "00.06.30-283.52"); //person born in 2000 -> validation works

            InValidtestSecurityNumbers = new Dictionary<int, string>();
            InValidtestSecurityNumbers.Add(1, "97,10,27-363,61");//fail reason commas
            InValidtestSecurityNumbers.Add(2, "97.10.27-363");   //fail reason missing number
            InValidtestSecurityNumbers.Add(3, "97.10.27-000.61");//fail reason 000 at birth number
            InValidtestSecurityNumbers.Add(4, "97.10.27 363.61");//fail reason space instead of -
            InValidtestSecurityNumbers.Add(5, "97.10.27-363.62");//fail reason wrong control number
        }

        [Fact]
        public void Test_Control_Valid() {
            foreach(int key in ValidtestSecurityNumbers.Keys) {
                bool result = SocialSecurityNumberChecker.checknormalSecurityNumber(ValidtestSecurityNumbers[key]);
                Assert.True(result);
            }
        }
        [Fact]
        public void Test_Expected_Invalid() {
            foreach(int key in InValidtestSecurityNumbers.Keys) {
                bool result = SocialSecurityNumberChecker.checknormalSecurityNumber(InValidtestSecurityNumbers[key]);
                Assert.False(result);
            }
        }

    }
}
