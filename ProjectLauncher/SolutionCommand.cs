using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console.Cli;

public class SolutionCommand : ScanCommand<SolutionCommandSettings>
{
	public override string GetContentName()
	{
		return "Solution(s)";
	}

	public override string GetProjectPattern()
	{
		return "*.sln";
	}

	public override void ExecuteCommand(List<string> files)
	{
		var visualStudioLauncher = new VisualStudioLauncher();

		files.ForEach(visualStudioLauncher.Launch);
	}
}
