using System;
using SlimJim.Model;

namespace SlimJim.Test.Model
{
	public class ProjectPrototypes
	{
		public readonly CsProj TheirProject3 = new CsProj
			{
				AssemblyName = "TheirProject3",
				Guid = "{499372E5-5DBF-4DB4-BB1A-9072395C9017}",
				Path = @"C:\Projects\TheirProject3\TheirProject3.csproj"
			};

		public readonly CsProj TheirProject2 = new CsProj
			{
				AssemblyName = "TheirProject2",
				Guid = "{58E0EE99-9DCA-45C4-AB04-48D67316F71D}",
				Path = @"C:\Projects\TheirProject2\TheirProject2.csproj"
			};

		public readonly CsProj TheirProject1 = new CsProj
			{
				AssemblyName = "TheirProject1",
				Guid = "{74CBCCEE-C805-49C3-9EB8-10B48CCC3A6F}",
				Path = @"C:\Projects\TheirProject1\TheirProject1.csproj"
			};

		public readonly CsProj MyProject = new CsProj
			{
				AssemblyName = "MyProject",
				Guid = "{E75347BE-2125-4325-818D-0ECC760F11BA}",
				Path = @"C:\Projects\MyProject\MyProject.csproj",
				TargetFrameworkVersion = "v3.5"
			};

		public readonly CsProj MyMultiFrameworkProject35 = new CsProj
			{
				AssemblyName = "MyMultiFrameworkProject",
				Guid = "{fba80161-8315-4a8b-91a4-bff7d2f0a968}",
				Path = @"C:\Projects\MyMultiFrameworkProject\MyMultiFrameworkProject-net35.csproj",
				TargetFrameworkVersion = "v3.5"
			};

		public readonly CsProj MyMultiFrameworkProject40 = new CsProj
			{
				AssemblyName = "MyMultiFrameworkProject",
				Guid = "{b8da3366-a3d6-4580-9b99-60aed5b01d5e}",
				Path = @"C:\Projects\MyMultiFrameworkProject\MyMultiFrameworkProject-net40.csproj",
				TargetFrameworkVersion = "v4.0"
			};

		public readonly CsProj Unrelated1 = new CsProj
			{
				AssemblyName = "Unrelated1",
				Guid = Guid.NewGuid().ToString("N"),
				Path = @"C:\Projects\Unrelated1\Unrelated1.csproj"
			};

		public readonly CsProj Unrelated2 = new CsProj
			{
				AssemblyName = "Unrelated2",
				Guid = Guid.NewGuid().ToString("N"),
				Path = @"C:\Projects\Unrelated2\Unrelated2.csproj"
			};

		public CsProj OurProject1 = new CsProj
		{
			AssemblyName = "OurProject1",
			Guid = "{021CD387-7FE9-4BAD-B57F-6D8ABFE73562}",
			Path = @"C:\Projects\OurProject1\OurProject1.csproj"
		};

		public CsProj OurProject2 = new CsProj
		{
			AssemblyName = "OurProject2",
			Guid = "{4A0CC937-5131-4B9F-AD9E-D6844BDD8EC3}",
			Path = @"C:\Projects\OurProject2\OurProject2.csproj"
		};
	}
}