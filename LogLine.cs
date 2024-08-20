using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lagg.Logging {
		internal readonly struct LogLine {
				public readonly DateTime TimeStamp;
				public readonly LogLevel Level;
				public readonly string Content;

				public LogLine() { throw new NotImplementedException(); }
				public LogLine(DateTime timeStamp, LogLevel level, string content) {
						this.TimeStamp = timeStamp;
						this.Level = level;
						this.Content = Regex.Escape(content.Trim());
				}
				public LogLine(LogLevel level, string content) : this(DateTime.Now, level, content) { }
				private const string tfmt = @"yyyy\-MM\-dd\ HH\:mm\ss";
				public string TimeString {
						get {
								return $"{this.TimeStamp.ToString(tfmt)}.{this.TimeStamp.Millisecond.ToString(@"000")}";
						}
				}

				private static int MaxKeyWordLen = Enum.GetValues<LogLevel>().Select(x => x.GetKeyword()).Max(x => x.Length);
				public string LevelString {
						get {
								return $"[{this.Level.GetKeyword().ToUpperInvariant()}]".PadRight(MaxKeyWordLen + 2, ' ');
						}
				}
				public string Line {
						get {
								return $"{TimeString}\t{LevelString}\t{this.Content}";
						}
				}
				public override string ToString() { return this.Line; }
		}
}
