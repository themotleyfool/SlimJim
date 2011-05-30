using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using log4net;

namespace SlimJim.Infrastructure
{
	public class ProjectFileFinder
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ProjectFileFinder));
		private readonly List<Regex> ignorePatterns;

		public ProjectFileFinder()
		{
			ignorePatterns = new List<Regex>();
			IgnorePatterns(@"^\.svn$", @"^\.hg$", @"^\.git$", "^bin$", "^obj$", "ReSharper");
		}

		public virtual List<FileInfo> FindAllProjectFiles(string startPath)
		{
			Log.InfoFormat("Searching for .csproj files at {0}", startPath);

			var root = new DirectoryInfo(startPath);
			var projectFiles = GetProjectFiles(root);

			return projectFiles;
		}

		private List<FileInfo> GetProjectFiles(DirectoryInfo directory)
		{
			var files = new List<FileInfo>();

			if (!PathIsIgnored(directory))
			{
				Log.Debug("Searching subdirectory: " + directory.FullName);

				FileInfo[] projects = directory.GetFiles("*.csproj");

				if (projects.Length > 0)
				{
					files.Add(projects[0]);
					Log.InfoFormat("Found project {0} at {1}.", projects[0].Name, projects[0].DirectoryName);
				}
				else
				{
					foreach (DirectoryInfo dir in directory.EnumerateDirectories())
					{
						files.AddRange(GetProjectFiles(dir));
					}
				}
			}

			return files;
		}

		public bool PathIsIgnored(DirectoryInfo directory)
		{
			return ignorePatterns.Exists(p => p.IsMatch(directory.Name));
		}

		public virtual void IgnorePatterns(params string[] patterns)
		{
			var regexes = new List<string>(patterns).ConvertAll(p => new Regex(p, RegexOptions.IgnoreCase));
			ignorePatterns.AddRange(regexes);
		}
	}
}
