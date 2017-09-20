using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace Common
{
    public class MySingletonActor : ReceiveActor
    {
        public MySingletonActor()
        {
            Ready();
        }

        protected override void PreStart()
        {
            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), Self, "Hi", Self);
            base.PreStart();
        }

        private void Ready()
        {
            ReceiveAny(m =>
            {
                Console.WriteLine(m);
            });
        }
    }
}
