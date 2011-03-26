using NUnit.Framework;
using SlimJim.Model;

namespace SlimJim.Test.Model
{
	[TestFixture]
	public class SolutionGenerationOptionsTests
	{
		[Test]
		public void SolutionOutputPathDefaultsToProjectsRootPath()
		{
			var options = new SlnGenerationOptions(@"C:\Projects");

			Assert.That(options.SlnOutputPath, Is.EqualTo(options.ProjectsRootDirectory));
		}

		[Test]
		public void SolutionOutputPathUsesGivenValueIfSet()
		{
			string slnOutputPath = @"C:\Projects\Solutions";
			var options = new SlnGenerationOptions(@"C:\Projects") {SlnOutputPath = slnOutputPath};

			Assert.That(options.SlnOutputPath, Is.EqualTo(slnOutputPath));
		}
	}
}