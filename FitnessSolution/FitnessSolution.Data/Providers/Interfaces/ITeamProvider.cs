using FitnessSolution.Data.Models;
using FitnessSolution.Infrastructure;

namespace FitnessSolution.Data.Providers.Interfaces
{
    public interface ITeamProvider
    {
        ResultObject<TeamDto> Create(TeamDto team);

        IList<TeamDto> GetList();

        TeamDto GetByTeamName(string teamName);

        ResultObject<string> Delete(Guid id);
    }
}
