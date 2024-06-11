using ModelsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
	public interface IKoordinateRepository
	{
		public void UpdateKoordinateRange(List<InstKoordinates> instKoordinates);
	}
}
