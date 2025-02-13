using Dapper;
using DataAccessLayer.Models;
using System.Data.SqlClient;

namespace DataAccessLayer.Mappers
{
    public class DapperMemberMapper : IMemberMapper
    {
        private string connectionString = "Data Source=.;Initial Catalog=MytestDB;Integrated Security=True;Connect Timeout=30";

        //public DapperMemberMapper(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}

        public async Task<USER> Create(USER user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = @"
                    INSERT INTO MEM_USER (cpyname, position, username, age)
                    OUTPUT INSERTED.ID
                    VALUES (@CpyName, @Position, @Use\rname, @Age)";

                // ID를 반환받아 설정
                user.Id = await connection.ExecuteScalarAsync<int>(query, user);
                return user;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "DELETE FROM MEM_USER WHERE id = @Id";
                var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0;
            }
        }

        public async Task<List<USER>> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT * FROM MEM_USER";
                var users = await connection.QueryAsync<USER>(query);
                return users.ToList();
            }
        }

        public async Task<USER> GetById(int? id)
        {
            if (id == null)
            {
                throw new Exception("ID cannot be null.");
            }

            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT * FROM MEM_USER WHERE id = @Id";
                var user = await connection.QueryFirstOrDefaultAsync<USER>(query, new { Id = id });

                if (user == null)
                {
                    throw new Exception($"No user found with ID {id}.");
                }

                return user;
            }
        }

        public async Task<USER> Update(USER user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = @"
                    UPDATE MEM_USER
                    SET cpyname = @CpyName,
                        position = @Position,
                        username = @Username,
                        age = @Age
                    WHERE id = @Id";

                var rowsAffected = await connection.ExecuteAsync(query, user);
                if (rowsAffected == 0)
                {
                    throw new Exception($"No user found with ID {user.Id}.");
                }

                return user;
            }
        }
    }
}
