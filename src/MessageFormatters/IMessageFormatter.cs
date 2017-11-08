namespace rabbitmq.log4net.appender.MessageFormatters
{
    public interface IMessageFormatter
    {
        bool CanApply(object messageObject);
        void Format(Message Message, object messageObject);
    }
}