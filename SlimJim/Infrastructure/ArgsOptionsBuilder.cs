using System;
using NDesk.Options;
using SlimJim.Model;

namespace SlimJim.Infrastructure
{
	public class ArgsOptionsBuilder
	{
		private SlnGenerationOptions options;
		private bool showHelp;

		public static SlnGenerationOptions BuildOptions(string[] args, string workingDirectory)
		{
			var builder = new ArgsOptionsBuilder();
			return builder.Build(args, workingDirectory);
		}

		private SlnGenerationOptions Build(string[] args, string workingDirectory)
		{
			options = new SlnGenerationOptions(workingDirectory);

			ProcessSwitches(args);

			return options;
		}

		private void ProcessSwitches(string[] args)
		{
			var optionSet = new OptionSet()
				.Add("r|root=", "{PATH} to the root directory where your projects reside", v => options.ProjectsRootDirectory = v)
				.Add("t|target=", "{NAME} of a target project (repeat for multiple targets)", v => options.TargetProjectNames.Add(v))
				.Add("s|search=", "additional {PATH} to search for additional projects to include outside of the root directory (repeat for multiple paths)", v => options.AdditionalSearchPaths.Add(v))
				.Add("o|out=", "directory {PATH} where you want the .sln file written", v => options.SlnOutputPath = v)
				.Add("v|version=", "Visual Studio {VERSION} compatibility (2008, 2010 default)", v => options.VisualStudioVersion = TryParseVersionNumber(v))
				.Add("n|name=", "alternate {NAME} for solution file", v => options.SolutionName = v)
				.Add("a|all", "include all efferent assembly references (omitted by default)", v => options.IncludeEfferentAssemblyReferences = true)
				.Add("h|help", "display the help screen", v => showHelp = true);

			optionSet.Parse(args);

			if (showHelp)
			{
				Console.WriteLine("Usage: slimjim [OPTIONS]+");
				Console.WriteLine("Generate a Visual Studio .sln file for a given directory of projects and one or more target project names.");
				Console.WriteLine();
				Console.WriteLine("Options:");
				optionSet.WriteOptionDescriptions(Console.Out);
			}
		}

		private VisualStudioVersion TryParseVersionNumber(string versionNumber)
		{
			VisualStudioVersion parsedVersion = VisualStudioVersion.ParseVersionString(versionNumber);

			if (parsedVersion == null)
			{
				parsedVersion = VisualStudioVersion.VS2010;
			}

			return parsedVersion;
		}
	}
}