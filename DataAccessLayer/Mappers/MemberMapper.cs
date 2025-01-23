using DataAccessLayer.Models;
using System.Data.SqlClient;

namespace DataAccessLayer.Mappers
{
    public class MemberMapper : IMemberMapper
    {
        //Data Source=.;Initial Catalog=MytestDB;Integrated Security=True;Trust Server Certificate=True;
        private string connectionStrinig = "Data Source=.;Initial Catalog=MytestDB;Integrated Security=True;Connect Timeout=30";

        public async Task<USER> Create(USER user)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionStrinig))
                {
                    await sqlConnection.OpenAsync();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = "INSERT INTO MEM_USER(cpyname, position, username, age) OUTPUT INSERTED.ID VALUES(@cpyname, @position, @username, @age)";
                    sqlCommand.Parameters.AddWithValue("@cpyname", user.CpyName);
                    sqlCommand.Parameters.AddWithValue("@position", user.Position);
                    sqlCommand.Parameters.AddWithValue("@username", user.Username);
                    sqlCommand.Parameters.AddWithValue("@age", user.Age);
                    int id = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());
                    user.Id = id;

                    return user;
                }
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public async Task<USER> Update(USER user)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionStrinig))
                {
                    await sqlConnection.OpenAsync();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = @"
                        UPDATE MEM_USER
                        SET cpyname = @cpyname,
                            position = @position,
                            username = @username,
                            age = @age
                        WHERE id = @id";

                    sqlCommand.Parameters.AddWithValue("@id", user.Id);
                    sqlCommand.Parameters.AddWithValue("@cpyname", user.CpyName);
                    sqlCommand.Parameters.AddWithValue("@position", user.Position);
                    sqlCommand.Parameters.AddWithValue("@username", user.Username);
                    sqlCommand.Parameters.AddWithValue("@age", user.Age);

                    int rowsAffected = await sqlCommand.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        throw new Exception($"No user found with ID {user.Id}.");
                    }

                    return user;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<USER>> GetAll()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionStrinig))
                {
                    await sqlConnection.OpenAsync();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = "SELECT * FROM MEM_USER";

                    using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                    {
                        List<USER> users = new List<USER>();
                        while (await reader.ReadAsync())
                        {
                            users.Add(new USER
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                CpyName = reader["cpyname"] as string,
                                Position = reader["position"] as string,
                                Username = reader["username"] as string,
                                Age = reader.GetInt32(reader.GetOrdinal("age"))
                            });
                        }
                        return users;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<USER> GetById(int? id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionStrinig))
                {
                    await sqlConnection.OpenAsync();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = "SELECT * FROM MEM_USER WHERE id = @id";
                    sqlCommand.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new USER
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                CpyName = reader["cpyname"] as string,
                                Position = reader["position"] as string,
                                Username = reader["username"] as string,
                                Age = reader.GetInt32(reader.GetOrdinal("age"))
                            };
                        }
                        throw new Exception($"해당 유저가 존재하지 않습니다. 아이디: {id}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionStrinig))
                {
                    await sqlConnection.OpenAsync();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = "DELETE FROM MEM_USER WHERE id = @id";
                    sqlCommand.Parameters.AddWithValue("@id", id);

                    int rowsAffected = await sqlCommand.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
