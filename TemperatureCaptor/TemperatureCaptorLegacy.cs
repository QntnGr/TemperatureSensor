using System.IO.MemoryMappedFiles;

namespace TemperatureCaptor
{
    //Do not modify
    public static class TemperatureCaptorLegacy
    {

        private const int Maximum = 60;
        private const int Minimum = -60;
        private static readonly object Lock = new();
        private static Task? _runningTask;
        private static readonly CancellationTokenSource CancellationTokenSource = new();
        private static MemoryMappedFile? _mmf;
        private static MemoryMappedViewAccessor? _viewAccessor;
        public static string SharedMemoryFile => nameof(TemperatureCaptorLegacy);

        internal static void Start()
        {
            lock (Lock)
            {
                if (_runningTask?.Status is TaskStatus.Running or TaskStatus.WaitingForActivation or TaskStatus.WaitingToRun or TaskStatus.Created)
                {
                    throw new InvalidOperationException("Captor is already running");
                }
                try
                {
#pragma warning disable CA1416
                    _mmf = MemoryMappedFile.CreateOrOpen(SharedMemoryFile, sizeof(double), MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.DelayAllocatePages, HandleInheritability.None);
#pragma warning restore CA1416
                    _viewAccessor = _mmf.CreateViewAccessor(0, sizeof(double), MemoryMappedFileAccess.ReadWrite);

                    _runningTask = Task.Run(async () =>
                    {
                        try
                        {
                            // ReSharper disable once AccessToDisposedClosure
                            while (!CancellationTokenSource.IsCancellationRequested)
                            {
                                var value = Random.Shared.NextDouble() * (Maximum - Minimum) + Minimum;

                                // ReSharper disable once AccessToDisposedClosure
                                _viewAccessor.Write(0, value);
                                Console.WriteLine($"{nameof(TemperatureCaptorLegacy)} : current temperature = {value}");

                                // ReSharper disable once AccessToDisposedClosure
                                await Task.Delay(500, CancellationTokenSource.Token);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex is OperationCanceledException or TimeoutException or TaskCanceledException)
                            {
                                Console.WriteLine($"Stopped {nameof(TemperatureCaptorLegacy)}");
                                CleanUp();
                                return;
                            }
                            await Console.Error.WriteLineAsync(ex.ToString());
                            CleanUp();
                            throw;
                        }
                    }, CancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    if (ex is OperationCanceledException or TimeoutException or TaskCanceledException)
                    {
                        Console.WriteLine($"Stopped {nameof(TemperatureCaptorLegacy)}");
                        CleanUp();
                        return;
                    }

                    Console.Error.WriteLine(ex.ToString());
                    CleanUp();
                    throw;
                }
            }
        }

        private static void CleanUp()
        {
            lock (Lock)
            {
                _viewAccessor?.Dispose();
                _mmf?.Dispose();
                CancellationTokenSource.Dispose();
                _runningTask?.Dispose();
            }
        }

        internal static void Stop()
        {
            lock (Lock)
            {
                if (_runningTask is { IsCompleted: true })
                    return;
                if (!CancellationTokenSource.IsCancellationRequested)
                    CancellationTokenSource.Cancel(true);
            }
        }
    }
}
