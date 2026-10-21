using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;

namespace beanbrew {
    public partial class App : Application {
        public static readonly bool DebugMode = true;
        private static readonly object ConsoleLock = new object();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        public App() {
            if (DebugMode) {
                AllocConsole();
                Info("Console allocated, debug mode is on.");
            }
        }

        public static void Log(string logType, string message, ConsoleColor color) {
            if (!DebugMode) {
                return;
            }

            lock (ConsoleLock) {
                ConsoleColor originalColor = Console.ForegroundColor;
                try {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"[{DateTime.Now:HH:mm:ss}] ");

                    Console.ForegroundColor = color;
                    Console.Write($"[{logType.ToUpper()}] ");

                    Console.ForegroundColor = originalColor;
                    Console.WriteLine(message);
                }
                finally {
                    Console.ForegroundColor = originalColor;
                }
            }
        }

        public static void Info(string message) => Log("INFO", message, ConsoleColor.Green);
        public static void Warn(string message) => Log("WARN", message, ConsoleColor.Yellow);
        public static void Debug(string message) => Log("DEBUG", message, ConsoleColor.Cyan);
        public static void Error(string message) => Log("ERROR", message, ConsoleColor.Red);
    }
}
