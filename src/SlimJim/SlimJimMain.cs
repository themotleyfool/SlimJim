using System;
using System.IO;

using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;

namespace SlimJim
{
    using Infrastructure;

    public class SlimJimMain
	{
		public static void Main(string[] args)
		{
			var consoleAppender = ConfigureLogging();

			var log = LogManager.GetLogger(typeof(SlnFileGenerator));
			var fileGenerator = new SlnFileGenerator();

			var optionsBuilder = new ArgsOptionsBuilder();
			var options = optionsBuilder.Build(args, Directory.GetCurrentDirectory());

			if (options.ShowHelp)
			{
				optionsBuilder.WriteHelpMessage();
				return;
			}

			consoleAppender.Threshold = options.LoggingThreshold;
			var solutionPath = fileGenerator.GenerateSolutionFile(options);
				
			if (options.OpenInVisualStudio)
			{
				log.InfoFormat("Opening {0} in Visual Studio {1}", solutionPath, options.VisualStudioVersion.Year);
				VisualStudioIntegration.OpenSolution(solutionPath, options.VisualStudioVersion);
			}
		}

		private static ManagedColoredConsoleAppender ConfigureLogging()
		{
			var appender = new ManagedColoredConsoleAppender
			{
				Threshold = Level.All,
				Layout = new PatternLayout("%message%newline"),
			};
			appender.AddMapping(new ManagedColoredConsoleAppender.LevelColors { Level = Level.Info, ForeColor = ConsoleColor.Gray });
			appender.AddMapping(new ManagedColoredConsoleAppender.LevelColors { Level = Level.Debug, ForeColor = ConsoleColor.DarkCyan});
			appender.AddMapping(new ManagedColoredConsoleAppender.LevelColors { Level = Level.Warn, ForeColor = ConsoleColor.Yellow });
			appender.AddMapping(new ManagedColoredConsoleAppender.LevelColors { Level = Level.Error, ForeColor = ConsoleColor.Red });
			appender.ActivateOptions();
			BasicConfigurator.Configure(appender);
			return appender;
		}
	}
}
