namespace FitnessSolution.Services.Models
{
    public class TeamStepCounts
    {
        public Guid? TeamId { get; set; }

        public string TeamName { get; set; }        

        public long StepCounts { get; set; }
    }
}
