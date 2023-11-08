using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BackendService.Application.RabbitMq
{
    public class Producer : IProducer
    {
        public void SendDetailMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection();
            using
            var channel = connection.CreateModel();
            channel.QueueDeclare("detail", exclusive: false);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: "detail", body: body);
        }
    }
}
