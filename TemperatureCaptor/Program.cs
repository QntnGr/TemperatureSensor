// See https://aka.ms/new-console-template for more information

using TemperatureCaptor;

try
{
    TemperatureCaptorLegacy.Start();
    Console.WriteLine("Press any button to terminate");
    Console.ReadKey(true);
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex);
    throw;
}

