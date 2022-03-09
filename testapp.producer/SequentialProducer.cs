using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace testapp.producer
{
    public class SequentialProducer
    {
        private readonly ClientConfig _clientConfig;
        private readonly ILogger _logger;
        private IList<string> _messagesProcessed;
        public SequentialProducer(ClientConfig clientConfig)
        {
            _clientConfig = clientConfig;
            _logger = LogManager.GetLogger("SequentialConsumer");
            _messagesProcessed = new List<string>();
        }

        public async void Send(string topic, string message)
        {
            var producerConfig = new ProducerConfig(_clientConfig);
            using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
            try
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Oops, something went wrong: {e}");
            }
        }
    }
}
