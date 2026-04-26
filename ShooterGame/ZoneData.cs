using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShooterGame
{
    public class ZoneData
    {
        public byte WallID;
        public byte FloorID;
        public byte CeilID;

        public ZoneData(byte WallID, byte FloorID, byte CeilID)
        {
            this.WallID = WallID;
            this.FloorID = FloorID;
            this.CeilID = CeilID;
        }
    }
}
