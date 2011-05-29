using System.IO;
using SlimJim.Infrastructure;

namespace SlimJim
{
	public class SlimJimMain
	{
		public static void Main(string[] args)
		{
			var fileGenerator = new SlnFileGenerator();
			var options = ArgsOptionsBuilder.BuildOptions(args, Directory.GetCurrentDirectory());
			
			fileGenerator.GenerateSolutionFile(options);
		}
	}
}
