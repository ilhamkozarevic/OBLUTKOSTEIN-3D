﻿﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics; // Potrebno za Stopwatch
using System.Media;       // Potrebno za SoundPlayer

namespace oblutkostein_3D
{
    public partial class Form1 : Form
    {

        int[] mapW = 
        {
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 7,
            7, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 7,
            7, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 7,
            7, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 7,
            7, 0, 0, 2, 0, 7, 0, 0, 0, 0, 0, 0, 7,
            7, 7, 2, 7, 0, 0, 0, 0, 0, 0, 0, 0, 7,
            7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7,
            7, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 7,
            7, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 7,
            7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7,
            7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
        };

        int[] mapF = 
        {
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2,
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2,
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
        };

        int[] mapC = 
        {
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        };

        int mapX = 13, mapY = 13, mapS = 64;
        int xo, yo; // X i Y offset - dimenzije zidova

        double playerX = 150, playerY = 150;
        int centerX, centerY;

        bool goUp, goDown, goLeft, goRight;

        double rotationSpeed = 4.0;
        double speed = 120.0;

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
        int numRays = 256;
        double fov = 60 * (Math.PI / 180.0);
        double screenX;

        public struct sprite
        {
            public int type;     //key, enemy
            public int state;    //on, off
            public int map;      //texture to show
            public int x, y, z;  //position
            public double w, h;
            public int health;
            public double hitTimer;
            public double speed;
            public double damageTimer;
        }

        double enemyAttackRange = 45.0;

        sprite[] sp = new sprite[4];

        double[] depth;

        Bitmap gunTexture;

        public struct Bullet
        {
            public double x, y;
            public double angle;
            public double speed;
            public double distanceTraveled;
            public bool active;
        }

        List<Bullet> bullets = new List<Bullet>();

        SoundPlayer shootSound;
        double shootTimer = 0;

        SoundPlayer walkSound;
        bool isWalkingSoundPlaying = false;
        double walkTimer = 0;

        SoundPlayer enemyHitSound;

        // A* pathfinding za enemy-je
        List<Point>[] enemyPaths = new List<Point>[4];
        double[] pathUpdateTimers = new double[4];

        public struct AStarNode
        {
            public int x, y;
            public double g, f;
            public int parentX, parentY;

            public AStarNode(int x, int y, double g, double h, int px, int py)
            {
                this.x = x; this.y = y;
                this.g = g; this.f = g + h;
                this.parentX = px; this.parentY = py;
            }
        }

        int playerHealth = 100;
        double screenFlashTimer = 0;

        int score = 0;
        int currentRound = 0;
        int enemiesToSpawn = 5;
        float enemyBaseSpeed = 100;

        bool isGameOver = false;

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

            sp[0].type = 1; sp[0].state = 1; sp[0].map = 0; sp[0].x = 150; sp[0].y = 150; sp[0].z = 5; sp[0].w = 1; sp[0].h = 1; sp[0].health = 5;//sprite 1

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

            shootSound = new SoundPlayer(@"SoundEffects\shoot.wav");
            walkSound = new SoundPlayer(@"SoundEffects\walk.wav");
            enemyHitSound = new SoundPlayer(@"SoundEffects\enemyHit.wav");

            NextRound();
        }

