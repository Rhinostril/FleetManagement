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

        public void BulkInsertDriver(List<Driver> drivers)
        {

            // TODO

        }

        public void BulkInsertFuelCard(List<FuelCard> fuelCards)
        {

            // TODO

        }

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
        }







    }
}
