using System;
using SlimJim.Model;

namespace SlimJim.Infrastructure
{
	public class ArgsOptionsBuilder
	{
		private SlnGenerationOptions options;

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
			foreach (string arg in args)
			{
				if (!arg.StartsWith("/"))
				{
					options.TargetProjectNames.Add(arg);
				}
				else
				{
					ProcessSwitch(arg);
				}
			}
		}

		private void ProcessSwitch(string arg)
		{
			string command = arg.Substring(1, 1);
			string value = arg.Substring(3);

			switch (command)
			{
				case "d":
					options.ProjectsRootDirectory = value;
					break;
				case "t":
				case "p":
					options.TargetProjectNames.Add(value);
					break;
				case "a":
					options.AdditionalSearchPaths.Add(value);
					break;
				case "o":
					options.SlnOutputPath = value;
					break;
				case "v":
					options.VisualStudioVersion = TryParseVersionNumber(value);
					break;
				case "n":
					options.SolutionName = value;
					break;
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