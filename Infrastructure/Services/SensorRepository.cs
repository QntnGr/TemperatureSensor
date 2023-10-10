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
                new(){Name = nameof(ColdLimitStateRule), LimitMin = 19},
                new(){Name = nameof(HotLimitStateRule),  LimitMin = 30},
                new(){Name = nameof(WarmLimitStateRule), LimitMin = 19, LimitMax = 30}
            };

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
                var rulesDb = TemperatureLimitStateRulesDb.FirstOrDefault(x => x.Name == temperatureLimitStateRule.Name);
                if (rulesDb == null)
                {
                    TemperatureLimitStateRulesDb.Add(new TemperatureLimitStateRuleDB
                    {
                        Name = temperatureLimitStateRule.Name,
                        LimitMax = temperatureLimitStateRule.LimitMax,
                        LimitMin = temperatureLimitStateRule.LimitMin
                    });
                }
                else
                {
                    rulesDb.Name = temperatureLimitStateRule.Name;
                    rulesDb.LimitMax = temperatureLimitStateRule.LimitMax;
                    rulesDb.LimitMin = temperatureLimitStateRule.LimitMin;
                }
            }

            return Task.CompletedTask;

        }

        public Task<Sensor> GetAsync(CancellationToken cancellationToken = default)
        {
            var temperatureStatesHistory = TemperatureStatesDb.Select(x =>
            {
                if (x.Name == "NONE")
                    return (ITemperatureState)new StateNone();
                if (x.Name == "WARM")
                    return (ITemperatureState)new StateWarm(x.Measure, x.MeasureDateTime);
                if (x.Name == "COLD")
                    return (ITemperatureState)new StateCold(x.Measure, x.MeasureDateTime);
                if (x.Name == "HOT")
                    return (ITemperatureState)new StateHot(x.Measure, x.MeasureDateTime);
                throw new ArgumentOutOfRangeException(nameof(x.Name), $"Unknown state type {x.Name}");
            }).OrderByDescending(x => x.MeasureDateTime)
              .ToList();

            var temperatureStateRules =TemperatureLimitStateRulesDb.Select(x =>
            {
                if (x.Name == nameof(ColdLimitStateRule))
                    return (ITemperatureLimitStateRule)new ColdLimitStateRule(x.LimitMin);
                if (x.Name == nameof(WarmLimitStateRule))
                    return (ITemperatureLimitStateRule)new WarmLimitStateRule(x.LimitMin, x.LimitMax!.Value);
                if (x.Name == nameof(HotLimitStateRule))
                    return (ITemperatureLimitStateRule)new HotLimitStateRule(x.LimitMin);
                throw new ArgumentOutOfRangeException(nameof(x.Name), $"Unknown state rule {x.Name}");
            }).ToList();

            var stateTemperatureState = temperatureStatesHistory.FirstOrDefault(x => x.Name == SensorDb.TemperatureState);

            var sensor = new Sensor(temperatureStateRules, stateTemperatureState, temperatureStatesHistory);
            return Task.FromResult(sensor);
        }
    }
}
