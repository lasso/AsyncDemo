var demoValueFactory = new DemoValueFactory(1, 100);

Console.Out.WriteLine();
Console.Out.WriteLine("A value between 1 and 100 will be produced every 10 seconds. Press enter to start producing values.");

Console.In.ReadLine();

// DemoValueFactory keeps track of its own cancellation token. The StartProducing() method returns a Task,
// but we don't want to await it because it will produce values indefinately in the background.
Task.Run(() => demoValueFactory.StartProducing());

Console.Out.WriteLine("Value production started. Press enter to stop producing values.");
Console.Out.WriteLine();

Console.In.ReadLine();

// The StopProducing() method does not return a Task, so there is no need to await it.
demoValueFactory.StopProducing();

Console.Out.WriteLine("Production stopped. Press enter to exit program.");

Console.In.ReadLine();