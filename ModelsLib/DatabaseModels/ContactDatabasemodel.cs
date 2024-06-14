using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib.DatabaseModels
{
	public class ContactDatabasemodel
	{
		public Guid Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime LastChangedAt { get; set; }
		public string? HomePage { get; set; }
		public string? Phone {  get; set; }
		public string? Email { get; set; }
	}

	
}
