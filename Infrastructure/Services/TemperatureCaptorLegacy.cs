namespace Infrastructure.Services
{
    //Do not modify
    public static class TemperatureCaptorLegacy
    {
        private static double _currentTemperature;
        private const int Maximum = 60;
        private const int Minimum = -60;
        private static readonly object Lock = new();
        private static volatile bool _isRunning;

        public static double GetCurrentMeasure()
        {
           lock (Lock)
           {
               return _currentTemperature;
           }
        }

        public static void Start()
        {
            if (_isRunning)
            {
                throw new InvalidOperationException("Captor is already running");
            }

            _isRunning = true;

            Task.Run(async () =>
            {
                while (true)
                {
                    var value = _currentTemperature = Random.Shared.NextDouble() * (Maximum - Minimum) + Minimum;
                    lock (Lock)
                    {
                        _currentTemperature = value;
                    }
                    await Task.Delay(500);
                }
            });
        }

        public static void Stop()
        {
            _isRunning = false;
        }
    }
}
