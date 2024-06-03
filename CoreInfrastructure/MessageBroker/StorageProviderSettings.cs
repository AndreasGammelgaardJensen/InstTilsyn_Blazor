using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.MessageBroker
{
    public class StorageProviderSettings
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}
