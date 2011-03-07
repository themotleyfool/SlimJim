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

		[Test, Ignore]
		public void GetsFilesFromFinderAndProcessesThemWithCsProjReader()
		{
			FileInfo file1 = SampleFileHelper.GetCsProjFile("Simple");
			FileInfo file2 = SampleFileHelper.GetCsProjFile("Simple");
			var finder = MockRepository.GenerateStrictMock<ProjectFileFinder>();
			var reader = MockRepository.GenerateStrictMock<CsProjReader>();
			var repository = new CsProjRepository();
			
			finder.Expect(f => f.FindAllProjectFiles(StartPath)).Return(new List<FileInfo>{file1, file2});
			var proj1 = new CsProj {AssemblyName = "Proj1"};
			var proj2 = new CsProj {AssemblyName = "Proj1"};
			reader.Expect(r => r.Read(file1)).Return(proj1);
			reader.Expect(r => r.Read(file2)).Return(proj2);

			//List<CsProj> projects = 
		}
	}
}