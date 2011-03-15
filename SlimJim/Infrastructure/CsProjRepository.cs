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

		public virtual List<CsProj> LookupCsProjsFromDirectory(string rootDirectory)
		{
			List<FileInfo> files = Finder.FindAllProjectFiles(rootDirectory);
			List<CsProj> projects = files.ConvertAll(f => Reader.Read(f));
			projects.RemoveAll(p => p == null);
			return projects;
		}

		public ProjectFileFinder Finder { get; set; }
		public CsProjReader Reader { get; set; }
	}
}