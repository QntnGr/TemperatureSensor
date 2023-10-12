namespace Domain.Entities
{
    public class StateNone : ITemperatureState 
    {
        public double Measure => 0;
        public DateTime MeasureDateTime { get; } = DateTime.UtcNow;
        public string Name => StateName;
        public static string StateName => "NONE";
    }
}
