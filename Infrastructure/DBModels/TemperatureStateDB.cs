namespace Infrastructure.DBModels
{
    internal sealed class TemperatureStateDB
    {
        public double Measure { get; set; }
        public DateTime MeasureDateTime { get; set; }
        public string Name { get; set; }
    }
}
