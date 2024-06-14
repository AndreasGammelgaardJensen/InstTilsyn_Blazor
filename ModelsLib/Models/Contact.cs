using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib.Models
{
	public class Contact
	{
		public Guid Id { get; set; }
		public string? HomePage { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }
	}
}
