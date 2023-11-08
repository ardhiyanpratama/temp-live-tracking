// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Net.Http.Headers;
using System;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};
//Create the RabbitMQ connection using connection factory details as i mentioned above
var connection = factory.CreateConnection();
//Here we create channel with session and model
using
var channel = connection.CreateModel();
//declare the queue after mentioning name and a few property related to that
channel.QueueDeclare("detail", exclusive: false);
//Set Event object which listen message from chanel which is sent by producer
var consumer = new EventingBasicConsumer(channel);
consumer.Received += async (model, eventArgs) => {
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Detail message received: {message}");

    //direct to url
    using (var httpClient = new HttpClient())
    {
        try
        {
            var url = "https://appmlivetracking/api/asdas";
            httpClient.BaseAddress = new Uri("https://appmlivetracking/api/asdas");

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response =
                await httpClient.PostAsync(url, new StringContent(message, Encoding.UTF8, "application/json")))
            {
                using (var content = response.Content)
                {
                    var result = await content.ReadAsStringAsync();
                    Console.WriteLine($"Detail direct url message: {result}");
                }
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Detail direct url message exception: {ex.Message}");
        }
        
    }

};
//read the message
channel.BasicConsume(queue: "detail", autoAck: true, consumer: consumer);
Console.ReadKey();
