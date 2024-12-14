using System.ComponentModel.DataAnnotations;

namespace StepCounter.API.Models
{
    public class CounterCreateModel
    {
        [Required]
        public string UserName { get; set; }

        public string TeamName { get; set; }
    }
}
