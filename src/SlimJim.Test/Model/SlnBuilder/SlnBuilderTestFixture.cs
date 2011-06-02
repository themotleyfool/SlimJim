using System;
using System.Collections.Generic;
using NUnit.Framework;
using SlimJim.Model;

namespace SlimJim.Test.Model.SlnBuilder
{
    public class SlnBuilderTestFixture
    {
        protected string targetProjectName;
        protected Sln solution;
        protected ProjectPrototypes projects;
        protected SlnGenerationOptions options;

        [SetUp]
        public void BeforeEach()
        {
            projects = new ProjectPrototypes();
            targetProjectName = projects.MyProject.AssemblyName;
            options = new SlnGenerationOptions(@"C:\Projects");
        }

        protected void GeneratePartialGraphSolution(string[] targetProjectNames, params CsProj[] projectsList)
        {
            var generator = new SlimJim.Model.SlnBuilder(new List<CsProj>(projectsList));
				options.AddTargetProjectNames(targetProjectNames);
            solution = generator.BuildSln(options);
        }
    }
}