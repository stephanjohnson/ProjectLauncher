using Spectre.Console;
using Spectre.Console.Cli;

public abstract class ScanCommand<TSettings> : Command<TSettings> where TSettings : ScanCommandSettings
{
	public abstract string GetProjectPattern();
	public abstract string GetContentName();
	public abstract void ExecuteCommand(List<string> files);

	public virtual List<string> FilterBasedOnContent(List<string> files)
	{
		return files;
	}

	public override int Execute(CommandContext context, TSettings settings)
	{
		var rootDirectory = new DirectoryInfo(settings.Directory);

		if (!rootDirectory.Exists)
		{
			AnsiConsole.Markup($"The specified directory '{rootDirectory.FullName}' does not exist.");
			return 1;
		}

		var projects = FindProjects(rootDirectory, GetProjectPattern());

		if (projects.Count == 0)
		{
			AnsiConsole.Markup($"No projects found in directory '{rootDirectory.FullName}' that match the pattern '{GetProjectPattern()}'.");
			return 1;
		}

		var filteredProjects = FilterBasedOnContent(projects);

		// Show the multi-select menu
		var choices = filteredProjects.ToDictionary(p => Path.GetFileName(p), p => p);
		var selection = AnsiConsole.Prompt(
			new MultiSelectionPrompt<string>()
				.Title("Select projects to process")
				.PageSize(10)
				.AddChoices(choices.Keys)
		);

		ExecuteCommand(selection.Select(s => choices[s]).ToList());

		return 0;
	}

	private List<string> FindProjects(DirectoryInfo rootDirectory, string projectPattern)
	{
		var projects = new List<string>();

		AnsiConsole.Status()
			.Start($"Scanning directory '{rootDirectory.FullName}'", context =>
			{

				foreach (var directory in rootDirectory.GetDirectories("*", SearchOption.AllDirectories))
				{
					var projectFiles = directory.GetFiles(projectPattern, SearchOption.TopDirectoryOnly);

					foreach (var projectFile in projectFiles)
					{
						projects.Add(projectFile.FullName);
					}
				}

			});

		return projects;
	}
}
