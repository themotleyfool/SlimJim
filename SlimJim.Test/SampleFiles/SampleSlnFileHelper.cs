using System;
using System.IO;

namespace SlimJim.Test.SampleSlnFiles
{
	public static class SampleSlnFileHelper
	{
		public static string GetFileContents(string name)
		{
			string dllFile = typeof (SampleSlnFileHelper).Assembly.CodeBase.Substring(8);
			string projectRoot = Directory.GetParent(dllFile).Parent.Parent.FullName;
			string sampleFolder = Path.Combine(projectRoot, "SampleSlnFiles");
			string slnFilePath = Path.Combine(sampleFolder, name + ".sln");

			using (var fileReader = new StreamReader(slnFilePath))
			{
				return fileReader.ReadToEnd();
			}
		}
	}
}