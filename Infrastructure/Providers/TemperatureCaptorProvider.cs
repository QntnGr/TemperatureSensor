using Domain.Ports;
using Infrastructure.Services;

namespace Infrastructure.Providers
{
    public class TemperatureCaptorProvider : ITemperatureCaptorProvider 
    {
        public double Measure()
        {
            return TemperatureCaptorLegacy.Measure();
        }
    }
}
