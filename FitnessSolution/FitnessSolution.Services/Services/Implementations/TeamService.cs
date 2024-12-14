using FitnessSolution.Data.Providers.Interfaces;
using FitnessSolution.Services.Services.Interfaces;
using FitnessSolution.Infrastructure;
using FitnessSolution.Services.Models;
using AutoMapper;
using FitnessSolution.Data.Models;

namespace FitnessSolution.Services.Services.Implementations
{
    public class TeamService : ITeamService
    {
        private readonly ITeamProvider _teamProvider;
        private readonly ICounterProvider _counterProvider;
        private readonly IMapper _mapper;

        public TeamService(ITeamProvider teamProvider, ICounterProvider counterProvider, IMapper mapper)
        {
            _teamProvider = teamProvider;
            _counterProvider = counterProvider;
            _mapper = mapper;
        }

        public IList<Team> GetList() => _mapper.Map<IList<TeamDto>, IList<Team>>(_teamProvider.GetList());

        public ResultObject<Team> Create(Team team) => _mapper.Map<ResultObject<Team>>(_teamProvider.Create(_mapper.Map<Team, TeamDto>(team)));

        public ResultObject<string> Delete(TeamDelete teamDelete)
        {
            if (teamDelete == null || (!teamDelete.Id.HasValue && string.IsNullOrWhiteSpace(teamDelete.Name)))
                return new ResultObject<string> { IsSuccess = false, Message = "Required data missing.", Data = string.Empty};
                        
            if (!teamDelete.Id.HasValue || teamDelete.Id == Guid.Empty)
            {
                var teamDto = _teamProvider.GetByTeamName(teamDelete.Name);
                if (teamDto == null)
                {
                    return new ResultObject<string> { IsSuccess = false, Message = "Team not found.", Data = teamDelete.Name };
                }
                teamDelete.Id = teamDto.Id;
            }           
           
            _counterProvider.DeleteTeamFromCounters(teamDelete.Id.Value);

            return _teamProvider.Delete(teamDelete.Id.Value);
        }

        public IList<TeamStepCounts> GetTeamsWithStepCounts()
        {
            var teams = _teamProvider.GetList();
            var counters = _counterProvider.GetList();
            var teamsWithStepCounts = counters.Where(i => i.TeamId.HasValue).GroupBy(i => i.TeamId).Select(
                i => new TeamStepCounts
                {
                    TeamId = i.Key,
                    TeamName = teams.FirstOrDefault(t => i.Key.HasValue && t.Id == i.Key.Value)?.Name ?? "Team Deleted",
                    StepCounts = i.Sum(s => s.Value)
                }).ToList();

            var noTeamStepCounts = counters.Where(i => !i.TeamId.HasValue).Sum(i => i.Value);
            if (noTeamStepCounts > 0)
                teamsWithStepCounts.Add(new TeamStepCounts {TeamName = "users without a team", StepCounts = noTeamStepCounts });

            return teamsWithStepCounts;
        }
    }
}
