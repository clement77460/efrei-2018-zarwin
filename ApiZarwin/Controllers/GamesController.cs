using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CholletJaworskiZarwin;
using MongoDB.Bson;
using Zarwin.Shared.Contracts.Output;
using Zarwin.Shared.Contracts.Input.Orders;
using Zarwin.Shared.Contracts.Input;
using System.Threading;
using Zarwin.Shared.Tests;
using ApiZarwin.Model;

namespace ApiZarwin.Controllers
{
    [Route("zarwin/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private DataSource ds = new DataSource();
        private readonly Games games;


        public GamesController(Games games)
        {
            this.games = games;
        }

        // GET zarwin/games/
        [HttpGet]
        public ActionResult<IEnumerable<Simulation>> Get()
        {
            return Ok(ds.ReadAllSimulationsAPI());
        }

        // GET zarwin/games/id
        [HttpGet("{id}", Name = "GetTurnState")]
        public ActionResult<TurnResult> GetTurnState(String id)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);

            if(simulation == null)
            {
                return NotFound("Simulation " + id + " non trouvée.");
            }

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

            if (simulation == null)
            {
                return NotFound("Simulation " + id + " non trouvée.");
            }

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
            Simulation simulation = ds.GetSpecificSimulation(id);

            if (simulation == null)
            {
                return NotFound("Simulation " + id + " non trouvée.");
            }

            return Ok(ds.IsSimulationRunning(id));
        }

