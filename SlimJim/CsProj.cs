using System;
using System.Collections.Generic;

namespace SlimJim
{
	public class CsProj
	{
		public string Path { get; set; }
		public string Name { get; set; }

		public string Guid { get; set; }

		public List<string> ProjectReferenceGuids { get; set; }
	}
}