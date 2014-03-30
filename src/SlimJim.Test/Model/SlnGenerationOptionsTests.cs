using NUnit.Framework;
using SlimJim.Model;
using System.IO;

namespace SlimJim.Test.Model
{
	[TestFixture]
	public class SlnGenerationOptionsTests : TestBase
	{
		private SlnGenerationOptions options;

		[Test]
		public void SolutionOutputPathDefaultsToProjectsRootPath()
		{
			options = new SlnGenerationOptions(GetSamplePath("Projects"));

			Assert.That(options.SlnOutputPath, Is.EqualTo(options.ProjectsRootDirectory));
		}

		[Test]
		public void SolutionOutputPathUsesGivenValueIfSet()
		{
			string slnOutputPath = GetSamplePath("Projects", "Solutions");
			options = new SlnGenerationOptions(GetSamplePath("Projects")) {SlnOutputPath = slnOutputPath};

			Assert.That(options.SlnOutputPath, Is.EqualTo(slnOutputPath));
		}

		[Test]
		public void UnspecifiedSolutionNameWithNoTargetProjectsUsesFolderName()
		{
			options = new SlnGenerationOptions(WorkingDirectory);
			Assert.That(options.SolutionName, Is.EqualTo("WorkingDir"));

			options = new SlnGenerationOptions(Path.Combine(WorkingDirectory, "SlumJim"));
			Assert.That(options.SolutionName, Is.EqualTo("SlumJim"));

			options = new SlnGenerationOptions(Path.Combine(WorkingDirectory, "SlumJim") + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar);
			Assert.That(options.SolutionName, Is.EqualTo("SlumJim"));

			options = new SlnGenerationOptions(Path.DirectorySeparatorChar.ToString());
			Assert.That(options.SolutionName, Is.EqualTo("SlimJim"));
		}

		[Test]
		public void AdditionalSearchPathsRootedAtProjectRoot()
		{
			var root = GetSamplePath ("Proj", "Root");
			options = new SlnGenerationOptions(root);
			var path1 = Path.Combine("..", "SearchPath");
			var path2 = Path.Combine("..", "..", "OtherPath", "Pork");
			options.AddAdditionalSearchPaths (path1, path2);

			Assert.That(options.AdditionalSearchPaths, Is.EqualTo(new[] {Path.Combine(root, path1), Path.Combine(root, path2)}));
		}

		[Test]
		public void RelativeSlnOutputPathRootedAtProjectsRoot()
		{
			var root = GetSamplePath ("Proj", "Root");
			options = new SlnGenerationOptions (root);
			options.SlnOutputPath = "Solutions";

			Assert.That(options.SlnOutputPath, Is.EqualTo(Path.Combine(root, "Solutions")));
		}

		[Test]
		public void RelativeProjectsRootDirIsRootedAtWorkingDir()
		{
			options = new SlnGenerationOptions(WorkingDirectory);
			options.ProjectsRootDirectory = Path.Combine("Proj", "Root");

			Assert.That(options.SlnOutputPath, Is.EqualTo (Path.Combine (WorkingDirectory, "Proj", "Root")));
		}
	}
}