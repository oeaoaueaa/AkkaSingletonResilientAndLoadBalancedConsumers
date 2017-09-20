using System;
using System.Configuration;
using Akka.Actor;
using Akka.Configuration.Hocon;

namespace AkkaSingleton2
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
