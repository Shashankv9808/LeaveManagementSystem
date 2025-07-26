using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Configuration
{
    public class IdentityUserRoleConfiguration :IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
             new IdentityUserRole<string>
             {
                 RoleId = "7f699909-5287-412c-87fc-244531eaa8c7",
                 UserId = "3c25edb0-e14e-46ea-a155-401cd22ba74e"
             });
        }
    }
}
