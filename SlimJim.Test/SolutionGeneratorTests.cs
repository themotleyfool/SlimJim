using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using SlimJim.Infrastructure;
using SlimJim.Model;

namespace SlimJim.Test
{
	[TestFixture]
	public class SolutionGeneratorTests
	{
		private const string RootDirectory = @"C:\TestFolder";
		private const string SlnFilePath = @"C:\TestFolder\TestFolder.sln";
		private SolutionFileGenerator generator;
		private ICsProjRegistry csProjRegistry;
		private CsProj proj;
		private CsProj projWithProjRef;

		[SetUp]
		public void BeforeEach()
		{
			csProjRegistry = MockRepository.GenerateStrictMock<ICsProjRegistry>();
			generator = new SolutionFileGenerator(csProjRegistry);
			 
			proj = new CsProj { AssemblyName = "MyProj", Path = @"C:\TestFolder\MyProj\MyProj.csproj", Guid="{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" };
			projWithProjRef = new CsProj
			                     	{
			                     		AssemblyName = "MyProj.Test", 
												Path = @"C:\TestFolder\MyProj.Test\MyProj.Test.csproj",
												Guid = "{07FC660C-0A00-4F39-B07A-8BEF9505C7D9}",
												ReferencedAssemblyNames = new List<string> { "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" }
			                     	};
		}

		[Test]
		public void SlnFileIsCreatedAtSlFilePath()
		{
			SetCsProjsFoundInRegistry();

			SolutionFileInfo fileHandle = generator.GenerateSolutionFile(RootDirectory, SlnFilePath);

			Assert.That(fileHandle.FullName, Is.EqualTo(SlnFilePath));
		}

		[Test]
		public void BlankSlnFileGeneratedForEmptyRootDirectory()
		{
			SetCsProjsFoundInRegistry();

			SolutionFileInfo file = generator.GenerateSolutionFile(RootDirectory, SlnFilePath);

			Assert.That(file.CsProjs, Is.EquivalentTo(new CsProj[]{}));
		}

		[Test]
		public void SlnFileWithOneProjectRefCreatedForRootDirWithOneProject()
		{
			SetCsProjsFoundInRegistry(proj);

			SolutionFileInfo file = generator.GenerateSolutionFile(RootDirectory, SlnFilePath);

			Assert.That(file.CsProjs, Is.EquivalentTo(new[] {proj}));
		}

		[Test]
		public void SlnFileWithTwoProjectRefsAndOneDependency()
		{

			SetCsProjsFoundInRegistry(proj);
		}

		private void SetCsProjsFoundInRegistry(params CsProj[] csProjsFound)
		{
			csProjRegistry.Stub(reg => reg.LookupCsProjsFromDirectory(RootDirectory)).Return(new List<CsProj>(csProjsFound));
		}

		[TearDown]
		public void AfterEach()
		{
			csProjRegistry.VerifyAllExpectations();
		}

	}
}