
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Zarwin.Shared.Contracts.Input;

namespace CholletJaworskiZarwin
{
    public class DataSource
    {
        private MongoClient client;
        private IMongoDatabase db;
        private IMongoCollection<Simulation> collection;

        public DataSource()
        {
            this.client = new MongoClient("mongodb://mongo");
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


        public bool IsSimulationRunning(String id)
        {

            if (GetSpecificSimulation(id) == null)
            {
                return false;
            }

            if(GetSpecificSimulation(id).isRunning==1)
                return true;

            return false;
        }


        public Simulation GetSpecificSimulation(String id)
        {
            var filter = Builders<Simulation>.Filter.Eq("idString", id);


            if (collection.Find(filter).CountDocuments() == 0)
            {
                //n'existe pas
                return null;

            }

            return collection.Find(filter).First();
        }


        public Simulation ReadAllSimulations()
        {
            
            List<Simulation> simulations = collection.AsQueryable().ToList<Simulation>();

            return simulations[0];
        }

        public List<Simulation> ReadAllSimulationsAPI()
        {

            List<Simulation> simulations = collection.AsQueryable().ToList<Simulation>();

            return simulations;
        }

        private void UpdateSimulation(Simulation simulation,FilterDefinition<Simulation> filtre)
        {

            collection.ReplaceOne(filtre, simulation);


        }

        public void UpdateRunningStatus(Simulation simulation,int status)
        {
            var updateDef = Builders<Simulation>.Update.Set(s => s.isRunning, status);
            collection.UpdateOne<Simulation>(s => s.Id == simulation.Id, updateDef);
        }

        public void InsertSimulation(Simulation simulation)
        {
            collection.InsertOne(simulation);
        }

        public void DeleteSimulation(String id)
        {
            if (GetSpecificSimulation(id) != null)
            {
                collection.DeleteOne(s => s.IdString == id);
            }

        }
        
    }
}
