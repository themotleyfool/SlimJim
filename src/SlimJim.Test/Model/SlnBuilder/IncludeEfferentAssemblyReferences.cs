using NUnit.Framework;

namespace SlimJim.Test.Model.SlnBuilder
{
    [TestFixture]
    public class IncludeEfferentAssemblyReferences : SlnBuilderTestFixture
    {
        [Test]
        public void EfferentAssemblyReferencesAreIncludedForAllProjectsInSln()
        {
            options.IncludeEfferentAssemblyReferences = true;
            projects.MyProject.ReferencesAssemblies(projects.TheirProject1, projects.TheirProject2);
            projects.TheirProject2.ReferencesProjects(projects.TheirProject3);
            projects.OurProject1.ReferencesAssemblies(projects.MyProject, projects.OurProject2);
            GeneratePartialGraphSolution(new[] {targetProjectName},
                                         projects.MyProject,
                                         projects.TheirProject1,
                                         projects.TheirProject2,
                                         projects.TheirProject3,
                                         projects.OurProject1,
                                         projects.OurProject2);
            Assert.That(solution.Projects, Is.EqualTo(new[]
                                                          {
                                                              projects.MyProject,
                                                              projects.TheirProject1,
                                                              projects.TheirProject2,
                                                              projects.TheirProject3,
                                                              projects.OurProject1,
                                                              projects.OurProject2
                                                          }));
        }
    }
}