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
		private const string StartPath = @"C:\Projects";
		private const string RootProjectName = "MyProject";

		[Test]
		public void GeneratesSlnFileForCurrentDirectory()
		{
			var repo = MockRepository.GenerateStrictMock<CsProjRepository>();
			var slnWriter = MockRepository.GenerateStrictMock<SlnFileWriter>();
			var gen = new SlnFileGenerator()
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