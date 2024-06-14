using ModelsLib.Models;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VuggestueTilsynScraperLib.Interfaces;

namespace VuggestueTilsynScraperLib.Scraping
{
    public class InstitutionDirector
    {
        private IInstitutionBuilder _builder;

        public IInstitutionBuilder Builder
        {
            set { _builder = value; }
        }


        public InstitutionFrontPageModel GetInstitutionFrontPageModel()
        {
            var inds = _builder.BuildBaseInformation();
            var addRess = _builder.BuildAdress();
            var pladser = _builder.BuildPladser();
            var contact = _builder.BuildInstitutionContact();
            var koordinates = _builder.BuildInstitutionCoordinates();
            var rapport = _builder.BuildInstitutionTilsynsRapport();

            //Validate reports


            inds.pladser = pladser;
            inds.InstitutionTilsynsRapports.Add(rapport);
            inds.address = addRess;
            inds.Koordinates = koordinates;
            inds.Contact = contact;

            return inds;
        }
    }
}
