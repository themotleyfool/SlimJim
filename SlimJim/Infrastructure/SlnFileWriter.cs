using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using SlimJim.Model;

namespace SlimJim.Infrastructure
{
	public class SlnFileWriter
	{
		public virtual void WriteSlnFile(Sln solution, string writeInDirectory)
		{
			using (var writer = XmlWriter.Create(GetOutputFilePath(writeInDirectory, solution)))
			{
				var serializer = new XmlSerializer(typeof (Sln));
				serializer.Serialize(writer, solution);
			}
		}

		private string GetOutputFilePath(string writeInDirectory, Sln solution)
		{
			return Path.Combine(writeInDirectory, solution.Name + ".sln");
		}
	}
}