namespace Domain.Entities
{
    public class StateWarm : ITemperatureState 
    {
        public double Measure { get; }
        public DateTime MeasureDateTime { get; }
        public string Name => StateName;
        public static string StateName => "WARM";

        public StateWarm(double measure, DateTime measureDateTime)
        {
            Measure = measure;
            MeasureDateTime = measureDateTime;
        }
    }
}
