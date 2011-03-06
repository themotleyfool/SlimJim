using System.Collections.Generic;

namespace SlimJim.Model
{
	public interface ICsProjRegistry
	{
		List<CsProj> LookupCsProjsFromDirectory(string rootDirectory);
	}
}