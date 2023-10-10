namespace Domain.Entities
{
    public class StateHot : ITemperatureState 
    {
        public double Measure { get; }
        public DateTime MeasureDateTime { get; }

        public string Name => "HOT";

        public StateHot(double measure, DateTime measureDateTime)
        {
            Measure = measure;
            MeasureDateTime = measureDateTime;
        }
        
    }
}
