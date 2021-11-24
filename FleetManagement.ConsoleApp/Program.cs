using System;
using FleetManagement.Business.Managers;
using FleetManagement.Business.Entities;
using FleetManagement.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace FleetManagement.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            InitRepository repo = new InitRepository();

            Console.WriteLine("Working...");

            List<FuelCard> fuelCards = new List<FuelCard>();

            for(int i = 0; i < 1000; i++)
            {
                FuelCard fuelCard = RandomFuelCard();
                fuelCards.Add(RandomFuelCard());
            }

            fuelCards.GroupBy(x => x.CardNumber).Select(y => y.First()).ToList();

            foreach(FuelCard c in fuelCards)
            {
                Console.WriteLine(c.ToString());
            }

            repo.BulkInsertFuelCard(fuelCards);

            Console.WriteLine("Done!");





        }


        // CREATION OF DRIVERS
        private static List<string> firstNames = new List<string>
        {
            "Liam",
            "Noah",
            "Oliver",
            "Elijah",
            "William",
            "James",
            "Benjamin",
            "Lucas",
            "Henry",
            "Alexander",
            "Olivia",
            "Emma",
            "Ava",
            "Charlotte",
            "Sophia",
            "Amelia",
            "Isabella",
            "Mia",
            "Evelyn",
            "Harper"
        };
        private static List<string> lastNames = new List<string>
        {
            "Smith",
            "Johnson",
            "Williams",
            "Brown",
            "Jones",
            "Garcia",
            "Miller",
            "Davis",
            "Rodriguez",
            "Martinez",
            "Hernandez",
            "Lopez",
            "Gonzalez",
            "Wilson",
            "Anderson",
            "Thomas",
            "Taylor",
            "Moore",
            "Jackson",
            "Martin",
            "Lee",
            "Perez",
            "Thompson",
            "White",
            "Harris",
            "Sanchez",
            "Clark",
            "Ramirez",
            "Lewis",
            "Robinson",
            "Walker",
            "Young",
            "Allen",
            "King",
            "Wright",
            "Scott",
            "Torres",
            "Nguyen",
            "Hill",
            "Flores",
            "Green",
            "Adams",
            "Nelson",
            "Baker",
            "Hall",
            "Rivera",
            "Campbell",
            "Mitchell",
            "Carter",
            "Roberts"
        };
        private static DateTime randomDateOfBirth()
        {
            Random r = new Random();
            return new DateTime(r.Next(1965, 2000), r.Next(1, 13), r.Next(1, 28));
        }
        private static Address randomAddress()
        {
            Random r = new Random();
            string c = "abcdefghijklmnopqrstuvwxyz";
            string v = "aeiouy";
            List<string> cities = new List<string> {
                "Aberdeen",
                "Armagh",
                "Bangor",
                "Bath",
                "Belfast",
                "Birmingham",
                "Bradfort",
                "Cambridge",
                "Cardiff",
                "Carlisle",
                "Chelmsford",
                "Chester",
                "Derby",
                "Dundee",
                "Durham",
                "Edinburgh",
                "Ely",
                "Glasgow",
                "Hereford",
                "Leeds",
                "Lancaster",
                "Leicester",
                "Lichfield",
                "Lincoln",
                "Liverpool",
                "London",
                "Manchester",
                "Newcastle",
                "Newport",
                "Norwich",
                "Nottingham",
                "Oxford",
                "Petersborough"
            };

            List<string> countries = new List<string> { "Wales", "Scotland", "England", "North-America", "Ireland" };

            string street = c[r.Next(1, 27)].ToString().ToUpper() + v[r.Next(1, 7)].ToString() + c[r.Next(1, 27)].ToString() + " Road";
            string houseNr = r.Next(1, 300).ToString();
            string postalCode = r.Next(1, 10).ToString() + r.Next(1, 10).ToString() + r.Next(1, 10).ToString() + r.Next(1, 10).ToString();

            Address address = new Address(street, houseNr, postalCode, cities[r.Next(1, cities.Count())], countries[r.Next(1, countries.Count())]);

            return address;
        }
        private static string randomSecurityNumber()
        {
            return "";
        }


        // LICENSE TYPES



        // CREATION OF VEHICLE FUELTYPES LIST
        private static List<(int, int)> VehicleFuelTypes()
        {
            // FuelType ID's = 61-75
            // Vehicle ID's = 1001-2000

            Random r = new Random();
            List<(int, int)> vehicleFuelTypes = new List<(int, int)>();

            for (int v = 1001; v < 2001; v++) // LOOP EVERY VEHICLE
            {
                int fuelKind = r.Next(1, 5);
                switch (fuelKind)
                {
                    case 1: // Petrol
                        for(int f = 61; f <= 64; f++)
                        {
                            vehicleFuelTypes.Add((v, f));
                        }
                        break;
                    case 2: // Diesel
                        for (int f = 65; f <= 68; f++)
                        {
                            vehicleFuelTypes.Add((v, f));
                        }
                        break;
                    case 3: // Other
                        for (int f = 69; f <= 72; f++)
                        {
                            vehicleFuelTypes.Add((v, f));
                        }
                        break;
                    case 4: // Electric
                        for (int f = 73; f <= 75; f++)
                        {
                            vehicleFuelTypes.Add((v, f));
                        }
                        break;
                }

            }

            return vehicleFuelTypes;
        }


        // CREATION OF DUMMY DATA LISTS
        private static List<string> licenseTypes = new List<string>
        {
            "AM",
            "A1",
            "A2",
            "A",
            "B",
            "C1",
            "C",
            "D1",
            "D",
            "BE",
            "C1E",
            "CE",
            "D1E",
            "DE",
            "G"
        };
        private static List<string> fuelTypes = new List<string>
        {
            "Petrol 98",
            "Petrol 95",
            "Premium Petrol",
            "Super Petrol",
            "Diesel",
            "Super Diesel",
            "Premium Diesel",
            "Bio Diesel",
            "LPG",
            "Hydrogen",
            "Ethanol",
            "Butane",
            "Electric Basic Charge",
            "Electric Fast Charge",
            "Electric Tesla Charge"
        };
        private static List<Vehicle> vehicles = new List<Vehicle>();


        // CREATION OF RANDOM VEHICLES
        private static List<string> carBrands = new List<string>
        {
            "BMW",
            "Audi",
            "Mercedes",
            "Volkswagen",
            "Volvo",
            "Porsche",
            "Ferrari",
            "Toyota",
            "Ford",
            "Bugatti",
            "Lamborghini",
            "Bentley",
            "Aston Martin",
            "Lexus"
        };
        private static List<string> carModels = new List<string>
        {
            "911 Turbo",
            "911 GT2 RS",
            "911 GT3 RS",
            "M1",
            "M2",
            "M3",
            "M4",
            "M5",
            "Golf",
            "Golf GTI",
            "Golf R",
            "AMG C63",
            "RS6",
            "RS3",
            "1",
            "2",
            "3",
            "4",
            "5"
        };
        private static List<string> carColors = new List<string>
        {
            "Red",
            "Magenta",
            "British Racing Green",
            "Blue",
            "Silver Metallic",
            "Black",
            "Black Matte",
            "Black Metallic",
            "Dark Blue",
            "White",
            "Grey",
            "Blue Grey",
            "Melange White",
            "Yellow",
            "Brown",
            "Green"
        };
        private static List<string> carType = new List<string>
        {
            "Personenwagen",
            "Sportwagen",
            "SUV",
            "Stationwagen",
            "Bestelwagen"
        };
        private static string RandomChassis()
        {
            Random r = new Random();
            string alph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string chassis = "";
            for (int i = 0; i < 8; i++)
            {
                chassis += alph[r.Next(0, 26)];
            }
            for (int i = 8; i < 17; i++)
            {
                chassis += r.Next(0, 10);
            }
            return chassis;
        }
        private static string RandomLicense()
        {
            Random r = new Random();
            string alph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string license = "";
            license += r.Next(0, 2);
            license += "-";
            for (int i = 0; i < 3; i++)
            {
                license += alph[r.Next(0, 26)];
            }
            license += "-";
            for (int i = 0; i < 3; i++)
            {
                license += r.Next(0, 10);
            }
            return license;
        }
        private static string RndOfList(List<string> list)
        {
            Random r = new Random();
            return list[r.Next(0, list.Count - 1)];
        }
        private static List<Vehicle> CreateVehicles()
        {
            Random r = new Random();
            List<Vehicle> vehicles = new List<Vehicle>();

            for (int i = 0; i < 1000; i++)
            {
                Vehicle vehicle = new Vehicle(RndOfList(carBrands), RndOfList(carModels), RandomChassis(), RandomLicense(), new List<FuelType> { new FuelType("Diesel") }, RndOfList(carType), RndOfList(carColors), r.Next(5, 8));
                vehicles.Add(vehicle);
            }

            return vehicles;
        }
        private static void TestBulkInsertVehicle()
        {
            try
            {
                InitRepository repo = new InitRepository();
                repo.BulkInsertVehicle(CreateVehicles());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // CREATION OF RANDOM FUELCARDS
        private static string RandomCardNumber()
        {
            Random r = new Random();
            string cardNr = "";
            for (int i = 0; i < 6; i++)
            {
                cardNr += r.Next(0, 10).ToString();
            }
            return cardNr;
        }
        private static string RandomPin()
        {
            string pin = "";
            Random r = new Random();
            for (int i = 0; i < 4; i++)
            {
                pin += r.Next(0, 10).ToString();
            }
            return pin;
        }
        private static DateTime RandomValidityDate()
        {
            Random r = new Random();
            DateTime dte = new DateTime(2021 + r.Next(5, 11), r.Next(1, 13), r.Next(1, 28));
            return dte;
        }
        private static FuelCard RandomFuelCard()
        {
            FuelCard fuelCard = new FuelCard(RandomCardNumber(), RandomValidityDate(), RandomPin(), true);
            return fuelCard;
        }







        private static void TestInsertLicenseTypes()
        {
            InitRepository repo = new InitRepository();
            foreach (string s in licenseTypes)
            {
                repo.InsertLicenseType(s);
                Console.WriteLine(s);
            }
        }
        private static void TestInsertFuelTypes()
        {
            InitRepository repo = new InitRepository();
            foreach (string s in fuelTypes)
            {
                repo.InsertFuelType(s);
                Console.WriteLine(s);
            }
        }
        private static void TestAddFuelCard()
        {
            FuelCardManager fuelCardManager = new FuelCardManager(new FuelCardRepository());

            FuelCard fuelCard = new FuelCard("123456789", new DateTime(2021, 10, 27), "0123", new List<FuelType>(), true);

            Console.WriteLine("TEST Add Fuelcard: ");
            Console.WriteLine(fuelCard.ToString());

            try
            {
                FuelCardRepository repo = new FuelCardRepository();
                repo.AddFuelCard(fuelCard);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} \n {ex}");
            }
        }
        private static void TestGetAllFuelCards()
        {
            try
            {
                FuelCardRepository repo = new FuelCardRepository();
                Console.WriteLine(repo.GetAllFuelCards().Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} \n {ex}");
            }
        }
        private static void TestGetFuelCard()
        {
            try
            {
                FuelCardRepository repo = new FuelCardRepository();
                Console.WriteLine(repo.GetFuelCard(1).ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        




    }

}




