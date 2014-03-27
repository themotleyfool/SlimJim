using System.IO;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using SlimJim.Infrastructure;

namespace SlimJim
{
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

		private static ColoredConsoleAppender ConfigureLogging()
		{
			var appender = new ColoredConsoleAppender
			{
				Threshold = Level.All,
				Layout = new PatternLayout("%message%newline"),
			};
			appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Info, ForeColor = ColoredConsoleAppender.Colors.White });
			appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Debug, ForeColor = ColoredConsoleAppender.Colors.Cyan });
			appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Warn, ForeColor = ColoredConsoleAppender.Colors.Yellow | ColoredConsoleAppender.Colors.HighIntensity });
			appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Error, ForeColor = ColoredConsoleAppender.Colors.Red | ColoredConsoleAppender.Colors.HighIntensity });
			appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Fatal, ForeColor = ColoredConsoleAppender.Colors.White | ColoredConsoleAppender.Colors.HighIntensity, BackColor = ColoredConsoleAppender.Colors.Red });
			appender.ActivateOptions();
			BasicConfigurator.Configure(appender);
			return appender;
		}
	}
}
