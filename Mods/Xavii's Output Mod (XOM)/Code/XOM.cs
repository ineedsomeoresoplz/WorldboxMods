using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using NeoModLoader.api;
using UnityEngine;

namespace XaviiOutputMod.Code
{
    
    
    
    public class XOM : BasicMod<XOM>
    {
        private const string LogsDirectoryName = "Logs";
        private const string LogFilePrefix = "console_";
        private const string LogFileExtension = ".log";
        private const string LatestPrefix = "LATEST_";

        private readonly object _fileLock = new();
        private StreamWriter _writer = null!;
        private string _logsDirectory = string.Empty;
        private ILogHandler _previousLogHandler = null!;
        private InterceptingLogHandler _interceptingLogHandler = null!;
        private static readonly object BufferLock = new();
        private static readonly System.Collections.Generic.List<BufferedEntry> BufferedEntries = new();
        private static bool _buffering = true;
        private static bool _earlyHooked;

        [ModuleInitializer]
        internal static void ModuleInitialize()
        {
            InstallEarlyHook();
        }

        protected override void OnModLoad()
        {
            _logsDirectory = Path.Combine(GetDeclaration().FolderPath, LogsDirectoryName);
            Directory.CreateDirectory(_logsDirectory);

            PrefixPreviousLatestLog();

            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
            string fileName = $"{LogFilePrefix}{timestamp}{LogFileExtension}";
            string filePath = Path.Combine(_logsDirectory, fileName);
            _writer = new StreamWriter(new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                AutoFlush = true
            };

            _previousLogHandler = Debug.unityLogger.logHandler;
            _interceptingLogHandler = new InterceptingLogHandler(this, _previousLogHandler);
            Debug.unityLogger.logHandler = _interceptingLogHandler;
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.Full);

            Application.logMessageReceivedThreaded += HandleLogMessage;
            Application.logMessageReceived += HandleLogMessage;
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
            TaskScheduler.UnobservedTaskException += HandleUnobservedTaskException;
            WriteLine($"=== XOM session started at {DateTime.UtcNow:O} ===");
            FlushBuffered();
        }

        private void PrefixPreviousLatestLog()
        {
            string[] files = GetExistingLogFiles();
            if (files.Length == 0) return;

            RemoveExistingLatestPrefix(files);
            files = GetExistingLogFiles();
            if (files.Length == 0) return;

            string latestFile = files
                .OrderByDescending(File.GetLastWriteTimeUtc)
                .First();

            string latestFileName = Path.GetFileName(latestFile);
            if (latestFileName.StartsWith(LatestPrefix, StringComparison.OrdinalIgnoreCase)) return;

            MoveFileAllowOverwrite(latestFile, Path.Combine(_logsDirectory, LatestPrefix + latestFileName));
        }

        private void RemoveExistingLatestPrefix(string[] logFiles)
        {
            foreach (string file in logFiles)
            {
                string fileName = Path.GetFileName(file);
                if (!fileName.StartsWith(LatestPrefix, StringComparison.OrdinalIgnoreCase)) continue;

                string trimmedName = fileName.Substring(LatestPrefix.Length);
                string destination = Path.Combine(_logsDirectory, trimmedName);
                MoveFileAllowOverwrite(file, destination);
            }
        }

        private string[] GetExistingLogFiles()
        {
            return Directory.GetFiles(_logsDirectory, $"*{LogFileExtension}", SearchOption.TopDirectoryOnly);
        }

        private static void MoveFileAllowOverwrite(string sourcePath, string destinationPath)
        {
            if (File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
            }

            File.Move(sourcePath, destinationPath);
        }

        private void HandleLogMessage(string condition, string stackTrace, LogType type)
        {
            var timestamp = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
            var builder = new StringBuilder();
            builder.Append('[').Append(timestamp).Append(']').Append(' ');
            builder.Append('[').Append(type).Append(']').Append(' ');
            builder.Append(condition);

            if (!string.IsNullOrWhiteSpace(stackTrace))
            {
                builder.AppendLine();
                builder.Append(stackTrace);
            }

            WriteLine(builder.ToString());
        }

