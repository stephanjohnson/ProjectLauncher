using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using System.Xml;
using Spectre.Console.Cli;

public class ConsoleCommand : ScanCommand<ConsoleCommandSettings>
{
	public override string GetContentName()
	{
		return "Console project(s)";
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
					XmlNode outputTypeNode = doc.SelectSingleNode("/Project/PropertyGroup/OutputType");
					if ((sdkNode != null && sdkNode.Value != "Microsoft.NET.Sdk.Web") && (outputTypeNode != null && outputTypeNode.InnerText == "Exe"))
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