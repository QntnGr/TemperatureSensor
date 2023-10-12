using Domain.Entities;
using Infrastructure.DBModels;
using Infrastructure.Ports;

namespace Infrastructure.Services
{
    public class SensorRepository : ISensorRepository
    {
        private static readonly SensorDB SensorDb = new() { TemperatureState = nameof(StateNone) };
        private static readonly IList<TemperatureStateDB> TemperatureStatesDb = new List<TemperatureStateDB>();
        private static readonly IList<TemperatureLimitStateRuleDB> TemperatureLimitStateRulesDb =
            new List<TemperatureLimitStateRuleDB>
            {
                new(){Name = ColdLimitStateRule.RuleName, LimitMin = 19},
                new(){Name = HotLimitStateRule.RuleName,  LimitMin = 30},
                new(){Name = WarmLimitStateRule.RuleName, LimitMin = 19, LimitMax = 30}
            };

        private static void MapStateRuleToStateRuleDb(TemperatureLimitStateRuleDB rule, ITemperatureLimitStateRule stateRule)
        {
            switch (stateRule)
            {
                case WarmLimitStateRule warmLimitStateRule:
                    rule.LimitMax = warmLimitStateRule.LimitMax;
                    rule.LimitMin = warmLimitStateRule.LimitMin;
                    rule.Name = warmLimitStateRule.Name;
                    break;
                case HotLimitStateRule hotLimitStateRule:
                    rule.LimitMax = hotLimitStateRule.Limit;
                    rule.LimitMin = hotLimitStateRule.Limit;
                    rule.Name = hotLimitStateRule.Name;
                    break;
                case ColdLimitStateRule coldLimitStateRule:
                    rule.LimitMax = coldLimitStateRule.Limit;
                    rule.LimitMin = coldLimitStateRule.Limit;
                    rule.Name = coldLimitStateRule.Name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateRule), $"Unknown type : Cannot convert {rule.GetType().Name} to {nameof(TemperatureLimitStateRuleDB)}.");
            }
        }

        public Task SaveAsync(Sensor sensor, CancellationToken cancellationToken = default)
        {
            SensorDb.TemperatureState = sensor.CurrentTemperatureState.Name;

            foreach (var temperatureState in sensor.TemperatureStatesAdded)
            {
                TemperatureStatesDb.Add(new TemperatureStateDB
                {
                    Measure = temperatureState.Measure,
                    Name = temperatureState.Name,
                    MeasureDateTime = temperatureState.MeasureDateTime
                });
            }

            foreach (var temperatureLimitStateRule in sensor.StateRules)
            {
                var ruleDb = TemperatureLimitStateRulesDb.FirstOrDefault(x => x.Name == temperatureLimitStateRule.Name);
                if (ruleDb == null)
                {
                    ruleDb = new TemperatureLimitStateRuleDB();
                    MapStateRuleToStateRuleDb(ruleDb, temperatureLimitStateRule);
                }
                else
                {
                    MapStateRuleToStateRuleDb(ruleDb, temperatureLimitStateRule);
                }
            }

            return Task.CompletedTask;
        }

        public Task<Sensor> GetAsync(CancellationToken cancellationToken = default)
        {
            var temperatureStatesHistory = TemperatureStatesDb.Select(x =>
            {
                if (x.Name == StateNone.StateName)
                    return (ITemperatureState)new StateNone();
                if (x.Name == StateWarm.StateName)
                    return (ITemperatureState)new StateWarm(x.Measure, x.MeasureDateTime);
                if (x.Name == StateCold.StateName)
                    return (ITemperatureState)new StateCold(x.Measure, x.MeasureDateTime);
                if (x.Name == StateHot.StateName)
                    return (ITemperatureState)new StateHot(x.Measure, x.MeasureDateTime);
                throw new ArgumentOutOfRangeException(nameof(x.Name), $"Unknown state type {x.Name}");
            }).OrderByDescending(x => x.MeasureDateTime)
              .ToList();

            var temperatureStateRules = TemperatureLimitStateRulesDb.Select(x =>
            {
                if (x.Name == ColdLimitStateRule.RuleName)
                    return (ITemperatureLimitStateRule)new ColdLimitStateRule(x.LimitMin);
                if (x.Name == WarmLimitStateRule.RuleName)
                    return (ITemperatureLimitStateRule)new WarmLimitStateRule(x.LimitMin, x.LimitMax);
                if (x.Name == HotLimitStateRule.RuleName)
                    return (ITemperatureLimitStateRule)new HotLimitStateRule(x.LimitMin);
                throw new ArgumentOutOfRangeException(nameof(x.Name), $"Unknown state rule {x.Name}");
            }).ToList();

            var stateTemperatureState = temperatureStatesHistory.FirstOrDefault(x => x.Name == SensorDb.TemperatureState);

            var sensor = new Sensor(temperatureStateRules, stateTemperatureState, temperatureStatesHistory);

            return Task.FromResult(sensor);
        }
    }
}
