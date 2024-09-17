﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace TestRabbitMQ
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var factory = new ConnectionFactory { HostName = "localhost" };
			using var connection = factory.CreateConnection();
			using var channel = connection.CreateModel();

			channel.QueueDeclare(queue: "magevent",
								 durable: false,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);

			Console.WriteLine("Waiting for messages.");

			var consumer = new EventingBasicConsumer(channel);
			consumer.Received += (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				Console.WriteLine($"[{message}] Received");
			};
			channel.BasicConsume(queue: "magevent",
								 autoAck: true,
								 consumer: consumer);

			Console.WriteLine("Press [enter] to exit.");
			Console.ReadLine();
		}
	}
}