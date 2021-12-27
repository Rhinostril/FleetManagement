using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using FleetManagement.Business.Interfaces;
using FleetManagement.Business.Entities;

namespace FleetManagement.Data.Repositories
{
    public class DataToListRepository
    {

        private string connectionString = $"Data Source=tcp:fleetmanagserver.database.windows.net,1433;Initial Catalog=dboFleetmanagement;Persist Security Info=False;User ID=fleetadmin;Password=$qlpassw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public DataToListRepository()
        {

        }

        public IReadOnlyList<string> GetAllLicenseTypes()
        {
            SqlConnection connection = GetConnection();
            string query = $"SELECT * FROM [LicenseType]";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader();
                    List<string> licenseTypes = new List<string>();
                    while (reader.Read())
                    {
                        licenseTypes.Add((string)reader["name"]);
                    }
                    return licenseTypes.AsReadOnly();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetAllFuelTypes()", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IReadOnlyList<FuelType> GetAllFuelTypes()
        {
            SqlConnection connection = GetConnection();
            string query = $"SELECT * FROM [FuelType]";
            using(SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader();
                    List<FuelType> fuelTypes = new List<FuelType>();
                    while (reader.Read())
                    {
                        fuelTypes.Add(new FuelType((int)reader["fuelTypeId"], (string)reader["name"]));
                    }
                    return fuelTypes.AsReadOnly();
                }
                catch(Exception ex)
                {
                    throw new Exception("GetAllFuelTypes()", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }


    }
}
