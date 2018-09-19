using System;
using System.Collections.Generic;
using System.Text;

namespace zombieLand
{
    class City
    {

        List<Soldier> soldiers;
        Wall wall;

        public City(int numberOfSoldiers)
        {
            wall = new Wall();
            soldiers = new List<Soldier>();

            this.createSoldiers(numberOfSoldiers);
        }


        private void createSoldiers(int numberOfSoldiers)
        {
            for(int i=0;i< numberOfSoldiers; i++)
            {
                soldiers.Add(new Soldier());
            }
        }


        public List<Soldier> getSoldiers() => this.soldiers;

        public Wall getWall() => this.wall;
        
        public void reduceSoldierHealth(int value)
        {
            soldiers[0].reduceHealth(value);

            if (soldiers[0].getHealth() == 0)
            {
                this.soldiers.RemoveAt(0);
                Console.Write("[soldat] MORT");
            }
        }
    }
}
