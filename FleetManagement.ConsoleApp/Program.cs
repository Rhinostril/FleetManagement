using System;
using FleetManagement.Business.Managers;
using FleetManagement.Business.Entities;
using FleetManagement.Data.Repositories;
using System.Collections.Generic;

namespace FleetManagement.ConsoleApp
{
    class Program
    {

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


        static void Main(string[] args)
        {
            // Testing
            //TestAddFuelCard();
            //TestGetAllFuelCards();
            //TestGetFuelCard();

            TestInsertFuelTypes();




        }

        private static void TestInsertLicenseTypes()
        {
            InitRepository repo = new InitRepository();
            foreach(string s in licenseTypes)
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }


}



