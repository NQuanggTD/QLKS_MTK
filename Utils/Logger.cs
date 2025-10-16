namespace QLKS.Utils
{
    public static class Logger
    {
        private static readonly string _logDirectory;
        private static readonly object _lock = new();

        static Logger()
        {
            _logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        public static void Info(string message)
        {
            Log("INFO", message);
        }

        public static void Warning(string message)
        {
            Log("WARNING", message);
        }

        public static void Error(string message)
        {
            Log("ERROR", message);
        }

        private static void Log(string level, string message)
        {
            lock (_lock)
            {
                try
                {
                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    var logMessage = $"[{timestamp}] [{level}] {message}";
                    
                    // Console output
                    Console.WriteLine(logMessage);
                    
                    // File output
                    var logFile = Path.Combine(_logDirectory, $"log_{DateTime.Now:yyyyMMdd}.txt");
                    File.AppendAllText(logFile, logMessage + Environment.NewLine);
                }
                catch
                {
                    // Ignore logging errors
                }
            }
        }
    }
}
