using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.Elasticsearch;

namespace Loggers
{
    public class SerilogLoggerWithAPM : ILogger
    {
        private readonly Logger _logger;
        private readonly string _fileName;
        private readonly IConfiguration _configuration;

        public SerilogLoggerWithAPM(string filename, IConfiguration configuration)
        {
            _fileName = filename;
            _configuration = configuration;
            _logger = CreateLogger();          
        }
        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(Exception exception)
        {
            _logger.Error(exception, string.Empty);
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(exception, message);
        }

        public void Info(string message)
        {
            _logger.Information(message);
        }


        private Logger CreateLogger()
        {
            string folderName = "Logs";
            string fileName = $"{folderName}{Path.DirectorySeparatorChar}{_fileName}-{Guid.NewGuid()}-.log";
            Logger logger = new LoggerConfiguration()
                .Enrich.WithElasticApmCorrelationInfo()
                .WriteTo.Elasticsearch((new ElasticsearchSinkOptions(GetStringList().Select(node => new Uri(node)))
                {
                    AutoRegisterTemplate = true,
                    //MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information,
                    TypeName = null,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    CustomFormatter = new EcsTextFormatter(),
                    ModifyConnectionSettings = x => x.BasicAuthentication(_configuration["ElasticConfiguration:Username"], _configuration["ElasticConfiguration:Password"]),
                    IndexFormat = $"serilog-etikal-{DateTime.UtcNow:yyyy-MM}",
                    BatchAction = ElasticOpType.Create,
                }))
                .WriteTo.File(fileName, rollingInterval: RollingInterval.Day)
            .CreateLogger();

            return logger;
        }

        private List<string> GetStringList()
        {
            List<string> Nodes = _configuration.GetSection("ElasticConfiguration:Uri").Get<List<string>>();
            return Nodes;
        }
    }
}
