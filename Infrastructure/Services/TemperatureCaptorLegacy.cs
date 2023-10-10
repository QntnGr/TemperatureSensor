namespace Infrastructure.Services
{
    //Do not modify
    public static class TemperatureCaptorLegacy
    {
        public static double Measure()
        {
            const int maximum = 60;
            const int minimum = -60;

            return Random.Shared.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
