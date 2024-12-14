using AutoMapper;
using FitnessSolution.Data.Models;
using FitnessSolution.Infrastructure;
using FitnessSolution.Services.Models;

namespace FitnessSolution.Services.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ConfigureMappings();
        }

        private void ConfigureMappings()
        {
            CreateMap<TeamDto, Team>();
            CreateMap<Team, TeamDto>();
            CreateMap<CounterDto, Counter>();
            CreateMap<Counter, CounterDto>();
            CreateMap<ResultObject<TeamDto>, ResultObject<Team>>().ForMember(d => d.Data, opt => opt.MapFrom(s => s.Data));
            CreateMap<ResultObject<CounterDto>, ResultObject<Counter>>().ForMember(d => d.Data, opt => opt.MapFrom(s => s.Data));
        }
    }
}
