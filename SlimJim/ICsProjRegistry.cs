using System.Collections.Generic;

namespace SlimJim
{
	public interface ICsProjRegistry
	{
		List<CsProj> LookupCsProjsFromDirectory(string rootDirectory);
	}
}