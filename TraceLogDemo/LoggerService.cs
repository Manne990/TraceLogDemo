using System;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(TraceLogDemo.LoggerService))]
namespace TraceLogDemo
{
    public class LoggerService : ILoggerService
    {
        private readonly StringBuilder _log = new StringBuilder();
        private string _logfilename;

        public void LogTrace(string message)
        {
            System.Diagnostics.Debug.WriteLine($"LogTrace: {message}");

            lock (_log)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(_logfilename))
                    {
                        var dt = DateTime.Now;
                        _logfilename = $"log-{dt.Year:0000}-{dt.Month:00}-{dt.Day:00}-{dt.Hour:00}-{dt.Minute:00}-{dt.Second:00}.txt";
                    }

                    _log.Append(DateTime.Now + ": " + message + Environment.NewLine);

                    var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.Personal)}/{_logfilename}";
                    var logdata = _log.ToString();

                    System.IO.File.WriteAllText(path, logdata);
                }
                catch (System.IO.IOException)
                {
                }
            }
        }
    }
}