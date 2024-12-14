using FitnessSolution.Infrastructure;
using FitnessSolution.Services.Models;

namespace FitnessSolution.Services.Services.Interfaces
{
    public interface ITeamService
    {
        IList<Team> GetList();

        ResultObject<Team> Create(Team team);

        ResultObject<string> Delete(TeamDelete teamDelete);        

        IList<TeamStepCounts> GetTeamsWithStepCounts();
    }
}
