using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib.Models.TabelModels
{
	public class ReportTabelModel
	{
		public Guid ReportId { get; set; }
		public string FileUrl { get; set; }
		public List<CriteriaModel> Criteria { get; set; }
	}
}
