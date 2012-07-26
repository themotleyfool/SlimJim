using System;
using System.Collections.Generic;

namespace SlimJim.Model
{
	public class Folder
	{
		public Folder()
		{
			ContentGuids = new List<string>();
		}

		public string FolderName { get; set; }
		public string Guid { get; set; }
		public IList<string> ContentGuids { get; private set; }

		public void AddContent(string guid)
		{
			ContentGuids.Add(guid);
		}

		public override string ToString()
		{
			return string.Format("FolderName: {0}", FolderName);
		}
	}
}