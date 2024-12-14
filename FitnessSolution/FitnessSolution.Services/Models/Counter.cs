namespace FitnessSolution.Services.Models
{
    public class Counter
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public Guid? TeamId { get; set; }

        public string TeamName { get; set; }

        public uint Value { get; set; }
    }
}
