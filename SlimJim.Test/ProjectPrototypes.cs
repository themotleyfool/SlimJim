using System;
using SlimJim.Model;

namespace SlimJim.Test
{
	public class ProjectPrototypes
	{
		public readonly CsProj TheirProject3 = new CsProj
			{
				AssemblyName = "TheirProject3",
				Guid = Guid.NewGuid().ToString("N"),
				Path = @"C:\Projects\TheirProject3\TheirProject3.csproj"
			};

		public readonly CsProj TheirProject2 = new CsProj
			{
				AssemblyName = "TheirProject2",
				Guid = Guid.NewGuid().ToString("N"),
				Path = @"C:\Projects\TheirProject2\TheirProject2.csproj"
			};

		public readonly CsProj TheirProject1 = new CsProj
			{
				AssemblyName = "TheirProject1",
				Guid = Guid.NewGuid().ToString("N"),
				Path = @"C:\Projects\TheirProject1\TheirProject1.csproj"
			};

		public readonly CsProj MyProject = new CsProj
			{
				AssemblyName = "MyProject",
				Guid = Guid.NewGuid().ToString("N"),
				Path = @"C:\Projects\MyProject\MyProject.csproj"
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
			Guid = Guid.NewGuid().ToString("N"),
			Path = @"C:\Projects\OurProject1\OurProject1.csproj"
		};

		public CsProj OurProject2 = new CsProj
		{
			AssemblyName = "OurProject2",
			Guid = Guid.NewGuid().ToString("N"),
			Path = @"C:\Projects\OurProject2\OurProject2.csproj"
		};
	}
}