        // GET zarwin/games/id/orders
        [HttpGet("{id}/orders")]
        public ActionResult<IEnumerable<Order>> GetAllOrders(String id)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);

            if (simulation == null)
            {
                return NotFound("Simulation " + id + " non trouvée.");
            }

            return Ok(this.games.GetAllOrders(id));
        }

        // GET zarwin/games/id/orders/{waveIndex}/{turnIndex}
        [HttpGet("{id}/orders/{waveIndex}/{turnIndex}")]
        public ActionResult<IEnumerable<Order>> GetAllOrdersAtTurnIndex(String id,int waveIndex,int turnIndex)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);
            if (simulation == null)
            {
                return NotFound("Simulation " + id + " non trouvée.");
            }
            List<Order> orders = new List<Order>();

            foreach (OrderWrapperMongoDB o in simulation.orders)
            {
                if(o.TurnIndex==turnIndex && o.WaveIndex==waveIndex)
                    orders.Add(o.ToOrder());
            }

            return Ok(orders);
        }

        // GET zarwin/games/id/orders/current
        [HttpGet("{id}/orders/current")]
        public ActionResult<IEnumerable<Order>> GetAllOrdersAtCurrentTurnIndex(String id)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);
            if (simulation == null)
            {
                return NotFound("Simulation " + id + " non trouvée.");
            }
            List<Order> orders = new List<Order>();


            int indexTurn = simulation.turnResults.Count ;
            int indexWave = simulation.waveResults.Count ;

            foreach (OrderWrapperMongoDB o in simulation.orders)
            {
                if (o.TurnIndex == indexTurn && o.WaveIndex == indexWave)
                    orders.Add(o.ToOrder());
            }

            return Ok(orders);
        }

        // POST zarwin/games
        [HttpPost]
        public ActionResult<TurnResult> PostLaunchGame([FromBody] Parameters value)
        {
            Simulation simulation = games.StartGame(value).simulation;

            return CreatedAtRoute(
                    "GetTurnState",
                    new
                    {
                        id = simulation.IdString
                    },
                    simulation);
        }

        // DELETE zarwin/games/{id}
        [HttpDelete("{id}")]
        public ActionResult<String> DeleteSimulation(String id)
        {
            if (ds.GetSpecificSimulation(id) == null)
            {
                return NotFound("Simulation " + id + " non trouvée.");
            }

            ds.DeleteSimulation(id);
            return Accepted();
        }


        // PUT zarwin/games/{id}/running
        [HttpPut("{id}/running")]
        public ActionResult<String> PutStartOldSimulation(String id, [FromBody] bool value)
        {
            if (value)
            {

                games.LaunchGame(id);
                return Ok("game " + id + " was launched");
                
            }
            else
            {
                games.DestroyGame(id);
                return Ok("game " + id + " was destroyed");
            }
        }

        // PUT zarwin/games/{id}/orders
        [HttpPut("{id}/orders")]
        public ActionResult<IEnumerable<Order>> PutChangeFuturOrders(String id, [FromBody] List<Order> value)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);
            if (simulation == null)
            {
                return NotFound("Simulation " + id + " non trouvée.");
            }
            int waveIndex = simulation.waveResults.Count;
            int turnIndex = simulation.turnResults.Count;

            List<Order> orders = this.games.GetAllOrders(id);

            foreach (Order o in value)
            {
                if (o.WaveIndex > waveIndex)
                {
                    orders.Add(o);
                }
                else
                {
                    if (o.WaveIndex >= waveIndex && o.TurnIndex > turnIndex)
                    {
                        orders.Add(o);
                    }
                }
            }

            //on sauvegarde en BD
            simulation.BuildOrderWrapperMongoDb(orders.ToArray());
            ds.SaveSimulation(simulation);

            //on regarde si la game est en train de tourner pour modifier en temps réel
            games.UpdateOrdersForAGame(id, orders.ToArray());

            return orders;

        }


        [HttpPut("{id}/orders/{waveIndex}/{turnIndex}")]
        public ActionResult<IEnumerable<Order>> PutChangeFuturOrdersForATurn(String id, int waveIndex,int turnIndex,[FromBody] List<Order> value)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);
            if(simulation == null)
            {
                return NotFound("Simulation " + id + " non trouvée.");
            }
            int actualWaveIndex = simulation.waveResults.Count;
            int actualTurnIndex = simulation.turnResults.Count;

            List<Order> everyOrders = games.GetAllOrders(id);
            List<Order> ordersForGivenTurn = new List<Order>();

            foreach (Order o in everyOrders)
            {
                if (o.WaveIndex == waveIndex && o.TurnIndex == turnIndex)
                {
                    ordersForGivenTurn.Add(o);
                }
            }

            if (HasToSwap(waveIndex, actualWaveIndex, turnIndex, actualTurnIndex))
            {
                //action du changement
                foreach (Order o in ordersForGivenTurn)
                {
                    everyOrders.Remove(o);
                }
                ordersForGivenTurn = new List<Order>();
                ordersForGivenTurn.AddRange(value.ToArray());
                everyOrders.AddRange(ordersForGivenTurn.ToArray());

                //on sauvegarde en BD
                simulation.BuildOrderWrapperMongoDb(everyOrders.ToArray());
                ds.SaveSimulation(simulation);

                //on regarde si la game est en train de tourner pour modifier en temps réel
                games.UpdateOrdersForAGame(id, everyOrders.ToArray());
            }

            return ordersForGivenTurn;
        }


        private bool HasToSwap(int waveIndex, int actualWaveIndex, int turnIndex, int actualTurnIndex)
        {
            if (waveIndex > actualWaveIndex)
            {
                return true;
            }
            else
            {
                if (waveIndex == actualWaveIndex)
                {
                    if (turnIndex > actualTurnIndex)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // PUT zarwin/games/{id}/orders/current
        [HttpPut("{id}/orders/current")]
        public ActionResult<IEnumerable<Order>> GetAllOrdersAtCurrentTurnIndexPut(String id)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);
            List<Order> orders = new List<Order>();


            int indexTurn = simulation.turnResults.Count;
            int indexWave = simulation.waveResults.Count;

            foreach (OrderWrapperMongoDB o in simulation.orders)
            {
                if (o.TurnIndex == indexTurn && o.WaveIndex == indexWave)
                    orders.Add(o.ToOrder());
            }

            return orders;
        }

    }
}
