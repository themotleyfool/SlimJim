using System.IO;
using log4net.Appender;
using log4net.Layout;
using SlimJim.Infrastructure;

namespace SlimJim
{
	public class SlimJimMain
	{
		public static void Main(string[] args)
		{
			var consoleAppender = new ConsoleAppender() { Layout = new PatternLayout("%message%newline") };
			log4net.Config.BasicConfigurator.Configure(consoleAppender);

			var fileGenerator = new SlnFileGenerator();
			var options = ArgsOptionsBuilder.BuildOptions(args, Directory.GetCurrentDirectory());
			
			fileGenerator.GenerateSolutionFile(options);
		}
	}
}
