using Domain.Ports;
using Infrastructure.Services;

namespace Infrastructure.Providers
{
    public class TemperatureCaptorProvider : ITemperatureCaptorProvider 
    {
        public TemperatureCaptorProvider()
        {
            TemperatureCaptorLegacy.Start();
        }

        public double Measure()
        {
            return TemperatureCaptorLegacy.GetCurrentMeasure();
        }
    }
}
