
using System.IO;

namespace SlimJim.Test
{
	public abstract class TestBase
	{
		protected string WorkingDirectory
		{
			get
			{
				return GetSamplePath("WorkingDir");
			}
		}

		protected string GetSamplePath(params string[] parts)
		{
			return Path.Combine(Path.DirectorySeparatorChar == '/' ? "/" : @"C:\", Path.Combine(parts));
		}
	}
	
}