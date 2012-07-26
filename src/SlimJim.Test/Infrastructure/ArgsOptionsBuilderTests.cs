using NUnit.Framework;
using SlimJim.Infrastructure;
using SlimJim.Model;

namespace SlimJim.Test.Infrastructure
{
	[TestFixture]
	public class ArgsOptionsBuilderTests
	{
		private SlnGenerationOptions options;
		private bool parseErrorHandled;
		private const string WorkingDirectory = @"C:\WorkingDir";

		[Test]
		public void TestDefaults()
		{
			options = ArgsOptionsBuilder.BuildOptions(new string[0], WorkingDirectory);

			Assert.That(options.ProjectsRootDirectory, Is.EqualTo(WorkingDirectory), "ProjectsRootDirectory");
			Assert.That(options.TargetProjectNames, Is.Empty, "TargetProjectNames");
			Assert.That(options.Mode, Is.EqualTo(SlnGenerationMode.FullGraph), "Mode");
			Assert.That(options.AdditionalSearchPaths, Is.Empty, "AdditionalSearchPaths");
			Assert.That(options.IncludeEfferentAssemblyReferences, Is.False, "IncludeEfferentAssemblyReferences");
			Assert.That(options.ShowHelp, Is.False, "ShowHelp");
			Assert.That(options.OpenInVisualStudio, Is.False, "OpenInVisualStudio");
		}

		[Test]
		public void SpecifiedProjectsRootDirectory()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--root", @"C:\MyProjects" }, WorkingDirectory);

			Assert.That(options.ProjectsRootDirectory, Is.EqualTo(@"C:\MyProjects"));
		}

		[Test]
		public void SpecifiedTargetProject()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--target", "MyProject" }, WorkingDirectory);

			Assert.That(options.TargetProjectNames, Is.EqualTo(new[] { "MyProject" }));
			Assert.That(options.SolutionName, Is.EqualTo("MyProject"));

		}

		[Test]
		public void SpecifiedMultipleTargetProjects()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--target", "MyProject", "--target", "YourProject" }, WorkingDirectory);

			Assert.That(options.TargetProjectNames, Is.EqualTo(new[] { "MyProject", "YourProject" }));
			Assert.That(options.SolutionName, Is.StringMatching("MyProject_YourProject"));
		}

		[Test]
		public void SpecifiedAdditionalSearchPaths()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--search", @"C:\OtherProjects", "--search", @"C:\MoreProjects" }, WorkingDirectory);

			Assert.That(options.AdditionalSearchPaths, Is.EqualTo(new[] { @"C:\OtherProjects", @"C:\MoreProjects" }));
		}

		[Test]
		public void SpecifiedSlnOuputPath()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--out", @"C:\MyProjects\Sln" }, WorkingDirectory);

			Assert.That(options.SlnOutputPath, Is.EqualTo(@"C:\MyProjects\Sln"));
		}

		[Test]
		public void SpecifiedVisualStudioVersion2008()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--version", "2008" }, WorkingDirectory);

			Assert.That(options.VisualStudioVersion, Is.EqualTo(VisualStudioVersion.VS2008));
		}

		[Test]
		public void SpecifiedVisualStudioVersion2010()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--version", "2010" }, WorkingDirectory);

			Assert.That(options.VisualStudioVersion, Is.EqualTo(VisualStudioVersion.VS2010));
		}

		[Test]
		public void InvalidVisualStudioVersionNumber()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--version", "dumb" }, WorkingDirectory);

			Assert.That(options.VisualStudioVersion, Is.EqualTo(VisualStudioVersion.VS2010));
		}

		[Test]
		public void SpecifiedSolutionName()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--name", "MyProjects" }, WorkingDirectory);

			Assert.That(options.SolutionName, Is.EqualTo("MyProjects"));
		}

		[Test]
		public void UnspecifiedSolutionNameWithSingleTargetProject()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--target", "MyProject" }, WorkingDirectory);

		}

		[Test]
		public void UnspecifiedSolutionNameWithMultipleTargetProjectsUsesFirstProjectNamePlusSuffix()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--target", "MyProject", "--target", "YourProject" }, WorkingDirectory);

		}
		
		[Test]
		public void IncludeEfferentAssemblyReferences()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { @"--all" }, WorkingDirectory);

			Assert.That(options.IncludeEfferentAssemblyReferences, Is.True, "IncludeEfferentAssemblyReferences");
		}

		[Test]
		public void IgnoresFolderNames()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] {"--ignore", "Folder1", "--ignore", "Folder2"}, WorkingDirectory);

			Assert.That(options.IgnoreDirectoryPatterns, Is.EqualTo(new[] {"Folder1", "Folder2"}));
		}

		[Test, Explicit]
		public void ShowHelp()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] {"--help"}, WorkingDirectory);
			// check console output
		}

		[Test]
		public void ShowHelpIsSetOnOptions()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--help" }, WorkingDirectory);

			Assert.That(options.ShowHelp, Is.True, "ShowHelp");
		}

		[Test]
		public void SpecifyOpenInVisualStudio()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--open" }, WorkingDirectory);

			Assert.That(options.OpenInVisualStudio, Is.True, "OpenInVisualStudio");
		}
	}
}