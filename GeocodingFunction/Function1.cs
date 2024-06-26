using System;
using CoreInfrastructure.Services.Geolocation;
using DataAccess.Database;
using DataAccess.Interface;
using DataAccess.Interfaces;
using GeolocationLib.Google;
using Microsoft.Azure.Functions.Worker;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ModelsLib.Models;
using ModelsLib.ResponseModels;
using Serilog;

namespace GeocodingFunction
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly DataContext _dataContext;
        private readonly IGoogleGeolocationService _googleGeplocationService;
        private readonly IInstitutionRepository _institutionRepository;
		private readonly IKoordinateRepository _kordinateRepository;
		private readonly int _batch_size = 5;

		public Function1(ILogger loggerFactory, DataContext dataContext, IGoogleGeolocationService googleGeplocationService, IInstitutionRepository institutionRepository, IKoordinateRepository kordinateRepository)
		{
			_logger = loggerFactory;
			_dataContext = dataContext;
			_googleGeplocationService = googleGeplocationService;
			_institutionRepository = institutionRepository;
			_kordinateRepository = kordinateRepository;
		}

		[Function("Function1")]
        public async Task Run([TimerTrigger("00:00:30")] TimerInfo myTimer)
        {
            _logger.Information($"C# Timer trigger function executed at: {DateTime.Now}");

			var instDb = _institutionRepository.GetInstitutions();
			var institutions = instDb.Where(x=>(x.Koordinates.lat == null || x.Koordinates.lng == null) && x.Koordinates.Try <= 3).ToList();

			if (!institutions.Any())
			{
				_logger.Information("Nothing to Update");
				return;

			}

			var updateList = new List<InstitutionFrontPageModel>();


			var handbreak = 0;

			while (institutions.Any())
			{
				if (handbreak == 600)
				{
					_logger.Warning("STOPPING DUE TO HANDBREAK");
					return;
				}

				var instForUpdate = institutions.Take(_batch_size).ToList();
				var addresses = instForUpdate.Select(inst => inst.address).ToList();

				var response = await _googleGeplocationService.GetKoordinatesFromAddressesAsync(addresses);

				var updatedKoortInst = LocationMapper.MapKoordinatesToInstitutions(response?.Results, instForUpdate);
				
				updatedKoortInst.ForEach(x => x.Koordinates.Try += 1);
				updateList.AddRange(updatedKoortInst);
				institutions = institutions.Skip(_batch_size).ToList();

				handbreak += _batch_size;
				_logger.Information($"retreived: {handbreak}");

			}

			_kordinateRepository.UpdateKoordinateRange(updateList.Select(inst => inst.Koordinates).ToList());

			_logger.Information($"Seccesfully updated koordinates");
		}
    }
}
