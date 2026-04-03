using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ShooterGame
{
    public static class Raycaster
    {
        public static int FOV = 60;

        private const int WINDOW_WIDTH  = 800;
        private const int WINDOW_HEIGHT = 600;

        private const int TILE_SIZE  = 20;
        private const int MAP_WIDTH  = 40;
        private const int MAP_HEIGHT = 30;
        private const int MAX_DOF    = 64;

        private const int RAYCASTER_RESOLUTION = 200;

        private const double TWO_PI =  Math.PI * 2;
        private const double DEG_IN_RAD = 0.0174532925; // vrijednost 1 stepena u radijanima

        public static void DrawRays3D(Graphics g)
        {
            double rayAngle = Player.angle - (DEG_IN_RAD * FOV / 2);

            double px = Player.x;
            double py = Player.y;

            for (int r = 0; r < RAYCASTER_RESOLUTION; r++)
            {
                if (rayAngle < 0      ) rayAngle += TWO_PI;
                if (rayAngle >= TWO_PI) rayAngle -= TWO_PI;

                double sinA = Math.Sin(rayAngle); // -sin jer je y koordinata suprotna u C#, olaksa matematiku
                double cosA = Math.Cos(rayAngle);
                double nTanA = -Math.Tan(rayAngle); // negative tangent function of the ray angle
                double nCotA = 1.0 / nTanA; // negative cotangent function of the ray angle

                double hRayX = px;
                double hRayY = py;
                double hDist = double.MaxValue;

                double vRayX = px;
                double vRayY = py;
                double vDist = double.MaxValue;

                double rDist = double.MaxValue; // ray distance

                Color wallColor = Color.Orange;

                // --- HORIZONTALNA KOMPOMENTA ---
                if (Math.Abs(sinA) > 0.0001)
                {
                    double rayX, rayY, xOffset, yOffset;

                    if (sinA > 0) // ugao od 0 do pi - gleda gore
                    {
                        rayY = Math.Floor(py / TILE_SIZE) * TILE_SIZE + TILE_SIZE;
                        rayX = (py - rayY) * nCotA + px;
                        yOffset = TILE_SIZE;
                        xOffset = -yOffset * nCotA;
                    }
                    else // ugao od pi do 2pi
                    {
                        rayY = Math.Floor(py / TILE_SIZE) * TILE_SIZE - 0.0001;
                        rayX = (py - rayY) * nCotA + px;
                        yOffset = -TILE_SIZE;
                        xOffset = -yOffset * nCotA;
                    }

                    for (int dof = 0; dof < MAX_DOF; dof++) // depth of field
                    {
                        // pretvare koordinate ekrana u koordinate niza

                        int mapX = (int)Math.Floor(rayX / TILE_SIZE);
                        int mapY = (int)Math.Floor(rayY / TILE_SIZE);

                        // ako zraka izadje izvan mape prekida se petlja
                        if (mapX < 0 || mapX >= MAP_WIDTH || mapY < 0 || mapY >= MAP_HEIGHT) break;

                        if (GameForm.currentLevel.map[mapX, mapY] == 1) // ako je pronadjen zid
                        {
                            hRayX = rayX;
                            hRayY = rayY;

                            double hRayDx = hRayX - px;
                            double hRayDy = hRayY - py;

                            hDist = Math.Sqrt((hRayDx * hRayDx) + (hRayDy * hRayDy)); // formula za udaljenost od koord. pocetka

                            break;
                        }

                        rayX += xOffset;
                        rayY += yOffset;
                    }
                }

                // --- VERTIKALNA KOMPONENTA ---

                if (Math.Abs(cosA) > 0.0001)
                {
                    double rayX, rayY, xOffset, yOffset;

                    if (cosA > 0) // ugao od 0 do pi - gleda desno
                    {
                        rayX = Math.Floor(px / TILE_SIZE) * TILE_SIZE + TILE_SIZE;
                        rayY = (px - rayX) * nTanA + py;
                        xOffset = TILE_SIZE;
                        yOffset = -xOffset * nTanA;
                    }
                    else // ugao od pi do 2pi
                    {
                        rayX = Math.Floor(px / TILE_SIZE) * TILE_SIZE - 0.0001;
                        rayY = (px - rayX) * nTanA + py;
                        xOffset = -TILE_SIZE;
                        yOffset = -xOffset * nTanA;
                    }

                    for (int dof = 0; dof < MAX_DOF; dof++) // depth of field
                    {
                        // pretvare koordinate ekrana u koordinate niza

                        int mapX = (int)Math.Floor(rayX / TILE_SIZE);
                        int mapY = (int)Math.Floor(rayY / TILE_SIZE);

                        // ako zraka izadje izvan mape prekida se petlja
                        if (mapX < 0 || mapX >= MAP_WIDTH || mapY < 0 || mapY >= MAP_HEIGHT) break;

                        if (GameForm.currentLevel.map[mapX, mapY] == 1) // ako je pronadjen zid
                        {
                            vRayX = rayX;
                            vRayY = rayY;

                            double vRayDx = vRayX - px;
                            double vRayDy = vRayY - py;

                            vDist = Math.Sqrt((vRayDx * vRayDx) + (vRayDy * vRayDy)); // formula za udaljenost od koord. pocetka

                            break;
                        }

                        rayX += xOffset;
                        rayY += yOffset;
                    }
                }

                if (hDist < vDist)
                {
                    g.DrawLine(new Pen(Color.Pink, 1), (int)px, (int)py, (int)hRayX, (int)hRayY);
                    wallColor = Color.FromArgb(255, 160, 0);
                    rDist = hDist;
                }
                else
                {
                    g.DrawLine(new Pen(Color.Magenta, 1), (int)px, (int)py, (int)vRayX, (int)vRayY);
                    wallColor = Color.FromArgb(225, 120, 0);
                    rDist = vDist;
                }

                rayAngle += ((double)FOV / RAYCASTER_RESOLUTION) * DEG_IN_RAD;

                // --- DRAW 3D WALLS ---

                rDist *= Math.Cos(rayAngle - Player.angle);

                double lineHeight = (TILE_SIZE * 600) / rDist;
                if (lineHeight > 600) lineHeight = 600; 
                double lineOffset = 300 - (lineHeight / 2);

                double lineWidth = (double)WINDOW_WIDTH / (double)RAYCASTER_RESOLUTION;
                double lineX = (r * (int)lineWidth) + 820;

                using (Pen wallPen = new Pen(wallColor, (int)lineWidth + 1))
                {
                    g.DrawLine(wallPen, (int)lineX, (int)lineOffset, (int)lineX, (int)lineHeight + (int)lineOffset);
                }
            }
        }
    }
}