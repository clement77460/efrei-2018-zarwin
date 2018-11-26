using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace CholletJaworskiZarwin
{
    class DataSource
    {
        private MongoClient client;
        private IMongoDatabase db;
        public DataSource()
        {
            this.client = new MongoClient();
            this.db = client.GetDatabase("zarwinDB");//créer et/ou utilise la base myFirstDb 
        }
        
        public void SaveSimulation(Simulation simulation)
        {
            var collection = db.GetCollection<Simulation>("simulations"); 
            collection.InsertOne(simulation);
        }
        
    }
}
