using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib.Models.RabbitMQ
{
    public class TilsynsRapportToExtraxtModel
    {
        public Guid id { get; set; }
        public string downloadUrl { get; set; }
        public Guid institutionId { get; set; }
        public string documentExtention { get; set; }

    }
}
