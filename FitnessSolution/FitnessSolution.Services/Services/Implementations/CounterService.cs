using AutoMapper;
using FitnessSolution.Data.Providers.Interfaces;
using FitnessSolution.Infrastructure;
using FitnessSolution.Services.Models;
using FitnessSolution.Services.Services.Interfaces;
using FitnessSolution.Data.Models;

namespace FitnessSolution.Services.Services.Implementations
{
    public class CounterService: ICounterService
    {
        private readonly ICounterProvider _counterProvider;        
        private readonly ITeamProvider _teamProvider;
        private readonly IMapper _mapper;

        public CounterService(ICounterProvider counterProvider, ITeamProvider teamProvider, IMapper mapper)
        {
            _counterProvider = counterProvider;           
            _teamProvider = teamProvider;
            _mapper = mapper;
        }

        public ResultObject<Counter> Create(Counter counter)
        {
            if (string.IsNullOrWhiteSpace(counter.UserName))
                return new ResultObject<Counter> { IsSuccess = false, Message = "Required data missing." };

            var existingCounter = _counterProvider.GetByUserName(counter.UserName);
            if (existingCounter != null)
                return new ResultObject<Counter> { IsSuccess = false, Message = "There is already a counter with this user." };

            // the case when the team is specified.
            TeamDto team = null;
            if (!string.IsNullOrWhiteSpace(counter.TeamName))
            {
                team = _teamProvider.GetByTeamName(counter.TeamName);
                if(team != null)
                {
                    counter.TeamId = team.Id;
                }
                else
                {
                    var createTeamResult = _teamProvider.Create(new TeamDto { Name = counter.TeamName });
                    if(createTeamResult != null && createTeamResult.IsSuccess && createTeamResult.Data != null && createTeamResult.Data.Id != Guid.Empty)
                    {
                        counter.TeamId = createTeamResult.Data.Id;
                        team = createTeamResult.Data;
                    }                    
                }
            }
            
            var result = _mapper.Map<ResultObject<CounterDto>, ResultObject<Counter>>(_counterProvider.Create(_mapper.Map<Counter, CounterDto>(counter)));
            if (team != null && result != null && result.IsSuccess && result.Data != null)
            {
                result.Data.TeamName = team.Name;
            }                

            return result ?? new ResultObject<Counter> { IsSuccess = false, Message = "Something went wrong." };
        }

        public IList<Counter> GetCountersByTeam(string team)
        {
            // get counters for users without team
            if (string.IsNullOrWhiteSpace(team))
                return _mapper.Map<IList<CounterDto>, IList<Counter>>(_counterProvider.GetListByTeamId(null));

            if (!Guid.TryParse(team, out var teamId) || teamId == Guid.Empty)
            {
                var teamDto = _teamProvider.GetByTeamName(team);
                if (teamDto == null)
                {
                    return new List<Counter>();
                }
                teamId = teamDto.Id;
            }

            return _mapper.Map<IList<CounterDto>, IList<Counter>>(_counterProvider.GetListByTeamId(teamId));
        }

        public ResultObject<Counter> IncrementValue(CounterIncrement counterIncrement)
        {
            if(counterIncrement == null || counterIncrement.Value == 0 || (!counterIncrement.Id.HasValue && string.IsNullOrWhiteSpace(counterIncrement.UserName)))
               return new ResultObject<Counter> { IsSuccess = false, Message = "Required data missing." };

            if (counterIncrement.Id.HasValue && counterIncrement.Id != Guid.Empty)
            {
                return _mapper.Map<ResultObject<CounterDto>, ResultObject<Counter>>(_counterProvider.IncrementValue(counterIncrement.Id.Value, counterIncrement.Value));                
            }

            return _mapper.Map<ResultObject<CounterDto>, ResultObject<Counter>>(_counterProvider.IncrementValue(counterIncrement.UserName, counterIncrement.Value));
        }

        public ResultObject<string> Delete(CounterDelete counterDelete)
        {
            if (counterDelete == null || (!counterDelete.Id.HasValue && string.IsNullOrWhiteSpace(counterDelete.UserName)))
                return new ResultObject<string> { IsSuccess = false, Message = "Required data missing.", Data = string.Empty };

            if (!counterDelete.Id.HasValue || counterDelete.Id == Guid.Empty)
            {
                var counterDto = _counterProvider.GetByUserName(counterDelete.UserName);
                if (counterDto == null)
                {
                    return new ResultObject<string> { IsSuccess = false, Message = "Counter not found.", Data = counterDelete.UserName };
                }
                counterDelete.Id = counterDto.Id;
            }           

            return _counterProvider.Delete(counterDelete.Id.Value);
        }

    }
}
