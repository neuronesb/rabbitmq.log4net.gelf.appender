using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net.Core;
using rabbitmq.log4net.appender.MessageFormatters;
using log4net;

namespace rabbitmq.log4net.appender
{
    public class Adapter
    {
        private readonly LogLevelMapper LogLevelMapper;
        private readonly IList<IMessageFormatter> messageObjectFormatters;

        private static readonly ExceptionMessageFormatter ExceptionMessageFormatter = new ExceptionMessageFormatter();

        public Adapter() : this(new LogLevelMapper()) { }

        public Adapter(LogLevelMapper LogLevelMapper)
            : this(LogLevelMapper, new List<IMessageFormatter>
                                           {
                                               new StringMessageFormatter(),
                                               ExceptionMessageFormatter,
                                               new DictionaryMessageFormatter(),
                                               new GenericObjectMessageFormatter(),
                                           }) { }

        public Adapter(LogLevelMapper LogLevelMapper, IList<IMessageFormatter> messageObjectFormatters)
        {
            this.LogLevelMapper = LogLevelMapper;
            this.messageObjectFormatters = messageObjectFormatters;
        }

        public string Facility { private get; set; }

        public Message Adapt(LoggingEvent loggingEvent)
        {
            var message = Message.EmptyMessage;
            message.Level = LogLevelMapper.Map(loggingEvent.Level);
            message.Timestamp = loggingEvent.TimeStamp;
            message["loggername"] = loggingEvent.LoggerName;
            message["loggerlevel"] = loggingEvent.Level.ToString();
            message["processname"] = Process.GetCurrentProcess().ProcessName;
            message["threadname"] = loggingEvent.ThreadName;

            if (loggingEvent!=null && loggingEvent.Repository != null)
                message["domain"] = !string.IsNullOrEmpty(loggingEvent.Repository.Name) ? loggingEvent.Repository.Name.Split('.').Last() : "";
            AddLocationInfo(loggingEvent, message);
            FormatMessage(message, loggingEvent);
            return message;
        }
        private void AddLocationInfo(LoggingEvent loggingEvent, Message Message)
        {
            if (loggingEvent.LocationInformation == null) return;
            //Message.File = loggingEvent.LocationInformation.FileName;
            //Message.Line = loggingEvent.LocationInformation.LineNumber;
        }

        private void FormatMessage(Message Message, LoggingEvent loggingEvent)
        {
            var messageFormatter = messageObjectFormatters.First(x => x.CanApply(loggingEvent.MessageObject));
            messageFormatter.Format(Message, loggingEvent.MessageObject);
            AppendExceptionInformationIfExists(Message, loggingEvent.ExceptionObject);

            if (string.IsNullOrWhiteSpace(Message.ShortMessage))
            {
                Message.ShortMessage = "Logged object of type: " + loggingEvent.MessageObject.GetType().FullName;
            }
        }

        private void AppendExceptionInformationIfExists(Message Message, Exception exceptionObject)
        {
            if (exceptionObject != null)
            {
                ExceptionMessageFormatter.Format(Message, exceptionObject);
            }
        }
    }
}