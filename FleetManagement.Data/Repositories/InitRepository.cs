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

            string query = "INSERT INTO FuelType (name)" +
                           "VALUES (@name)";

            using(SqlCommand command = new SqlCommand())
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

        public void InsertFuelType(FuelType fuelType)
        {
            SqlConnection connection = getConnection();

            string query = "INSERT INTO FuelType (name)" +
                           "VALUES (@name)";

            using (SqlCommand command = new SqlCommand())
            {
                try
                {
                    connection.Open();

                    command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                    command.Parameters["@name"].Value = fuelType.FuelName;

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

    }
}
