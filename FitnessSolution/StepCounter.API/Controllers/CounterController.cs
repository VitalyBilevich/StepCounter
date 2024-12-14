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
    public class CounterController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICounterService _counterService;
        public CounterController(ICounterService counterService, IMapper mapper)
        {
            _counterService = counterService;
            _mapper = mapper;
        }

        [HttpGet("ListByTeam")]
        public IList<CounterByTeamModel> GetCountersByTeam(string team)
        {
            return _mapper.Map<IList<Counter>, IList<CounterByTeamModel>>(_counterService.GetCountersByTeam(team));
        }

        [HttpPost("Create")]
        public ResultObject<Counter> CreateCounter([FromBody] CounterCreateModel counterCreateModel)
        {
            return _counterService.Create(_mapper.Map<CounterCreateModel, Counter>(counterCreateModel));
        }

        [HttpPost("Delete")]
        public ResultObject<string> DeleteCounter([FromBody] CounterDeleteModel counterDeleteModel)
        {
            return _counterService.Delete(_mapper.Map<CounterDeleteModel, CounterDelete>(counterDeleteModel));
        }

        [HttpPost("Increment")]
        public ResultObject<Counter> IncrementCounterValue([FromBody] CounterIncrementModel counterIncrementModel)
        {
            return _counterService.IncrementValue(_mapper.Map<CounterIncrementModel, CounterIncrement>(counterIncrementModel));
        }
    }
}
