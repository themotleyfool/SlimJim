using System.Collections.Generic;
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

		private void MakeSolution(string name, params CsProj[] csProjs)
		{
			solution = new Sln
				{
					Name = name,
					Guid = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}",
					Projects = new List<CsProj>(csProjs)
				};
		}

		private void TestRender()
		{
			renderer = new SlnFileRenderer(solution);

			string slnContents = renderer.Render();
			string expected = SampleFileHelper.GetSlnFileContents(solution.Name);

			Assert.That(slnContents, Is.EqualTo(expected));
		}
	}
}