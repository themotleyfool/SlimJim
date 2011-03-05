using System;
using System.Collections.Generic;

namespace SlimJim
{
	public class CsProj
	{
		public string Guid { get; set; }
		public string Path { get; set; }
		public string AssemblyName { get; set; }
		public List<string> ReferencedAssemblyNames { get; set; }
		public List<string> ReferencedProjectGuids { get; set; }
	}
}