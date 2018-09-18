using System;
using System.Collections.Generic;
using System.Text;

namespace zombieLand
{
    class Game
    {
        City city;
        Wave wave;

        public Game()
        {
            this.city = new City(3);
            this.wave = new Wave(10);
        }

        public void gameLoop()
        {

            foreach(Soldier sold in city.getSoldiers())
            {
                sold.toString();
            }


            foreach (Walker walk in wave.getWalkers())
            {
                walk.toString();
            }

            
            this.attack();
        }

        private void attack()
        {
            
            this.approachPhase();

            while (city.getSoldiers().Count != 0 && wave.getWalkers().Count != 0)
            {
                this.siegePhase();
            }

            this.diplayAttackWinner();

        }

        private void diplayAttackWinner()
        {
            if (city.getSoldiers().Count == 0)
            {
                Console.Write("les zombies ont gagnés");
            }
            else
            {
                Console.Write("les soldats ont gagnés");
            }
            Console.ReadLine();
        }

        private void siegePhase()
        {
            //tant qu'il y a des soldats et zombies
            
            this.soldiersAttackingWalkers();
            this.walkersAttackingWall();
            
        }

        private void soldiersAttackingWalkers()
        {
            foreach(Soldier soldier in city.getSoldiers())
            {
                if (wave.getWalkers().Count != 0) // on vérifie si la liste n'est pas vide
                {
                    wave.killWalker();
                }
            }
        }

        private void walkersAttackingWall()
        {
            foreach (Walker walker in wave.getWalkers())
            {
                if (city.getWall().getHealth() != 0)
                {
                    city.getWall().reduceHealth(1);
                }
                else //on attaque les soldats
                {
                    if (city.getSoldiers().Count != 0) //on vérifie si la liste n'est pas vide
                    {
                        city.getSoldiers()[0].reduceHealth();
                    }
                }
            }
        }

        private void approachPhase()
        {
            Console.WriteLine("La horde approche !!");
        }

    }
}

