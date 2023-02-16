using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Spectre.Console;
using Spectre.Console.Cli;

public class WebCommand : ScanCommand<WebCommandSettings>
{
	public override string GetContentName()
	{
		return "Web project(s)";
	}

	public override string GetProjectPattern()
	{
		return "*.csproj";
	}

	public override void ExecuteCommand(List<string> files)
	{
		var dotnetRunner = new DotnetRunner();

		files.ForEach(dotnetRunner.Run);
	}

	public override List<string> FilterBasedOnContent(List<string> files)
	{
		var projects = new List<string>();

		XmlDocument doc = new XmlDocument();

		AnsiConsole.Progress()
			.AutoClear(true)
			.HideCompleted(true)
			.Start(context =>
			{
				var scanningTask = context.AddTask("[green]Scanning projects[/]", autoStart: true, maxValue: files.Count());

				files.ForEach(file => {
					doc.Load(file);

					XmlNode sdkNode = doc.SelectSingleNode("/Project/@Sdk");
					if (sdkNode != null && sdkNode.Value == "Microsoft.NET.Sdk.Web")
					{
						projects.Add(file);
					}
					scanningTask.Increment(1);
				});

				while (!context.IsFinished)
				{
					scanningTask.Increment(1);
					context.Refresh();					
				}
			});

		return projects;
	}
}
