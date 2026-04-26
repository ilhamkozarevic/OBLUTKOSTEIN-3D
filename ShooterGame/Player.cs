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
        public static float x;
        public static float y;

        public static int width = 6;
        public static int height = 6;

        public static Rectangle hitbox;

        public static bool up = false;
        public static bool down = false;
        public static bool left = false;
        public static bool right = false;

        // Vektori za smjer kretanja (Movement Direction Vectors)
        private static float moveDirX = 0; // -1 lijevo, 0 stoji, 1 desno
        private static float moveDirY = 0; // -1 gore, 0 stoji, 1 dole

        // vektori za brzinu
        private static float xVel = 0;
        private static float yVel = 0;

        public static float acceleration = 127f; // ubrzanje
        public static float friction = 20f;     // trenje (usporava)
        public static float terminalVel = 60f;  // maksimalna brzina

        // ZA ROTIRANJE

        public static float angle; // ugao u radijanima, 0 - desno

        // Vektori za smjer igraca (Player Direction Vectors)
        public static float dirX;
        public static float dirY;

        public static bool camLeft = false;
        public static bool camRight = false;

        public static void HandleMovement()
        {
            // 3D movement sistem

            float fwdX = (float)Math.Cos(angle);
            float fwdY = (float)Math.Sin(angle);

            float sideX = fwdY;
            float sideY = -fwdX;

            moveDirX = 0;
            moveDirY = 0;

            if (up)    { moveDirY +=  fwdY; moveDirX += fwdX;  }
            if (down)  { moveDirY -=  fwdY; moveDirX -= fwdX;  }
            if (left)  { moveDirY += sideY; moveDirX += sideX; }
            if (right) { moveDirY -= sideY; moveDirX -= sideX; }

            float directionVector = (float)Math.Sqrt(moveDirX * moveDirX + moveDirY * moveDirY); // vektorski zbir vektora smjera x i y
            if (directionVector > 0)
            {
                moveDirX /= directionVector;
                moveDirY /= directionVector;
            }

            xVel += moveDirX * acceleration * GameForm.deltaTime;
            yVel += moveDirY * acceleration * GameForm.deltaTime;

            if (moveDirX == 0) xVel -= xVel * friction * GameForm.deltaTime;
            if (moveDirY == 0) yVel -= yVel * friction * GameForm.deltaTime;

            float velocityVector = (float)Math.Sqrt(xVel * xVel + yVel * yVel); // vektorski zbir brzina po x i y osi
            if (velocityVector > terminalVel)
            {
                xVel = xVel / velocityVector * terminalVel;
                yVel = yVel / velocityVector * terminalVel;
            }

            Player.xVel *= (float)Math.Abs(GameControls.xAxis);
            Player.yVel *= (float)Math.Abs(GameControls.yAxis);

            x += xVel * GameForm.deltaTime;
            HandleCollisionsX();

            y += yVel * GameForm.deltaTime;
            HandleCollisionsY();
        }

        public static void HandleRotation()
        {
            if (camLeft)  angle -= 2.5f * GameForm.deltaTime;
            if (camRight) angle += 2.5f * GameForm.deltaTime;
            
            if (angle > 2 * Math.PI) angle -= 2f * (float)Math.PI;
            if (angle < 0)           angle += 2f * (float)Math.PI;

            dirX = x + 24f * (float)Math.Cos(angle);
            dirY = y + 24f * (float)Math.Sin(angle);
        }

        private static void UpdateHitbox()
        {
            int xBound = (xVel > 0) ? (int)Math.Ceiling(x) : (int)Math.Floor(x);
            int yBound = (yVel > 0) ? (int)Math.Ceiling(y) : (int)Math.Floor(y);

            hitbox = new Rectangle
            (
                xBound - width,
                yBound - height, 
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

                if (xVel > 0) x = rect.Left  - width;
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
