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
        public double LimitMax { get; }
        public string Name => RuleName;
        public static string RuleName => nameof(WarmLimitStateRule);

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
            Limit = limit;
        }

        public double Limit { get; }
        public string Name => RuleName;
        public static string RuleName => nameof(HotLimitStateRule);
        public ITemperatureState? ResolveState(double measure)
        {
            return measure >= Limit ? new StateHot(measure, DateTime.UtcNow) : null;
        }
    }

    public class ColdLimitStateRule : ITemperatureLimitStateRule
    {
        public ColdLimitStateRule(double limit)
        {
            Limit = limit;
        }

        public static string RuleName => nameof(ColdLimitStateRule);

        public double Limit { get; }
        public string Name => RuleName;

        public ITemperatureState? ResolveState(double measure)
        {
            return measure < Limit ? new StateCold(measure, DateTime.UtcNow) : null;
        }
    }
}
