using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ShooterGame
{
    public static class Player
    {
        // ZA KRETANJE I POZICIJU
        public static double x;
        public static double y;

        public static int width = 4;
        public static int height = 4;

        public static Rectangle hitbox;

        public static bool up = false;
        public static bool down = false;
        public static bool left = false;
        public static bool right = false;

        // Vektori za smjer kretanja (Movement Direction Vectors)
        private static double moveDirX = 0; // -1 lijevo, 0 stoji, 1 desno
        private static double moveDirY = 0; // -1 gore, 0 stoji, 1 dole

        // vektori za brzinu
        private static double xVel = 0;
        private static double yVel = 0;

        public static double acceleration = 1000; // ubrzanje
        public static double friction = 20;     // trenje (usporava)
        public static double terminalVel = 255;  // maksimalna brzina

        // ZA ROTIRANJE

        public static double angle; // ugao u radijanima, 0 - desno

        // Vektori za smjer igraca (Player Direction Vectors)
        public static double dirX;
        public static double dirY;

        public static bool camLeft = false;
        public static bool camRight = false;

        public static void HandleMovement()
        {
            moveDirX = 0;
            moveDirY = 0;

            if (up)    moveDirY = -1;
            if (down)  moveDirY =  1;
            if (left)  moveDirX = -1;
            if (right) moveDirX =  1;

            double directionVector = Math.Sqrt(moveDirX * moveDirX + moveDirY * moveDirY); // vektorski zbir vektora smjera x i y
            if (directionVector > 0)
            {
                moveDirX /= directionVector;
                moveDirY /= directionVector;
            }

            xVel += moveDirX * acceleration * GameForm.deltaTime;
            yVel += moveDirY * acceleration * GameForm.deltaTime;

            if (moveDirX == 0) xVel -= xVel * friction * GameForm.deltaTime;
            if (moveDirY == 0) yVel -= yVel * friction * GameForm.deltaTime;

            double velocityVector = Math.Sqrt(xVel * xVel + yVel * yVel); // vektorski zbir brzina po x i y osi
            if (velocityVector > terminalVel)
            {
                xVel = xVel / velocityVector * terminalVel;
                yVel = yVel / velocityVector * terminalVel;
            }

            Player.xVel *= Math.Abs(GameControls.xAxis);
            Player.yVel *= Math.Abs(GameControls.yAxis);

            x += xVel * GameForm.deltaTime;
            HandleCollisionsX();

            y  += yVel * GameForm.deltaTime;
            HandleCollisionsY();
        }

        public static void HandleRotation()
        {
            if (camLeft) angle -= 0.01;
            if (camRight) angle += 0.01;

            if (angle > 2 * Math.PI) angle -= 2 * Math.PI;
            if (angle < 0) angle += 2 * Math.PI;

            dirX = x + 24 * Math.Cos(angle);
            dirY = y + 24 * Math.Sin(angle);
        }

        private static void UpdateHitbox()
        {
            hitbox = new Rectangle
            (
                (int)Math.Round(x) - width, 
                (int)Math.Round(y) - height, 
                2 * width, 
                2 * height
            );
        }

        private static void HandleCollisionsX()
        {
            UpdateHitbox();

            foreach (Rectangle rect in GameForm.currentLevel.collisionRects)
            {
                if (!hitbox.IntersectsWith(rect)) continue;

                if (xVel > 0) x = rect.Left - width;
                if (xVel < 0) x = rect.Right + width;

                xVel = 0;

                UpdateHitbox();
            }
        }

        private static void HandleCollisionsY()
        {
            UpdateHitbox();

            foreach (Rectangle rect in GameForm.currentLevel.collisionRects)
            {
                if (!hitbox.IntersectsWith(rect)) continue;

                if (yVel > 0) y = rect.Top - height;
                if (yVel < 0) y = rect.Bottom + height;

                yVel = 0;

                UpdateHitbox();
            }
        }

        public static void Shoot()
        {
            // TODO Bullet logic
        }
    }
}
