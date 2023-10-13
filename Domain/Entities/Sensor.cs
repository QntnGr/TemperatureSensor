using Domain.Ports;

namespace Domain.Entities
{
    //Aggregate
    //Single entity aggregate
    //https://deviq.com/domain-driven-design/aggregate-pattern
    public class Sensor : IAggregate<Sensor>
    {
        private readonly Dictionary<Type, ITemperatureLimitStateRule> _stateRulesMap;
        private readonly object _lock;
        private readonly ICollection<ITemperatureState> _temperatureStatesHistory;
        private readonly ICollection<ITemperatureState> _temperatureStatesAdded;

        //Aggregate Root
        public Sensor Root => this;

        public Sensor(IEnumerable<ITemperatureLimitStateRule> stateRules)
        {
            _currentState = new StateNone();
            _lock = new object();
            _stateRulesMap = stateRules.ToDictionary(x => x.GetType(), x => x);
            _temperatureStatesHistory = new List<ITemperatureState>();
            _temperatureStatesAdded = new List<ITemperatureState>();
        }

        public Sensor(IEnumerable<ITemperatureLimitStateRule> stateRules, ITemperatureState? temperatureState = null, ICollection<ITemperatureState>? temperatureStatesHistory = null)
        {
            _currentState = temperatureState ?? new StateNone();
            _lock = new object();
            _stateRulesMap = stateRules.ToDictionary(x => x.GetType(), x => x);
            _temperatureStatesHistory = temperatureStatesHistory ?? new List<ITemperatureState>();
            _temperatureStatesAdded = new List<ITemperatureState>();
        }

        private ITemperatureState _currentState;
        public ITemperatureState CurrentTemperatureState
        {
            get
            {
                lock (_lock)
                {
                    return _currentState;
                }
            }
        }

        public IEnumerable<ITemperatureLimitStateRule> StateRules => _stateRulesMap.Values;
        public IEnumerable<ITemperatureState> TemperatureStatesHistory => _temperatureStatesHistory;
        public IEnumerable<ITemperatureState> TemperatureStatesAdded => _temperatureStatesAdded;

        public void ChangeTemperatureLimitStateRule(ITemperatureLimitStateRule newRule)
        {
            var type = newRule.GetType();
            lock (_lock)
            {
                if (_stateRulesMap.ContainsKey(type))
                    _stateRulesMap[newRule.GetType()] = newRule;
                else throw new ArgumentOutOfRangeException(nameof(newRule), $"Unknown limit state rule type {type.Name} for Sensor");
            }
        }

        public void ChangeTemperatureMeasure(double measure)
        {
            lock (_lock) {
                foreach (var rule in StateRules)
                {
                    var state = rule.ResolveState(measure);
                    if (state == null)
                        continue;

                    _currentState = state;
                    _temperatureStatesAdded.Add(state);
                    _temperatureStatesHistory.Add(state);
                    return;
                }

                throw new ArgumentOutOfRangeException(nameof(measure), $"Cannot find matching measure rule for {measure} temperature.");
            }
        }
    }
}
