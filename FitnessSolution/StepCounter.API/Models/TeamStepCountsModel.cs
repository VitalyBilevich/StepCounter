namespace StepCounter.API.Models
{
    public class TeamStepCountsModel
    {
        public Guid? TeamId { get; set; }

        public string TeamName { get; set; }

        public long StepCounts { get; set; }
    }
}
