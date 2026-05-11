using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShooterGame
{
    public struct Bullet
    {
        public double x, y;
        public double angle;
        public double speed;
        public double distanceTraveled;
        public bool active;
    }
}
