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
    public class LicenseTypeRepository : ILicenseTypeRepository
    {

        private string connectionString = $"Data Source=tcp:fleetmanagserver.database.windows.net,1433;Initial Catalog=dboFleetmanagement;Persist Security Info=False;User ID=fleetadmin;Password=$qlpassw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public IReadOnlyList<LicenseType> GetAllLicenseTypes()
        {
            SqlConnection connection = GetConnection();
            string query = $"SELECT * FROM LicenseType";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader();
                    List<LicenseType> licenseTypes = new List<LicenseType>();
                    while (reader.Read())
                    {
                        licenseTypes.Add(new LicenseType((int)reader["licenseTypeId"], (string)reader["name"]));
                    }
                    return licenseTypes.AsReadOnly();
                }
                catch (Exception ex)
                {
                    throw new Exception("LicenseTypeRepository - GetAllLicenseTypes()", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void AddLicenseTypeToDriver(int licenseTypeId, int driverId)
        {
            SqlConnection connection = GetConnection();
            string query = $"INSERT INTO [DriverLicenseType] (driverId, licenseTypeId) VALUES (@driverId, @licenseTypeId)";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                    command.Parameters["@driverId"].Value = driverId;
                    command.Parameters.Add(new SqlParameter("@licenseTypeId", SqlDbType.Int));
                    command.Parameters["@licenseTypeId"].Value = licenseTypeId;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("LicenseTypeRepository - AddLicenseTypeToDriver", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void RemoveLicenseTypeFromDriver(int licenseTypeId, int driverId)
        {
            SqlConnection connection = GetConnection();
            string query = $"REMOVE FROM [DriverLicenseType] WHERE driverId=@driverId AND licenseTypeId=@licenseTypeId";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@driverId", SqlDbType.Int));
                    command.Parameters["@driverId"].Value = driverId;
                    command.Parameters.Add(new SqlParameter("@licenseTypeId", SqlDbType.Int));
                    command.Parameters["@licenseTypeId"].Value = licenseTypeId;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("LicenseTypeRepository - RemoveLicenseTypeFromDriver", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }





    }
}
