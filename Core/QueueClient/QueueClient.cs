using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using static Core.QueueClient.QueueCallback;

namespace Core.QueueClient
{
    public class QueueClient : IDisposable
    {
        private readonly string _queueName;
        private readonly string _exchange;
        private readonly string _severity;
        private readonly IBasicProperties _basicProperties;
        private readonly IModel _channel;
        private bool _disposed;


        public QueueClient(string exchange, string severity)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _exchange = exchange;

            _channel = connection.CreateModel();
            _channel.BasicQos(0, 1, false);
            _channel.ExchangeDeclare(exchange: _exchange,
                                     type: "direct");

            _basicProperties = _channel.CreateBasicProperties();
            _basicProperties.Persistent = true;
            _basicProperties.DeliveryMode = 2;

            _queueName = _channel.QueueDeclare(durable: true).QueueName;

            _severity = severity;
            _channel.QueueBind(queue: _queueName,
                                  exchange: _exchange,
                                  routingKey: _severity);
        }

        public QueueClient(string exchange, string severity, Callback receiveCallbackMethod)
            : this(exchange, severity)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                receiveCallbackMethod(Encoding.UTF8.GetString(ea.Body), ea.RoutingKey);
            };
            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="QueueClient"/> class.
        /// </summary>
        ~QueueClient()
        {
            Dispose(false);
        }

        public void Publish(object payload)
        {
            string message = JsonConvert.SerializeObject(payload);
            Publish(message);
        }

        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Publish(string message)
        {
            byte[] body = Encoding.UTF8.GetBytes(message);
            if (_channel.IsOpen)
                _channel.BasicPublish(_exchange, _severity, _basicProperties, body);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _channel.Dispose();
                }
                _disposed = true;
            }
        }
    }


    public class QueueCallback
    {
        public delegate void Callback(string payload, string routinKey);
    }

}
