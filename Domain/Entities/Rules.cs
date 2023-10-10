namespace Domain.Entities
{
    public class WarmLimitStateRule : ITemperatureLimitStateRule
    {
        public WarmLimitStateRule(double min, double max)
        {
            LimitMin = min;
            LimitMax = max;
        }

        public double LimitMin { get; }
        public double? LimitMax { get; }
        public string Name => nameof(WarmLimitStateRule);

        public ITemperatureState? ResolveState(double measure)
        {
            if (measure >= LimitMin && measure < LimitMax)
                return new StateWarm(measure, DateTime.UtcNow);
            return null;
        }
    }

    public class HotLimitStateRule : ITemperatureLimitStateRule
    {
        public HotLimitStateRule(double limit)
        {
            LimitMin = limit;
        }

        public double LimitMin { get; }
        public double? LimitMax => null;
        public string Name => nameof(HotLimitStateRule);

        public ITemperatureState? ResolveState(double measure)
        {
            return measure >= LimitMin ? new StateHot(measure, DateTime.UtcNow) : null;
        }
    }

    public class ColdLimitStateRule : ITemperatureLimitStateRule
    {
        public ColdLimitStateRule(double limit)
        {
            LimitMin = limit;
        }

        public double LimitMin { get; }
        public double? LimitMax => null;
        public string Name => nameof(ColdLimitStateRule);

        public ITemperatureState? ResolveState(double measure)
        {
            return measure < LimitMin ? new StateCold(measure, DateTime.UtcNow) : null;
        }
    }
}
