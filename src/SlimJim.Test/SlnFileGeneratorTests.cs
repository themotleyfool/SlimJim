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
		private const string ProjectsDir = @"C:\Projects";
		private const string TargetProject = "MyProject";
		private SlnFileGenerator gen;
		private SlnFileWriter slnWriter;
		private CsProjRepository repo;
		private SlnBuilder slnBuilder;
		private SlnGenerationOptions options;
		private readonly List<CsProj> projects = new List<CsProj>();
		private readonly Sln createdSlnObject = new Sln("Sln");

		[SetUp]
		public void BeforeEach()
		{
			repo = MockRepository.GenerateStrictMock<CsProjRepository>();
			slnWriter = MockRepository.GenerateStrictMock<SlnFileWriter>();
			slnBuilder = MockRepository.GenerateStrictMock<SlnBuilder>(new List<CsProj>());

			gen = new SlnFileGenerator()
			{
				ProjectRepository = repo,
				SlnWriter = slnWriter
			};

			SlnBuilder.OverrideDefaultBuilder(slnBuilder);
			options = new SlnGenerationOptions(ProjectsDir);
		}

		[Test]
		public void CreatesOwnInstancesOfRepositoryAndWriter()
		{
			gen = new SlnFileGenerator();
			Assert.That(gen.ProjectRepository, Is.Not.Null, "Should have created instance of CsProjRepository.");
			Assert.That(gen.SlnWriter, Is.Not.Null, "Should have created instance of SlnFileWriter.");
		}

		[Test]
		public void GeneratesSlnFileForGivenOptions()
		{
			options.TargetProjectNames.Add(TargetProject);

			repo.Expect(r => r.LookupCsProjsFromDirectory(options)).Return(projects);
			slnBuilder.Expect(bld => bld.BuildSln(options)).Return(createdSlnObject);
			slnWriter.Expect(wr => wr.WriteSlnFile(createdSlnObject, ProjectsDir));

			gen.GenerateSolutionFile(options);
		}

		[TearDown]
		public void AfterEach()
		{
			repo.VerifyAllExpectations();
			slnWriter.VerifyAllExpectations();
			slnBuilder.VerifyAllExpectations();
		}
	}
}