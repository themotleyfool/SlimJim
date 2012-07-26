using System;
using System.Collections.Generic;
using System.IO;
using log4net;

namespace SlimJim.Model
{
	public class Sln
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Sln));

		public Sln(string name)
			:this(name, System.Guid.NewGuid().ToString("B"))
		{
		}

		public Sln(string name, string guid)
		{
			Name = name;
			Guid = guid;
			Projects = new List<CsProj>();
			Version = VisualStudioVersion.VS2010;
		}

		private readonly IDictionary<string, Folder> folders = new Dictionary<string, Folder>();

		public string Name { get; private set; }
		public string Guid { get; private set; }
		public VisualStudioVersion Version { get; set; }

		private string projectsRootDirectory;
		public string ProjectsRootDirectory
		{
			get
			{
				return projectsRootDirectory;
			}
			set
			{
				if (value != null && (value.EndsWith("/") || value.EndsWith(@"\")))
				{
					projectsRootDirectory = value.Substring(0, value.Length - 1);
				}
				else
				{
					projectsRootDirectory = value;
				}
			}
		}
		public List<CsProj> Projects { get; private set; }

		public IEnumerable<Folder> Folders
		{
			get
			{
				return folders.Count > 0 ? folders.Values : null;
			}
		}

		public void AddProjects(params CsProj[] csProjs)
		{
			foreach (CsProj proj in csProjs)
			{
				if (!Projects.Contains(proj))
				{
					Projects.Add(proj);
					AddProjectToFolder(proj);
				}
			}
		}

		private void AddProjectToFolder(CsProj proj)
		{
			if (string.IsNullOrEmpty(ProjectsRootDirectory) || !proj.Path.StartsWith(ProjectsRootDirectory, StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			var relativeFolderPath = GetSolutionFolderPath(proj);

			if (string.IsNullOrEmpty(relativeFolderPath)) return;

			var folder = GetOrCreateSolutionFolder(relativeFolderPath);
			folder.AddContent(proj.Guid);
		}

		private string GetSolutionFolderPath(CsProj proj)
		{
			var relativeProjectPath = proj.Path.Substring(ProjectsRootDirectory.Length + 1);

			return Path.GetDirectoryName(Path.GetDirectoryName(relativeProjectPath));
		}

		private Folder GetOrCreateSolutionFolder(string relativeFolderPath)
		{
			Folder folder;

			if (folders.TryGetValue(relativeFolderPath, out folder)) return folder;

			Log.Debug("Creating solution folder for " + relativeFolderPath);

			var newFolder = new Folder { FolderName = Path.GetFileName(relativeFolderPath), Guid = System.Guid.NewGuid().ToString("B") };

			var parentFolderPath = Path.GetDirectoryName(relativeFolderPath);
			if (!string.IsNullOrEmpty(parentFolderPath))
			{
				var parentFolder = GetOrCreateSolutionFolder(parentFolderPath);

				parentFolder.AddContent(newFolder.Guid);
			}

			folders[relativeFolderPath] = newFolder;

			return newFolder;
		}
	}
}