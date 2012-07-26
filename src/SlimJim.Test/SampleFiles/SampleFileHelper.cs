using System;
using System.IO;

namespace SlimJim.Test.SampleFiles
{
	public static class SampleFileHelper
	{
		private const string CsProjFileType = "csproj";

		public static string GetSlnFileContents(string name)
		{
			return GetFileContents(name, "sln");
		}

		public static string GetCsProjFileContents(string name)
		{
			return GetFileContents(name, CsProjFileType);
		}

		private static string GetFileContents(string name, string fileType)
		{
			FileInfo file = GetFile(name, fileType);

			return File.ReadAllText(file.FullName);
		}

		private static FileInfo GetFile(string name, string fileType)
		{
			string sampleFolder = GetSampleFolder();
			string typeFolder = Path.Combine(sampleFolder, fileType + @"\");
			string filePath = Path.Combine(typeFolder, name + "." + fileType);
			return new FileInfo(filePath);
		}

		private static string GetSampleFolder()
		{
			string dllFile = typeof (SampleFileHelper).Assembly.CodeBase.Substring(8);
			string projectRoot = Directory.GetParent(dllFile).Parent.Parent.FullName;
			return Path.Combine(projectRoot, @"SampleFiles\");
		}

		public static FileInfo GetCsProjFile(string name)
		{
			return GetFile(name, CsProjFileType);
		}

		public static string GetSampleFileSystemPath()
		{
			return Path.Combine(GetSampleFolder(), @"SampleFileSystem\");
		}
	}
}