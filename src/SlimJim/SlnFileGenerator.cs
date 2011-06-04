using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using SlimJim.Infrastructure;
using SlimJim.Model;

namespace SlimJim
{
	public class SlnFileGenerator
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(SlnFileGenerator));
		public CsProjRepository ProjectRepository { get; set; }
		public SlnFileWriter SlnWriter { get; set; }

		public SlnFileGenerator()
		{
			ProjectRepository = new CsProjRepository();
			SlnWriter = new SlnFileWriter();
		}

		public string GenerateSolutionFile(SlnGenerationOptions options)
		{
			LogSummary(options);

			List<CsProj> projects = ProjectRepository.LookupCsProjsFromDirectory(options);
			Sln solution = SlnBuilder.GetSlnBuilder(projects).BuildSln(options);
			return SlnWriter.WriteSlnFile(solution, options.SlnOutputPath).FullName;
		}

		private void LogSummary(SlnGenerationOptions options)
		{
			Log.InfoFormat("SlimJim solution file generator.");
			Log.InfoFormat("");
			Log.InfoFormat("----------------------------------------");
			Log.InfoFormat("Target projects:             {0}", SummarizeTargetProjects(options));
			Log.InfoFormat("Destination:                 {0}", Path.Combine(options.SlnOutputPath, options.SolutionName + ".sln"));
			Log.InfoFormat("Visual Studio Version:       {0}", options.VisualStudioVersion);
			Log.InfoFormat("Dinosaur:                    {0}", GetDinosaur());
			Log.InfoFormat("----------------------------------------");
			Log.InfoFormat("");
		}

		private string GetDinosaur()
		{
			var dinosaurNumber = DateTime.Now.Ticks % 3;

			switch (dinosaurNumber)
			{
				case 0:
					return "Tyranosaurus Rex";
				case 1:
					return "Triceratops";
				default:
					return "Giganotosaurus";
			}
		}

		private string SummarizeTargetProjects(SlnGenerationOptions options)
		{
			var targets = string.Join(", ", options.TargetProjectNames);

			return string.IsNullOrEmpty(targets) ? "<none>" : targets;
		}
	}
}