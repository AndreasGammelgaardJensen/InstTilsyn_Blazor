﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.MessageBroker
{
    public interface IPublisher
    {
        public void PublishMessage(string message);
    }
}
