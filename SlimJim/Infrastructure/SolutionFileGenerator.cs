using System.Collections.Generic;
using SlimJim.Model;

namespace SlimJim.Infrastructure
{
	public class SolutionFileGenerator
	{
		private readonly ICsProjRegistry csProjRegistry;

		public SolutionFileGenerator(ICsProjRegistry csProjRegistry)
		{
			this.csProjRegistry = csProjRegistry;
		}

		public SolutionFileInfo GenerateSolutionFile(string rootDirectory, string slnFilePath)
		{
			List<CsProj> projects = csProjRegistry.LookupCsProjsFromDirectory(rootDirectory);
			var solutionFile = new SolutionFileInfo(slnFilePath, "")
			                   	{
			                   		CsProjs = new List<CsProj>(projects)
			                   	};
			return solutionFile;
		}
	}
}