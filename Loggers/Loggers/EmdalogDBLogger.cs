using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.IO;

namespace Loggers
{
    public class EmdalogDBLogger : ILogger
    {
        private readonly Logger _logger;
        private readonly string _connectString;
        private readonly string _ActionDescription;
        private readonly string _Session_Id;
        private readonly string _IPAddress;
        private readonly string _AppURL;
        private readonly Guid? _LogRef;
        private readonly bool? _eSignConsent;
        private readonly string _KYCID;
        private readonly Guid _ID;
        private readonly string _RequestPacket;
        private readonly string _ResponsePacket;
        private readonly int _RequestID;


        public EmdalogDBLogger(string connectString, string ActionDescription, string Session_Id , string IPAddress, string AppURL , Guid? LogRef , bool? eSignConsent , string KYCID , Guid ID , string RequestPacket, string ResponsePacket , int RequestID)
        {
            _connectString = connectString;
            _ActionDescription = ActionDescription;
            _Session_Id = Session_Id;
            _IPAddress = IPAddress;
            _AppURL = AppURL;
            _LogRef = LogRef;
            _eSignConsent = eSignConsent;
            _KYCID = KYCID;
            _ID = ID;
            _RequestPacket = RequestPacket;
            _ResponsePacket = ResponsePacket;
            _RequestID = RequestID;
            _logger = CreateLogger();          
        }
        public void Debug(string message)
        {
          
        }

        public void Error(string message)
        {
          
        }

        public void Error(Exception exception)
        {
           
        }

        public void Error(string message, Exception exception)
        {
           
        }

        public void Info(string message )
        {

            _logger
                 .ForContext("LogId", _ID)
                .ForContext("TransactionID", _RequestID)
                .ForContext("ActionDescription", _ActionDescription)
                .ForContext("Session_Id", _Session_Id)
                .ForContext("IPAddress", _IPAddress)
                .ForContext("AppURL", _AppURL)
                .ForContext("eSignConsent", _eSignConsent)
                .ForContext("TransactionID", _RequestID)
                .ForContext("RequestPacket", _RequestPacket)
                .ForContext("ResponsePacket", _ResponsePacket)
                .ForContext("LogRef", _LogRef)
                .ForContext("KYCID", _KYCID)
                .Information(message);
            
        }


        private Logger CreateLogger()
        {
            var columnOpts = new ColumnOptions();
            columnOpts.Store.Remove(StandardColumn.Properties);
            columnOpts.Store.Remove(StandardColumn.Message);
            columnOpts.Store.Remove(StandardColumn.MessageTemplate);
            columnOpts.Store.Remove(StandardColumn.Level);
            columnOpts.Store.Remove(StandardColumn.Exception);
           
           
            List<SqlColumn> sqlColumns = new List<SqlColumn>();
            sqlColumns.Add(new SqlColumn() { ColumnName = "LogId", DataType = System.Data.SqlDbType.UniqueIdentifier  , AllowNull = false  });
            sqlColumns.Add(new SqlColumn() { ColumnName = "ActionDescription", DataType = System.Data.SqlDbType.NVarChar , DataLength = 4000 });
            sqlColumns.Add(new SqlColumn() { ColumnName = "Session_Id", DataType = System.Data.SqlDbType.NVarChar, DataLength = 4000 });
            sqlColumns.Add(new SqlColumn() { ColumnName = "IPAddress", DataType = System.Data.SqlDbType.NVarChar, DataLength = 4000 });
            sqlColumns.Add(new SqlColumn() { ColumnName = "AppURL", DataType = System.Data.SqlDbType.NVarChar, DataLength = 4000 });
            sqlColumns.Add(new SqlColumn() { ColumnName = "LogRef", DataType = System.Data.SqlDbType.UniqueIdentifier, AllowNull = true,});
            sqlColumns.Add(new SqlColumn() { ColumnName = "eSignConsent", DataType = System.Data.SqlDbType.Bit,AllowNull = true  });
            sqlColumns.Add(new SqlColumn() { ColumnName = "KYCID", DataType = System.Data.SqlDbType.NVarChar, DataLength = 4000 });
            sqlColumns.Add(new SqlColumn() { ColumnName = "TransactionID", DataType = System.Data.SqlDbType.NVarChar, DataLength = 4000 });
            sqlColumns.Add(new SqlColumn() { ColumnName = "RequestPacket", DataType = System.Data.SqlDbType.NVarChar});
            sqlColumns.Add(new SqlColumn() { ColumnName = "ResponsePacket", DataType = System.Data.SqlDbType.NVarChar  });

            columnOpts.AdditionalColumns = sqlColumns;

            Logger logger = new LoggerConfiguration().WriteTo.MSSqlServer(connectionString: _connectString, sinkOptions: new MSSqlServerSinkOptions { TableName = "Emdah-Log", AutoCreateSqlTable = true }, columnOptions: columnOpts)
                .CreateLogger();
            return logger;
        }
    }
}
