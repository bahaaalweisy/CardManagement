using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Loggers
{
    public class LoggerFactory
    {
        private static IConfiguration _configuration;

        public static void setConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// Create a Search Criteria Logger
        /// </summary>
        /// <returns>A new Instance for the logger</returns>
        public static ILogger CreateLogger(bool isDatabase, string connectionString, string LogName = "")
        {
            Boolean.TryParse(_configuration["IsEnabledAPMElasticLog"], out bool IsEnabledAPM);

            ILogger logger;
            if (isDatabase)
            {
                if (IsEnabledAPM)
                {
                    logger = new SerilogDBLoggerWithAPM(connectionString, LogName, _configuration);
                }
                else
                {
                    logger = new SerilogDBLogger(connectionString, LogName, _configuration);
                }
            }
            else
            {
                string fileLoggerName = "Exceptions";
                if (LogName != string.Empty)
                {
                    fileLoggerName = LogName;
                }
                if (IsEnabledAPM)
                {
                    logger = new SerilogLoggerWithAPM(fileLoggerName, _configuration);
                }
                else
                {
                    logger = new SerilogLogger(fileLoggerName, _configuration);
                }
            }
            return logger;

        }

        public static ILogger CreateEmdaLogger(string connectString, string ActionDescription, string Session_Id, string IPAddress, string AppURL, Guid? LogRef, bool? eSignConsent, string KYCID, Guid ID, string RequestPacket, string ResponsePacket, int RequestId)
        {
            ILogger logger;
            logger = new EmdalogDBLogger(connectString, ActionDescription, Session_Id, IPAddress, AppURL, LogRef, eSignConsent, KYCID, ID, RequestPacket, ResponsePacket, RequestId);
            return logger;
        }
    }
}
