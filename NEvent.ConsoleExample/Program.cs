using NEvent.Interfaces;
using NEvent.DependencyInjection;
using NEvent.ConsoleExample.Events;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

IServiceCollection services = new ServiceCollection();

services.AddNEventLogging();
services.AddNEvent(eventAssemblies: [typeof(ButtonMessageHandler).Assembly]);

IServiceProvider serviceProvider = services.BuildServiceProvider();

IEventAggregator eventAggregator = serviceProvider.GetService<IEventAggregator>()!;
eventAggregator.TrySubscribe<ButtonMessageArgs>();

Button btn = new(eventAggregator);

await btn.SendMessageAsync("I Love you God.");

await btn.SendMessageAsync("Hi (This messages will be catched by filter.)");
await btn.SendMessageAsync(" ");

ButtonMessageHandler buttonMessageHandler = serviceProvider.GetService<ButtonMessageHandler>()!;
eventAggregator.TryUnSubscribe(buttonMessageHandler);

await btn.SendMessageAsync("ButtonMessageHandler UnSubscribed!");
