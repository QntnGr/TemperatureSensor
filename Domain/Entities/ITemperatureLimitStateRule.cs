namespace Domain.Entities
{
    public interface ITemperatureLimitStateRule
    {
        public double LimitMin { get; }
        public double? LimitMax { get; }

        public string Name { get; }

        public ITemperatureState? ResolveState(double measure);
    }
}
