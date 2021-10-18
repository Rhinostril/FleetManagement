using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Business.Tools {
    public static class SocialSecurityNumberChecker {
        //this is an entire class on its own because it might be reused later by other software

        public static bool checknormalSecurityNumber(string securitynumber) {
            bool validationsucceeded = false;
            if(securitynumber.Length == 15) {
                string DateSubString = securitynumber.Substring(0, 8); //Example: 97.10.27
                char[] DateArray = DateSubString.ToCharArray();
                if(Char.IsDigit(DateArray[0]) && Char.IsDigit(DateArray[1]) && DateArray[2] == '.' && Char.IsDigit(DateArray[3]) && Char.IsDigit(DateArray[4]) && DateArray[5] == '.' && Char.IsDigit(DateArray[6]) && Char.IsDigit(DateArray[7])) {
                    string[] DateSplitInto3 = DateSubString.Split('.');
                    int yearNumber = int.Parse(DateSplitInto3[0]);
                    int monthNumber = int.Parse(DateSplitInto3[1]);
                    if(monthNumber >= 1 && monthNumber <= 12) {
                        int dayNumber = int.Parse(DateSplitInto3[2]);
                        bool yearIsAfter2000 = false;
                        string graphicalInterpretationOfyear = "";
                        if(yearNumber <= 30) {
                            graphicalInterpretationOfyear = "20" + DateSplitInto3[0]; // if the user is born after 2000 you should add 20 to the method that checks the nr of days in the month;
                            yearIsAfter2000= true;
                        }
                        else {
                            graphicalInterpretationOfyear = "19" + DateSplitInto3[0];
                        }
                        int daysInTheMonthAndYear = DateTime.DaysInMonth(int.Parse(graphicalInterpretationOfyear), monthNumber);
                        if(dayNumber <= daysInTheMonthAndYear) {
                            //after this the  date is presumed to be okay
                            //next up is the validation of the birth number;
                            bool char8Iscorrect = false;
                            if(securitynumber.ElementAt(8)== '-') {
                                char8Iscorrect = true;
                            }
                            string amountOfBirths = securitynumber.Substring(9, 3); //Example 345
                            if(amountOfBirths.All(char.IsDigit) && amountOfBirths != "000" && char8Iscorrect) {
                                //we know the second number is only digits, and not 000
                                if(securitynumber.ElementAt(12) == '.') {
                                    string combinednumberstring = DateSplitInto3[0] + DateSplitInto3[1] + DateSplitInto3[2] + amountOfBirths;
                                    if(yearIsAfter2000) {
                                        combinednumberstring = "2" + DateSplitInto3[0] + DateSplitInto3[1] + DateSplitInto3[2] + amountOfBirths; //extra validation needed for people born after 2000
                                    }
                                    int numberToDivide = int.Parse(combinednumberstring); //Example  900201999
                                    int computedmodulo = numberToDivide % 97;
                                    int computedControlNumber = 97 - computedmodulo;
                                    string computedcontrolnumberAstext = computedControlNumber.ToString("00"); // should make numbers less than 10 into "02"
                                    if(computedcontrolnumberAstext == securitynumber.Substring(13, 2)) {
                                        validationsucceeded = true;
                                    }//this fails if security number validation is off
                                }//this fails if the char between validaiton number and birthnumber is not '.'
                            }//this fails because either the birthnumber is not all digits/ is 000 or the 8th char is not '.'
                        }//this fails if for example a user gives 30 of february as a date
                    }//this fails if the given month is not 1-12 which is the limited version of validation
                }//this fails because one of the date characters is incorrect, at this point 99.99.99 is allowed still
            }//this fails the test because string is not 15 characters long

            return validationsucceeded;
        }
        public static void checkAdvancedSecurityNumber(string SecurityNumber) {
            //TODO build a more extensive method of checking that also checks for special cases
        }
    }
}
