using NEvent.Interfaces;
using NEvent.DependencyInjection;
using NEvent.ConsoleExample.Events;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

IServiceCollection serviceCollection = new ServiceCollection();

serviceCollection.AddNEventLogging();
serviceCollection.AddNEvent(eventAssemblies: typeof(ButtonMessageHandler).Assembly);

ServiceProvider provider = serviceCollection.BuildServiceProvider();

IEventAggregator eventAggregator = provider.GetService<IEventAggregator>()!;    
Button btn = new(eventAggregator);

ButtonMessageHandler buttonMessageHandler = (ButtonMessageHandler)provider.GetService<IEventHandler<ButtonMessageArgs>>()!;
eventAggregator.Subscribe(buttonMessageHandler);

btn.SendMessage("Te amo Deus. | I Love you God.");

btn.SendMessage("Hi (This message will be catched by filter.)");

eventAggregator.UnSubscribe(buttonMessageHandler);
btn.SendMessage("Se eu ver, deu ruim! | If I see, It's wrong!");

Console.WriteLine("Finished!!!");