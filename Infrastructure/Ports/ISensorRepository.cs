using Domain.Entities;

namespace Infrastructure.Ports
{
    public interface ISensorRepository
    {
        public Task SaveAsync(Sensor sensor, CancellationToken cancellationToken = default);
        public Task<Sensor> GetAsync(CancellationToken cancellationToken = default);
    }
}
