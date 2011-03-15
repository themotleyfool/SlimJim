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
		private const string StartPath = @"C:\Projects";
		private const string RootProjectName = "MyProject";

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
			repo = MockRepository.GenerateStrictMock<CsProjRepository>();
			slnWriter = MockRepository.GenerateStrictMock<SlnFileWriter>();
			gen = new SlnFileGenerator()
				{
					ProjectRepository = repo,
					SlnWriter = slnWriter
				};

			var projs = new List<CsProj>();
			repo.Expect(r => r.LookupCsProjsFromDirectory(StartPath)).Return(projs);
			slnWriter.Expect(wr => wr.WriteSlnFile(Arg<Sln>.Matches(s => s.Name.Equals(RootProjectName)), Arg.Is(StartPath)));

			gen.GeneratePartialGraphSolutionFile(StartPath, RootProjectName);

			repo.VerifyAllExpectations();
			slnWriter.VerifyAllExpectations();
		}
	}
}