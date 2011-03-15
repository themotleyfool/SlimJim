using System.Collections.Generic;
using NUnit.Framework;
using SlimJim.Model;

namespace SlimJim.Test.Model
{
	[TestFixture]
	public class SlnBuilderTests
	{
		private string rootProjectName;
		private Sln solution;
		private ProjectPrototypes projects;

		[SetUp]
		public void BeforeEach()
		{
			projects = new ProjectPrototypes();
			rootProjectName = projects.MyProject.AssemblyName;
		}

		[Test]
		public void SlnNameIsEqualToRootProjectName()
		{
			GeneratePartialGraphSolution(rootProjectName);
			Assert.That(solution.Name, Is.EqualTo(rootProjectName));
		}

		[Test]
		public void EmptySln()
		{
			GeneratePartialGraphSolution(rootProjectName);
			Assert.That(solution.Projects, Is.Empty);
		}

		[Test]
		public void ProjectListDoesNotContainRootProject()
		{
			GeneratePartialGraphSolution(rootProjectName, projects.Unrelated1);
			Assert.That(solution.Projects, Is.Empty);
		}

		[Test]
		public void ReferencesAssemblyNotInProjectsList()
		{
			projects.MyProject.ReferencesAssemblies(projects.TheirProject1);
			GeneratePartialGraphSolution(rootProjectName, projects.MyProject);
			Assert.That(solution.Projects, Is.EqualTo(new[] {projects.MyProject}));
		}

		[Test]
		public void SingleProjectSln()
		{
			GeneratePartialGraphSolution(rootProjectName, projects.MyProject);
			Assert.That(solution.Projects, Is.EqualTo(new[] {projects.MyProject}));
		}

		[Test]
		public void SingleEfferentAssemblyReference()
		{
			projects.MyProject.ReferencesAssemblies(projects.TheirProject1);
			GeneratePartialGraphSolution(rootProjectName, projects.MyProject, projects.TheirProject1);
			Assert.That(solution.Projects, Is.EqualTo(new[] {projects.MyProject}));
		}

		[Test]
		public void UnrelatedProjectListProducesSingleProjectGraph()
		{
			GeneratePartialGraphSolution(rootProjectName, projects.MyProject, projects.Unrelated1, projects.Unrelated2);
			Assert.That(solution.Projects, Is.EqualTo(new[] {projects.MyProject}));
		}

		[Test]
		public void SingleEfferentAssemblyReferenceAndUnRelatedProjectInList()
		{
			projects.MyProject.ReferencesAssemblies(projects.TheirProject1);
			GeneratePartialGraphSolution(rootProjectName, projects.MyProject, projects.TheirProject1, projects.Unrelated1);
			Assert.That(solution.Projects, Is.EqualTo(new[] { projects.MyProject }));
		}

		[Test]
		public void SingleEfferentAssemblyReferenceToSubtree()
		{
			projects.MyProject.ReferencesAssemblies(projects.TheirProject1);
			projects.TheirProject1.ReferencesAssemblies(projects.TheirProject2, projects.TheirProject3);
			GeneratePartialGraphSolution(rootProjectName, new[]
				{
					projects.MyProject, projects.TheirProject1, projects.TheirProject2, projects.TheirProject3
				});
			Assert.That(solution.Projects, Is.EqualTo(new[]
				{
					projects.MyProject
				}));
		}

		[Test]
		public void SingleEfferentProjectReference()
		{
			projects.MyProject.ReferencesProjects(projects.TheirProject1);
			GeneratePartialGraphSolution(rootProjectName, projects.MyProject, projects.TheirProject1);
			Assert.That(solution.Projects, Is.EqualTo(new[] {projects.MyProject, projects.TheirProject1}));
		}

