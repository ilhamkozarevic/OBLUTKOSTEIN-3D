using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShooterGame
{
    public struct Sprite
    {
        public int type;
        public int state;
        public int map;
        public float x, y, z;
        public float w, h;
        public int health;
        public double hitTimer;
        public double speed;
        public double damageTimer;
    }
}