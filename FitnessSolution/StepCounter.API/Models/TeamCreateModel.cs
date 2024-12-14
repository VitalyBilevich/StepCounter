using System.ComponentModel.DataAnnotations;

namespace StepCounter.API.Models
{
    public class TeamCreateModel
    {
        [Required]
        public string Name { get; set; }
    }
}
