using System.IO;
using SlimJim.Infrastructure;

namespace SlimJim
{
	public class SlimJimMain
	{
		public static void Main(string[] args)
		{
			string rootProjectName = args[0];
			var fileGenerator = new SlnFileGenerator
				{
					ProjectRepository = new CsProjRepository
						{
							Finder = new ProjectFileFinder(),
							Reader = new CsProjReader()
						},
					SlnWriter = new SlnFileWriter()
				};

			fileGenerator.GeneratePartialGraphSolutionFile(Directory.GetCurrentDirectory(), rootProjectName);
		}
	}
}
