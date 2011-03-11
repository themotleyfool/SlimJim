using System;
using System.Collections.Generic;
using SlimJim.Infrastructure;
using SlimJim.Model;

namespace SlimJim
{
	public class SlnFileGenerator
	{
		public CsProjRepository ProjectRepository { get; set; }
		public SlnFileWriter SlnWriter { get; set; }

		public void GeneratePartialGraphSolutionFile(string directory, string rootProjectName)
		{
			List<CsProj> projects = ProjectRepository.LookupCsProjsFromDirectory(directory);
			Sln solution = new SlnGenerator(projects).GeneratePartialGraphSln(rootProjectName);
			SlnWriter.WriteSlnFile(solution, directory);
		}
	}
}