using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib.Models
{
    public class InstKoordinates
    {
        public Guid Id { get; set; }

        public double? lat { get; set; }

        public double? lng { get; set; }

        public int Try { get; set; }
    }
}
