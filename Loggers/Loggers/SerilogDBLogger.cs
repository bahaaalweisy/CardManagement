using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;

namespace Loggers
{
    public class SerilogDBLogger : ILogger
    {
        private readonly Logger _logger;
        private readonly string _transactionCode;
        private readonly string _connectString;
        private readonly IConfiguration _configuration;
        public SerilogDBLogger(string connectString,string transactionCode, IConfiguration configuration)
        {
            _connectString = connectString;
            _transactionCode = transactionCode;
            _configuration = configuration;
            _logger = CreateLogger();          
        }
        public void Debug(string message)
        {
            _logger.ForContext("TransactionCode", _transactionCode).Debug(message);
        }

        public void Error(string message)
        {
            _logger.ForContext("TransactionCode", _transactionCode).Error(message);
        }

        public void Error(Exception exception)
        {
            _logger.ForContext("TransactionCode", _transactionCode).Error(exception, string.Empty);
        }

        public void Error(string message, Exception exception)
        {
            _logger.ForContext("TransactionCode", _transactionCode).Error(exception, message, new object[] { _transactionCode });
        }

        public void Info(string message)
        {
            _logger.ForContext("TransactionCode", _transactionCode).Information(message, new string[] { _transactionCode });
        }


        private Logger CreateLogger()
        {

            Logger logger = new LoggerConfiguration().WriteTo.MSSqlServer(connectionString: _connectString, sinkOptions: new MSSqlServerSinkOptions { TableName = "Log" , AutoCreateSqlTable = true })
                .CreateLogger();
            return logger;
        }
    }
}
