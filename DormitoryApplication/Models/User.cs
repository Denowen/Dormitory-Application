using Microsoft.EntityFrameworkCore;

namespace DormitoryApplication.Models
{
    [Keyless]
    public class User
    {
        public string Name { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string SchoolId { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int DormsId { get; set; }
        public int DormTypeId { get; set; }

    }
    [Keyless]
    public class UserContext: DbContext
    {
        public DbSet<User> User { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"data source=DESKTOP-N9HBLJE; database=Dormitory_App; integrated security=SSPI;");
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");


        }




    }
}
