using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using log4net;
using Microsoft.Win32;
using SlimJim.Model;

namespace SlimJim.Infrastructure
{
	public class VisualStudioIntegration
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(SlnFileGenerator));

		public static void OpenSolution(string solutionPath, VisualStudioVersion visualStudioVersion)
		{
			var devenvPath = FindDevEnv(visualStudioVersion);

			if (devenvPath == null || !File.Exists(devenvPath))
			{
				Log.ErrorFormat("Unable to locate Visual Studio {0} install directory.", visualStudioVersion.Year);
				return;
			}

			var info = new ProcessStartInfo(devenvPath, '"' + solutionPath + '"');

			Process.Start(info);
		}

		private static string FindDevEnv(VisualStudioVersion version)
		{
			string key = @"Software\Microsoft\VisualStudio\" + version.PathVersionNumber;
			string wowKey = @"Software\Wow6432Node\Microsoft\VisualStudio\" + version.PathVersionNumber;

			var r = Registry.LocalMachine.OpenSubKey(wowKey) ?? Registry.LocalMachine.OpenSubKey(key);

			if (r == null) return null;

			var val = r.GetValue("InstallDir");

			return val == null ? null : Path.Combine(val.ToString(), "devenv.exe");
		}
	}
}