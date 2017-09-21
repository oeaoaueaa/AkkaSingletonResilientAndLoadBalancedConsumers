using System;
using System.Linq;
using Akka.Actor;
using Akka.Cluster;

namespace Common
{
    public class ManagerSingletonActor : ReceiveActor
    {
        private readonly string _id;

        public ManagerSingletonActor(string id)
        {
            _id = id;
            Ready();
        }

        public static Props CreateProps(string id)
        {
            return Props.Create<ManagerSingletonActor>(id);
        }

        protected override void PreStart()
        {
            Console.WriteLine($"PreStart {_id}");
            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), Self, 0, Self);
            base.PreStart();
        }

        protected override void PostRestart(Exception reason)
        {
            Console.WriteLine($"PostRestart {_id}");
            base.PostRestart(reason);
        }

        private void Ready()
        {
            Receive<string>(m =>
            {
                
                Console.WriteLine($"{_id}: {m}");
            });
            ReceiveAny(msg =>
            {
                var cluster = Cluster.Get(Context.System);

                var membersWithWorkers =
                    cluster.State.Members.Where(m => m.Roles.Contains("workers", StringComparer.OrdinalIgnoreCase) &&
                                                      m.Status == MemberStatus.Up).ToList();

                var workersSelection = Enumerable.Range(1, 3).Select(iWorker =>
                    membersWithWorkers.Select(m => $"{m.Address}/user/worker{iWorker}").ToList()).ToList();

                var roundRobinIndex = 0;

                int Next()
                {
                    return roundRobinIndex++ % membersWithWorkers.Count;
                }

                workersSelection.ForEach(workerSelections =>
                {
                    var activeIndex = Next();
                    for (int i = 0; i < workerSelections.Count; i++)
                    {
                        // TODO don't enable before disabling all other
                        var actorSelection = Context.System.ActorSelection(workerSelections[i]);
                        actorSelection.Tell(i == activeIndex); // enable/disable 
                    }
                });
            });
        }
    }
}
