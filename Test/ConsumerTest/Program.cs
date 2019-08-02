using Messages;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var adapter = new BuiltinHandlerActivator())
            {
                adapter.Handle<Job>(async job =>
                {
                    Console.WriteLine("Processing job {0}", job.JobNumber);

                    // await Task.Delay(1);
                });

                Configure.With(adapter)
                    .Logging(l => l.ColoredConsole(LogLevel.Warn))
                    .Transport(t => t.UseRabbitMq("amqp://10.5.106.57", "consumer"))
                    .Options(o => o.SetMaxParallelism(5))
                    .Start();

                adapter.Bus.Subscribe<Job>().Wait();

                Console.WriteLine("Consumer listening - press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
