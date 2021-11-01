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
        
        private string connectionString = $"Data Source=fleetmanagserver.database.windows.net;Persist Security Info=True;User ID=fleetadmin;Password=$qlpassw0rd";
        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
        public void AddAddress(Address address)
        {
            SqlConnection connection = getConnection();

            string query = "INSERT INTO dbo.Address (street, houseNr, postalCode, city, country)" +
                           "VALUES (@street, @houseNr, @postalCode, @city, @country)";

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    //command.Parameters.Add(new SqlParameter("@addressId", SqlDbType.Int));       zonder id
                    command.Parameters.Add(new SqlParameter("@street", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@houseNr", SqlDbType.DateTime));
                    command.Parameters.Add(new SqlParameter("@postalCode", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@city", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@country", SqlDbType.Bit));

                    //command.Parameters["@addressId"].Value = address.AddressID;                zonder id
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

        public bool AddressExists(Address address)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM dbo.Address WHERE addressId=@addressId";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@addressId", SqlDbType.Int));

                    command.Parameters["@addressId"].Value = address.AddressID;

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
            throw new NotImplementedException();
        }

        public void DeleteAddress(Address address)
        {
            throw new NotImplementedException();
        }

        public void UpdateAddress(Address address)
        {
            throw new NotImplementedException();
        }






    }
}
