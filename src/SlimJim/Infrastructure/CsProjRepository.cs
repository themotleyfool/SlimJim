using System.Collections.Generic;
using System.IO;
using SlimJim.Model;

namespace SlimJim.Infrastructure
{
	public class CsProjRepository : ICsProjRepository
	{
		public CsProjRepository()
		{
			Finder = new ProjectFileFinder();
			Reader = new CsProjReader();
		}

		public virtual List<CsProj> LookupCsProjsFromDirectory(SlnGenerationOptions options)
		{
			List<FileInfo> files = FindAllProjectFiles(options);

			List<CsProj> projects = ReadProjectFilesIntoCsProjObjects(files);

			return projects;
		}

		private List<FileInfo> FindAllProjectFiles(SlnGenerationOptions options)
		{
			List<FileInfo> files = Finder.FindAllProjectFiles(options.ProjectsRootDirectory);

			foreach (string path in options.AdditionalSearchPaths)
			{
				files.AddRange(Finder.FindAllProjectFiles(path));
			}

			return files;
		}

		private List<CsProj> ReadProjectFilesIntoCsProjObjects(List<FileInfo> files)
		{
			List<CsProj> projects = files.ConvertAll(f => Reader.Read(f));
			projects.RemoveAll(p => p == null);
			return projects;
		}

		public ProjectFileFinder Finder { get; set; }
		public CsProjReader Reader { get; set; }
	}
}