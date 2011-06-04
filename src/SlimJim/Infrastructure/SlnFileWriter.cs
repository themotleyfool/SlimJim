using System.IO;
using System.Reflection;
using log4net;
using SlimJim.Model;

namespace SlimJim.Infrastructure
{
	public class SlnFileWriter
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public virtual FileInfo WriteSlnFile(Sln solution, string writeInDirectory)
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

			Log.Info("Solution file written to " + outputFile.FullName);

			return outputFile;
		}

		private string GetOutputFilePath(string writeInDirectory, Sln solution)
		{
			return Path.Combine(writeInDirectory, solution.Name + ".sln");
		}
	}
}