        private void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                WriteException("UnhandledException", exception, e.IsTerminating);
            }
            else
            {
                WriteLine($"[{DateTime.UtcNow:O}] [UnhandledException] {e.ExceptionObject}");
            }
        }

        private void HandleUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var exception = e.Exception;
            WriteException("UnobservedTaskException", exception, false);
            e.SetObserved();
        }

        private void WriteException(string source, Exception exception, bool isTerminating)
        {
            var builder = new StringBuilder();
            builder.Append('[').Append(DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture)).Append(']').Append(' ');
            builder.Append('[').Append(source).Append(']').Append(' ');
            builder.Append(exception.GetType().FullName).Append(": ").Append(exception.Message);
            if (isTerminating)
            {
                builder.Append(" | Terminating");
            }

            var current = exception;
            while (current != null)
            {
                builder.AppendLine();
                builder.Append(current.StackTrace);
                current = current.InnerException;
                if (current != null)
                {
                    builder.AppendLine();
                    builder.Append("Inner: ").Append(current.GetType().FullName).Append(": ").Append(current.Message);
                }
            }

            WriteLine(builder.ToString());
        }

        private void WriteLine(string content)
        {
            try
            {
                lock (_fileLock)
                {
                    _writer?.WriteLine(content);
                }
            }
            catch
            {
            }
        }

        private void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= HandleLogMessage;
            Application.logMessageReceived -= HandleLogMessage;
            AppDomain.CurrentDomain.UnhandledException -= HandleUnhandledException;
            TaskScheduler.UnobservedTaskException -= HandleUnobservedTaskException;
            if (Debug.unityLogger.logHandler == _interceptingLogHandler && _previousLogHandler != null)
            {
                Debug.unityLogger.logHandler = _previousLogHandler;
            }
            lock (_fileLock)
            {
                _writer?.Flush();
                _writer?.Dispose();
                _writer = null;
            }
        }

        private void FlushBuffered()
        {
            lock (BufferLock)
            {
                _buffering = false;
                foreach (var entry in BufferedEntries)
                {
                    HandleLogMessage(entry.Condition, entry.StackTrace, entry.Type);
                }
                BufferedEntries.Clear();
            }
        }

        private static void RouteEarly(string condition, string stackTrace, LogType type)
        {
            lock (BufferLock)
            {
                if (!_buffering && Instance != null)
                {
                    Instance.HandleLogMessage(condition, stackTrace, type);
                }
                else
                {
                    BufferedEntries.Add(new BufferedEntry
                    {
                        Condition = condition,
                        StackTrace = stackTrace,
                        Type = type
                    });
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void InstallEarlyHook()
        {
            if (_earlyHooked) return;
            _earlyHooked = true;
            var currentHandler = Debug.unityLogger.logHandler;
            Debug.unityLogger.logHandler = new EarlyLogHandler(currentHandler);
        }

        private class InterceptingLogHandler : ILogHandler
        {
            private readonly XOM _owner;
            private readonly ILogHandler _inner;

            public InterceptingLogHandler(XOM owner, ILogHandler inner)
            {
                _owner = owner;
                _inner = inner;
            }

            public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
            {
                try
                {
                    var message = args != null && args.Length > 0 ? string.Format(format, args) : format;
                    _owner.HandleLogMessage(message, string.Empty, logType);
                }
                catch
                {
                }

                _inner?.LogFormat(logType, context, format, args);
            }

            public void LogException(Exception exception, UnityEngine.Object context)
            {
                try
                {
                    _owner.WriteException("LogException", exception, false);
                }
                catch
                {
                }

                _inner?.LogException(exception, context);
            }
        }

        private class EarlyLogHandler : ILogHandler
        {
            private readonly ILogHandler _inner;

            public EarlyLogHandler(ILogHandler inner)
            {
                _inner = inner;
            }

            public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
            {
                try
                {
                    var message = args != null && args.Length > 0 ? string.Format(format, args) : format;
                    RouteEarly(message, string.Empty, logType);
                }
                catch
                {
                }

                _inner?.LogFormat(logType, context, format, args);
            }

            public void LogException(Exception exception, UnityEngine.Object context)
            {
                try
                {
                    RouteEarly(exception.GetType().FullName + ": " + exception.Message, exception.StackTrace ?? string.Empty, LogType.Exception);
                }
                catch
                {
                }

                _inner?.LogException(exception, context);
            }
        }

        private class BufferedEntry
        {
            public string Condition = string.Empty;
            public string StackTrace = string.Empty;
            public LogType Type;
        }
    }
}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal sealed class ModuleInitializerAttribute : Attribute { }
}
