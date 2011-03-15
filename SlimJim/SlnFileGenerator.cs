using System.Collections.Generic;
using SlimJim.Infrastructure;
using SlimJim.Model;

namespace SlimJim
{
	public class SlnFileGenerator
	{
		public CsProjRepository ProjectRepository { get; set; }
		public SlnFileWriter SlnWriter { get; set; }

		public SlnFileGenerator()
		{
			ProjectRepository = new CsProjRepository();
			SlnWriter = new SlnFileWriter();
		}

		public void GeneratePartialGraphSolutionFile(string directory, string rootProjectName)
		{
			List<CsProj> projects = ProjectRepository.LookupCsProjsFromDirectory(directory);
			Sln solution = new SlnBuilder(projects).BuildPartialGraphSln(rootProjectName);
			SlnWriter.WriteSlnFile(solution, directory);
		}
	}
}