using NUnit.Framework;
using SlimJim.Infrastructure;

namespace SlimJim.Test.Infrastructure
{
	[TestFixture]
	public class ArgsOptionsBuilderTests
	{
		private SlnGenerationOptions options;
		private const string WorkingDirectory = @"C:\WorkingDir";
		private const string TargetProject = "TargetProject";

		[Test]
		public void TargetProjectOnly()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] {TargetProject}, WorkingDirectory);

			Assert.That(options.ProjectsRootDirectory, Is.EqualTo(WorkingDirectory));
			Assert.That(options.TargetProjectName, Is.EqualTo(TargetProject));
			Assert.That(options.Mode, Is.EqualTo(SlnGenerationMode.Partial));

		}

		[Test]
		public void NoArgs()
		{
			options = ArgsOptionsBuilder.BuildOptions(new string[0], WorkingDirectory);

			Assert.That(options.ProjectsRootDirectory, Is.EqualTo(WorkingDirectory));
			Assert.That(options.TargetProjectName, Is.Null);
			Assert.That(options.Mode, Is.EqualTo(SlnGenerationMode.Full));
			Assert.That(options.AdditionalSearchPaths, Is.Empty);
		}

		[Test]
		public void SpecifiedProjectsRootDirectory()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { @"/d:C:\MyProjects" }, WorkingDirectory);

			Assert.That(options.ProjectsRootDirectory, Is.EqualTo(@"C:\MyProjects"));
		}

		[Test]
		public void SpecifiedTargetProject()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { @"/p:MyProject" }, WorkingDirectory);

			Assert.That(options.TargetProjectName, Is.EqualTo("MyProject"));
		}

		[Test]
		public void SpecifiedAdditionalSearchPaths()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { @"/a:C:\OtherProjects", @"/a:C:\MoreProjects" }, WorkingDirectory);

			Assert.That(options.AdditionalSearchPaths, Is.EqualTo(new[] { @"C:\OtherProjects", @"C:\MoreProjects" }));
		}

		[Test]
		public void SpecifiedSlnOuputPath()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { @"/o:C:\MyProjects\Sln" }, WorkingDirectory);

			Assert.That(options.SlnOutputPath, Is.EqualTo(@"C:\MyProjects\Sln"));
		}
	}
}