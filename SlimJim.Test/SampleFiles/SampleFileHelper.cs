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

			using (var fileReader =file.OpenText())
			{
				return fileReader.ReadToEnd();
			}
		}

		private static FileInfo GetFile(string name, string fileType)
		{
			string dllFile = typeof (SampleFileHelper).Assembly.CodeBase.Substring(8);
			string projectRoot = Directory.GetParent(dllFile).Parent.Parent.FullName;
			string sampleFolder = Path.Combine(projectRoot, @"SampleFiles\" + fileType + @"\");
			string slnFilePath = Path.Combine(sampleFolder, name + "." + fileType);
			return new FileInfo(slnFilePath);
		}

		public static FileInfo GetCsProjFile(string name)
		{
			return GetFile(name, CsProjFileType);
		}
	}
}