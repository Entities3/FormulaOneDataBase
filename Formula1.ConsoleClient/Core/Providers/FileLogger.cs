namespace Formula1.ConsoleClient.Core.Providers
{
    using Contracts;
    using log4net;

    public class FileLogger : ILogger
    {
        private static FileLogger instance;
        private static ILog log;

        private FileLogger()
        {
            log = LogManager.GetLogger(typeof(FileLogger));
        }

        public static FileLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FileLogger();
                }

                return instance;
            }
        }

        public void Info(string msg)
        {
            log.Info(msg);
        }

        public void Error(string msg)
        {
            log.Error(msg);
        }

        public void Fatal(string msg)
        {
            log.Fatal(msg);
        }
    }
}
