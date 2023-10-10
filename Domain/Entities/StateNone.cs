namespace Domain.Entities
{
    public class StateNone : ITemperatureState 
    {
        public double Measure { get; }
        public DateTime MeasureDateTime { get; }
        public string Name => "NONE";

        public StateNone()
        {
            Measure = 0;
            MeasureDateTime = DateTime.UtcNow;
        }
    }
}
