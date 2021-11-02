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
    public class AddressRepository : IAddressRepository
    {
      
        private string connectionString = $"Data Source=tcp:fleetmanagserver.database.windows.net,1433;Initial Catalog=dboFleetmanagement;Persist Security Info=False;User ID=fleetadmin;Password=$qlpassw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
        

        // EXISTS
        public bool AddressExists(Address address)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM Address WHERE street=@street AND houseNr=@houseNr AND postalCode=@postalCode AND city=@city AND country=@country";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@street", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@houseNr", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@postalCode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@city", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@country", SqlDbType.NVarChar));

                    command.Parameters["@street"].Value = address.Street;
                    command.Parameters["@houseNr"].Value = address.HouseNr;
                    command.Parameters["@postalCode"].Value = address.PostalCode;
                    command.Parameters["@city"].Value = address.City;
                    command.Parameters["@country"].Value = address.Country;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
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
        public bool AddressExists(int addressID)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM Address WHERE addressId=@addressId";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@addressId", SqlDbType.Int));

                    command.Parameters["@addressId"].Value = addressID;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
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

        // INSERT UPDATE DELETE
        public void AddAddress(Address address)
        {
            SqlConnection connection = getConnection();

            string query = "INSERT INTO Address (street, houseNr, postalCode, city, country)" +
                           "VALUES (@street, @houseNr, @postalCode, @city, @country)";

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@street", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@houseNr", SqlDbType.DateTime));
                    command.Parameters.Add(new SqlParameter("@postalCode", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@city", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@country", SqlDbType.Bit));

                    command.Parameters["@street"].Value = address.Street;
                    command.Parameters["@houseNr"].Value = address.HouseNr;
                    command.Parameters["@postalCode"].Value = address.PostalCode;
                    command.Parameters["@city"].Value = address.City;
                    command.Parameters["@country"].Value = address.Country;

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
        public void UpdateAddress(Address address)
        {
            SqlConnection connection = getConnection();

            string query = "UPDATE Address" +
                           "SET street=@street, houseNr=@houseNr, postalCode=@postalCode, city=@city, country=@country" +
                           "WHERE addressId=@addressId";

            using(SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@street", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@houseNr", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@postalCode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@city", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@country", SqlDbType.NVarChar));

                    command.Parameters["@street"].Value = address.Street;
                    command.Parameters["@houseNr"].Value = address.Street;
                    command.Parameters["@postalCode"].Value = address.Street;
                    command.Parameters["@city"].Value = address.Street;
                    command.Parameters["@country"].Value = address.Street;

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

        }
        public void DeleteAddress(Address address)
        {
            SqlConnection connection = getConnection();
            
            string query = "DELETE FROM Address WHERE addressId=@addressId";
            
            using(SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@addressId", SqlDbType.Int));
                    command.Parameters["@addressId"].Value = address.AddressID;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

        }


        // END OF REPOSITORY
    }
}
