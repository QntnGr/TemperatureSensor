using System.IO.MemoryMappedFiles;
using Domain.Ports;
using TemperatureCaptor;

namespace Infrastructure.Providers
{
    public sealed class TemperatureCaptorProvider : ITemperatureCaptorProvider, IDisposable
    {
        private readonly MemoryMappedViewAccessor _viewAccessor;
        private readonly MemoryMappedFile _mappedFile;

        public TemperatureCaptorProvider()
        {
#pragma warning disable CA1416
            _mappedFile = MemoryMappedFile.CreateOrOpen(TemperatureCaptorLegacy.SharedMemoryFile, sizeof(double), MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.DelayAllocatePages, HandleInheritability.None);
#pragma warning restore CA1416
            _viewAccessor = _mappedFile.CreateViewAccessor(0, sizeof(double), MemoryMappedFileAccess.Read);
        }

        public double Measure()
        {
           return _viewAccessor.ReadDouble(0);
        }

        public void Dispose()
        {
            _viewAccessor.Dispose();
            _mappedFile.Dispose();
        }
    }
}
