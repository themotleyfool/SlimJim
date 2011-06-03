using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;

namespace SlimJim.Infrastructure
{
	public class ProjectFileFinder
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
				SearchDirectoryForProjects(directory, files);
			}

			return files;
		}

		private void SearchDirectoryForProjects(DirectoryInfo directory, List<FileInfo> files)
		{
			FileInfo[] projects = directory.GetFiles("*.csproj");

			if (projects.Length > 0)
			{
				AddProjectFile(projects, files);
			}
			else
			{
				RecurseChildDirectories(directory, files);
			}
		}

		private void RecurseChildDirectories(DirectoryInfo directory, List<FileInfo> files)
		{
			foreach (DirectoryInfo dir in directory.EnumerateDirectories())
			{
				files.AddRange(GetProjectFiles(dir));
			}
		}

		private void AddProjectFile(FileInfo[] projects, List<FileInfo> files)
		{
			files.Add(projects[0]);

			Log.Debug(projects[0].FullName);
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
