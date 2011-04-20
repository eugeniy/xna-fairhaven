using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefense
{
    public abstract class Tower
    {
        protected int radius = 5;
        protected int damage = 25;
        protected int price = 100;

        public Tower()
        {
        }
    }
}
