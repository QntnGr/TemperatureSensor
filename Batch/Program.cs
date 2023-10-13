// See https://aka.ms/new-console-template for more information

using Application.Services;
using Domain.Entities;
using Infrastructure.Providers;
using Infrastructure.Services;

using var captorProvider = new TemperatureCaptorProvider();
var sensorRepository = new SensorRepository();
var appServiceApp = new SensorApplicationService(sensorRepository, captorProvider);

Console.WriteLine("...............Measuring...............");
for (var i = 0; i < 10; i++)
{
    Thread.Sleep(1000);
    var state = await appServiceApp.GetTemperatureAsync();
    Console.WriteLine(state.PrintState());
}

Thread.Sleep(1000);
Console.WriteLine("...............Printing History...............");
var temperatureHistory = await appServiceApp.GetTemperatureHistoryAsync();
foreach (var temperatureState in temperatureHistory)
{
    Console.WriteLine(temperatureState.PrintState());
}

Thread.Sleep(1000);
Console.WriteLine("...............Changing Limit Rules...............");
var coldRule = new ColdLimitStateRule(-50);
var warmRule = new WarmLimitStateRule(-50, 30);
await appServiceApp.ChangeTemperatureLimitStateRuleAsync(coldRule);
await appServiceApp.ChangeTemperatureLimitStateRuleAsync(warmRule);
Console.WriteLine("...............Measuring...............");
for (var i = 0; i < 10; i++)
{
    Thread.Sleep(1000);
    var state = await appServiceApp.GetTemperatureAsync();
    Console.WriteLine(state.PrintState());
}


Console.ReadKey(true);
