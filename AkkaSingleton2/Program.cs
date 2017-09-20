using System;
using System.Configuration;
using Akka.Actor;
using Akka.Cluster.Tools.Singleton;
using Akka.Configuration.Hocon;
using Common;

namespace AkkaSingleton2
{
    class Program
    {
        static void Main(string[] args)
        {
            var akkaConfigSection = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
            var actorSystem = ActorSystem.Create("AkkaSingleton", akkaConfigSection);

            //var singletonActorRef = actorSystem.ActorOf(ClusterSingletonManager.Props(
            //        singletonProps: Props.Create<MySingletonActor>(),
            //        terminationMessage: PoisonPill.Instance,
            //        settings: ClusterSingletonManagerSettings.Create(actorSystem).WithSingletonName("singleton").WithRole("singletons")),//.WithRole("singletons")),
            //    name: "consumer");

            var singletonActor1Ref = actorSystem.ActorOf(ClusterSingletonManager.Props(
                    singletonProps: Props.Create<MySingletonActor>(),
                    terminationMessage: PoisonPill.Instance,
                    settings: ClusterSingletonManagerSettings.Create(actorSystem).WithSingletonName("singleton1").WithRole("singletons")),// .WithRole("singleton1")),
                name: "consumer1");

            Console.ReadLine();
        }
    }
}
