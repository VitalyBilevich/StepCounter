using System.ComponentModel.DataAnnotations;

namespace StepCounter.API.Models
{
    public class CounterIncrementModel
    {
        public Guid? Id { get; set; }

        public string UserName { get; set; }

        [Required]
        public uint Value { get; set; }
    }
}
