using NUnit.Framework;

namespace SlimJim.Test.Model.SlnBuilder
{
	[TestFixture]
	public class IncludeAllProjectsInSln : SlnBuilderTestFixture
	{
		[Test]
		public void WithNoTargetsInOptionsAllProjectsAreIncluded()
		{
			GeneratePartialGraphSolution(new string[0], projects.MyProject, projects.OurProject1, 
				projects.TheirProject1, projects.Unrelated1);
			Assert.That(solution.Projects, Is.EqualTo(new[] {projects.MyProject, projects.OurProject1, 
				projects.TheirProject1, projects.Unrelated1}));
		}
	}
}