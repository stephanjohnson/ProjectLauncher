using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Spectre.Console;

public class VisualStudioLauncher
{
	public void Launch(string solutionPath)
	{
		var visualStudioPath = FindVisualStudioPath();
		if (string.IsNullOrEmpty(visualStudioPath))
		{
			AnsiConsole.WriteLine("Visual Studio not found.");
			return;
		}

		AnsiConsole.Status()
			.Start($"Opening \"{solutionPath}\".", context =>
			{

				var startInfo = new ProcessStartInfo
				{
					FileName = visualStudioPath,
					Arguments = $"\"{solutionPath}\"",
					UseShellExecute = false
				};

				var process = new Process { StartInfo = startInfo };
				process.Start();
			});
	}

	public static List<string> FindVisualStudioPaths()
	{
		var visualStudioPaths = new List<string>();

		AnsiConsole.Status()
			.Start($"Finding your version of Visual Studio.", context =>
			{
				var programFilesFolders = new List<string> { Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) };
				if (Environment.Is64BitOperatingSystem)
				{
					programFilesFolders.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
				}

				foreach (var programFilesFolder in programFilesFolders)
				{
					var visualStudioFolders = Directory.GetDirectories(programFilesFolder, "Microsoft Visual Studio*", SearchOption.TopDirectoryOnly);
					foreach (var visualStudioFolder in visualStudioFolders)
					{
						var versions = new[] { "2019", "2022" };
						var editions = new[] { "Community", "Professional", "Enterprise" };
						foreach (var version in versions)
						{
							foreach (var edition in editions)
							{
								var devenvPath = Path.Combine(visualStudioFolder, version, edition, "Common7", "IDE", "devenv.exe");
								if (File.Exists(devenvPath))
								{
									visualStudioPaths.Add(devenvPath);
								}
							}
						}
					}
				}
			});

		return visualStudioPaths;
	}

	private string FindVisualStudioPath()
	{
		var visualStudioPaths = FindVisualStudioPaths();

		if (visualStudioPaths.Count == 0)
		{
			return null;
		}
		else if (visualStudioPaths.Count == 1)
		{
			return visualStudioPaths[0];
		}
		else
		{
			var choices = visualStudioPaths.Select(p => (p, Path.GetFileName(p))).ToList();
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<(string Path, string Name)>()
					.Title("Select Visual Studio version")
					.PageSize(10)
					.AddChoices(choices));

			return selection.Item1;
		}
	}
}
