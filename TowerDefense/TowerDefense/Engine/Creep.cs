using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefense
{
    public abstract class Creep
    {
        protected int health = 100;
        protected int speed = 100;

        public Creep()
        {
        }
    }
}
