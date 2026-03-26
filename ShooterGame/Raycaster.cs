using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ShooterGame
{
    public static class Raycaster
    {
        public static void DrawRays3D(Graphics g)
        {
            double HALF_PI = Math.PI / 2;

            int r, mapX, mapY, dof;
            double rayX, rayY, rayAngle, xOffset, yOffset;

            rayAngle = Player.angle;

            rayX = 0;
            rayY = 0;
            xOffset = 0;
            yOffset = 0;

            // check horizontal lines
            for (r = 0; r < 1; r++)
            {
                dof = 0;
                double aTan = -1 / Math.Tan(rayAngle);

                if (rayAngle > Math.PI) // looking down
                {
                    rayY = (((int)(Player.y) / 20) * 20) - 0.0001;
                    rayX = (Player.y - rayY) * aTan + Player.x;
                    yOffset -= 20;
                    xOffset = -yOffset * aTan;
                }

                if (rayAngle < Math.PI) // looking up
                {
                    rayY = (((int)(Player.y) / 20) * 20) + 20;
                    rayX = (Player.y - rayY) * aTan + Player.x;
                    yOffset += 20;
                    xOffset = -yOffset * aTan;
                }

                if (rayAngle == 0 || rayAngle == Math.PI)
                {
                    rayX = Player.x;
                    rayY = Player.y;
                    dof = 16;
                }

                while (dof < 16)
                {
                    mapX = (int)(rayX) / 20;
                    mapY = (int)(rayY) / 20;

                    if (mapX >= 0 && mapX < 40 && mapY >= 0 && mapY < 30)
                    {
                        if (GameForm.currentLevel.map[mapX, mapY] == 1)
                        {
                            dof = 16;
                        }
                        else
                        {
                            rayX += xOffset;
                            rayY += yOffset;
                            dof++;
                        }
                    }
                    else
                    {
                        dof = 16;
                    }
                }

                if (rayX < 1000 && rayY < 1000)
                {
                    g.DrawLine(new Pen(Color.Green, 12), (int)Math.Round(Player.x), (int)Math.Round(Player.y), (int)Math.Round(rayX), (int)Math.Round(rayY));

                }
                // vertical lines
                for (r = 0; r < 1; r++)
                {
                    dof = 0;
                    double nTan = -Math.Tan(rayAngle);

                    if (rayAngle > HALF_PI && rayAngle < 3 * HALF_PI) // looking left
                    {
                        rayX = (((int)(Player.x) / 20) * 20) - 0.0001;
                        rayY = (Player.x - rayX) * nTan + Player.y;
                        xOffset -= 20;
                        yOffset = -xOffset * nTan;
                    }

                    if (rayAngle < HALF_PI || rayAngle > 3 * HALF_PI) // looking right
                    {
                        rayX = (((int)(Player.x) / 20) * 20) + 20;
                        rayY = (Player.x - rayX) * nTan + Player.y;
                        xOffset += 20;
                        yOffset = -xOffset * nTan;
                    }

                    if (rayAngle == 0 || rayAngle == Math.PI)
                    {
                        rayX = Player.x;
                        rayY = Player.y;
                        dof = 16;
                    }

                    while (dof < 16)
                    {
                        mapX = (int)(rayX) / 20;
                        mapY = (int)(rayY) / 20;

                        if (mapX >= 0 && mapX < 40 && mapY >= 0 && mapY < 30)
                        {
                            if (GameForm.currentLevel.map[mapX, mapY] == 1)
                            {
                                dof = 16;
                            }
                            else
                            {
                                rayX += xOffset;
                                rayY += yOffset;
                                dof++;
                            }
                        }
                        else
                        {
                            dof = 16;
                        }
                    }

                    if (rayX < 1000 && rayY < 1000)
                    {
                        g.DrawLine(new Pen(Color.Red, 4), (int)Math.Round(Player.x), (int)Math.Round(Player.y), (int)Math.Round(rayX), (int)Math.Round(rayY));
                    }
                }
            }
        }
    }
}