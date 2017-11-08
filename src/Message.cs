using System;
using System.Collections.Generic;
using System.Globalization;

namespace rabbitmq.log4net.appender
{
    public class Message : Dictionary<string, object>
    {
        public string FullMessage
        {
            get { return ValueAs<string>("full_message"); }
            set { SetValueAs(value, "full_message"); }
        }

        public string Host
        {
            get { return ValueAs<string>("host"); }
            set { SetValueAs(value, "host"); }
        }

        public long Level
        {
            get { return ValueAs<long>("level"); }
            set { SetValueAs(value, "level"); }
        }

        public string ShortMessage
        {
            get { return ValueAs<string>("short_message"); }
            set { SetValueAs(value, "short_message"); }
        }

        public DateTime Timestamp
        {
            get
            {
                if (!ContainsKey("timestamp"))
                    return DateTime.MinValue;

                object val = this["timestamp"];
                DateTime  value;
                
                bool parsed = DateTime.TryParse(val as string, out value);
                return parsed ? value : DateTime.MinValue;
            }
            set
            {
                if (!ContainsKey("timestamp"))
                    Add("timestamp", value.ToString("yyyy-MM-dd HH:mm:ss.fffzzz"));
                else
                    this["timestamp"] = value.ToString("yyyy - MM - dd HH: mm:ss.fffzzz");
            }
        }

        private T ValueAs<T>(string key)
        {
            if (!ContainsKey(key))
                return default(T);

            return (T) this[key];
        }

        private void SetValueAs(object value, string key)
        {
            if (!ContainsKey(key))
                Add(key, value);
            else
                this[key] = value;
        }

        public static Message EmptyMessage
        {
            get
            {
                return new Message
                {
                    //Version = "1.0",
                    Host = Environment.MachineName,
                    //File = "",
                    //Line = ""
                };
            }
        }
    }
}