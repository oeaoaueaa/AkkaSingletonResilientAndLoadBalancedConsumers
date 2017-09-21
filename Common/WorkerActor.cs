using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace Common
{
    public class WorkerActor :ReceiveActor
    {       
        private readonly string _id;
        private bool _active;

        public WorkerActor(string id)
        {
            _id = id;
            _active = false;
            Ready();
        }

        private void Ready()
        {
            Receive<bool>(active => _active = active);
            ReceiveAny(m =>
            {
                if (_active) // TODO not very efficient, better schedule only when active
                {
                    Console.WriteLine($"WorkerActor({_id}) {Self.Path}: {m}");
                }
            });
        }

        public static Props CreateProps(string id)
        {
            return Props.Create<WorkerActor>(id);
        }

        protected override void PreStart()
        {
            Console.WriteLine($"WorkerActor({_id}) PreStart");
            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), Self, 0, Self);
            base.PreStart();
        }

        protected override void PostRestart(Exception reason)
        {
            Console.WriteLine($"WorkerActor({_id}) PostRestart");
            base.PostRestart(reason);
        }
    }
}
