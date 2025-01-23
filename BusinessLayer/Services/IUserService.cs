using DataAccessLayer.DTO;

namespace BusinessLayer.Services
{
    public interface IUserService
    {
        public Task CreateUser(UserDTO userDTO);
        Task UpdateUser(UserDTO userDTO);
        Task<List<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUserById(int id);
        Task<bool> DeleteUser(int id);
    }
}
