using CholletJaworskiZarwin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Input.Orders;

namespace ApiZarwin.Model
{
    public class Games
    {

        private DataSource ds = new DataSource();
        public List<Game> games=new List<Game>();

        public void LaunchGame(String id)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);
            Game game = new Game(simulation, false);
            games.Add(game);

            CancellationTokenSource taskToken = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                //problème selection de touche(Version 01 projet)
                if (simulation.turnInit == null)
                {
                    game.InitTurn();
                }
                else
                {
                    game.ApproachTurn(simulation.turnResults.Count);
                }

            }, taskToken.Token);
            
        }


        public void StartGame(Parameters parameters)
        {
            Game game = new Game(parameters, false);
            games.Add(game);

            game.InitTurn();
        }

        public void DestroyGame(String id)
        {

            foreach(Game game in games.ToArray())
            {
                if (game.simulation.IdString == id)
                {
                    game.stopSimulation=true;
                    games.Remove(game);
                }
            }

        }

        public List<Order> GetAllOrders(String id)
        {
            Simulation simulation = ds.GetSpecificSimulation(id);
            List<Order> orders = new List<Order>();

            foreach (OrderWrapperMongoDB o in simulation.orders)
            {
                orders.Add(o.ToOrder());
            }

            return orders;
        }

        public void UpdateOrdersForAGame(String id,Order[] newOrders)
        {
            foreach(Game game in games)
            {
                if (game.simulation.IdString == id)
                {
                    game.UpdateCityOrders(newOrders);
                }
            }
        }

    }
}
