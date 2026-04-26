using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShooterGame
{
    public partial class Level : UserControl
    {
        public byte[,] mapW;
        public byte[,] mapF;
        public byte[,] mapC;

        private Dictionary<string, ZoneData> zones = new Dictionary<string, ZoneData>()
        {
            {"room_grass_sky", new ZoneData(0, 2, 1)},
            {"wall_sponge", new ZoneData(3, 1, 1)}
        };

        public List<Rectangle> collisionRects;

        public Level()
        {
            InitializeComponent();

            this.ClientSize = new Size(800, 600);

            // Objasnjeno u GameForm
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Jos optimizacija kako ne bi trebali racunati da forma moze bit transparentna
            // (to je moguce u C#), da windows ne painta bijelo preko, da mi kontrolisemo ekran u potpunosti...
            // Sve ovo daje bolje performanse
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);

            collisionRects = new List<Rectangle>();

            mapW = new byte[40, 30];
            mapF = new byte[40, 30];
            mapC = new byte[40, 30];


            // set na neke default vrijednosti
            for (int i = 0; i < 40; i ++)
            {
                for (int j = 0; j < 30; j++)
                {
                    mapW[i, j] = 0;
                    mapF[i, j] = 2;
                    mapC[i, j] = 1;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            List<Label> walls = new List<Label>();
            collisionRects.Clear();

            foreach (Label wall in this.Controls)
            {
                if (wall.Tag.ToString().ToLower() == "wall")
                {
                    int startX = (int)(Math.Round(wall.Left / 20.0));
                    int endX = (int)(Math.Round(wall.Right / 20.0));
                    int startY = (int)(Math.Round(wall.Top / 20.0));
                    int endY = (int)(Math.Round(wall.Bottom / 20.0));

                    walls.Add(wall);
                    for (int i = startX; i < endX; i++)
                    {
                        for (int j = startY; j < endY; j++)
                        {
                            if (i >= 40 || j >= 30 || i < 0 || j < 0) continue;

                            mapW[i, j] = 1;
                            collisionRects.Add(new Rectangle(20 * i, 20 * j, 20, 20));
                        }
                    }
                    
                }
            }

            foreach (Label wall in walls)
            {
                wall.Hide();
                this.Controls.Remove(wall);
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
 	        base.OnPaint(e);

            e.Graphics.Clear(Color.Black);

            //DrawGrid(e.Graphics);
            if (this.DesignMode) return; // ako smo u designeru ne izvrsava se kod

            //DrawLevel(e.Graphics);
            //DrawPlayer(e.Graphics);
            Raycaster.DrawRays3D(e.Graphics);
            

            // DEBUG
            /*
            using (Font f = new Font(FontFamily.GenericMonospace, 18))
            {
                e.Graphics.DrawString(GameControls.currentMouseX + "\n" + GameControls.lastMouseX, f, Brushes.Green, 20, 20);
            }

            // TODO Draw Enemies etc.
            */
        }

        public void DrawLevel(Graphics g) 
        {
            using (SolidBrush s = new SolidBrush(Color.Orange))
            {
                foreach (Rectangle rect in collisionRects)
                {
                    g.FillRectangle(s, rect);
                }
            }
        }

        public void DrawPlayer(Graphics g)
        {
            using (SolidBrush s = new SolidBrush(Color.Blue))
            {
                g.FillEllipse(s, (int)Player.x - Player.width, (int)Player.y - Player.height, 2 * Player.width, 2 * Player.height);
                g.DrawRectangle(new Pen(Color.Red, 1), Player.hitbox);
                g.DrawLine(new Pen(Color.Yellow, 2), (int)Player.x, (int)Player.y, (int)Player.dirX, (int)Player.dirY);
            }
        }

        public void DrawGrid(Graphics g)
        {
            using (Pen p = new Pen(Color.DarkSlateGray, 1))
            {
                for (int i = 0; i <= ClientRectangle.Width; i += 20)
                {
                    g.DrawLine(p, i, 0, i, ClientRectangle.Height);
                }

                for (int i = 0; i <= ClientRectangle.Height; i += 20)
                {
                    g.DrawLine(p, 0, i, ClientRectangle.Width, i);
                }
            }
        }
    }
}
