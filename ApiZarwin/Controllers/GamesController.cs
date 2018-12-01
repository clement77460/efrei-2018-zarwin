using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CholletJaworskiZarwin;
using MongoDB.Bson;
using Zarwin.Shared.Contracts.Output;
using Zarwin.Shared.Contracts.Input.Orders;

namespace ApiZarwin.Controllers
{
    [Route("zarwin/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        DataSource ds = new DataSource();


        // GET zarwin/games/
        [HttpGet]
        public ActionResult<IEnumerable<Simulation>> Get()
        {
            return ds.ReadAllSimulationsAPI();
        }

        // GET zarwin/games/id
        [HttpGet("{id}")]
        public ActionResult<TurnResult> GetTurnState(String id)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);

            int indexTurn = simulation.turnResults.Count - 1;
            if (indexTurn >= 0)
            {
                //on Prend le dernier TurnResult
                return simulation.turnResults[indexTurn];
            }

            int indexWave = simulation.waveResults.Count - 1;
            indexTurn = simulation.waveResults[indexWave].Turns.Length - 1;

            return simulation.waveResults[indexWave].Turns[indexTurn];
        }

        // GET zarwin/games/id/history
        [HttpGet("{id}/history")]
        public ActionResult<IEnumerable<TurnResult>> GetAllTurns(String id)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);
            List<TurnResult> turns = new List<TurnResult>();

            foreach(WaveResult wr in simulation.waveResults)
            {
                turns.AddRange(wr.Turns.ToArray());
            }

            turns.AddRange(simulation.turnResults.ToArray());

            return turns;
        }


        // GET zarwin/games/{id}/running
        [HttpGet("{id}/running")]
        public ActionResult<bool> GetIsRunning(String id)
        {
            return ds.IsSimulationRunning(id);
        }

        // GET zarwin/games/id/orders
        [HttpGet("{id}/orders")]
        public ActionResult<IEnumerable<Order>> GetAllOrders(String id)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);
            List<Order> orders = new List<Order>();

            foreach(OrderWrapperMongoDB o in simulation.orders)
            {
                orders.Add(o.ToOrder());
            }

            return orders;
        }

        // GET zarwin/games/id/orders/{waveIndex}/{turnIndex}
        [HttpGet("{id}/orders/{waveIndex}/{turnIndex}")]
        public ActionResult<IEnumerable<Order>> GetAllOrdersAtTurnIndex(String id,int waveIndex,int turnIndex)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);
            List<Order> orders = new List<Order>();

            foreach (OrderWrapperMongoDB o in simulation.orders)
            {
                if(o.TurnIndex==turnIndex && o.WaveIndex==waveIndex)
                    orders.Add(o.ToOrder());
            }

            return orders;
        }

        // GET zarwin/games/id/orders/current
        [HttpGet("{id}/orders/current")]
        public ActionResult<IEnumerable<Order>> GetAllOrdersAtCurrentTurnIndex(String id)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);
            List<Order> orders = new List<Order>();


            int indexTurn = simulation.turnResults.Count ;
            int indexWave = simulation.waveResults.Count ;

            foreach (OrderWrapperMongoDB o in simulation.orders)
            {
                if (o.TurnIndex == indexTurn && o.WaveIndex == indexWave)
                    orders.Add(o.ToOrder());
            }

            return orders;
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
