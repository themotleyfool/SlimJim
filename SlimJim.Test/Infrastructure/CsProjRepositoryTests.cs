using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using SlimJim.Infrastructure;
using SlimJim.Model;
using SlimJim.Test.SampleFiles;

namespace SlimJim.Test.Infrastructure
{
	[TestFixture]
	public class CsProjRepositoryTests
	{
		private const string StartPath = @"C:\Projects";
		private readonly FileInfo file1 = SampleFileHelper.GetCsProjFile("Simple");
		private readonly FileInfo file2 = SampleFileHelper.GetCsProjFile("Simple");
		private readonly CsProj proj1 = new CsProj {AssemblyName = "Proj1"};
		private readonly CsProj proj2 = new CsProj {AssemblyName = "Proj1"};
		private ProjectFileFinder finder;
		private CsProjReader reader;
		private CsProjRepository repository;

		[SetUp]
		public void BeforeEach()
		{
			finder = MockRepository.GenerateStrictMock<ProjectFileFinder>();
			reader = MockRepository.GenerateStrictMock<CsProjReader>();
			repository = new CsProjRepository
			{
				Finder = finder,
				Reader = reader
			};
		}

		[Test]
		public void CreatesOwnInstancesOfFinderAndReader()
		{
			repository = new CsProjRepository();
			Assert.That(repository.Finder, Is.Not.Null, "Should have created instance of CsProjFinder.");
			Assert.That(repository.Reader, Is.Not.Null, "Should have created instance of CsProjReader.");
		}

		[Test]
		public void GetsFilesFromFinderAndProcessesThemWithCsProjReader()
		{
			finder.Expect(f => f.FindAllProjectFiles(StartPath)).Return(new List<FileInfo>{file1, file2});
			reader.Expect(r => r.Read(file1)).Return(proj1);
			reader.Expect(r => r.Read(file2)).Return(proj2);

			List<CsProj> projects = repository.LookupCsProjsFromDirectory(StartPath);

			Assert.That(projects, Is.EqualTo(new[]{proj1, proj2}));
		}

		[Test]
		public void GracefullyHandlesNullsFromReader()
		{
			finder.Expect(f => f.FindAllProjectFiles(StartPath)).Return(new List<FileInfo> { file1, file2 });
			reader.Expect(r => r.Read(file1)).Return(proj1);
			reader.Expect(r => r.Read(file2)).Return(null);

			List<CsProj> projects = repository.LookupCsProjsFromDirectory(StartPath);

			Assert.That(projects, Is.EqualTo(new[] { proj1 }));
		}
	}
}