
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Zarwin.Shared.Contracts.Input;

namespace CholletJaworskiZarwin
{
    class DataSource
    {
        private MongoClient client;
        private IMongoDatabase db;
        private IMongoCollection<Simulation> collection;

        public DataSource()
        {
            this.client = new MongoClient();
            this.db = client.GetDatabase("zarwinDB");//créer et/ou utilise la base myFirstDb 
            this.collection = db.GetCollection<Simulation>("simulations");
        }
        
        public void SaveSimulation(Simulation simulation)
        {
            var filter = Builders<Simulation>.Filter.Eq("_id", simulation.Id);

            if (collection.Find(filter).CountDocuments() == 0)
            {
                //le document n'existe pas encore donc on insert
                this.InsertSimulation(simulation);
                
            }
            else
            {
                //le document existe donc on update
                this.UpdateSimulation(simulation,filter);
            }
            
            
        }

        public Simulation ReadAllSimulations()
        {
            
            List<Simulation> simulations = collection.AsQueryable().ToList<Simulation>();

            return simulations[0];
        }

        public void UpdateSimulation(Simulation simulation,FilterDefinition<Simulation> filtre)
        {

            collection.ReplaceOne(filtre, simulation);


        }

        public void InsertSimulation(Simulation simulation)
        {
            collection.InsertOne(simulation);
        }
        
    }
}
