using FitnessSolution.Infrastructure;
using FitnessSolution.Services.Models;

namespace FitnessSolution.Services.Services.Interfaces
{
    public interface ICounterService
    {
        ResultObject<Counter> Create(Counter counter);

        /// <summary>
        /// Get a list of all counters in the team.
        /// </summary>
        /// <param name="team">team name or team guid id as string</param>
        /// <returns></returns>
        IList<Counter> GetCountersByTeam(string team);

        /// <summary>
        /// Increment the value of a stored counter.
        /// </summary>
        /// <param name="counterIncrement">contains increment value and user name or counter id</param>
        /// <returns></returns>
        ResultObject<Counter> IncrementValue(CounterIncrement counterIncrement);

        ResultObject<string> Delete(CounterDelete counterDelete);
    }
}
