using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //Default Data Seeding when Migration is applied
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Defaul roles
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "7b9f7fc2-38b5-484f-aaef-ce52333cf198",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                },
                new IdentityRole
                {
                    Id = "5ffad0b4-16ae-4493-9fdf-7c027680e719\r\n",
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR"
                },
                new IdentityRole
                {
                    Id = "7f699909-5287-412c-87fc-244531eaa8c7",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
                );
            //Default user
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "3c25edb0-e14e-46ea-a155-401cd22ba74e",
                Email = "admin@localhost.com",
                NormalizedEmail = "ADMIN@LOCALHOST.COM",
                UserName = "admin@localhost.com",
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Admin@123"),
                EmailConfirmed = true,
                FirstName = "Default",
                LastName = "Administrator",
                DateOfBirth = new DateOnly(1998, 1, 1)
            });
            //Assigning the default user to the default role of Administrator
            builder.Entity<IdentityUserRole<string>>().HasData(
             new IdentityUserRole<string>
             {
                 RoleId = "7b9f7fc2-38b5-484f-aaef-ce52333cf198",
                 UserId = "3c25edb0-e14e-46ea-a155-401cd22ba74e"
             });

        }
        //Syntax: DbSet is a EF core class which shall helps generate queries anything in database. LeaveType is a structure(Entity) of the data, LeaveTypes shall be the table name. This statement help create migration.
        public DbSet<LeaveType> LeaveTypes { get; set; }
    }
}
