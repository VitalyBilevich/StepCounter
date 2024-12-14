using FitnessSolution.Data.Models;
using FitnessSolution.Infrastructure;

namespace FitnessSolution.Data.Providers.Interfaces
{
    public interface ICounterProvider
    {
        ResultObject<CounterDto> Create(CounterDto counter);

        IList<CounterDto> GetList();

        IList<CounterDto> GetListByTeamId(Guid? teamId);

        CounterDto GetByUserName(string userName);

        CounterDto GetById(Guid id);

        ResultObject<CounterDto> IncrementValue(string userName, uint value);

        ResultObject<CounterDto> IncrementValue(Guid id, uint value);

        ResultObject<string> Delete(Guid id);

        void DeleteTeamFromCounters(Guid teamId);
    }
}
