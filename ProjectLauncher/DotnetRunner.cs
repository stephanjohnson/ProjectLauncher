using System.Diagnostics;
using Spectre.Console;

public class DotnetRunner
{
	public void Run(string path)
	{
		var cmdPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
		var directoryPath = new FileInfo(path).Directory.FullName;
		Console.WriteLine(directoryPath);
		var processStartInfo = new ProcessStartInfo
		{
			FileName = "cmd.exe",
			Arguments = $"/C \"cd /d '{directoryPath}' & dotnet run\"",
			WorkingDirectory = directoryPath,
			UseShellExecute = true,
			CreateNoWindow = false
		};

		AnsiConsole.Status()
			.Start($"Starting \"{Path.GetFileName(path)}\".", context =>
			{
				Process.Start(processStartInfo);
			});
	}
}
