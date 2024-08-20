using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagg.Logging {
		public enum LogLevel {
				/// <summary>
				/// keyword: emerg
				/// System is unusable
				/// </summary>
				Emergency = 0,

				/// <summary>
				/// keyword: alert
				/// Action must be taken immediately
				/// </summary>
				Alert = 1,

				/// <summary>
				/// keyword: crit
				/// Critical conditions
				/// </summary>
				Critical = 2,

				/// <summary>
				/// keyword: err
				/// Error conditions
				/// </summary>
				Error = 3,

				/// <summary>
				/// keyword: warning
				/// Warning conditions
				/// </summary>
				Warning = 4,

				/// <summary>
				/// keyword: notice
				/// Normal but significant conditions
				/// </summary>
				Notice = 5,

				/// <summary>
				/// keyword: info
				/// Informational messages
				/// </summary>
				Informational = 6,

				/// <summary>
				/// keyword: debug
				/// Debug-level messages
				/// </summary>
				Debug = 7,
		}

		public static class LogLevelExtentions {
				public static string GetKeyword(this LogLevel logLevel) {
						switch (logLevel) {
								default:
										return @"";
								case LogLevel.Emergency:
										return @"emerg";
								case LogLevel.Alert:
										return @"alert";
								case LogLevel.Critical:
										return @"crit";
								case LogLevel.Error:
										return @"err";
								case LogLevel.Warning:
										return @"warning";
								case LogLevel.Notice:
										return @"notice";
								case LogLevel.Informational:
										return @"info";
								case LogLevel.Debug:
										return @"debug";
						}
				}
		}
}
