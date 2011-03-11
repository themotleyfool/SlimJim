using System;
using System.Collections.Generic;
using System.IO;

namespace SlimJim.Infrastructure
{
	public class ProjectFileFinder
	{
		private readonly List<string> ignorePaths;

		public ProjectFileFinder()
		{
			ignorePaths = new List<string>();
		}

		public virtual List<FileInfo> FindAllProjectFiles(string startPath)
		{
			var root = new DirectoryInfo(startPath);

			var projectFiles = GetProjectFiles(root);

			return projectFiles;
		}

		private List<FileInfo> GetProjectFiles(DirectoryInfo directory)
		{
			var files = new List<FileInfo>();
			FileInfo[] projects = directory.GetFiles("*.csproj");
			
			if (!PathIsIgnored(directory))
			{
				if (projects.Length > 0)
				{
					files.Add(projects[0]);
					Console.WriteLine("Found project " + projects[0].Name);
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

		private bool PathIsIgnored(DirectoryInfo directory)
		{
			return ignorePaths.Contains(directory.Name.ToLower());
		}

		public virtual void IgnorePaths(params string[] paths)
		{
			var lowerCasedPaths = new List<string>(paths).ConvertAll(p => p.ToLower());
			ignorePaths.AddRange(lowerCasedPaths);
		}
	}
}
