using DataAccessLayer.Models;

namespace DataAccessLayer.Mappers
{
    public interface IMemberMapper
    {
        Task<USER> Create(USER user);
        Task<USER> Update(USER user);
        Task<List<USER>> GetAll();
        Task<USER> GetById(int? id);
        Task<bool> Delete(int id);
    }
}