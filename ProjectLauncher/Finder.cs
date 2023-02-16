using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

public class Finder
{
	public List<string> FindWebProjects(DirectoryInfo rootDirectory)
	{
		var projects = FindProjects(rootDirectory);
		var webProjects = new List<string>();

		projects.ForEach(project =>
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(project);

			XmlNode sdkNode = doc.SelectSingleNode("/Project/@Sdk");
			if (sdkNode != null && sdkNode.Value == "Microsoft.NET.Sdk.Web")
			{
				webProjects.Add(project);
			}
		});

		return webProjects;
	}

	public List<string> FindConsoleProjects(DirectoryInfo rootDirectory)
	{
		var projects = FindProjects(rootDirectory);
		var consoleProjects = new List<string>();

		projects.ForEach(project =>
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(project);

			XmlNode sdkNode = doc.SelectSingleNode("/Project/@Sdk");
			XmlNode outputTypeNode = doc.SelectSingleNode("/Project/PropertyGroup/OutputType");
			if ((sdkNode != null && sdkNode.Value != "Microsoft.NET.Sdk.Web") && (outputTypeNode != null && outputTypeNode.InnerText == "Exe"))
			{
				consoleProjects.Add(project);
			}
		});

		return consoleProjects;
	}


	public List<string> FindProjects(DirectoryInfo rootDirectory)
	{
		var projects = new List<string>();

		foreach (var directory in rootDirectory.GetDirectories("*", SearchOption.AllDirectories))
		{
			projects.AddRange(directory.GetFiles("*.csproj").Select(m => m.FullName));
		}

		return projects;
	}

	public List<string> FindSolutions(DirectoryInfo rootDirectory)
	{
		var solutions = new List<string>();

		foreach (var directory in rootDirectory.GetDirectories("*", SearchOption.AllDirectories))
		{
			solutions.AddRange(directory.GetFiles("*.sln").Select(m => m.FullName));
		}

		return solutions;
	}
}
