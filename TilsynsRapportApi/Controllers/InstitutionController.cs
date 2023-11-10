using DataAccess.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLib.DatabaseModels;
using ModelsLib.Models;
using ModelsLib.Models.TabelModels;
using TilsynsRapportApi.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TilsynsRapportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionController : ControllerBase
    {
        private readonly IInstitutionTableRepository _repository;

        public InstitutionController(IInstitutionTableRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<InstitutionController>
        [HttpGet]
        public async Task<IEnumerable<InstitutionTableModel>> Get()
        {

            var tabelModel = await _repository.GetInstitutionTableModels();

            return tabelModel;
        }

        // GET api/<InstitutionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<InstitutionController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<InstitutionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InstitutionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
