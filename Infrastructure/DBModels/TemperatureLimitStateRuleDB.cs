namespace Infrastructure.DBModels
{
    internal sealed class TemperatureLimitStateRuleDB
    {
        public double LimitMin { get; set; }
        public double? LimitMax { get; set; }
        public string Name { get; set; }
    }
}
