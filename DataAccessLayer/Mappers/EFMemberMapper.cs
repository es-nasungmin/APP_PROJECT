using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Mappers
{
    public class EFMemberMapper : IMemberMapper
    {
        private readonly ApplicationDbContext _context;

        public EFMemberMapper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<USER> Create(USER user)
        {
            await _context.MEM_USER.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<USER> Update(USER user)
        {
            var existingUser = await _context.MEM_USER.FindAsync(user.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"No user found with ID {user.Id}.");
            }

            _context.Entry(existingUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<List<USER>> GetAll()
        {
            return await _context.MEM_USER.ToListAsync();
        }

        public async Task<USER> GetById(int? id)
        {
            var user = await _context.MEM_USER.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"해당 유저가 존재하지 않습니다. 아이디: {id}");
            }
            return user;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _context.MEM_USER.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.MEM_USER.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}