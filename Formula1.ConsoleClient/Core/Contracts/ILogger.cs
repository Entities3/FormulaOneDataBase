namespace Formula1.ConsoleClient.Core.Contracts
{
   public interface ILogger
    {
        void Info(string msg);

        void Error(string msg);

        void Fatal(string msg);
    }
}
