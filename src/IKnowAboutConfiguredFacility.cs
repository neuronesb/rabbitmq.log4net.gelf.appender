using System;

namespace rabbitmq.log4net.appender
{
    public interface IKnowAboutConfiguredFacility
    {
        void UseToCall(Action<string> facilitySettingAction);
    }
}
