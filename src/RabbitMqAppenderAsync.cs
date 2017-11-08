using System.Threading;
using RabbitMQ.Client;
using log4net.Core;

namespace rabbitmq.log4net.appender
{
    public class RabbitMqAppenderAsync : RabbitMqAppender
    {
        public RabbitMqAppenderAsync() : this(new Adapter()) { }

        public RabbitMqAppenderAsync(Adapter Adapter) : base(Adapter) { }

        protected override void Append(LoggingEvent loggingEvent)
        {
            ThreadPool.QueueUserWorkItem(AsyncAppend, loggingEvent);
        }

        protected override void SafeShutDownForModel()
        {
            lock (model)
            {
                base.SafeShutDownForModel();
            }
        }

        private void AsyncAppend(object state)
        {
            var loggingEvent = state as LoggingEvent;

            if (loggingEvent == null) return;

            lock (model)
            {
                EnsureConnectionIsOpen();
                var messageBody = Adapter.Adapt(loggingEvent).AsJson();
                model.BasicPublish(Exchange, "log4net.appender", true, null, messageBody.AsByteArray());
            }
        }
    }
}
