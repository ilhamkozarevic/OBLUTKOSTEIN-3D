using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace oblutkostein_3D
{
    public partial class Form1 : Form
    {
        SolidBrush cetkaMiniMap = new SolidBrush(Color.Yellow);
        Pen olovkaDirection = new Pen(Color.Orange);
        Pen olovkaZid = new Pen(Color.Red, 8);

        double playerX = 300, playerY = 300;
        double speed = 5.0;

        // Igrac delta X, igrac delta Y (X i Y koraci igraca za odredjeni ugao), igrac Ugao (ugao u radijanima pod kojim igrac gleda)
        double playerdX, playerdY, playerA = 0.0;

        //Ray casting
        //X i Y koordinate na mapi, pozicija na mapi (map index), broj koraka koje ray pravi dok ne udari u zid
        int mx, my, mp, dof;
        //Trenutna X i Y pozicija vrha ray-a, ugao pod kojim ray putuje, offset/korak za koji se ray pomjera do sljedece minimap linije
        //te finalna udaljenost do najblizeg zida
        double rx, ry, ra, xoff, yoff, disT;

        bool goUp, goDown, goLeft, goRight;

        int mapX = 8, mapY = 8, mapS = 64;

        int[] map = 
        {
            1, 1, 1, 1, 1, 1, 1, 1,
            1, 0, 1, 0, 0, 0, 0, 1,
            1, 0, 1, 0, 0, 0, 0, 1,
            1, 0, 1, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 0, 1, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 1,
            1, 1, 1, 1, 1, 1, 1, 1,
        };

        public Form1()
        {
            InitializeComponent();

            // Sprečava treperenje tako što prvo iscrta sve u memoriji, pa onda prikaže gotovu sliku (bez crtanja jednog po jednog elementa)
            this.DoubleBuffered = true;

            //Client size ne racuna title forme sto nam odgovara, za razliku od Size (512 + 1 zbog praznog mjesta na dnu minimape)
            this.ClientSize = new Size(1024, 513);

            Timer gameTimer = new Timer();
            gameTimer.Interval = 20;
            gameTimer.Enabled = true;
            gameTimer.Tick += new EventHandler(GameTimer_Tick);
            gameTimer.Start();

            //Prvo racunanje deltaX i deltaY za pocetni ugao
            playerdX = Math.Cos(playerA) * speed;
            playerdY = Math.Sin(playerA) * speed;
        }

        private double distance(double ax, double ay, double bx, double by, double ang)
        {
            return (Math.Sqrt((bx - ax) * (bx - ax) + (by - ay) * (by - ay)));
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int xo, yo; // X i Y offset - dimenzije zidova

            //Nacrtaj minimapu
            for (int y = 0; y < mapY; y++)
            {
                for (int x = 0; x < mapX; x++)
                {
                    //Zbog memorijske produktivnosti umjesto 2D niza koristili smo 1D niz i pristupali elementima kao da je 2D niz preko formule: map[y * mapX + x]
                    if (map[y * mapX + x] == 1) { cetkaMiniMap.Color = Color.White; } else { cetkaMiniMap.Color = Color.Black; }
                    xo = x * mapS;
                    yo = y * mapS;

                    g.FillRectangle(cetkaMiniMap, xo + 1, yo + 1, mapS - 1, mapS - 1);
                }
            }

            //Nacrtaj igraca na minimapi
            cetkaMiniMap.Color = Color.Yellow;
            g.FillEllipse(cetkaMiniMap, (int)playerX, (int)playerY, 8, 8);

            //Nacrtaj liniju direkcije igraca na minimapi
            //centerX i centerY predstavljaju koordinate sredine naseg igraca na minimapi (odakle krece linija direkcije)
            int centerX = (int)playerX + 4;
            int centerY = (int)playerY + 4;
            // Mnozimo deltaX i deltaY sa 5 da dobijemo zeljenu duzinu linije direkcije
            g.DrawLine(olovkaDirection, centerX, centerY, (int)(centerX + playerdX * 5), (int)(centerY + playerdY * 5));

            //Nacrtaj ray
            ra = playerA - 0.0174533 * 30;
            if (ra < 0) ra += 2 * Math.PI;
            if (ra > 2 * Math.PI) ra -= 2 * Math.PI;

            for (int r = 0; r < 60; r++)
            {
                //Provjeri horizontalne linije
                dof = 0;
                //Udaljenost od igraca do tacke gdje ray udara prvi horizontalni zid (na pocetku je 1000000 jer jos ne znamo gdje je zid)
                //X i Y koordinate gdje je ray udario horizontalni zid
                double disH = 1000000, hx = playerX, hy = playerY;

                double aTan = -1.0 / Math.Tan(ra);

                //Igrac gleda gore
                if (ra > Math.PI)
                {
                    //Pomocu trigonometrijske funkcije tangens pronalazimo koordinate najblize horizontalne linije, te X i Y korake do sljedece horizontalne linije
                    ry = ((int)(playerY / 64) * 64) - 0.0001;
                    rx = (playerY - ry) * aTan + playerX;
                    yoff = -64;
                    xoff = -yoff * aTan;
                }
                //Igrac gleda dole
                if (ra < Math.PI)
                {
                    //Pomocu trigonometrijske funkcije tangens pronalazimo koordinate najblize horizontalne linije, te X i Y korake do sljedece horizontalne linije
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
                    mx = (int)(rx / 64); // Dijelimo sa 64 da dobijemo kolonu 
                    my = (int)(ry / 64); // Dijelimo sa 64 da dobijemo red
                    mp = my * mapX + mx; // Pretvaramo 2D (red, kolona) u indeks za 1D niz map[]

                    if (mp > 0 && mp < mapX * mapY && map[mp] == 1) //Provjera da li je pogodjen horizontalni zid
                    {
                        //Pohranjujemo podatke o X i Y poziciji gdje je horizontalni zid pogodjen,
                        //te pomocu pitagorine teoreme izracunavamo udaljenost ray-a od igraca do tog pogodjenog horizontalnog zida
                        hx = rx;
                        hy = ry;
                        disH = distance(playerX, playerY, hx, hy, ra);
                        dof = 8; // Ray pogodio horizontalni zid - prekini provjeru za taj ray

                    }
                    else
                    {
                        // Sljedeca horizontalna linija...
                        rx += xoff;
                        ry += yoff;
                        dof++;
                    }
                }

                //Provjeri vertikalne linije
                dof = 0;
                //Udaljenost od igraca do tacke gdje ray udara prvi vertikalni zid (na pocetku je 1000000 jer jos ne znamo gdje je zid)
                //X i Y koordinate gdje je ray udario vertikalni zid
                double disV = 1000000, vx = playerX, vy = playerY;

                double nTan = -Math.Tan(ra);

                //Igrac gleda lijevo
                if (ra > Math.PI / 2 && ra < 3 * Math.PI / 2)
                {
                    //Pomocu trigonometrijske funkcije negativni tangens pronalazimo koordinate najblize vertikalne linije,
                    //te X i Y korake do sljedece vertikalne linije
                    rx = ((int)(playerX / 64) * 64) - 0.0001;
                    ry = (playerX - rx) * nTan + playerY;
                    xoff = -64;
                    yoff = -xoff * nTan;
                }
                //Igrac gleda desno
                if (ra < Math.PI / 2 || ra > 3 * Math.PI / 2)
                {
                    //Pomocu trigonometrijske funkcije negativni tangens pronalazimo koordinate najblize vertikalne linije,
                    //te X i Y korake do sljedece vertikalne linije
                    rx = ((int)(playerX / 64) * 64) +64;
                    ry = (playerX - rx) * nTan + playerY;
                    xoff = 64;
                    yoff = -xoff * nTan;
                }
                //Igrac gleda gore ili dolje
                if (ra == 0 || ra == Math.PI)
                {
                    rx = playerX;
                    ry = playerY;
                    dof = 8;
                }
                while (dof < 8)
                {
                    mx = (int)(rx / 64); // Dijelimo sa 64 da dobijemo kolonu 
                    my = (int)(ry / 64); // Dijelimo sa 64 da dobijemo red
                    mp = my * mapX + mx; // Pretvaramo 2D (red, kolona) u indeks za 1D niz map[]

                    if (mp > 0 && mp < mapX * mapY && map[mp] == 1) //Provjera da li je pogodjen vertikalni zid
                    {
                        //Pohranjujemo podatke o X i Y poziciji gdje je vertikalni zid pogodjen,
                        //te pomocu pitagorine teoreme izracunavamo udaljenost ray-a od igraca do tog pogodjenog vertikalnog zida
                        vx = rx;
                        vy = ry;
                        disV = distance(playerX, playerY, vx, vy, ra);
                        dof = 8; // Ray pogodio vertikalni zid - prekini provjeru za taj ray
                    }
                    else
                    {
                        // Sljedeca vertikalna linija...
                        rx += xoff;
                        ry += yoff;
                        dof++;
                    }
                }

                if (disV < disH) // Pogodjen prvo vertikalni zid
                {
                    rx = vx;
                    ry = vy;
                    disT = disV;
                    olovkaZid.Color = Color.FromArgb(230, 0, 0);
                }
                if (disH < disV) // Pogodjen prvo horizontalni zid
                {
                    rx = hx;
                    ry = hy;
                    disT = disH;
                    olovkaZid.Color = Color.FromArgb(178, 0, 0);
                }
                olovkaDirection.Color = Color.Red;
                g.DrawLine(olovkaDirection, (int)centerX, (int)centerY, (int)rx, (int)ry);
                
                //Nacrtaj 3D zidove

                // Popravljanje "fish-eye" efekta
                double ca = playerA - ra;
                if (ca < 0) ca += 2 * Math.PI;
                if (ca > 2 * Math.PI) ca -= 2 * Math.PI;
                disT = disT * Math.Cos(ca);

                double lineH = (mapS * 320) / disT;
                double lineOff = 160 - lineH / 2;
                if (lineH > 320) lineH = 320;
                g.DrawLine(olovkaZid, (int)(r * 8 + 530), (int)lineOff, (int)(r * 8 + 530), (int)(lineH + lineOff));

                ra += 0.0174533;
                if (ra < 0) ra += 2 * Math.PI;
                if (ra > 2 * Math.PI) ra -= 2 * Math.PI;
            }

        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (!(goUp && goDown))
            {
                if (goUp)
                {
                    playerX += playerdX;
                    playerY += playerdY;
                }
                if (goDown)
                {
                    playerX -= playerdX;
                    playerY -= playerdY;
                }
            }
            //Za kretanje igraca naprijed/nazad ne dodajemo vise fiksan korak zato sto igrac ne mora gledati pod uglom od 90 stepeni
            //Zato racunamo korake deltaX i deltaY koji se racunaju u zavisnosti od ugla pod kojim igrac gleda (u radijanima)
            if (!(goLeft && goRight))
            {
                if (goLeft)
                {
                    playerA -= 0.1;
                    if (playerA < 0) playerA += 2.0 * Math.PI;
                    playerdX = Math.Cos(playerA) * speed;
                    playerdY = Math.Sin(playerA) * speed;
                }
                
                if (goRight)
                {
                    playerA += 0.1;
                    if (playerA > 2.0 * Math.PI) playerA -= 2.0 * Math.PI;
                    playerdX = Math.Cos(playerA) * speed;
                    playerdY = Math.Sin(playerA) * speed;
                }
            }
            //Posto nam sada tipke A i D ne slize za kretanje nego za rotiranje rotacija se racuna pomocu ugla playerA koji se uvecava ili smanjuje u radijanima
            //u zavisnosti od tipke, te se vrijednosti koraka po osama (deltaX i deltaY) racunaju pomocu trigonometrijskih funkcija sinusa i kosinusa od ugla
            //pod kojim gleda igrac, te vrijednosti brzine rotacije

            // Proglasava trenutnu sliku starom i ponovo poziva dogadjaj Paint
            this.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) goUp = true;
            if (e.KeyCode == Keys.S) goDown = true;
            if (e.KeyCode == Keys.A) goLeft = true;
            if (e.KeyCode == Keys.D) goRight = true;
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