		[Test]
		public void SingleEfferentProjectReferenceToSubtree()
		{
			projects.MyProject.ReferencesProjects(projects.TheirProject1);
			projects.TheirProject1.ReferencesProjects(projects.TheirProject2, projects.TheirProject3);
			GeneratePartialGraphSolution(rootProjectName, new[]
				{
					projects.MyProject, projects.TheirProject1, projects.TheirProject2, projects.TheirProject3
				});
			Assert.That(solution.Projects, Is.EqualTo(new[]
				{
					projects.MyProject, projects.TheirProject1, projects.TheirProject2, projects.TheirProject3
				}));
		}

		[Test]
		public void ReferencesProjectNotInProjectsList()
		{
			projects.MyProject.ReferencesProjects(projects.TheirProject1);
			GeneratePartialGraphSolution(rootProjectName, projects.MyProject);
			Assert.That(solution.Projects, Is.EqualTo(new[] {projects.MyProject}));
		}

		[Test]
		public void MultipleAfferentAssemblyReferences()
		{
			projects.OurProject1.ReferencesAssemblies(projects.MyProject);
			projects.OurProject2.ReferencesAssemblies(projects.MyProject);
			GeneratePartialGraphSolution(rootProjectName, projects.MyProject, projects.OurProject1, projects.OurProject2);
			Assert.That(solution.Projects, Is.EqualTo(new[] {projects.MyProject, projects.OurProject1, projects.OurProject2}));
		}

		[Test]
		public void MultipleAfferentProjectReferences()
		{
			projects.OurProject1.ReferencesProjects(projects.MyProject);
			projects.OurProject2.ReferencesProjects(projects.MyProject);
			GeneratePartialGraphSolution(rootProjectName, projects.MyProject, projects.OurProject1, projects.OurProject2);
			Assert.That(solution.Projects, Is.EqualTo(new[] { projects.MyProject, projects.OurProject1, projects.OurProject2 }));
		}

		[Test]
		public void AfferentAssemblyReferenceReferencingOtherProjects()
		{
			projects.OurProject1.ReferencesAssemblies(projects.MyProject);
			projects.OurProject1.ReferencesAssemblies(projects.Unrelated1);
			projects.OurProject1.ReferencesAssemblies(projects.Unrelated2);
			GeneratePartialGraphSolution(rootProjectName, new[]
				{
					projects.MyProject, projects.OurProject1, projects.Unrelated1, projects.Unrelated2
				});
			Assert.That(solution.Projects, Is.EqualTo(new[]
				{
					projects.MyProject, projects.OurProject1
				}));
		}

		[Test]
		public void MixedAfferentAndEfferentReferences()
		{
			projects.OurProject1.ReferencesAssemblies(projects.MyProject);
			projects.OurProject1.ReferencesProjects(projects.TheirProject1);
			projects.OurProject2.ReferencesProjects(projects.MyProject);
			projects.OurProject2.ReferencesAssemblies(projects.TheirProject2);
			projects.MyProject.ReferencesAssemblies(projects.TheirProject1, projects.TheirProject2);
			projects.MyProject.ReferencesProjects(projects.TheirProject3);
			projects.TheirProject3.ReferencesAssemblies(projects.Unrelated1);
			projects.TheirProject3.ReferencesProjects(projects.Unrelated2);
			GeneratePartialGraphSolution(rootProjectName, new[]
				{
					projects.MyProject, projects.TheirProject1, projects.TheirProject2, projects.TheirProject3,
					projects.OurProject1, projects.OurProject2, projects.Unrelated1, projects.Unrelated2
				});
			Assert.That(solution.Projects, Is.EqualTo(new[]
				{
					projects.MyProject, projects.TheirProject3, projects.Unrelated2,
					projects.OurProject1, projects.TheirProject1, projects.OurProject2
				}));
		}

		private void GeneratePartialGraphSolution(string rootProjectName, params CsProj[] projectsList)
		{
			var generator = new SlnBuilder(new List<CsProj>(projectsList));
			solution = generator.BuildPartialGraphSln(rootProjectName);
		}
	}
}