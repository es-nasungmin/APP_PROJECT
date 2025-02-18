using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // 데이터베이스 테이블을 나타내는 DbSet 추가
        public DbSet<USER> MEM_USER { get; set; }
    }
}