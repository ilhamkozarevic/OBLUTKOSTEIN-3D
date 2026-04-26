using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace ShooterGame
{
    public static class GameControls
    {
        public static double xAxis = 1.0;
        public static double yAxis = 1.0;

        public static double xCam = 1.0;
        public static double yCam = 1.0;

        public const int DEADZONE = 200;
        public const int RAW_RESOLUTION = 4096;

        public static Keys moveUp = Keys.W;
        public static Keys moveDown = Keys.S;
        public static Keys moveLeft = Keys.A;
        public static Keys moveRight = Keys.D;

        public static Keys camLeft = Keys.Q;
        public static Keys camRight = Keys.E;

        // MOUSE CONTROLS
        public static int deltaMouseX;

        public static int currentMouseX;
        public static int lastMouseX;

        // ovdje ce da bude kod za serial

        public static SerialPort sp = new SerialPort();

        // x i y ose ce da idu od -1.0 do 1.0, 
        // broj cemo primat od 1 do 4096 pa cemo 
        // pretvorit u double i dijelit sa 1000 (mozda se plan i promijeni)
    }
}
