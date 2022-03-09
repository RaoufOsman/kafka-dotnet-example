using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace testapp.consumer
{
    public class ParallelConsumer
    {
        private ClientConfig _clientConfig;
        public ParallelConsumer(ClientConfig clientConfig)
        {
            _clientConfig = clientConfig;
        }
    }
}
