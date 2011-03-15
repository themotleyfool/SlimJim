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

		public void GenerateSolutionFile(SlnGenerationOptions options)
		{
			List<CsProj> projects = ProjectRepository.LookupCsProjsFromDirectory(options.ProjectsRootDirectory);
			Sln solution = new SlnBuilder(projects).BuildPartialGraphSln(options.TargetProjectName);
			SlnWriter.WriteSlnFile(solution, options.ProjectsRootDirectory);
		}
	}
}