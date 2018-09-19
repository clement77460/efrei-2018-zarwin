using System;
using System.Collections.Generic;
using System.Text;

namespace zombieLand
{
    class Game
    {
        City city;

        public Game()
        {
            this.city = new City(3);
            
        }

        public void gameLoop()
        {
            while (city.getSoldiers().Count != 0)
            {
                Wave wave = new Wave(10);
                this.attack(wave);
            }
            
        }

        private void attack(Wave wave)
        {
            
                
                this.approachPhase();

                while (city.getSoldiers().Count != 0 && wave.getWalkers().Count != 0)
                {
                    this.siegePhase(wave);
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

        private void siegePhase(Wave wave)
        {
            //tant qu'il y a des soldats et zombies
            
            this.soldiersAttackingWalkers(wave);
            this.walkersAttackingWall(wave);
            
        }

        private void soldiersAttackingWalkers(Wave wave)
        {
            foreach(Soldier soldier in city.getSoldiers())
            {
                if (wave.getWalkers().Count != 0) // on vérifie si la liste n'est pas vide
                {
                    wave.killWalker();
                }
            }
        }

        private void walkersAttackingWall(Wave wave)
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
                        city.reduceSoldierHealth(1);
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

