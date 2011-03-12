using System.Collections.Generic;
using NUnit.Framework;
using SlimJim.Infrastructure;
using SlimJim.Model;
using SlimJim.Test.SampleFiles;

namespace SlimJim.Test.Infrastructure
{
	[TestFixture]
	public class SlnFileRendererTests
	{
		[Test]
		public void EmptySolution()
		{
			Sln solution = new Sln
				{
					Name = "BlankSolution",
					Projects = new List<CsProj>()
				};

			var renderer = new SlnFileRenderer(solution);

			string slnContents = renderer.Render();
			string expected = SampleFileHelper.GetSlnFileContents(solution.Name);

			Assert.That(slnContents, Is.EqualTo(expected));
		}
	}
}