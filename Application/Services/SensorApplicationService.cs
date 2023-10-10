using Application.Ports;
using Domain.Entities;
using Domain.Ports;
using Infrastructure.Ports;

namespace Application.Services
{
    public class SensorApplicationService : ISensorIApplicationService
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly ITemperatureCaptorProvider _temperatureCaptorProvider;

        public SensorApplicationService(ISensorRepository sensorRepository, ITemperatureCaptorProvider temperatureCaptorProvider)
        {
            _sensorRepository = sensorRepository;
            _temperatureCaptorProvider = temperatureCaptorProvider;
        }
        public async Task<ITemperatureState> GetTemperatureAsync(CancellationToken cancellationToken = default)
        {
            var sensor = await _sensorRepository.GetAsync(cancellationToken);
            sensor.MeasureTemperature(_temperatureCaptorProvider);
            await _sensorRepository.SaveAsync(sensor, cancellationToken);
            return sensor.CurrentTemperatureState;
        }

        public async Task<IEnumerable<ITemperatureState>> GetTemperatureHistoryAsync(CancellationToken cancellationToken = default)
        {
           var sensor = await _sensorRepository.GetAsync(cancellationToken);
           return sensor.TemperatureStatesHistory
               .OrderByDescending(x => x.MeasureDateTime)
               .Take(15);
        }

        public async Task ChangeTemperatureLimitStateRuleAsync(ITemperatureLimitStateRule newRule,CancellationToken cancellationToken = default)
        {
            var sensor = await _sensorRepository.GetAsync(cancellationToken);
            sensor.ChangeTemperatureLimitStateRule(newRule);
            await _sensorRepository.SaveAsync(sensor, cancellationToken);
        }
    }
}
