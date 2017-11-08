using System.Collections.Generic;

namespace rabbitmq.log4net.appender.MessageFormatters
{
    public class StringMessageFormatter : IMessageFormatter
    {
        private const int MaximumShortMessageLength = 250;

        readonly List<string> stringTypeList = new List<string>
        {
            "System.String","log4net.Util.SystemStringFormat","Common.Logging.Factory.AbstractLogger+StringFormatFormattedMessage"
        };

        public bool CanApply(object messageObject)
        {
            return stringTypeList.Contains(messageObject.GetType().FullName);
        }

        public void Format(Message Message, object messageObject)
        {
            var message = messageObject.ToString();
            if (message.Length > MaximumShortMessageLength)
            {
                Message.FullMessage = message;
                Message.ShortMessage = message.TruncateString(MaximumShortMessageLength);
            }
            else
            {
                Message.ShortMessage = message;
            }
        }
    }
}