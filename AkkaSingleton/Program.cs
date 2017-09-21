using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Tools.Singleton;
using Akka.Configuration.Hocon;
using Akka.Routing;
using Common;

namespace AkkaSingleton
{
    class Program
    {
        static void Main(string[] args)
        {
            var akkaConfigSection = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
            var actorSystem = ActorSystem.Create("AkkaSingleton", akkaConfigSection);

            var worker1 = actorSystem.ActorOf(WorkerActor.CreateProps("1"), "worker1");
            var worker2 = actorSystem.ActorOf(WorkerActor.CreateProps("2"), "worker2");
            var worker3 = actorSystem.ActorOf(WorkerActor.CreateProps("3"), "worker3");

            var singletonActorRef = actorSystem.ActorOf(ClusterSingletonManager.Props(
                    singletonProps: ManagerSingletonActor.CreateProps("1"),
                    terminationMessage: PoisonPill.Instance,
                    settings: ClusterSingletonManagerSettings.Create(actorSystem).WithSingletonName("singleton").WithRole("singletons")),
                name: "ClusterSingletonManager");

            Console.ReadLine();

        }
    }    
}
