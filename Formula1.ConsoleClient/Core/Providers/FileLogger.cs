namespace Formula1.ConsoleClient.Core.Providers
{
    using Contracts;
    using log4net;

    public class FileLogger : ILogger
    {
        private ILog log;

        public FileLogger(ILog log)
        {
            this.log = log;
        }

        public void Info(string msg)
        {
            this.log.Info(msg);
        }

        public void Error(string msg)
        {
            this.log.Error(msg);
        }

        public void Fatal(string msg)
        {
            this.log.Fatal(msg);
        }
    }
}
