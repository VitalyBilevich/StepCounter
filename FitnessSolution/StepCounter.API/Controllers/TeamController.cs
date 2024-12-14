using AutoMapper;
using FitnessSolution.Infrastructure;
using FitnessSolution.Services.Models;
using FitnessSolution.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StepCounter.API.Models;

namespace StepCounter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public TeamController(ITeamService teamService, IMapper mapper)
        {
            _teamService = teamService;
            _mapper = mapper;
        }

        [HttpPost("Create")]        
        public ResultObject<TeamModel> CreateTeam([FromBody] TeamCreateModel teamCreateModel)
        {
            return _mapper.Map<ResultObject<Team>, ResultObject<TeamModel>>(_teamService.Create(_mapper.Map<TeamCreateModel, Team>(teamCreateModel)));
        }

        [HttpPost("Delete")]
        public ResultObject<string> DeleteTeam([FromBody] TeamDeleteModel teamDeleteModel)
        {
            return _teamService.Delete(_mapper.Map<TeamDeleteModel, TeamDelete>(teamDeleteModel));
        }

        [HttpGet("ListWithStepCounts")]        
        public IList<TeamStepCountsModel> GetTeamsWithStepCounts()
        {
            return _mapper.Map<IList<TeamStepCounts>, IList<TeamStepCountsModel>>(_teamService.GetTeamsWithStepCounts());
        }
    }
}
