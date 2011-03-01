using System.IO;

namespace SlimJim
{
	public class SolutionFileGenerator
	{
		private string rootDirectory;

		public SolutionFileGenerator(string rootDirectory)
		{
			this.rootDirectory = rootDirectory;
		}

		public FileInfo GenerateSolutionFile()
		{
			string slnFileName = Path.GetDirectoryName(rootDirectory) + ".sln";
			string slnFilePath = Path.Combine(rootDirectory, slnFileName);
			return new FileInfo(slnFilePath);
		}
	}
}