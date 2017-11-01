﻿using MarginTrading.BrokerBase.Settings;
using MarginTrading.Common.RabbitMq;

namespace MarginTrading.OrderRejectedBroker
{
    public class Settings : BrokerSettingsBase
    {
        public Db Db { get; set; }
        public RabbitMqQueues RabbitMqQueues { get; set; }
    }
    
    public class Db
    {
        public string HistoryConnString { get; set; }
    }
    
    public class RabbitMqQueues
    {
        public RabbitMqQueueInfo OrderRejected { get; set; }
    }
}