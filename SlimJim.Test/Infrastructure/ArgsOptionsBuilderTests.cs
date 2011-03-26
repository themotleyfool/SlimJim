using NUnit.Framework;
using SlimJim.Infrastructure;
using SlimJim.Model;

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
			Assert.That(options.TargetProjectNames, Is.EqualTo(new[] {TargetProject}));
			Assert.That(options.Mode, Is.EqualTo(SlnGenerationMode.PartialGraph));
		}

		[Test]
		public void NoArgs()
		{
			options = ArgsOptionsBuilder.BuildOptions(new string[0], WorkingDirectory);

			Assert.That(options.ProjectsRootDirectory, Is.EqualTo(WorkingDirectory));
			Assert.That(options.TargetProjectNames, Is.Empty);
			Assert.That(options.Mode, Is.EqualTo(SlnGenerationMode.FullGraph));
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

			Assert.That(options.TargetProjectNames, Is.EqualTo(new[] {"MyProject"}));
		}

		[Test]
		public void SpecifiedMultipleTargetProjects()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] {@"/p:MyProject", "/p:YourProject"}, WorkingDirectory);

			Assert.That(options.TargetProjectNames, Is.EqualTo(new[] {"MyProject", "YourProject"}));
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

		[Test]
		public void SpecifiedVisualStudioVersion90()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] {@"/v:VS2008"}, WorkingDirectory);

			Assert.That(options.VisualStudioVersion, Is.EqualTo(VisualStudioVersion.VS2008));
		}

		[Test]
		public void SpecifiedVisualStudioVersion10()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { @"/v:VS2010" }, WorkingDirectory);

			Assert.That(options.VisualStudioVersion, Is.EqualTo(VisualStudioVersion.VS2010));
		}

		[Test]
		public void InvalidVisualStudioVersionNumber()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] {@"/v:dumb"}, WorkingDirectory);

			Assert.That(options.VisualStudioVersion, Is.EqualTo(VisualStudioVersion.VS2010));
		}

		[Test]
		public void SpecifiedSolutionName()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] {@"/n:MyProjects"}, WorkingDirectory);

			Assert.That(options.SolutionName, Is.EqualTo("MyProjects"));
		}

		[Test]
		public void UnspecifiedSolutionNameWithSingleTargetProject()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] {@"/p:MyProject"}, WorkingDirectory);

			Assert.That(options.SolutionName, Is.EqualTo("MyProject"));
		}

		[Test] 
		public void UnspecifiedSolutionNameWithMultipleTargetProjectsUsesFirstProjectNamePlusSuffix()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { @"/p:MyProject", @"/p:YourProject" }, WorkingDirectory);

			Assert.That(options.SolutionName, Is.StringMatching("MyProject.+"));
		}

		[Test]
		public void UnspecifiedSolutionNameWithNoTargetProjectsUsesFolderName()
		{
			options = ArgsOptionsBuilder.BuildOptions(new string[] {}, WorkingDirectory);
			Assert.That(options.SolutionName, Is.EqualTo("WorkingDir"));

			options = ArgsOptionsBuilder.BuildOptions(new string[] { }, @"R:\Code\Projects\CSharp\SlumJim");
			Assert.That(options.SolutionName, Is.EqualTo("SlumJim"));
			
			options = ArgsOptionsBuilder.BuildOptions(new string[] { }, @"R:\Code\Projects\CSharp\SlumJim\");
			Assert.That(options.SolutionName, Is.EqualTo("SlumJim"));

			options = ArgsOptionsBuilder.BuildOptions(new string[] { }, @"R:\");
			Assert.That(options.SolutionName, Is.EqualTo("R"));

			options = ArgsOptionsBuilder.BuildOptions(new string[] { }, @"\");
			Assert.That(options.SolutionName, Is.EqualTo("SlimJim"));
		}

		[Test]
		public void TPrefixAlsoSetsTargetProject()
		{
			options = ArgsOptionsBuilder.BuildOptions(new[] { @"/t:MyProject" }, WorkingDirectory);

			Assert.That(options.TargetProjectNames, Is.EqualTo(new[] {"MyProject"}));
		}
	}
}