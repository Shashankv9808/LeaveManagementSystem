using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Syntax: DbSet is a EF core class which shall helps generate queries anything in database. LeaveType is a structure(Entity) of the data, LeaveTypes shall be the table name. This statement help create migration.
        public DbSet<LeaveType> LeaveTypes { get; set; }
    }
}
