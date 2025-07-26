using AutoMapper;
using LeaveManagementSystem.Models.LeaveAllocations;
using LeaveManagementSystem.Models.LeaveTypes;
using LeaveManagementSystem.Models.Periods;

namespace LeaveManagementSystem.MappingProfiles
{
    public class LeaveAllocationsAutoMapperProfile : Profile
    {
        public LeaveAllocationsAutoMapperProfile()
        {
            // Pseudocode plan:
            // 1. Use ForMember for mapping nested properties if LeaveType is not null, to avoid ForPath complexity unless LeaveType is always initialized.
            // 2. If LeaveType is always non-null, ForPath is fine, but can be simplified by mapping the whole LeaveType object if possible.
            // 3. If LeaveTypeVM and LeaveType have similar structure, use CreateMap<LeaveType, LeaveTypeVM> and let AutoMapper handle nested mapping.

            CreateMap<LeaveAllocation, LeaveAllocationVM>()
                .ForMember(dest => dest.LeaveType, opt => opt.MapFrom(src => src.LeaveType));
            CreateMap<Period, PeriodVM>();
            CreateMap<ApplicationUser, EmployeeListVM>()
                .ForMember(dest => dest.EmployeeId, opt=> opt.MapFrom(src => src.Id));
            CreateMap<LeaveAllocation, LeaveAllocationEditVM>();
        }
    }
}
