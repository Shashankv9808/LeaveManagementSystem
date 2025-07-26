using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LeaveManagementSystem.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            //Default user
            builder.HasData(new ApplicationUser
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
        }
    }
}
