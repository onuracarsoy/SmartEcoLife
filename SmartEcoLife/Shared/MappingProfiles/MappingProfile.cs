using AutoMapper;
using SmartEcoLife.Features.FinancialRecords;
using SmartEcoLife.Features.Goals;
using SmartEcoLife.Features.Users;
using SmartEcoLife.Shared.Dtos.FinancialRecordDtos;
using SmartEcoLife.Shared.Dtos.GoalDtos;
using SmartEcoLife.Shared.Dtos.UsersDtos;

namespace SmartEcoLife.Shared.MappingProfiles
{
    public class MappingProfile
        : Profile
    {
        public MappingProfile()
        {

            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PreferredCurrency, opt => opt.MapFrom(src => "TRY"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow)).ReverseMap();

            CreateMap<FinancialRecord, FinancialRecordDto>()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
               .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
               .ReverseMap()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<RecordType>(src.Type, true)));

            CreateMap<Goal, GoalDto>().ReverseMap();
        



        }
    }

}
