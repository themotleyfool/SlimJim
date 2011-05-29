using System;
using System.Collections.Generic;
using NUnit.Framework;
using SlimJim.Model;

namespace SlimJim.Test.Model.SlnBuilder
{
    public class SlnBuilderTestFixture
    {
        protected string rootProjectName;
        protected Sln solution;
        protected ProjectPrototypes projects;
        protected SlnGenerationOptions options;

        [SetUp]
        public void BeforeEach()
        {
            projects = new ProjectPrototypes();
            rootProjectName = projects.MyProject.AssemblyName;
            options = new SlnGenerationOptions(@"C:\Projects");
        }

        protected void GeneratePartialGraphSolution(string[] targetProjectNames, params CsProj[] projectsList)
        {
            var generator = new SlimJim.Model.SlnBuilder(new List<CsProj>(projectsList));
            Array.ForEach(targetProjectNames, n => options.TargetProjectNames.Add(n));
            solution = generator.BuildPartialGraphSln(options);
        }
    }
}