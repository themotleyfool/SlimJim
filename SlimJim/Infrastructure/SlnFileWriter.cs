using System.IO;
using SlimJim.Model;

namespace SlimJim.Infrastructure
{
	public class SlnFileWriter
	{
		public virtual void WriteSlnFile(Sln solution, string writeInDirectory)
		{
			using (var writer = new StreamWriter(GetOutputFilePath(writeInDirectory, solution)))
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