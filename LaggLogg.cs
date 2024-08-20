using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Lagg.Logging {
		public static class LaggLogger {
				#region Init
				private static bool InitRun = false;
				private static void Init() {
						if (LaggLogger.InitRun) { return; }
						LaggLogger.Info($"Logging Started for: {Process.GetCurrentProcess().ProcessName}");
						AppDomain.CurrentDomain.ProcessExit += (sender, e) => {
								LaggLogger.Info($"Logging Stopped for: {Process.GetCurrentProcess().ProcessName}");
						};

						LaggLogger.InitRun = true;
				}
				#endregion

				#region File Logging
				public static bool FileLoggingEnabled { get; private set; } = false;
				public static LogLevel FileLogLevel = LogLevel.Debug;
				private static FileInfo? LogFile = null;
				private static FileStream? LogFileStream = null;
				private static StreamWriter? LogFileWriter = null;
				private static Object LogFileLock = new Object();
				private static string CommonApplicationDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
				private static string ExeDirPath = AppDomain.CurrentDomain.BaseDirectory;
				private static DirectoryInfo DefaultLogDir = new(Path.Join(ExeDirPath, "Logs"));
				private static string DefaultLogFileName = DateTime.Now.ToString(@"yyyy\-MM\-dd") + @".log";
				private static FileInfo DefaultLogFile = new(Path.Join(DefaultLogDir.FullName, DefaultLogFileName));
				public static void EnableFileLogging() { EnableFileLogging(DefaultLogFile); }
				public static void EnableFileLogging(DirectoryInfo logDir) { EnableFileLogging(Path.Join(logDir.FullName, DefaultLogFileName)); }
				public static void EnableFileLogging(DirectoryInfo logDir, string fileName) { EnableFileLogging(Path.Join(logDir.FullName, fileName)); }
				public static void EnableFileLogging(string filePath) { EnableFileLogging(new FileInfo(filePath)); }
				public static void EnableFileLogging(FileInfo file) {
						if (!file.Directory!.Exists) { file.Directory!.Create(); }
						if (!file.Exists) { file.Create(); }
						LogFile = file;
						FileLoggingEnabled = true;

						LogFileStream = new FileStream(LogFile.FullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
						LogFileWriter = new(LogFileStream);
						AppDomain.CurrentDomain.ProcessExit += (sender, e) => {
								LogFileWriter.Dispose();
								LogFileStream.Dispose();
						};

				}
				#endregion

				#region ConsoleLogging
				public static bool ConsoleLoggingEnabled = true;
				public static LogLevel ConsoleLogLevel = LogLevel.Warning;
				private static readonly KeyValuePair<LogLevel, ConsoleColor>[] DefaultLabelColors = [
						new (LogLevel.Emergency,ConsoleColor.Red),
						new (LogLevel.Alert,ConsoleColor.Red),
						new (LogLevel.Critical,ConsoleColor.Red),
						new (LogLevel.Error,ConsoleColor.Red),
						new (LogLevel.Warning,ConsoleColor.Yellow),
						new (LogLevel.Informational, ConsoleColor.Green),
						new (LogLevel.Debug,ConsoleColor.Green),
				];
				private static object ConsoleLock = new();
				private static ConcurrentDictionary<LogLevel, ConsoleColor> LabelColors = new(DefaultLabelColors);
				public static void SetLabelColor(LogLevel level, ConsoleColor color) { LabelColors[level] = color; }
				#endregion

				#region Log Writing
				private static void TryWriteToFile(ref LogLine line) {
						if (!FileLoggingEnabled) { return; }
						if (line.Level > FileLogLevel) { return; }
						lock (LogFileLock) {
								LogFileWriter!.WriteLine(line.Line);
						}
				}

				private static void TryWriteToConsole(ref LogLine line) {
						if (!ConsoleLoggingEnabled) { return; }
						if (line.Level > ConsoleLogLevel) { return; }

						ConsoleColor resetColor = Console.ForegroundColor;
						ConsoleColor labelColor = LabelColors[line.Level];
						lock (ConsoleLock) {
								if (Console.CursorLeft != 0) { Console.SetCursorPosition(0, Console.CursorTop + 1); }

								Console.Write(line.TimeString);
								Console.Write('\t');
								Console.ForegroundColor = labelColor;
								Console.Write(line.LevelString);
								Console.Write('\t');
						}
				}


				public static void Log(LogLevel level, string message) {
						Init();
						LogLine line = new(level, message);
						TryWriteToFile(ref line);
						TryWriteToConsole(ref line);
				}
				public static void Debug(string message) { Log(LogLevel.Debug, message); }
				public static void Info(string message) { Log(LogLevel.Informational, message); }
				public static void Notice(string message) { Log(LogLevel.Notice, message); }
				public static void Warning(string message) { Log(LogLevel.Warning, message); }
				public static void Error(string message) { Log(LogLevel.Error, message); }
				public static void Critical(string message) { Log(LogLevel.Critical, message); }
				public static void Alert(string message) { Log(LogLevel.Alert, message); }
				public static void Emergency(string message) { Log(LogLevel.Emergency, message); }
				#endregion
		}
}
