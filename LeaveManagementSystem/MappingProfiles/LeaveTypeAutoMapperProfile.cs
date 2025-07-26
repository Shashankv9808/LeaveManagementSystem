using AutoMapper;
using LeaveManagementSystem.Models.LeaveTypes;

namespace LeaveManagementSystem.MappingProfiles
{
    public class LeaveTypeAutoMapperProfile :Profile
    {
        public LeaveTypeAutoMapperProfile()
        {
            CreateMap<LeaveType, LeaveTypeReadOnlyVM>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.LeaveTypeID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.LeaveTypeName));
            CreateMap<LeaveType, LeaveTypeCreateVM>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.LeaveTypeName));
            CreateMap<LeaveTypeEditVM, LeaveType>()
                .ForMember(dest => dest.LeaveTypeID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src => src.Days));
        }
    }
}
