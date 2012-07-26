using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
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

			var renderer = new SlnFileRenderer(solution);
			var fileContents = renderer.Render();

			if (outputFile.Exists && ContentsUnmodified(outputFile, fileContents))
			{
				Log.Info("Solution file is unmodified from previous generation.");
				return outputFile;
			}

			using (var writer = new StreamWriter(outputFile.Open(FileMode.Create)))
			{
				writer.Write(fileContents);
			}

			Log.Info("Solution file written to " + outputFile.FullName);

			return outputFile;
		}

		private bool ContentsUnmodified(FileInfo outputFile, string fileContents)
		{
			var previousContents = File.ReadAllText(outputFile.FullName);
			
			fileContents = Regex.Replace(fileContents, @"Project\(.+\)", "Project");
			previousContents = Regex.Replace(previousContents, @"Project\(.+\)", "Project");

			return fileContents == previousContents;
		}

		private string GetOutputFilePath(string writeInDirectory, Sln solution)
		{
			return Path.Combine(writeInDirectory, solution.Name + ".sln");
		}
	}
}