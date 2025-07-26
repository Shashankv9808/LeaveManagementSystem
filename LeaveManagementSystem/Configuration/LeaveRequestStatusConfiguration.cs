using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Configuration
{
    public class LeaveRequestStatusConfiguration : IEntityTypeConfiguration<LeaveRequestStatus>
    {
        public void Configure(EntityTypeBuilder<LeaveRequestStatus> builder)
        {
            builder.HasData(
                new LeaveRequestStatus
                {
                    LeaveRequestStatusID = 1,
                    Name = "Pending"
                },
                new LeaveRequestStatus
                {
                    LeaveRequestStatusID = 2,
                    Name = "Approved"
                },
                new LeaveRequestStatus
                {
                    LeaveRequestStatusID = 3,
                    Name = "Declined"
                },
                new LeaveRequestStatus
                {
                    LeaveRequestStatusID = 4,
                    Name = "Cancelled"
                }
            );
        }
    }
}
