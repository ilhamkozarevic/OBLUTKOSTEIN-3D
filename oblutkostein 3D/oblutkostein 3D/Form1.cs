﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics; // Potrebno za Stopwatch

namespace oblutkostein_3D
{
    public partial class Form1 : Form
    {

        int[] mapW = 
        {
            3, 3, 3, 3, 3, 3, 3, 3,
            3, 0, 0, 3, 0, 0, 0, 3,
            3, 0, 0, 4, 0, 3, 0, 3,
            3, 3, 4, 3, 0, 0, 0, 1,
            3, 0, 0, 0, 0, 0, 0, 3,
            3, 0, 0, 0, 0, 3, 0, 2,
            3, 0, 0, 0, 0, 0, 0, 3,
            3, 3, 3, 3, 3, 3, 3, 3,
        };

        int[] mapF = 
        {
            2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2,
            3, 3, 3, 3, 3, 3, 3, 3,
            3, 3, 3, 3, 3, 3, 3, 3,
            2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2,
            3, 3, 3, 3, 3, 3, 3, 3,
            3, 3, 3, 3, 3, 3, 3, 3,
        };

        int[] mapC = 
        {
            2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2,
            1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1,
            2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2,
            1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1,
        };

        int mapX = 8, mapY = 8, mapS = 64;
        int xo, yo; // X i Y offset - dimenzije zidova

        double playerX = 300, playerY = 300;
        int centerX, centerY;

        bool goUp, goDown, goLeft, goRight;

        double rotationSpeed = 4.0;
        double speed = 100.0;

        double playerdX, playerdY, playerA = 0.0;

        Stopwatch stopwatch = new Stopwatch();
        double lastTime = 0, currentTime, deltaTime;

        Graphics g;
        SolidBrush cetkaMiniMap = new SolidBrush(Color.Yellow);
        Pen olovkaDirection = new Pen(Color.Orange);
        Pen olovkaZid = new Pen(Color.Red, 8);

        //Ray casting
        //X i Y koordinate na mapi, pozicija na mapi (map index), broj koraka koje ray pravi dok ne udari u zid
        int mx, my, mp, dof;
        //Trenutna X i Y pozicija vrha ray-a, ugao pod kojim ray putuje, offset/korak za koji se ray pomjera do sljedece minimap linije, te finalna udaljenost do najblizeg zida
        double rx, ry, ra, xoff, yoff, disT;
        double disH, hx, hy, aTan;
        double disV, vx, vy, nTan;

        double lineH, lineH_full, lineOff;

        int vmt = 0, hmt = 0;
        int hitWallType = 0;
        double shade = 1.0;

        double ty;
        double tx;

        double ty_step;
        double ty_off;

        int texOffset;

        int ty_idx, tx_idx;


        double dy, deg, raFix;


        // "Fish-eye" effect fix
        double ca;

        int y;

        int ipx, ipx_add_xo, ipx_sub_xo;
        int ipy, ipy_add_yo, ipy_sub_yo;

        int viewWidth = 512;
        int numRays = 128;
        double fov = 60 * (Math.PI / 180.0);
        double screenX;

        public Form1()
        {
            InitializeComponent();

            this.ClientSize = new Size(1024, viewWidth);

            this.DoubleBuffered = true;

            stopwatch.Start();

            Application.Idle += GameLoop;

            playerdX = Math.Cos(playerA);
            playerdY = Math.Sin(playerA);
        }

        //double degToRad(double a) { return a * Math.PI / 180.0; }

        //double FixAng(double a) { if (a > 360) { a -= 360; } if (a < 0) { a += 360; } return a; }

