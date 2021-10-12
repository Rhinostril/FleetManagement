
using FleetManagement.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Tools;
using Xunit;

namespace FleetManagement.Business.Entities.Tests {

    public class SecurityNumberTests {
        private Dictionary<int, string> testSecurityNumbers;

        public SecurityNumberTests() {
            testSecurityNumbers = new Dictionary<int, string>();
            testSecurityNumbers.Add(1, "97.10.27-363.61");
            testSecurityNumbers.Add(2, "97,10,27-363,61");//fail reason commas
            testSecurityNumbers.Add(3, "97.10.27-363");   //fail reason missing number
            testSecurityNumbers.Add(4, "97.10.27-000.61");//fail reason 000 number
            testSecurityNumbers.Add(5, "97.10.27 363.61");//fail reason space instead of -
            testSecurityNumbers.Add(6, "97.10.27-363.62");//fail reason wrong control number
        }

        [Fact]
        public void Test_Control_Valid() {
            bool result = SocialSecurityNumberChecker.checknormalSecurityNumber(testSecurityNumbers[1]);
            Assert.True(result);
        }
        [Fact]
        public void Test_Expected_Invalid() {
            for(int i =2; i <= 6 ;i++) {

                bool result = SocialSecurityNumberChecker.checknormalSecurityNumber(testSecurityNumbers[i]);
                Assert.False(result);
            }
        }

    }
}
