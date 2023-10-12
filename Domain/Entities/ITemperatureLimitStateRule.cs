namespace Domain.Entities
{
    public interface ITemperatureLimitStateRule
    {
        public string Name { get; }

        public ITemperatureState? ResolveState(double measure);
    }
}
