using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
	config.AddCommand<WebCommand>("web");
	config.AddCommand<ConsoleCommand>("console");
	config.AddCommand<SolutionCommand>("sln");
});

return app.Run(args);