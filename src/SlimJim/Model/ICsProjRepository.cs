using System.Collections.Generic;

namespace SlimJim.Model
{
	public interface ICsProjRepository
	{
		List<CsProj> LookupCsProjsFromDirectory(SlnGenerationOptions options);
	}
}