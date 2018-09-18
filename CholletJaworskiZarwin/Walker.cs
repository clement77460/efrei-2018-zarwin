using System;
using System.Collections.Generic;
using System.Text;

namespace zombieLand
{
    class Walker
    {

        static int id = 0;

        int idWalker;
        public Walker()
        {
            this.idWalker = id;
            Walker.id++;
        }


        public void toString()
        {
            Console.WriteLine("je suis le zombie numero : {0}", idWalker);
        }

    }
}
