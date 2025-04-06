using Microsoft.EntityFrameworkCore;

namespace AspireWithGarnet.Web
{
    public class SampleDbContext(DbContextOptions<SampleDbContext> options) : DbContext(options)
    {
        public DbSet<Member> Members => Set<Member>();
    }

    public class Member
    {
        public int Id { get; set; }
        public required string StudentNumber { get; set; }
        public required string Name { get; set; }
    }
}
