using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using SlimJim.Infrastructure;
using SlimJim.Model;

namespace SlimJim.Test
{
	[TestFixture]
	public class SlnFileGeneratorTests
	{
		private SlnFileGenerator gen;
		private SlnFileWriter slnWriter;
		private CsProjRepository repo;
		private const string ProjectsDir = @"C:\Projects";
		private const string TargetProject = "MyProject";

		[SetUp]
		public void BeforeEach()
		{
			repo = MockRepository.GenerateStrictMock<CsProjRepository>();
			slnWriter = MockRepository.GenerateStrictMock<SlnFileWriter>();

			gen = new SlnFileGenerator()
			{
				ProjectRepository = repo,
				SlnWriter = slnWriter
			};
		}

		[Test]
		public void CreatesOwnInstancesOfRepositoryAndWriter()
		{
			gen = new SlnFileGenerator();
			Assert.That(gen.ProjectRepository, Is.Not.Null, "Should have created instance of CsProjRepository.");
			Assert.That(gen.SlnWriter, Is.Not.Null, "Should have created instance of SlnFileWriter.");
		}

		[Test]
		public void GeneratesSlnFileForCurrentDirectory()
		{
			var projs = new List<CsProj>();
			repo.Expect(r => r.LookupCsProjsFromDirectory(ProjectsDir)).Return(projs);
			slnWriter.Expect(wr => wr.WriteSlnFile(Arg<Sln>.Matches(s => s.Name.Equals(TargetProject)), Arg.Is(ProjectsDir)));

			var options = new SlnGenerationOptions
				{
					ProjectsRootDirectory = ProjectsDir,
					TargetProjectName = TargetProject
				};
			gen.GenerateSolutionFile(options);
		}

		[TearDown]
		public void AfterEach()
		{
			repo.VerifyAllExpectations();
			slnWriter.VerifyAllExpectations();
		}
	}
}