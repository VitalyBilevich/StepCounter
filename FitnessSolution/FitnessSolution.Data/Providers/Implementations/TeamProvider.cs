using FitnessSolution.Data.Models;
using FitnessSolution.Data.Providers.Interfaces;
using FitnessSolution.Infrastructure;
using Microsoft.Extensions.Caching.Memory;


namespace FitnessSolution.Data.Providers.Implementations
{
    public class TeamProvider : ITeamProvider
    {
        private readonly IMemoryCache _memoryCache;        
        private readonly string _storageName = "Teams";

        public TeamProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        private Dictionary<Guid, TeamDto> _storage
        {
            get
            {
                var storage = _memoryCache.GetOrCreate(_storageName, cacheEntry =>
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(2); return new Dictionary<Guid, TeamDto>();
                });

                return storage ?? new Dictionary<Guid, TeamDto>();
            }

            set
            {
                _memoryCache.Set(_storageName, value, TimeSpan.FromDays(2));
            }
        }

        public IList<TeamDto> GetList()
        {
            return _storage.Select(i => i.Value).ToList();
        }

        public TeamDto GetByTeamName(string teamName) => _storage.Values.FirstOrDefault(i => string.Equals(i.Name.Trim(), teamName.Trim(), comparisonType: StringComparison.OrdinalIgnoreCase));

        public ResultObject<TeamDto> Create(TeamDto team)
        {
            if (string.IsNullOrWhiteSpace(team?.Name))
                return new ResultObject<TeamDto> { IsSuccess = false, Message = "Required data missing." };

            var teams = _storage;
            if (teams.Any(i => string.Equals(i.Value.Name.Trim(), team.Name.Trim(), comparisonType: StringComparison.OrdinalIgnoreCase)))
            {
                return new ResultObject<TeamDto> { IsSuccess = false, Message = "Team with this name already exists." };
            }

            team.Id = Guid.NewGuid();
            teams.Add(team.Id, team);
            _storage = teams;

            return new ResultObject<TeamDto> { IsSuccess = true, Data = team };
        }

        public ResultObject<string> Delete(Guid id)
        {
            var teams = _storage;           
            teams.Remove(id);
            _storage = teams;

            return new ResultObject<string> { IsSuccess = true, Data = id.ToString() };
        }
    }
}
