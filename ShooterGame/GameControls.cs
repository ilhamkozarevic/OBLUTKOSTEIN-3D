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
        public static float xAxis = 1.0f;
        public static float yAxis = 1.0f;

        public static float xCam = 1.0f;
        public static float yCam = 1.0f;

        public static byte lsButton;
        public static byte rsButton;

        public const int DEADZONE = 400;
        public const int RAW_RESOLUTION = 4096;
        public const int D_L_MIN = (RAW_RESOLUTION / 2) - DEADZONE;
        public const int U_R_MIN = (RAW_RESOLUTION / 2) + DEADZONE;

        public static Keys moveUp      = Keys.W;
        public static Keys moveDown    = Keys.S;
        public static Keys moveLeft    = Keys.A;
        public static Keys moveRight   = Keys.D;

        public static Keys camLeft     = Keys.Q;
        public static Keys camRight    = Keys.E;

        public static Keys interactKey = Keys.F;

        // MOUSE CONTROLS
        public static int deltaMouseX;

        public static int currentMouseX;
        public static int lastMouseX;

        private static SerialPort port;

        public static void InitController()
        {
            port = new SerialPort("COM12", 9600);

            if (port.IsOpen)
            {
                port.Close();
            }

            port.Open();

            port.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
        }

        // for handling received data

        private static string data;
        private static int  leftX, leftY;
        private static int  rightX, rightY;
        private static byte leftB, rightB;
        private static string[] parts;

        private static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            data  = port.ReadLine();
            parts = data.Split(',');

            if (parts.Length > 5)
            {
                leftX  = int.Parse(parts[0]);
                leftY  = int.Parse(parts[1]);
                rightX = int.Parse(parts[2]);
                rightY = int.Parse(parts[3]);
                leftB  = byte.Parse(parts[4]);
                rightB = byte.Parse(parts[5]);
            }

            //Console.Write(leftX + ", " + leftY + ", " + rightX + ", " + rightY + ", " + leftB + ", " + rightB + "\n");

            Player.left = false;
            Player.right = false;
            Player.up = false;
            Player.down = false;

            lsButton = leftB;
            rsButton = rightB;

            if (leftX < D_L_MIN)
            {
                Player.left = true;
                xAxis = 1.0f;
            }

            if (leftX > U_R_MIN)
            {
                Player.right = true;
                xAxis = 1.0f;
            }

            if (leftY < D_L_MIN)
            {
                Player.up = true;
                yAxis = 1.0f;
            }

            if (leftY > U_R_MIN)
            {
                Player.down = true;
                yAxis = 1.0f;
            }


            if (rightX < D_L_MIN)
            {
                Player.camLeft = true;
                xCam = 1.0f;
            }
            else
            {
                Player.camLeft = false;
                xCam = 1.0f;
            }

            if (rightX > U_R_MIN)
            {
                Player.camRight = true;
                xCam = 1.0f;
            }
            else
            {
                Player.camRight = false;
            }
        }
    

        // x i y ose ce da idu od -1.0 do 1.0, 
        // broj cemo primat od 1 do 4096 pa cemo 
        // pretvorit u double i dijelit sa 1000 (mozda se plan i promijeni)
    }
}
