namespace FitnessSolution.Data.Models
{
    public class CounterDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public Guid? TeamId { get; set; }

        public uint Value { get; set; }
    }
}
