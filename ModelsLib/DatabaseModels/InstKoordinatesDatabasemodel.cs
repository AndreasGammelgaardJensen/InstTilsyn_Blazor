using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib.DatabaseModels
{
    public class InstKoordinatesDatabasemodel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastChangedAt { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }
    }
}
