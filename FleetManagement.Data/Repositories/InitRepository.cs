using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using FleetManagement.Business.Entities;

namespace FleetManagement.Data.Repositories
{
    public class InitRepository
    {

        private string connectionString = $"Data Source=tcp:fleetmanagserver.database.windows.net,1433;Initial Catalog=dboFleetmanagement;Persist Security Info=False;User ID=fleetadmin;Password=$qlpassw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public void InsertLicenseType(string licenseType)
        {
            SqlConnection connection = getConnection();

            string query = "INSERT INTO LicenseType (name)" +
                           "VALUES (@name)";

            using(SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();

                    command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                    command.Parameters["@name"].Value = licenseType;

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void InsertFuelType(string fuelType)
        {
            SqlConnection connection = getConnection();

            string query = "INSERT INTO FuelType (name)" +
                           "VALUES (@name)";

            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();

                    command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                    command.Parameters["@name"].Value = fuelType;

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        // BULK COPY

        public void BulkInsertAddress(List<Address> addresses)
        {

            // TODO

        }

        public void BulkInsertLicenseTypes(List<string> licenseTypes)
        {
            using (SqlBulkCopy bc = new SqlBulkCopy(connectionString))
            {
                DataTable dt = new DataTable("LicenseType");

                dt.Columns.Add(new DataColumn("name", typeof(string)));

                foreach (string s in licenseTypes)
                {
                    dt.Rows.Add(s);
                }

                bc.DestinationTableName = "LicenseType";

                try
                {
                    bc.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
        }


        public void BulkInsertDriver(List<Driver> drivers)
        {
            using (SqlBulkCopy bc = new SqlBulkCopy(connectionString))
            {
                DataTable dt = new DataTable("driver");

                dt.Columns.Add(new DataColumn("driverId", typeof(int)));
                dt.Columns.Add(new DataColumn("firstname", typeof(string)));
                dt.Columns.Add(new DataColumn("lastname", typeof(string)));
                dt.Columns.Add(new DataColumn("dateOfBirth", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("addressId", typeof(int)));
                dt.Columns.Add(new DataColumn("securityNumber", typeof(int)));
                dt.Columns.Add(new DataColumn("vehicleId", typeof(int)));
                dt.Columns.Add(new DataColumn("fuelcardId", typeof(int)));

                foreach (var driver in drivers)
                {
                    dt.Rows.Add(null, driver.DriverID, driver.FirstName, driver.LastName, driver.DateOfBirth, driver.Address, driver.SecurityNumber, driver.Vehicle, driver.FuelCard);
                }

                bc.DestinationTableName = "driver";

                try
                {
                    bc.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }

        }

        public void BulkInsertFuelCard(List<FuelCard> fuelCards)
        {
            using (SqlBulkCopy bc = new SqlBulkCopy(connectionString))
            {
                DataTable dt = new DataTable("FuelCard");

                dt.Columns.Add(new DataColumn("fuelCardId", typeof(int)));
                dt.Columns.Add(new DataColumn("cardNumber", typeof(string)));
                dt.Columns.Add(new DataColumn("validityDate", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("pin", typeof(string)));
                dt.Columns.Add(new DataColumn("isEnabled", typeof(bool)));
                dt.Columns.Add(new DataColumn("driverId", typeof(int)));

                foreach (FuelCard card in fuelCards)
                {
                    dt.Rows.Add(null, card.CardNumber, card.ValidityDate, card.Pin, true, null);
                }

                bc.DestinationTableName = "FuelCard";

                try
                {
                    bc.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }

        } // DONE

        public void BulkInsertVehicle(List<Vehicle> vehicles)
        {
            using(SqlBulkCopy bc = new SqlBulkCopy(connectionString))
            {
                DataTable dt = new DataTable("Vehicle");

                dt.Columns.Add(new DataColumn("vehicleId", typeof(int)));
                dt.Columns.Add(new DataColumn("brand", typeof(string)));
                dt.Columns.Add(new DataColumn("model", typeof(string)));
                dt.Columns.Add(new DataColumn("chassisNumber", typeof(string)));
                dt.Columns.Add(new DataColumn("licensePlate", typeof(string)));
                dt.Columns.Add(new DataColumn("vehicleType", typeof(string)));
                dt.Columns.Add(new DataColumn("color", typeof(string)));
                dt.Columns.Add(new DataColumn("doors", typeof(int)));

                foreach(var vehicle in vehicles)
                {
                    dt.Rows.Add(null,vehicle.Brand, vehicle.Model, vehicle.ChassisNumber, vehicle.LicensePlate, vehicle.VehicleType, vehicle.Color, vehicle.Doors);
                }

                bc.DestinationTableName = "Vehicle";

                try
                {
                    bc.WriteToServer(dt);
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
        } // DONE

    
        // CONNECTING FUELTYPES TO VEHICLES IN TABLE VEHICLEFUELTYPES
        public void BulkInsertVehicleFuelType(List<(int, int)> vehicleFueltypes)
        {
            using (SqlBulkCopy bc = new SqlBulkCopy(connectionString))
            {
                DataTable dt = new DataTable("VehicleFuelType");

                dt.Columns.Add(new DataColumn("vehicleId", typeof(int)));
                dt.Columns.Add(new DataColumn("fuelTypeId", typeof(int)));

                foreach (var vehicleFuelType in vehicleFueltypes)
                {
                    dt.Rows.Add(vehicleFuelType.Item1, vehicleFuelType.Item2);
                }

                bc.DestinationTableName = "VehicleFuelType";

                try
                {
                    bc.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }

        } // DONE

        public void BulkInsertFuelCardFuelType(List<(int, int)> fuelCardFueltypes)
        {
            using (SqlBulkCopy bc = new SqlBulkCopy(connectionString))
            {
                DataTable dt = new DataTable("FuelCardFuelType");

                dt.Columns.Add(new DataColumn("fuelCardId", typeof(int)));
                dt.Columns.Add(new DataColumn("fuelTypeId", typeof(int)));

                foreach (var fuelCardFuelType in fuelCardFueltypes)
                {
                    dt.Rows.Add(fuelCardFuelType.Item1, fuelCardFuelType.Item2);
                }

                bc.DestinationTableName = "FuelCardFuelType";

                try
                {
                    bc.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }

        }


    }
}
