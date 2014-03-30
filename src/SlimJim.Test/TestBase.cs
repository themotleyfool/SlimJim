using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using SlimJim.Infrastructure;
using SlimJim.Model;
using System.IO;

namespace SlimJim.Test
{
	public abstract class TestBase
	{
		protected string WorkingDirectory
		{
			get
			{
				return GetSamplePath("WorkingDir");
			}
		}

		protected string GetSamplePath(params string[] parts)
		{
			return Path.Combine(Path.DirectorySeparatorChar == '/' ? "/" : @"C:\", Path.Combine(parts));
		}
	}
	
}