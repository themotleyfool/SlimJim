using System.IO;
using SlimJim.Model;

namespace SlimJim.Infrastructure
{
	public class SlnFileWriter
	{
		public virtual void WriteSlnFile(Sln solution, string writeInDirectory)
		{
			var outputFile = new FileInfo(GetOutputFilePath(writeInDirectory, solution));

			if (!outputFile.Directory.Exists)
			{
				outputFile.Directory.Create();
			}

			using (var writer = new StreamWriter(outputFile.Open(FileMode.Create)))
			{
				var renderer = new SlnFileRenderer(solution);
				string fileContents = renderer.Render();
				writer.Write(fileContents);
			}
		}

		private string GetOutputFilePath(string writeInDirectory, Sln solution)
		{
			return Path.Combine(writeInDirectory, solution.Name + ".sln");
		}
	}
}