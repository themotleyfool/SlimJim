using System.IO;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using SlimJim.Infrastructure;

namespace SlimJim
{
	public class SlimJimMain
	{
		public static void Main(string[] args)
		{
			var consoleAppender = new ConsoleAppender()
			                      	{
			                      		Layout = new PatternLayout("%message%newline"),
			                      		Threshold = Level.Info
			                      	};
			log4net.Config.BasicConfigurator.Configure(consoleAppender);

			var log = LogManager.GetLogger(typeof(SlnFileGenerator));
			var fileGenerator = new SlnFileGenerator();

			var optionsBuilder = new ArgsOptionsBuilder();
			var options = optionsBuilder.Build(args, Directory.GetCurrentDirectory());

			if (options.ShowHelp)
			{
				optionsBuilder.WriteHelpMessage();
			}
			else
			{
				var solutionPath = fileGenerator.GenerateSolutionFile(options);
				
				if (options.OpenInVisualStudio)
				{
					log.InfoFormat("Opening {0} in Visual Studio {1}", solutionPath, options.VisualStudioVersion.Year);
					VisualStudioIntegration.OpenSolution(solutionPath, options.VisualStudioVersion);
				}
			}
		}
	}
}