        private double Heuristic(int x1, int y1, int x2, int y2)
        {
            double dx = x2 - x1, dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private List<Point> FindPath(int startX, int startY, int endX, int endY)
        {
            if (startX == endX && startY == endY) return new List<Point>();

            var openList = new List<AStarNode>();
            var visited = new Dictionary<int, AStarNode>();

            openList.Add(new AStarNode(startX, startY, 0,
                Heuristic(startX, startY, endX, endY), -1, -1));

            int[] dx = { 0, 0, 1, -1, 1, 1, -1, -1 };
            int[] dy = { 1, -1, 0, 0, 1, -1, 1, -1 };
            double[] costs = { 1, 1, 1, 1, 1.414, 1.414, 1.414, 1.414 };

            while (openList.Count > 0)
            {
                int bestIdx = 0;
                for (int i = 1; i < openList.Count; i++)
                    if (openList[i].f < openList[bestIdx].f) bestIdx = i;

                var curr = openList[bestIdx];
                openList.RemoveAt(bestIdx);

                int key = curr.y * mapX + curr.x;
                if (visited.ContainsKey(key)) continue;
                visited[key] = curr;

                if (curr.x == endX && curr.y == endY)
                {
                    var path = new List<Point>();
                    int cx = curr.x, cy = curr.y;
                    while (!(cx == startX && cy == startY))
                    {
                        path.Add(new Point(cx, cy));
                        var node = visited[cy * mapX + cx];
                        int px = node.parentX, py = node.parentY;
                        cx = px; cy = py;
                    }
                    path.Reverse();
                    return path;
                }

                for (int d = 0; d < 8; d++)
                {
                    int nx = curr.x + dx[d];
                    int ny = curr.y + dy[d];

                    if (nx < 0 || nx >= mapX || ny < 0 || ny >= mapY) continue;
                    if (mapW[ny * mapX + nx] > 0) continue;

                    if (d >= 4 && (mapW[curr.y * mapX + nx] > 0 ||
                                   mapW[ny * mapX + curr.x] > 0)) continue;

                    int nkey = ny * mapX + nx;
                    if (visited.ContainsKey(nkey)) continue;

                    double g = curr.g + costs[d];
                    openList.Add(new AStarNode(nx, ny, g,
                        Heuristic(nx, ny, endX, endY), curr.x, curr.y));
                }
            }

            return null; // Nema puta
        }

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

            g.FillRectangle(Brushes.DarkSlateGray, 0, 0, ClientRectangle.Width, ClientRectangle.Height / 2);
            g.FillRectangle(Brushes.DimGray, 0, ClientRectangle.Height / 2, ClientRectangle.Width, ClientRectangle.Height);
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
                    dof = 20;
                }
                while (dof < 20)
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
                        dof = 20;

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
                    dof = 20;
                }
                while (dof < 20)
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
                        dof = 20;
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
                /*
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
                */
                depth[r] = disT;

                ra += fov / numRays;
                if (ra < 0) ra += 2 * Math.PI;
                if (ra > 2 * Math.PI) ra -= 2 * Math.PI;
            }

            // --- CRTANJE SPRITE-OVA ---
            for (int i = 0; i < sp.Length; i++)
            {
                // Preskoči ako neprijatelj ne postoji ili je mrtav
                if (sp[i].state == 0) continue;

                double sx = sp[i].x - playerX;
                double sy = sp[i].y - playerY;
                double dist = Math.Sqrt(sx * sx + sy * sy);

                double spriteAngle = Math.Atan2(sy, sx) - playerA;
                while (spriteAngle <= -Math.PI) spriteAngle += 2 * Math.PI;
                while (spriteAngle > Math.PI) spriteAngle -= 2 * Math.PI;

                // Smanjio sam distancu sa 10 na 1 da ih vidiš i kad su blizu
                if (dist > 1 && Math.Abs(spriteAngle) < (fov / 1.1))
                {
                    // VAŽNO: Koristi 64.0 umjesto mapS ako su ti neprijatelji mali!
                    double baseSize = Math.Abs((64.0 * viewHeight) / dist);

                    double spriteWidth = baseSize * sp[i].w;
                    double spriteHeight = baseSize * sp[i].h;

                    double zOffset = (sp[i].z * viewHeight) / dist;

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
                                int spriteOffset = sp[i].map * 3072;

                                using (Bitmap tempCol = new Bitmap(1, 32))
                                {
                                    for (int py = 0; py < 32; py++)
                                    {
                                        int pIdx = spriteOffset + (py * 32 + col) * 3;
                                        int rOriginal = Textures.SpriteTextures[pIdx];
                                        int gOriginal = Textures.SpriteTextures[pIdx + 1];
                                        int bOriginal = Textures.SpriteTextures[pIdx + 2];

                                        if (rOriginal == 255 && gOriginal == 0 && bOriginal == 255)
                                        {
                                            tempCol.SetPixel(0, py, Color.Transparent);
                                        }
                                        else
                                        {
                                            Color finalColor;
                                            if (sp[i].hitTimer > 0)
                                            {
                                                int rFlash = Math.Min(rOriginal + 100, 255);
                                                int gFlash = (int)(gOriginal * 0.4);
                                                int bFlash = (int)(bOriginal * 0.4);
                                                finalColor = Color.FromArgb(rFlash, gFlash, bFlash);
                                            }
                                            else
                                            {
                                                finalColor = Color.FromArgb(rOriginal, gOriginal, bOriginal);
                                            }
                                            tempCol.SetPixel(0, py, finalColor);
                                        }
                                    }
                                    g.DrawImage(tempCol, x1, (int)(screenY_pos - spriteHeight / 2), currentColumnWidth, (int)spriteHeight);
                                }
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

            int currentFrameOffset = (shootTimer > 0) ? 3072 : 0;

            using (Bitmap currentGunFrame = new Bitmap(32, 32))
            {
                for (int py = 0; py < 32; py++)
                {
                    for (int px = 0; px < 32; px++)
                    {
                        int pIdx = currentFrameOffset + (py * 32 + px) * 3;

                        Color c = Color.FromArgb(
                            Textures.gunTexture[pIdx],
                            Textures.gunTexture[pIdx + 1],
                            Textures.gunTexture[pIdx + 2]
                        );

                        if (c.R == 255 && c.G == 0 && c.B == 255)
                            currentGunFrame.SetPixel(px, py, Color.Transparent);
                        else
                            currentGunFrame.SetPixel(px, py, c);
                    }
                }

                
                g.DrawImage(currentGunFrame, gunX, gunY, gunWidth, gunHeight);
            }


            // --- CRTANJE METAKA ---
            foreach (var b in bullets)
            {
                double bX = b.x - playerX;
                double bY = b.y - playerY;
                double bulletDist = Math.Sqrt(bX * bX + bY * bY);

                double bulletRelativeAngle = Math.Atan2(bY, bX) - playerA;
                while (bulletRelativeAngle <= -Math.PI) bulletRelativeAngle += 2 * Math.PI;
                while (bulletRelativeAngle > Math.PI) bulletRelativeAngle -= 2 * Math.PI;

                double fixDistance = bulletDist * Math.Cos(bulletRelativeAngle);
                if (fixDistance < 0.1) continue; // Preskoči ako je preblizu ili iza

                if (Math.Abs(bulletRelativeAngle) < (fov / 2.0))
                {
                    double bulletSize = (10.0 * viewHeight) / fixDistance;
                    if (bulletSize < 2) bulletSize = 2;
                    if (bulletSize > 50) bulletSize = 40;

                    int screenX = (int)((bulletRelativeAngle / fov) * viewWidth + (viewWidth / 2.0));
                    int screenY = viewHeight / 2;

                    int bW = (int)bulletSize;
                    int bH = (int)(bulletSize * 0.5);

                    g.FillEllipse(Brushes.OrangeRed, screenX - bW, screenY - bH, bW * 2, bH * 2);

                    g.FillEllipse(Brushes.Yellow, screenX - bW / 2, screenY - bH / 2, bW, bH);

                    g.FillEllipse(Brushes.White, screenX - bW / 4, screenY - bH / 4, bW / 2, bH / 2);
                }
            }

            // CRTANJE HIT EFFEKTA
            if (screenFlashTimer > 0)
            {
                using (SolidBrush redFlash = new SolidBrush(Color.FromArgb(100, 255, 0, 0)))
                {
                    g.FillRectangle(redFlash, 0, 0, viewWidth, viewHeight);
                }
            }

            g.DrawString("HP: " + playerHealth, new Font("Arial", 16), Brushes.White, 10, 10);

            g.DrawString("SCORE: " + score, new Font("Arial", 16, FontStyle.Bold), Brushes.Gold, 10, 40);
            g.DrawString("ROUND: " + currentRound, new Font("Arial", 16, FontStyle.Bold), Brushes.LightBlue, 10, 70);

            // UMRO SI
            if (isGameOver)
            {
                using (SolidBrush overlay = new SolidBrush(Color.FromArgb(200, 0, 0, 0)))
                {
                    g.FillRectangle(overlay, 0, 0, viewWidth, viewHeight);
                }

                using (Font bigFont = new Font("Impact", 48, FontStyle.Bold))
                {
                    string text = "YOU DIED";
                    Size textSize = TextRenderer.MeasureText(text, bigFont);
                    g.DrawString(text, bigFont, Brushes.Red, (viewWidth - textSize.Width) / 2, (viewHeight / 2) - 50);
                }

                using (Font smallFont = new Font("Arial", 14, FontStyle.Regular))
                {
                    string scoreText = "Final Score: " + score;
                    string restartText = "Press 'R' to Restart";

                    g.DrawString(scoreText, smallFont, Brushes.White, (viewWidth - 150) / 2, (viewHeight / 2) + 40);
                    g.DrawString(restartText, smallFont, Brushes.Gold, (viewWidth - 170) / 2, (viewHeight / 2) + 70);
                }
            }
        }

        void NextRound()
        {
            currentRound++;
            enemiesToSpawn = 1 + currentRound;
            float currentSpeed = enemyBaseSpeed + (currentRound * 10f); 

            sp = new sprite[enemiesToSpawn];
            enemyPaths = new List<Point>[enemiesToSpawn];
            pathUpdateTimers = new double[enemiesToSpawn];

            int[,] spawnPoints = {
            { 1, 7 }, { 2, 7 }, { 1, 10 }, { 2, 10 },
            { 4, 1 }, { 5, 1 }, { 6, 1 },  { 7, 1  },
            { 8, 1 }, { 4, 7 }, { 5, 7 },  { 6, 7  }
             };

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                sp[i] = new sprite();
                sp[i].type = 1;
                sp[i].state = 1;
                sp[i].health = 3; 
                sp[i].map = 0;
                sp[i].w = 1.0;
                sp[i].h = 1.0;
                sp[i].z = 10;
                sp[i].speed = currentSpeed;

                int idx = i % spawnPoints.GetLength(0);
                sp[i].x = spawnPoints[idx, 0] * 64 + 32;
                sp[i].y = spawnPoints[idx, 1] * 64 + 32;
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
            if (isGameOver) return; 

            if (playerHealth <= 0)
            {
                isGameOver = true;
                walkSound.Stop();
                return;
            }

            isWalkingSoundPlaying = false;

            if (shootTimer > 0)
            {
                shootTimer -= dt;
            }

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

                    if (canMoveX) { playerX += playerdX * speed * dt; isWalkingSoundPlaying = true; }
                    if (canMoveY) { playerY += playerdY * speed * dt; isWalkingSoundPlaying = true; }
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

                    if (canMoveX) { playerX -= playerdX * speed * dt; isWalkingSoundPlaying = true; }
                    if (canMoveY) { playerY -= playerdY * speed * dt; isWalkingSoundPlaying = true; }
                }
            }

            if (isWalkingSoundPlaying && shootTimer <= 0) 
            {
                walkTimer -= dt;
                if (walkTimer <= 0)
                {
                    walkSound.Play();
                    walkTimer = 0.4;
                }
            }
            else if (!isWalkingSoundPlaying && shootTimer <= 0)
            {
                walkTimer = 0;
                walkSound.Stop();
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

            UpdateBullets(dt);
            UpdateEnemies(dt);

            bool roundOver = true;

            if (sp == null || sp.Length == 0)
            {
                roundOver = true;
            }
            else
            {
                for (int i = 0; i < sp.Length; i++)
                {
                    if (sp[i].state == 1)
                    {
                        roundOver = false;
                        break;
                    }
                }
            }

            if (roundOver)
            {
                NextRound();
            }
        }

        private void UpdateBullets(double dt)
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                Bullet b = bullets[i];

                double nextX = b.x + Math.Cos(b.angle) * b.speed * dt;
                double nextY = b.y + Math.Sin(b.angle) * b.speed * dt;
                double distStep = b.speed * dt;

                int mx = (int)(nextX / 64);
                int my = (int)(nextY / 64);
                int mp = my * mapX + mx;

                if (mp < 0 || mp >= mapX * mapY || mapW[mp] > 0 || b.distanceTraveled > 2000)
                {
                    bullets.RemoveAt(i);
                    continue;
                }

                bool hitEnemy = false;
                for (int s = 0; s < sp.Length; s++)
                {
                    if (sp[s].state == 1 && sp[s].type == 1)
                    {
                        double distToEnemy = distance(nextX, nextY, sp[s].x, sp[s].y, 0);

                        if (distToEnemy < 30)
                        {
                            enemyHitSound.Play();
                            sp[s].health -= 1;
                            sp[s].hitTimer = 0.15;
                            hitEnemy = true;

                            double pushX = Math.Cos(b.angle) * 15;
                            double pushY = Math.Sin(b.angle) * 15;

                            if (mapW[(int)(sp[s].y / 64) * mapX + (int)((sp[s].x + pushX) / 64)] == 0) sp[s].x += (int)pushX;
                            if (mapW[(int)((sp[s].y + pushY) / 64) * mapX + (int)(sp[s].x / 64)] == 0) sp[s].y += (int)pushY;

                            if (sp[s].health <= 0)
                            {
                                sp[s].state = 0;
                                score += 20;
                            }
                            break;
                        }
                    }
                }

                if (hitEnemy)
                {
                    bullets.RemoveAt(i);
                    continue;
                }

                b.x = nextX;
                b.y = nextY;
                b.distanceTraveled += distStep;
                bullets[i] = b;
            }
        }

        private void UpdateEnemies(double dt)
        {
            int stopDistance = 35;

            if (screenFlashTimer > 0) screenFlashTimer -= dt;

            for (int i = 0; i < sp.Length; i++)
            {
                if (sp[i].state != 1 || sp[i].type != 1) continue;

                double dx = sp[i].x - playerX;
                double dy = sp[i].y - playerY;
                double distToPlayer = Math.Sqrt(dx * dx + dy * dy);

                if (distToPlayer < enemyAttackRange && sp[i].damageTimer <= 0)
                {
                    playerHealth -= 2;
                    sp[i].damageTimer = 1.0; 
                    screenFlashTimer = 0.2;
                    if (playerHealth <= 0) playerHealth = 0;
                }

                if (sp[i].damageTimer > 0) sp[i].damageTimer -= dt;

                if (sp[i].hitTimer > 0)
                {
                    sp[i].hitTimer -= dt;
                }

                pathUpdateTimers[i] -= dt;
                if (pathUpdateTimers[i] <= 0)
                {
                    pathUpdateTimers[i] = 0.3;
                    int ex = (int)(sp[i].x / 64);
                    int ey = (int)(sp[i].y / 64);
                    int px = (int)(playerX / 64);
                    int py = (int)(playerY / 64);
                    enemyPaths[i] = FindPath(ex, ey, px, py);
                }

                double moveX = 0, moveY = 0;

                if (enemyPaths[i] != null && enemyPaths[i].Count > 0)
                {
                    Point nextCell = enemyPaths[i][0];
                    double targetX = nextCell.X * 64 + 32;
                    double targetY = nextCell.Y * 64 + 32;

                    double wdx = targetX - sp[i].x;
                    double wdy = targetY - sp[i].y;
                    double wdist = Math.Sqrt(wdx * wdx + wdy * wdy);

                    if (wdist < 10 && enemyPaths[i].Count > 1)
                    {
                        enemyPaths[i].RemoveAt(0);
                        nextCell = enemyPaths[i][0];
                        targetX = nextCell.X * 64 + 32;
                        targetY = nextCell.Y * 64 + 32;
                        wdx = targetX - sp[i].x;
                        wdy = targetY - sp[i].y;
                        wdist = Math.Sqrt(wdx * wdx + wdy * wdy);
                    }

                    if (wdist > 0)
                    {
                        moveX = (wdx / wdist) * sp[i].speed * dt;
                        moveY = (wdy / wdist) * sp[i].speed * dt;
                    }
                }

                if (distToPlayer > enemyAttackRange)
                {
                    if (moveX != 0 || moveY != 0)
                    {
                        int newX = sp[i].x + (int)moveX;
                        int newY = sp[i].y + (int)moveY;

                        int checkX = (moveX > 0) ? newX + stopDistance : newX - stopDistance;
                        if (mapW[(sp[i].y / 64) * mapX + (checkX / 64)] == 0)
                        {
                            sp[i].x = newX;
                        }

                        int checkY = (moveY > 0) ? newY + stopDistance : newY - stopDistance;
                        if (mapW[(checkY / 64) * mapX + (sp[i].x / 64)] == 0)
                        {
                            sp[i].y = newY;
                        }
                    }
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
                if (mapW[ipy_add_yo * mapX + ipx_add_xo] == 2) { mapW[ipy_add_yo * mapX + ipx_add_xo] = 0; }
            }

            if (e.KeyCode == Keys.Q)
            {
                if (shootTimer <= 0)
                {
                    Bullet newBullet = new Bullet();
                    newBullet.active = true;
                    newBullet.angle = playerA;
                    newBullet.speed = 1000.0;

                    double offset = 25.0;
                    newBullet.x = playerX + Math.Cos(playerA) * offset;
                    newBullet.y = playerY + Math.Sin(playerA) * offset;

                    newBullet.distanceTraveled = 0;
                    bullets.Add(newBullet);

                    shootTimer = 0.2;

                    shootSound.Play();
                }
            }

            if (e.KeyCode == Keys.R && isGameOver)
            {
                RestartGame();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) goUp = false;
            if (e.KeyCode == Keys.S) goDown = false;
            if (e.KeyCode == Keys.A) goLeft = false;
            if (e.KeyCode == Keys.D) goRight = false;
        }

        void RestartGame()
        {
            playerHealth = 100;
            score = 0;
            currentRound = 0;
            isGameOver = false;

            playerX = 150;
            playerY = 150;

            bullets.Clear();
            NextRound(); 
        }
    }
}