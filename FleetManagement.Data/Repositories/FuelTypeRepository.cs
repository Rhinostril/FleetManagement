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
    public class FuelTypeRepository : IFuelTypeRepository
    {

        private string connectionString = $"Data Source=tcp:fleetmanagserver.database.windows.net,1433;Initial Catalog=dboFleetmanagement;Persist Security Info=False;User ID=fleetadmin;Password=$qlpassw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public void AddFuelTypeToFuelCard(int fuelTypeId, int fuelCardId)
        {
            SqlConnection connection = GetConnection();
            string query = $"INSERT INTO [FuelCardFuelType] (fuelCardId, fuelTypeId) VALUES (@fuelCardId, @fuelTypeId)";
            using(SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@fuelCardId", SqlDbType.Int));
                    command.Parameters["@fuelCardId"].Value = fuelCardId;
                    command.Parameters.Add(new SqlParameter("@fuelTypeId", SqlDbType.Int));
                    command.Parameters["@fuelTypeId"].Value = fuelTypeId;
                    command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    throw new Exception("FuelTypeRepository - AddFuelTypToFuelCard", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void AddFuelTypeToVehicle(int fuelTypeId, int vehicleId)
        {
            SqlConnection connection = GetConnection();
            string query = $"INSERT INTO [VehicleFuelType] (vehicleId, fuelTypeId) VALUES (@vehicleId, @fuelTypeId)";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@vehicleId", SqlDbType.Int));
                    command.Parameters["@vehicleId"].Value = vehicleId;
                    command.Parameters.Add(new SqlParameter("@fuelTypeId", SqlDbType.Int));
                    command.Parameters["@fuelTypeId"].Value = fuelTypeId;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("FuelTypeRepository - AddFuelTypeToVehicle", ex);
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
            string query = $"SELECT * FROM FuelType";
            using (SqlCommand command = connection.CreateCommand())
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
                catch (Exception ex)
                {
                    throw new Exception("FuelTypeRepository - GetAllFuelTypes()", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void RemoveFuelTypeFromFuelCard(int fuelTypeId, int fuelCardId)
        {
            SqlConnection connection = GetConnection();
            string query = $"DELETE FROM [FuelCardFuelType] WHERE fuelCardId=@fuelCardId AND fuelTypeId=@fuelTypeId";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@fuelCardId", SqlDbType.Int));
                    command.Parameters["@fuelCardId"].Value = fuelCardId;
                    command.Parameters.Add(new SqlParameter("@fuelTypeId", SqlDbType.Int));
                    command.Parameters["@fuelTypeId"].Value = fuelTypeId;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("FuelTypeRepository - AddFuelTypeToVehicle", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void RemoveFuelTypeFromVehicle(int fuelTypeId, int vehicleId)
        {
            SqlConnection connection = GetConnection();
            string query = $"DELETE FROM [VehicleFuelType] WHERE vehicleId=@vehicleId AND fuelTypeId=@fuelTypeId";
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@vehicleId", SqlDbType.Int));
                    command.Parameters["@vehicleId"].Value = vehicleId;
                    command.Parameters.Add(new SqlParameter("@fuelTypeId", SqlDbType.Int));
                    command.Parameters["@fuelTypeId"].Value = fuelTypeId;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("FuelTypeRepository - AddFuelTypeToVehicle", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
