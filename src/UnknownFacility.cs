using System;

namespace rabbitmq.log4net.appender
{
    internal class UnknownFacility : IKnowAboutConfiguredFacility
    {
        public void UseToCall(Action<string> facilitySettingAction)
        {
        }
    }
}