﻿using System;
using FleetManagement.Business.Managers;
using FleetManagement.Business.Entities;
using FleetManagement.Data.Repositories;
using System.Collections.Generic;

namespace FleetManagement.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Testing
            //TestAddFuelCard();
            TestGetAllFuelCards();






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

    }


}



