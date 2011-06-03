using System;
using System.IO;
using System.Text;
using log4net;
using NDesk.Options;
using SlimJim.Model;

namespace SlimJim.Infrastructure
{
	public class ArgsOptionsBuilder
	{
		private static ILog Log = LogManager.GetLogger(typeof (ArgsOptionsBuilder));
		private SlnGenerationOptions options;
		private OptionSet optionSet;

		public static SlnGenerationOptions BuildOptions(string[] args, string workingDirectory)
		{
			var builder = new ArgsOptionsBuilder();
			return builder.Build(args, workingDirectory);
		}

		public SlnGenerationOptions Build(string[] args, string workingDirectory)
		{
			options = new SlnGenerationOptions(workingDirectory);

			ProcessArguments(args);

			return options;
		}

		private void ProcessArguments(string[] args)
		{
			optionSet = new OptionSet
			            	{
			            		{ "t|target=", "{NAME} of a target project (repeat for multiple targets)", 
			            			v => options.TargetProjectNames.Add(v) },
			            		{ "r|root=", "{PATH} to the root directory where your projects reside (optional, defaults to working directory)", 
			            			v => options.ProjectsRootDirectory = v },
			            		{ "s|search=", "additional {PATH}(s) to search for projects to include outside of the root directory (repeat for multiple paths)",
			            			v => options.AddAdditionalSearchPaths(v) },
			            		{ "o|out=", "directory {PATH} where you want the .sln file written", 
			            			v => options.SlnOutputPath = v },
			            		{ "v|version=", "Visual Studio {VERSION} compatibility (2008, 2010 default)", 
			            			v => options.VisualStudioVersion = TryParseVersionNumber(v) },
			            		{ "n|name=", "alternate {NAME} for solution file", 
			            			v => options.SolutionName = v},
			            		{ "a|all", "include all efferent assembly references (omitted by default)", 
			            			v => options.IncludeEfferentAssemblyReferences = true },
			            		{ "h|help", "display the help screen", 
			            			v => options.ShowHelp = true },
			            		{ "i|ignore=", "ignore directories whose name matches the given {REGEX_PATTERN} (repeat for multiple ignores)", 
			            			v => options.AddIgnoreDirectoryPatterns(v) },
								{ "open", "open the solution in Visual Studio", 
			            			v => options.OpenInVisualStudio = true }
			            	};

			try
			{
				optionSet.Parse(args);
			}
			catch (OptionException optEx)
			{
				if (ParseError != null)
				{
					ParseError(optEx.Message);
				}
			}
		}

		public void WriteHelpMessage()
		{
			var helpMessage = new StringBuilder();
			using (var helpMessageWriter = new StringWriter(helpMessage))
			{
				helpMessageWriter.WriteLine("Usage: slimjim [OPTIONS]+");
				helpMessageWriter.WriteLine(
					"Generate a Visual Studio .sln file for a given directory of projects and one or more target project names.");
				helpMessageWriter.WriteLine();
				helpMessageWriter.WriteLine("Options:");
				optionSet.WriteOptionDescriptions(helpMessageWriter);
			}

			Log.Info(helpMessage.ToString());
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

		public event Action<string> ParseError;
		public event Action<string> ShowHelp;
	}
}