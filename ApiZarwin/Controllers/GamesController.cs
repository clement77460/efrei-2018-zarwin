using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CholletJaworskiZarwin;
using MongoDB.Bson;

namespace ApiZarwin.Controllers
{
    [Route("zarwin/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        DataSource ds = new DataSource();
        

        // GET zarwin/games/{id}
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Simulation>> Get()
        {
            return ds.ReadAllSimulationsAPI();
        }

        // GET zarwin/games/{id}/running
        [HttpGet("{id}/running")]
        public ActionResult<Simulation> Get(String id)
        {
            return ds.IsSimulationExisting(id);
        }

        // POST zarwin/games
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT zarwin/games/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE zarwin/games/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
