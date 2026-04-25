﻿﻿﻿﻿﻿using System;
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
        SolidBrush cetkaPodKrov = new SolidBrush(Color.Blue);

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

        int viewWidth = 1024;
        int viewHeight = 512;
        int numRays = 128;
        double fov = 60 * (Math.PI / 180.0);
        double screenX;

        public struct sprite
        {
            public int type;     //key, enemy
            public int state;    //on, off
            public int map;      //texture to show
            public int x, y, z;  //position
            public double w, h;
        }

        sprite[] sp = new sprite[4];

        double[] depth;

        Bitmap gunTexture;

        public Form1()
        {
            InitializeComponent();

            depth = new double[numRays];

            this.Text = "Oblutkostein 3D";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.ClientSize = new Size(viewWidth, viewHeight);

            this.DoubleBuffered = true;

            stopwatch.Start();

            Application.Idle += GameLoop;

            playerdX = Math.Cos(playerA);
            playerdY = Math.Sin(playerA);

            sp[0].type = 1; sp[0].state = 1; sp[0].map = 0; sp[0].x = 150; sp[0].y = 150; sp[0].z = 5; sp[0].w = 1; sp[0].h = 1; //sprite 1

            gunTexture = new Bitmap(32, 32);

            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    int pIdx = (y * 32 + x) * 3;

                    Color c = Color.FromArgb(
                        Textures.gunTexture[pIdx],
                        Textures.gunTexture[pIdx + 1],
                        Textures.gunTexture[pIdx + 2]
                    );

                    if (c.R == 255 && c.G == 0 && c.B == 255)
                        gunTexture.SetPixel(x, y, Color.Transparent);
                    else
                        gunTexture.SetPixel(x, y, c);
                }
            }
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

            /*
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
            */

            /*
            //Nacrtaj igraca na minimapi
            cetkaMiniMap.Color = Color.Yellow;
            g.FillEllipse(cetkaMiniMap, (int)playerX, (int)playerY, 8, 8);
            */

            //Nacrtaj liniju direkcije igraca na minimapi
            centerX = (int)playerX + 4;
            centerY = (int)playerY + 4;
            //g.DrawLine(olovkaDirection, centerX, centerY, (int)(centerX + playerdX * 5), (int)(centerY + playerdY * 5));


            //-----RAY CASTING-----
            ra = playerA - (fov / 2.0);
            if (ra < 0) ra += 2 * Math.PI;
            if (ra > 2 * Math.PI) ra -= 2 * Math.PI;

            g.FillRectangle(Brushes.DarkSlateBlue, 0, 0, ClientRectangle.Width, ClientRectangle.Height / 2);
            g.FillRectangle(Brushes.Black, 0, ClientRectangle.Height / 2, ClientRectangle.Width, ClientRectangle.Height);
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
                //olovkaDirection.Color = Color.Red;
                //g.DrawLine(olovkaDirection, (int)centerX, (int)centerY, (int)rx, (int)ry);

                //-----FISH-EYE EFFECT FIX-----
                ca = playerA - ra;
                if (ca < 0) ca += 2 * Math.PI;
                if (ca > 2 * Math.PI) ca -= 2 * Math.PI;
                disT = disT * Math.Cos(ca);
                if (disT < 0.1) disT = 0.1;

                //-----CRTANJE ZIDOVA-----
                lineH = (mapS * viewHeight) / disT;
                lineH_full = lineH;
                lineOff = (viewHeight / 2.0) - lineH / 2;
                if (lineH > viewHeight) lineH = viewHeight;
                if (lineOff < 0) lineOff = 0;

                ty_step = 32.0 / lineH_full;
                ty_off = 0.0;

                if (lineH_full > viewHeight)
                {
                    ty_off = (lineH_full - (double)viewHeight) / 2.0;
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

                screenX = r * (double)(viewWidth / numRays);
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

                    cetkaPodKrov.Color = Color.FromArgb(red, green, blue);
                    g.FillRectangle(cetkaPodKrov, (int)screenX, (int)lineOff + y, rayWidth, 1);

                    ty += ty_step;
                }

                //-----CRTANJE PODA I KROVA-----
                for (y = (int)(lineOff + lineH); y < viewHeight; y++)
                {
                    dy = y - (viewHeight / 2.0);
                    deg = ra;
                    raFix = Math.Cos(playerA - ra);

                    tx = playerX / 2 + Math.Cos(deg) * (viewHeight / 2.0) * 32 / dy / raFix;
                    ty = playerY / 2 + Math.Sin(deg) * (viewHeight / 2.0) * 32 / dy / raFix;

                    int tx_idx = (int)tx & 31;
                    int ty_idx = (int)ty & 31;

                    // --- CRTANJE PODA ---
                    int floorType = mapF[(int)(ty / 32.0) * mapX + (int)(tx / 32.0)];
                    int floorOffset = (floorType - 1) * 3072;
                    if (floorOffset < 0) floorOffset = 0;

                    int floorPixel = floorOffset + (ty_idx * 32 + tx_idx) * 3;

                    int fR = Textures.AllTextures[floorPixel + 0];
                    int fG = Textures.AllTextures[floorPixel + 1];
                    int fB = Textures.AllTextures[floorPixel + 2];

                    cetkaPodKrov.Color = Color.FromArgb(fR, fG, fB);
                    g.FillRectangle(cetkaPodKrov, (int)screenX, y, rayWidth, 1);


                    // --- CRTANJE KROVA ---
                    int ceilingType = mapC[(int)(ty / 32.0) * mapX + (int)(tx / 32.0)];
                    int ceilingOffset = (ceilingType - 1) * 3072;
                    if (ceilingOffset < 0) ceilingOffset = 0;

                    int ceilPixel = ceilingOffset + (ty_idx * 32 + tx_idx) * 3;

                    int cR = Textures.AllTextures[ceilPixel + 0];
                    int cG = Textures.AllTextures[ceilPixel + 1];
                    int cB = Textures.AllTextures[ceilPixel + 2];

                    cetkaPodKrov.Color = Color.FromArgb(cR, cG, cB);
                    g.FillRectangle(cetkaPodKrov, (int)screenX, viewHeight - y, rayWidth, 1);
                }

                depth[r] = disT;

                ra += fov / numRays;
                if (ra < 0) ra += 2 * Math.PI;
                if (ra > 2 * Math.PI) ra -= 2 * Math.PI;
            }

            // --- CRTANJE SPRITE-OVA ---

            double sx = sp[0].x - playerX;
            double sy = sp[0].y - playerY;
            double dist = Math.Sqrt(sx * sx + sy * sy);

            double spriteAngle = Math.Atan2(sy, sx) - playerA;
            while (spriteAngle <= -Math.PI) spriteAngle += 2 * Math.PI;
            while (spriteAngle > Math.PI) spriteAngle -= 2 * Math.PI;

            if (dist > 10 && Math.Abs(spriteAngle) < (fov / 1.1))
            {
                double baseSize = Math.Abs((mapS * viewHeight) / dist);

                double spriteWidth = baseSize * sp[0].w;
                double spriteHeight = baseSize * sp[0].h;

                double zOffset = (sp[0].z * viewHeight) / dist;

                double screenX_pos = (spriteAngle / fov) * viewWidth + (viewWidth / 2.0);
                double screenY_pos = (viewHeight / 2.0) + zOffset;

                int startX = (int)(screenX_pos - spriteWidth / 2);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                for (int col = 0; col < 32; col++)
                {
                    double xStartPrecise = startX + (col * spriteWidth / 32.0);
                    double xEndPrecise = startX + ((col + 1) * spriteWidth / 32.0);

                    int x1 = (int)Math.Floor(xStartPrecise);
                    int x2 = (int)Math.Ceiling(xEndPrecise);
                    int currentColumnWidth = (x2 - x1) + 1;

                    if (x2 >= 0 && x1 < viewWidth)
                    {
                        int checkX = Math.Max(0, Math.Min(viewWidth - 1, x1));
                        int r = (checkX * numRays) / viewWidth;

                        if (dist < depth[r])
                        {
                            int spriteOffset = sp[0].map * 3072;

                            using (Bitmap tempCol = new Bitmap(1, 32))
                            {
                                for (int py = 0; py < 32; py++)
                                {
                                    int pIdx = spriteOffset + (py * 32 + col) * 3;
                                    Color c = Color.FromArgb(
                                        Textures.SpriteTextures[pIdx],
                                        Textures.SpriteTextures[pIdx + 1],
                                        Textures.SpriteTextures[pIdx + 2]);

                                    if (c.R == 255 && c.G == 0 && c.B == 255)
                                        tempCol.SetPixel(0, py, Color.Transparent);
                                    else
                                        tempCol.SetPixel(0, py, c);
                                }

                                g.DrawImage(tempCol, x1, (int)(screenY_pos - spriteHeight / 2), currentColumnWidth, (int)spriteHeight);
                            }
                        }
                    }
                }
            }

            // --- CRTANJE PISTOLJA  ---
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            int gunWidth = (int)(viewHeight * 0.35);
            int gunHeight = (int)(viewHeight * 0.35);

            int gunX = (viewWidth / 2) - (gunWidth / 2);

            int gunY = viewHeight - gunHeight + 20;

            if (goUp || goDown || goLeft || goRight)
            {
                double bobSpeed = currentTime * 10;
                gunX += (int)(Math.Sin(bobSpeed) * 5);
                gunY += (int)(Math.Abs(Math.Cos(bobSpeed) * 5));
            }

            g.DrawImage(gunTexture, gunX, gunY, gunWidth, gunHeight);

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
                xo = 0; if (playerdX < 0) { xo = -20; } else { xo = 20; }
                yo = 0; if (playerdY < 0) { yo = -20; } else { yo = 20; }

                ipx = (int)(playerX / 64.0);
                ipx_add_xo = (int)((playerX + xo) / 64.0);
                ipx_sub_xo = (int)((playerX - xo) / 64.0);
                ipy = (int)(playerY / 64.0);
                ipy_add_yo = (int)((playerY + yo) / 64.0);
                ipy_sub_yo = (int)((playerY - yo) / 64.0);


                if (goUp)
                {
                    bool canMoveX = mapW[ipy * mapX + ipx_add_xo] == 0;
                    bool canMoveY = mapW[ipy_add_yo * mapX + ipx] == 0;

                    for (int i = 0; i < sp.Length; i++)
                    {
                        if (sp[i].state == 1)
                        {
                            double nx = playerX + playerdX * speed * dt;
                            double ny = playerY + playerdY * speed * dt;

                            if (distance(nx, playerY, sp[i].x, sp[i].y, 0) < 20) canMoveX = false;
                            if (distance(playerX, ny, sp[i].x, sp[i].y, 0) < 20) canMoveY = false;
                        }
                    }

                    if (canMoveX) { playerX += playerdX * speed * dt; }
                    if (canMoveY) { playerY += playerdY * speed * dt; }
                }

                if (goDown)
                {
                    bool canMoveX = mapW[ipy * mapX + ipx_sub_xo] == 0;
                    bool canMoveY = mapW[ipy_sub_yo * mapX + ipx] == 0;

                    for (int i = 0; i < sp.Length; i++)
                    {
                        if (sp[i].state == 1)
                        {
                            double nx = playerX - playerdX * speed * dt;
                            double ny = playerY - playerdY * speed * dt;

                            if (distance(nx, playerY, sp[i].x, sp[i].y, 0) < 20) canMoveX = false;
                            if (distance(playerX, ny, sp[i].x, sp[i].y, 0) < 20) canMoveY = false;
                        }
                    }

                    if (canMoveX) { playerX -= playerdX * speed * dt; }
                    if (canMoveY) { playerY -= playerdY * speed * dt; }
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