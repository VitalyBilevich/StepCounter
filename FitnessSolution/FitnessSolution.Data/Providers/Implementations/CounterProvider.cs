using FitnessSolution.Data.Models;
using FitnessSolution.Data.Providers.Interfaces;
using FitnessSolution.Infrastructure;
using Microsoft.Extensions.Caching.Memory;

namespace FitnessSolution.Data.Providers.Implementations
{
    public class CounterProvider: ICounterProvider
    {
        private readonly IMemoryCache _memoryCache;
        private readonly string _storageName = "Counters";        

        public CounterProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        private Dictionary<Guid, CounterDto> _storage
        {
            get
            {
                var storage = _memoryCache.GetOrCreate(_storageName, cacheEntry =>
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(2); return new Dictionary<Guid, CounterDto>();
                });

                return storage ?? new Dictionary<Guid, CounterDto>();
            }

            set
            {
                _memoryCache.Set(_storageName, value, TimeSpan.FromDays(2));
            }
        }

        public IList<CounterDto> GetList()
        {
            return _storage.Select(i => i.Value).ToList();
        }

        public CounterDto GetById(Guid id) => _storage.Values.FirstOrDefault(i => i.Id == id);

        public CounterDto GetByUserName(string userName) => _storage.Values.FirstOrDefault(i => string.Equals(i.UserName.Trim(), userName.Trim(), comparisonType: StringComparison.OrdinalIgnoreCase));

        public ResultObject<CounterDto> Create(CounterDto counter)
        {
            if (string.IsNullOrWhiteSpace(counter.UserName))
                return new ResultObject<CounterDto> { IsSuccess = false, Message = "Required data missing." };

            var counters = _storage;     
            if (counters.Any(i => i.Value.UserName == counter.UserName))
            {
                return new ResultObject<CounterDto> { IsSuccess = false, Message = "There is already a counter with this user." };              
            }            

            counter.Id = Guid.NewGuid();
            counters.Add(counter.Id, counter);
            _storage = counters;

            return new ResultObject<CounterDto> { IsSuccess = true, Data = counter };
        }

        public ResultObject<CounterDto> IncrementValue(Guid id, uint value)
        {
            var counter = GetById(id);
            if (counter == null)
            {
                return new ResultObject<CounterDto> { IsSuccess = false, Message = "Counter not found." };
            }

            counter.Value += value;
            _storage = _storage;

            return new ResultObject<CounterDto> { IsSuccess = true, Data = counter };
        }

        public ResultObject<CounterDto> IncrementValue(string userName, uint value)
        {          
            var counter = GetByUserName(userName);
            if(counter == null)
            {
                return new ResultObject<CounterDto> { IsSuccess = false, Message = "Counter not found." };
            }

            counter.Value += value;
            _storage = _storage;

            return new ResultObject<CounterDto> { IsSuccess = true, Data = counter };
        }       

        public IList<CounterDto> GetListByTeamId(Guid? teamId) => _storage.Where(i => i.Value.TeamId == teamId).Select(i => i.Value).ToList();
        
        public ResultObject<string> Delete(Guid id)
        {
            var counters = _storage;
            counters.Remove(id);
            _storage = counters;

            return new ResultObject<string> { IsSuccess = true, Data = id.ToString() };
        }

        public void DeleteTeamFromCounters(Guid teamId)
        {
            var counters = GetListByTeamId(teamId).ToList();
            counters.ForEach(i => i.TeamId = null);
            _storage = _storage;
        }

    }
}