        private double distance(double ax, double ay, double bx, double by, double ang)
        {
            return (Math.Sqrt((bx - ax) * (bx - ax) + (by - ay) * (by - ay)));
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;

            //-----CRTANJE MINIMAPE-----//
            for (y = 0; y < mapY; y++)
            {
                for (int x = 0; x < mapX; x++)
                {
                    if (mapW[y * mapX + x] > 0) { cetkaMiniMap.Color = Color.White; } else { cetkaMiniMap.Color = Color.Black; }
                    xo = x * mapS;
                    yo = y * mapS;

                    g.FillRectangle(cetkaMiniMap, xo + 1, yo + 1, mapS - 1, mapS - 1);
                }
            }

            //Nacrtaj igraca na minimapi
            cetkaMiniMap.Color = Color.Yellow;
            g.FillEllipse(cetkaMiniMap, (int)playerX, (int)playerY, 8, 8);

            //Nacrtaj liniju direkcije igraca na minimapi
            centerX = (int)playerX + 4;
            centerY = (int)playerY + 4;
            g.DrawLine(olovkaDirection, centerX, centerY, (int)(centerX + playerdX * 5), (int)(centerY + playerdY * 5));


            //-----RAY CASTING-----
            ra = playerA - (fov/2.0);
            if (ra < 0) ra += 2 * Math.PI;
            if (ra > 2 * Math.PI) ra -= 2 * Math.PI;

            g.FillRectangle(Brushes.DarkSlateBlue, 530, 0, 512, 160);
            g.FillRectangle(Brushes.Black, 530, 160, 512, 160);
            for (int r = 0; r < numRays; r++)
            {
                vmt = 0;
                hmt = 0;

                //HORIZONTALNA PROVJERA
                dof = 0;
                disH = 1000000;
                hx = playerX;
                hy = playerY;
                aTan = -1.0 / Math.Tan(ra);

                //Igrac gleda dole
                if (ra > Math.PI)
                {
                    ry = ((int)(playerY / 64) * 64) - 0.0001;
                    rx = (playerY - ry) * aTan + playerX;
                    yoff = -64;
                    xoff = -yoff * aTan;
                }
                //Igrac gleda gore
                if (ra < Math.PI)
                {
                    ry = ((int)(playerY / 64) * 64) + 64;
                    rx = (playerY - ry) * aTan + playerX;
                    yoff = 64;
                    xoff = -yoff * aTan;
                }
                //Igrac gleda ravno lijevo ili ravno desno (ray nikada ne pogadja horizontalnu liniju)
                if (ra == 0 || ra == Math.PI)
                {
                    rx = playerX;
                    ry = playerY;
                    dof = 8;
                }
                while (dof < 8)
                {
                    mx = (int)(rx / 64);
                    my = (int)(ry / 64);
                    mp = my * mapX + mx;

                    if (mp > 0 && mp < mapX * mapY && mapW[mp] > 0) //Provjera da li je pogodjen horizontalni zid
                    {
                        hmt = mapW[mp];

                        hx = rx;
                        hy = ry;
                        disH = distance(playerX, playerY, hx, hy, ra);
                        dof = 8;

                    }
                    else
                    {
                        rx += xoff;
                        ry += yoff;
                        dof++;
                    }
                }

                //VERTIKALNA PROVJERA
                dof = 0;
                disV = 1000000;
                vx = playerX;
                vy = playerY;
                nTan = -Math.Tan(ra);

                //Igrac gleda lijevo
                if (ra > Math.PI / 2 && ra < 3 * Math.PI / 2)
                {
                    rx = ((int)(playerX / 64) * 64) - 0.0001;
                    ry = (playerX - rx) * nTan + playerY;
                    xoff = -64;
                    yoff = -xoff * nTan;
                }
                //Igrac gleda desno
                if (ra < Math.PI / 2 || ra > 3 * Math.PI / 2)
                {
                    rx = ((int)(playerX / 64) * 64) + 64;
                    ry = (playerX - rx) * nTan + playerY;
                    xoff = 64;
                    yoff = -xoff * nTan;
                }
                //Igrac gleda ravno dole ili ravno gore (ray nikada ne pogadja vertikalnu liniju)
                if (ra == 0 || ra == Math.PI)
                {
                    rx = playerX;
                    ry = playerY;
                    dof = 8;
                }
                while (dof < 8)
                {
                    mx = (int)(rx / 64);
                    my = (int)(ry / 64);
                    mp = my * mapX + mx;

                    if (mp > 0 && mp < mapX * mapY && mapW[mp] > 0) //Provjera da li je pogodjen vertikalni zid
                    {
                        vmt = mapW[mp];

                        vx = rx;
                        vy = ry;
                        disV = distance(playerX, playerY, vx, vy, ra);
                        dof = 8;
                    }
                    else
                    {
                        rx += xoff;
                        ry += yoff;
                        dof++;
                    }
                }

                shade = 1.0;
                hitWallType = 0;

                if (disV < disH) // Pogodjen prvo vertikalni zid
                {
                    shade = 0.5;
                    rx = vx;
                    ry = vy;
                    disT = disV;
                    hitWallType = vmt;
                    olovkaZid.Color = Color.FromArgb(230, 0, 0);
                }
                if (disH < disV) // Pogodjen prvo horizontalni zid
                {
                    rx = hx;
                    ry = hy;
                    disT = disH;
                    hitWallType = hmt;
                    olovkaZid.Color = Color.FromArgb(178, 0, 0);
                }

                //NACRTAJ RAY
                olovkaDirection.Color = Color.Red;
                g.DrawLine(olovkaDirection, (int)centerX, (int)centerY, (int)rx, (int)ry);

                //-----FISH-EYE EFFECT FIX-----
                ca = playerA - ra;
                if (ca < 0) ca += 2 * Math.PI;
                if (ca > 2 * Math.PI) ca -= 2 * Math.PI;
                disT = disT * Math.Cos(ca);
                if (disT < 0.1) disT = 0.1;

                //-----CRTANJE ZIDOVA-----
                lineH = (mapS * 320) / disT;
                lineH_full = lineH;
                lineOff = 160 - lineH / 2;
                if (lineH > 320) lineH = 320;
                if (lineOff < 0) lineOff = 0;

                ty_step = 32.0 / lineH_full;
                ty_off = 0.0;

                if (lineH_full > 320)
                {
                    ty_off = (lineH_full - 320.0) / 2.0;
                }

                ty = ty_off * ty_step;

                if (shade == 1)
                {
                    tx = (rx / 2.0) % 32;
                    if (ra > Math.PI) tx = 31 - tx;
                }
                else
                {
                    tx = (ry / 2.0) % 32;
                    if (ra > Math.PI / 2 && ra < 3 * Math.PI / 2) tx = 31 - tx;
                }

                screenX = 530 + (r * (double)(viewWidth / numRays));
                int rayWidth = (int)(viewWidth / numRays);

                texOffset = (hitWallType - 1) * 3072;
                if (texOffset < 0) texOffset = 0;

                for (y = 0; y < lineH; y++)
                {
                    ty_idx = (int)ty & 31;
                    tx_idx = (int)tx & 31;

                    int pixel = texOffset + (ty_idx * 32 + tx_idx) * 3;

                    int red = (int)(Textures.AllTextures[pixel + 0] * shade);
                    int green = (int)(Textures.AllTextures[pixel + 1] * shade);
                    int blue = (int)(Textures.AllTextures[pixel + 2] * shade);

                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(red, green, blue)))
                    {
                        g.FillRectangle(brush, (int)screenX, (int)lineOff + y, rayWidth, 1);
                    }

                    ty += ty_step;
                }
                /*
                //-----CRTANJE PODA-----
                for (y = (int)(lineOff + lineH); y < 320; y++)
                {
                    dy = y - (320 / 2.0);
                    deg = ra;
                    raFix = Math.Cos(playerA - ra);

                    tx = playerX / 2 + Math.Cos(deg) * 158 * 32 / dy / raFix;
                    ty = playerY / 2 + Math.Sin(deg) * 158 * 32 / dy / raFix;

                    mp = mapF[(int)(ty / 32.0) * mapX + (int)(tx / 32.0)] * 32 * 32;

                    pixelColor = (int)(allTextures[((int)(ty) & 31) * 32 + ((int)(tx) & 31) + mp]) * 255;
                    olovkaZid.Color = Color.FromArgb((int)(pixelColor / 1.3), (int)(pixelColor / 1.3), pixelColor);

                    g.DrawLine(olovkaZid, (int)(screenX), (int)y, (int)(screenX), (int)y + 1);


                    //-----CRTANJE KROVA-----
                    mp = mapC[(int)(ty / 32.0) * mapX + (int)(tx / 32.0)] * 32 * 32;

                    pixelColor = (int)(allTextures[((int)(ty) & 31) * 32 + ((int)(tx) & 31) + mp]) * 255;
                    olovkaZid.Color = Color.FromArgb((int)(pixelColor / 2.0), (int)(pixelColor / 1.2), (int)(pixelColor / 2.0));

                    g.DrawLine(olovkaZid, (int)(screenX), (int)320 - y, (int)(screenX), (int)320 - y + 1);
                }
                */
                ra += fov / numRays;
                if (ra < 0) ra += 2 * Math.PI;
                if (ra > 2 * Math.PI) ra -= 2 * Math.PI;
            }

        }

        private void GameLoop(object sender, EventArgs e)
        {
            currentTime = stopwatch.Elapsed.TotalSeconds;
            deltaTime = currentTime - lastTime;
            lastTime = currentTime;

            UpdateGame(deltaTime);

            this.Invalidate();
        }

        private void UpdateGame(double dt)
        {

            if (!(goUp && goDown))
            {
                xo = 0;
                yo = 0;

                if (playerdX < 0) { xo = -20; } else { xo = 20; }
                if (playerdY < 0) { yo = -20; } else { yo = 20; }

                ipx = (int)(playerX / 64.0);
                ipx_add_xo = (int)((playerX + xo) / 64.0);
                ipx_sub_xo = (int)((playerX - xo) / 64.0);

                ipy = (int)(playerY / 64.0);
                ipy_add_yo = (int)((playerY + yo) / 64.0);
                ipy_sub_yo = (int)((playerY - yo) / 64.0);

                if (goUp)
                {
                    if (mapW[ipy * mapX + ipx_add_xo] == 0) { playerX += playerdX * speed * dt; } //Sudar sa vertikalnim zidom - kretanje naprijed
                    if (mapW[ipy_add_yo * mapX + ipx] == 0) { playerY += playerdY * speed * dt; } //Sudar sa horizontalnim zidom - kretanje naprijed
                    if (mapW[(int)(playerY / 64.0) * mapX + (int)(playerX / 64.0)] != 0) { playerX -= playerdX * speed * dt; playerY -= playerdY * speed * dt; } //Sudar dijagonalno - kretanje naprijed
                }
                if (goDown)
                {
                    if (mapW[ipy * mapX + ipx_sub_xo] == 0) { playerX -= playerdX * speed * dt; } // Sudar sa vertikalnim zidom - kretanje nazad
                    if (mapW[ipy_sub_yo * mapX + ipx] == 0) { playerY -= playerdY * speed * dt; } // Sudar sa horizontalnim zidom - kretanje nazad
                    if (mapW[(int)(playerY / 64.0) * mapX + (int)(playerX / 64.0)] != 0) { playerX += playerdX * speed * dt; playerY += playerdY * speed * dt; } // Sudar dijagonalno - kretanje nazad
                }
            }

            if (!(goLeft && goRight))
            {
                if (goLeft)
                {
                    playerA -= rotationSpeed * dt;
                    if (playerA < 0) playerA += 2.0 * Math.PI;
                    playerdX = Math.Cos(playerA);
                    playerdY = Math.Sin(playerA);
                }

                if (goRight)
                {
                    playerA += rotationSpeed * dt;
                    if (playerA > 2.0 * Math.PI) playerA -= 2.0 * Math.PI;
                    playerdX = Math.Cos(playerA);
                    playerdY = Math.Sin(playerA);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) goUp = true;
            if (e.KeyCode == Keys.S) goDown = true;
            if (e.KeyCode == Keys.A) goLeft = true;
            if (e.KeyCode == Keys.D) goRight = true;
            if (e.KeyCode == Keys.E)
            {
                xo = 0;
                if (playerdX < 0) { xo = -25; } else { xo = 25; }
                yo = 0;
                if (playerdY < 0) { yo = -25; } else { yo = 25; }

                ipx = (int)(playerX / 64.0);
                ipx_add_xo = (int)((playerX + xo) / 64.0);

                ipy = (int)(playerY / 64.0);
                ipy_add_yo = (int)((playerY + yo) / 64.0);
                if (mapW[ipy_add_yo * mapX + ipx_add_xo] == 4) { mapW[ipy_add_yo * mapX + ipx_add_xo] = 0; }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) goUp = false;
            if (e.KeyCode == Keys.S) goDown = false;
            if (e.KeyCode == Keys.A) goLeft = false;
            if (e.KeyCode == Keys.D) goRight = false;
        }
    }
}