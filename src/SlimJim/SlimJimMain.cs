using System.IO;
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

			var fileGenerator = new SlnFileGenerator();

			var optionsBuilder = new ArgsOptionsBuilder();
			var options = optionsBuilder.Build(args, Directory.GetCurrentDirectory());

			if (options.ShowHelp)
			{
				optionsBuilder.WriteHelpMessage();
			}
			else
			{
				fileGenerator.GenerateSolutionFile(options);				
			}
		}
	}
}
