using DataAccess.Database;
using DataAccess.Interface;
using ModelsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class KoordinateRepository : IKoordinateRepository
	{
		private readonly DataContext _dbContext;

		public KoordinateRepository(DataContext dbContext)
		{
			_dbContext = dbContext;
		}

		public void UpdateKoordinateRange(List<InstKoordinates> instKoordinates)
		{
			var updateIds = instKoordinates.Select(x => x.Id).ToList();
			var dbmodels = _dbContext.InstKoordinatesDatabasemodel.Where(x => updateIds.Contains(x.Id)).ToList();

			dbmodels.ForEach(x =>
			{
				var instKoordinatesmodel = instKoordinates.Find(instModel => instModel.Id == x.Id);
				x.lng = instKoordinatesmodel.lng;
				x.lat = instKoordinatesmodel.lat;
				x.LastChangedAt = DateTime.Now;
				x.Try = instKoordinatesmodel.Try;
			});

			_dbContext.SaveChanges();

		}
	}
}
