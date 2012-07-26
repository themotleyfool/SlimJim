using NUnit.Framework;
using SlimJim.Infrastructure;
using SlimJim.Model;
using SlimJim.Test.Model;
using SlimJim.Test.SampleFiles;

namespace SlimJim.Test.Infrastructure
{
	[TestFixture]
	public class SlnFileRendererTests
	{
		private Sln solution;
		private SlnFileRenderer renderer;
		private ProjectPrototypes projects;

		[SetUp]
		public void BeforeEach()
		{
			projects = new ProjectPrototypes();
		}

		[Test]
		public void EmptySolution()
		{
			MakeSolution("BlankSolution");

			TestRender();
		}

		[Test]
		public void SingleProjectSolution()
		{
			MakeSolution("SingleProject", projects.MyProject);

			TestRender();
		}

		[Test]
		public void ThreeProjectSolution()
		{
			MakeSolution("ThreeProjects", projects.MyProject, projects.OurProject1, projects.OurProject2);

			TestRender();
		}

		[Test]
		public void ManyProjectSolution()
		{
			MakeSolution("ManyProjects", projects.MyProject, projects.OurProject1, projects.OurProject2,
				projects.TheirProject1, projects.TheirProject2, projects.TheirProject3);

			TestRender();
		}

		[Test]
		public void VisualStudio2008Solution()
		{
			MakeSolution("VS2008");
			solution.Version = VisualStudioVersion.VS2008;

			TestRender();
		}

		private void MakeSolution(string name, params CsProj[] csProjs)
		{
			solution = new Sln(name, "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}");
			solution.AddProjects(csProjs);
		}

		private void TestRender()
		{
			renderer = new SlnFileRenderer(solution);

			string actualContents = renderer.Render().Replace("\r\n", "\n");
			string expectedContents = SampleFileHelper.GetSlnFileContents(solution.Name).Replace("\r\n", "\n");

			Assert.That(actualContents, Is.EqualTo(expectedContents));
		}
	}
}