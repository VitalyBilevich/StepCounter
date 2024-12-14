using AutoMapper;
using FitnessSolution.Infrastructure;
using FitnessSolution.Services.Models;
using StepCounter.API.Models;

namespace StepCounter.API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ConfigureMappings();
        }

        private void ConfigureMappings()
        {
            CreateMap<TeamCreateModel, Team>();
            CreateMap<CounterCreateModel, Counter>();
            CreateMap<Counter, CounterByTeamModel>();
            CreateMap<CounterIncrementModel, CounterIncrement>();
            CreateMap<TeamStepCounts, TeamStepCountsModel>();
            CreateMap<TeamModel, Team>();
            CreateMap<Team, TeamModel>();
            CreateMap<CounterDeleteModel, CounterDelete>();
            CreateMap<TeamDeleteModel, TeamDelete>();
            CreateMap<ResultObject<TeamModel>, ResultObject<Team>>().ForMember(d => d.Data, opt => opt.MapFrom(s => s.Data));
            CreateMap<ResultObject<Team>, ResultObject<TeamModel>>().ForMember(d => d.Data, opt => opt.MapFrom(s => s.Data));
        }
    }
}
