namespace Domain.Entities
{
    public interface ITemperatureState
    {
        public double Measure { get; }
        public DateTime MeasureDateTime { get; }
        public string Name { get; }

        public string PrintState()
        {
            return $"Name : {Name}\r\nMeasure : {Measure}\r\nMeasureDateTime : {MeasureDateTime:dd-MM-yyyy hh:mm:ss}";
        }
    }
}
