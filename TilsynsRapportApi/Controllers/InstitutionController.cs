using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLib.DatabaseModels;
using TilsynsRapportApi.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TilsynsRapportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionController : ControllerBase
    {
        private readonly BaseContext _dbContext;

        public InstitutionController(BaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<InstitutionController>
        [HttpGet]
        public async Task<IEnumerable<InstitutionFrontPageModelDatabasemodel>> Get()
        {
            var allePageDbModels = await _dbContext.InstitutionFrontPageModel.ToListAsync();
            return allePageDbModels;
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
