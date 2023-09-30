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

        public decimal? lat { get; set; }

        public decimal? lng { get; set; }
    }
}
