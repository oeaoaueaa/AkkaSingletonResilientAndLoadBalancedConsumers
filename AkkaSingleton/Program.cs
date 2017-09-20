using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration.Hocon;

namespace AkkaSingleton
{
    class Program
    {
        static void Main(string[] args)
        {
            var akkaConfigSection = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
            var actorSystem = ActorSystem.Create("AkkaSingleton", akkaConfigSection);

            //var subscribers = actorSystem.ActorOf(Props.Create(() => new Subscriber()), "subscriber");

            Console.ReadLine();
        }
    }
}
