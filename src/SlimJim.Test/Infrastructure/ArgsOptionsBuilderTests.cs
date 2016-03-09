using log4net.Core;
using NUnit.Framework;
using SlimJim.Infrastructure;
using SlimJim.Model;

namespace SlimJim.Test.Infrastructure
{
	[TestFixture]
	public class ArgsOptionsBuilderTests : TestBase
	{
		private SlnGenerationOptions options;

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
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--root", WorkingDirectory }, WorkingDirectory);

			Assert.That(options.ProjectsRootDirectory, Is.EqualTo(WorkingDirectory));
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
			var otherDir = GetSamplePath("OtherProjects");
			var moreProjects = GetSamplePath("MoreProjects");
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--search", otherDir, "--search", moreProjects }, WorkingDirectory);

			Assert.That(options.AdditionalSearchPaths, Is.EqualTo(new[] { otherDir, moreProjects }));
		}

		[Test]
		public void SpecifiedSlnOuputPath()
		{
			var slnDir = GetSamplePath(WorkingDirectory, "Sln");
			options = ArgsOptionsBuilder.BuildOptions(new[] { "--out", slnDir }, WorkingDirectory);

			Assert.That(options.SlnOutputPath, Is.EqualTo(slnDir));
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

			Assert.That(options.VisualStudioVersion, Is.EqualTo(VisualStudioVersion.VS2015));
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

		[Test]
		public void DefaultThresholdIsInfo()
		{
			options = ArgsOptionsBuilder.BuildOptions(new string[0], WorkingDirectory);

			Assert.That(options.LoggingThreshold, Is.EqualTo(Level.Info));
		}

		[Test]
		public void ExtraVerbose()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "-vv" }, WorkingDirectory);

			Assert.That(options.LoggingThreshold, Is.EqualTo(Level.Trace));
		}

		[Test]
		public void Quiet()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { "-q" }, WorkingDirectory);

			Assert.That(options.LoggingThreshold, Is.EqualTo(Level.Warn));
		}
	}
}