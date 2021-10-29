using System;
using FleetManagement.Business.Managers;
using FleetManagement.Business.Entities;
using FleetManagement.Data.Repositories;

namespace FleetManagement.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Testing
            TestAddFuelCard();






        }

        private static void TestAddFuelCard()
        {
            FuelCardManager fuelCardManager = new FuelCardManager(new FuelCardRepository());

            FuelCard fuelCard = new FuelCard("123456789", new DateTime(2021, 10, 27), "0123", new FuelType("Diesel"), true);

            Console.WriteLine("TEST Add Fuelcard: ");
            Console.WriteLine(fuelCard.ToString());

            try
            {
                fuelCardManager.AddFuelCard(fuelCard);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} \n {ex.StackTrace} \n {ex.Source} \n {ex.InnerException}");
            }
        }

    }


}



