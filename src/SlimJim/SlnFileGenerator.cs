using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using log4net;

namespace SlimJim
{
    using Infrastructure;
    using Model;

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

			if (options.FixHintPaths)
			{
				new HintPathConverter().ConvertHintPaths(solution, options);
			}

			if (options.ConvertReferences)
			{
				new ReferenceConverter().ConvertToProjectReferences(solution);
			}
			else if (options.RestoreReferences)
			{
				new ReferenceConverter().RestoreAssemblyReferences(solution);
			}

			if (options.RestoreHintPaths)
			{
				new HintPathConverter().RestoreHintPaths(solution, options);
			}

			WarnOnMissingNuGetTargets(projects, options);

			return SlnWriter.WriteSlnFile(solution, options.SlnOutputPath).FullName;
		}

		private void WarnOnMissingNuGetTargets(List<CsProj> projects, SlnGenerationOptions options)
		{
			var nugetTargetsPath = Path.Combine(options.SlnOutputPath, ".nuget", "nuget.targets");
			if (projects.Any(p => p.UsesMSBuildPackageRestore) && !File.Exists(nugetTargetsPath))
			{
				Log.WarnFormat(
					"One or more of the projects included use MSBuild-based NuGet package restore. " +
					"To ensure these projects build correctly from the generated solution, copy nuget.targets " +
					"from the solution to {0}.", nugetTargetsPath);
			}
		}

		private void LogSummary(SlnGenerationOptions options)
		{
			Log.InfoFormat("SlimJim solution file generator.");
			Log.InfoFormat("");
			Log.InfoFormat("----------------------------------------");
			Log.InfoFormat("Target projects:\t{0}", SummarizeTargetProjects(options));
			Log.InfoFormat("Destination:\t\t{0}", Path.Combine(options.SlnOutputPath, options.SolutionName + ".sln"));
			Log.InfoFormat("Project References:\t{0}", options.ConvertReferences ? "Convert" : options.RestoreReferences ? "Restore" : "Do Nothing");
			Log.InfoFormat("Hint Paths:\t\t{0}", options.FixHintPaths ? "Adjust" : options.RestoreReferences ? "Restore" : "Do Nothing");
			Log.InfoFormat("Visual Studio Version:\t{0}", options.VisualStudioVersion);
			Log.InfoFormat("Dinosaur:\t\t{0}", GetDinosaur());
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