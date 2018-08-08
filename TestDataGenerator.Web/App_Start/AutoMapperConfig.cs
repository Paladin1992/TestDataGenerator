using AutoMapper;
using TestDataGenerator.Data.Models;
using TestDataGenerator.Web.Models;

namespace TestDataGenerator.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SetupProfile>();
                cfg.AllowNullDestinationValues = false;
            });

            return config.CreateMapper();
        }

        public class SetupProfile : Profile
        {
            public SetupProfile()
            {
                CreateMap<UserSetup, UserSetupViewModel>().ReverseMap();

                CreateMap<UserSetup, ListItemUserSetupViewModel>()
                    .ForMember(dest => dest.FieldCount, opt => opt.MapFrom(src => src.Fields.Count));

                CreateMap<ListItemUserSetupViewModel, UserSetup>()
                    .ForMember(dest => dest.Fields, opt => opt.Ignore());
            }
        }
    }
}