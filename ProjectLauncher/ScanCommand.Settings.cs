using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console.Cli;

public class ScanCommandSettings : CommandSettings
{
	[Description("The directory to scan for projects.")]
	[CommandArgument(0, "[directory]")]
	public string Directory { get; set; } = System.IO.Directory.GetCurrentDirectory();
}
