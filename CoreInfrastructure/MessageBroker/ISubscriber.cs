using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.MessageBroker
{
    public interface ISubscriber
    {
        public void Subscribe(CancellationToken stoppingToken, IEventHandler eventHandler);
    }
}
