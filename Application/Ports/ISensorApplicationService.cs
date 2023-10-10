using Domain.Entities;

namespace Application.Ports
{
    public interface ISensorIApplicationService
    {
        public Task<ITemperatureState> GetTemperatureAsync(CancellationToken cancellationToken = default);
        public Task<IEnumerable<ITemperatureState>> GetTemperatureHistoryAsync(CancellationToken cancellationToken = default);
        public Task ChangeTemperatureLimitStateRuleAsync(ITemperatureLimitStateRule newRule, CancellationToken cancellationToken = default);
    }
}
