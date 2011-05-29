using NUnit.Framework;
using SlimJim.Infrastructure;
using SlimJim.Model;

namespace SlimJim.Test.Model
{
	[TestFixture]
	public class SolutionGenerationOptionsTests
	{
		private SlnGenerationOptions options;
		private const string WorkingDirectory = @"C:\WorkingDir";

		[Test]
		public void SolutionOutputPathDefaultsToProjectsRootPath()
		{
			options = new SlnGenerationOptions(@"C:\Projects");

			Assert.That(options.SlnOutputPath, Is.EqualTo(options.ProjectsRootDirectory));
		}

		[Test]
		public void SolutionOutputPathUsesGivenValueIfSet()
		{
			string slnOutputPath = @"C:\Projects\Solutions";
			options = new SlnGenerationOptions(@"C:\Projects") {SlnOutputPath = slnOutputPath};

			Assert.That(options.SlnOutputPath, Is.EqualTo(slnOutputPath));
		}

		[Test]
		public void UnspecifiedSolutionNameWithNoTargetProjectsUsesFolderName()
		{
			options = new SlnGenerationOptions(WorkingDirectory);
			Assert.That(options.SolutionName, Is.EqualTo("WorkingDir"));

			options = new SlnGenerationOptions(@"R:\Code\Projects\CSharp\SlumJim");
			Assert.That(options.SolutionName, Is.EqualTo("SlumJim"));

			options = new SlnGenerationOptions(@"R:\Code\Projects\CSharp\SlumJim\");
			Assert.That(options.SolutionName, Is.EqualTo("SlumJim"));

			options = new SlnGenerationOptions(@"R:\");
			Assert.That(options.SolutionName, Is.EqualTo("R"));

			options = new SlnGenerationOptions(@"\");
			Assert.That(options.SolutionName, Is.EqualTo("SlimJim"));
		}
	}
}