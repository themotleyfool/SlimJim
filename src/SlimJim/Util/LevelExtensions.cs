using System;
using System.Collections.Generic;
using log4net.Core;

namespace SlimJim.Util
{
	public static class LevelExtensions
	{
		private static readonly IList<Level> LoggerLevels = new List<Level>
		{
			Level.Fatal,
			Level.Error,
			Level.Warn,
			Level.Info,
			Level.Debug,
			Level.Trace,
			Level.All
		};

		public static Level DecreaseVerbosity(this Level level)
		{
			return level.AdjustLevelWithinBounds(-1);
		}

		public static Level IncreaseVerbosity(this Level level)
		{
			return level.AdjustLevelWithinBounds(1);
		}

		public static Level AdjustLevelWithinBounds(this Level level, int amount)
		{
			var index = LoggerLevels.IndexOf(level) + amount;
			index = Math.Min(Math.Max(index, 0), LoggerLevels.Count - 1);
			return LoggerLevels[index];
		}

	}
}
