using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using testapp.db;

namespace testapp.consumer
{
    public class SequentialConsumerService : IHostedService
    {
        private ClientConfig _clientConfig;
        private ILogger _logger;

        public SequentialConsumerService(ClientConfig clientConfig)
        {
            _clientConfig = clientConfig;
            _logger = LogManager.GetLogger("SequentialConsumer");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumerConfig = new ConsumerConfig(_clientConfig);
            consumerConfig.GroupId = "sequential-group";
            consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
            consumerConfig.EnableAutoCommit = false;

            using var builder = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            builder.Subscribe("SequentialTopic");
            var cancelToken = new CancellationTokenSource();

            try
            {
                while (true)
                {
                    var consumer = builder.Consume(cancelToken.Token);

                    Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");

                    var dbMessage = new Message()
                    {
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        MessageText = $"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}"
                    };

                    using var db = new SqliteContext();
                    db.Add(dbMessage);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                builder.Close();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
