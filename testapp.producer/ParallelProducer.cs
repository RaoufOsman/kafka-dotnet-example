using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace testapp.producer
{
    public class ParallelProducer
    {
        private readonly ClientConfig _clientConfig;
        public ParallelProducer(ClientConfig clientConfig)
        {
            _clientConfig = clientConfig;
        }
    }
}
