using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using log4net.Core;

namespace SlimJim.Model
{
	public class SlnGenerationOptions
	{
		private const string DefaultSolutionName = "SlimJim";
		private string projectsRootDirectory;
		private string solutionName;
		private string slnOutputPath;
		private readonly List<string> additionalSearchPaths;
		private readonly List<string> ignoreDirectoryPatterns;

		public SlnGenerationOptions(string workingDirectory)
		{
			ProjectsRootDirectory = workingDirectory;
			additionalSearchPaths = new List<string>();
			ignoreDirectoryPatterns = new List<string>();
			TargetProjectNames = new List<string>();
			VisualStudioVersion = VisualStudioVersion.VS2015;
			LoggingThreshold = Level.Info;
		}

		public List<string> TargetProjectNames { get; private set; }

		public string ProjectsRootDirectory
		{
			get { return projectsRootDirectory; }
			set
			{
				projectsRootDirectory = ResolvePath(value);
			}
		}

		public VisualStudioVersion VisualStudioVersion { get; set; }
		public bool SkipAfferentAssemblyReferences { get; set; }
		public bool IncludeEfferentAssemblyReferences { get; set; }
		public bool ShowHelp { get; set; }
		public bool ConvertReferences { get; set; }
		public bool RestoreReferences { get; set; }
		public bool FixHintPaths { get; set; }
		public bool RestoreHintPaths { get; set; }
		public bool OpenInVisualStudio { get; set; }
		public Level LoggingThreshold { get; set; }

		public List<string> AdditionalSearchPaths
		{
			get
			{
				return additionalSearchPaths.ConvertAll(ResolvePath);
			}
		}

		private string ResolvePath(string p)
		{
			return !Path.IsPathRooted(p) ? Path.Combine(ProjectsRootDirectory, p) : p;
		}

		public string SlnOutputPath
		{
			get { return slnOutputPath != null ? ResolvePath(slnOutputPath) : ProjectsRootDirectory; }
			set { slnOutputPath = value; }
		}

		public string SolutionName
		{
			get
			{
				if (string.IsNullOrEmpty(solutionName))
				{
					if (TargetProjectNames.Count > 0)
					{
						return string.Join("_", TargetProjectNames);
					}

					if (!string.IsNullOrEmpty(ProjectsRootDirectory))
					{
						return GetLastSegmentNameOfProjectsRootDirectory();
					}

					return DefaultSolutionName;
				}

				return solutionName;
			}
			set { solutionName = value; }
		}

		private string GetLastSegmentNameOfProjectsRootDirectory()
		{
			var dir = new DirectoryInfo(ProjectsRootDirectory);
            
			if (string.IsNullOrEmpty(dir.Name) || dir.FullName == dir.Root.FullName)
			{
				return DefaultSolutionName;
			}
			return dir.Name;
		}

		public SlnGenerationMode Mode
		{
			get
			{
				return TargetProjectNames.Count == 0
					? SlnGenerationMode.FullGraph
					: SlnGenerationMode.PartialGraph;
			}
		}

		public List<string> IgnoreDirectoryPatterns
		{
			get { return ignoreDirectoryPatterns; }
		}

		public void AddAdditionalSearchPaths(params string[] searchPaths)
		{
			additionalSearchPaths.AddRange(searchPaths);
		}

		public void AddTargetProjectNames(params string[] targetProjectNames)
		{
			TargetProjectNames.AddRange(targetProjectNames);
		}

		public void AddIgnoreDirectoryPatterns(params string [] patterns)
		{
			ignoreDirectoryPatterns.AddRange(patterns);
		}
	}
}
