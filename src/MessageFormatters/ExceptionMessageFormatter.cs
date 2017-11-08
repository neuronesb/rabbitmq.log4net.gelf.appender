using System;

namespace rabbitmq.log4net.appender.MessageFormatters
{
    public class ExceptionMessageFormatter : IMessageFormatter
    {
        public bool CanApply(object messageObject)
        {
            return messageObject is Exception;
        }

        public void Format(Message Message, object messageObject)
        {
            var exception = (Exception) messageObject;

            if (string.IsNullOrEmpty(Message.ShortMessage))
            {
                Message.ShortMessage = exception.Message;
            }
            else
            {
                Message["exceptionmessage"] = exception.Message;
            }

            if (string.IsNullOrEmpty(Message.FullMessage))
            {
                Message.FullMessage = exception.ToString();
            }
            else
            {
                Message["exceptionmessage"] = exception.Message;
            }

            Message["exception"] = exception.ToString();
            Message["exceptiontype"] = messageObject.GetType().FullName;
            Message["exceptionstacktrace"] = exception.StackTrace;

            if (exception.InnerException == null) return;

            Message[""] = exception.InnerException.GetType().FullName;
            Message["innerexceptionmessage"] = exception.InnerException.Message;
        }
    }
}
