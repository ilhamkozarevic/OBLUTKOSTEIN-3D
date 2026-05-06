using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ShooterGame
{
    public partial class GameForm : Form
    {
        // Za delta time
        private Stopwatch stopwatch;
        public static float deltaTime;
        private long lastTime;

        // Za levele
        public static Level currentLevel;

        public GameForm()
        {
            this.ClientSize = new Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            //FontLoader.LoadFont(Properties.Resources.PixelifySans_VariableFont_wght);

            // Ove dvije linije koda ukljucuju optimizovani double buffer,
            // sto znaci da se slika koju treba prikazati prvo spremi u buffer 
            // prije prikazivanja na ekran kako bi se sprijecilo flickeranje i treptanje ekrana,
            // jer je slika vec ucitana prije prikaza i spremna da se pokaze unaprijed
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.KeyPreview = true;
        }

        // IN THE BEGINNING
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Player.x = 400;
            Player.y = 300;
            LoadLevel(new Level1());

            stopwatch = Stopwatch.StartNew();
            Application.Idle += GameLoop;
            lastTime = stopwatch.ElapsedMilliseconds; // set last time to current time

            //GameControls.InitController();
        }

        // GAME LOOP
        private void GameLoop(object sender, EventArgs e)
        {
            deltaTime = (stopwatch.ElapsedMilliseconds - lastTime) / 1000.0f;
            lastTime = stopwatch.ElapsedMilliseconds;

            if (deltaTime > 0.08) deltaTime = 0.08f;

            Player.HandleMovement();
            Player.HandleRotation();

            currentLevel.Invalidate();
        }

        // CONTROLS / KEYS
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            e.SuppressKeyPress = true;

            GameControls.xAxis = 1.0f;
            GameControls.yAxis = 1.0f;
            if (e.KeyCode == GameControls.moveUp) Player.up = true;
            if (e.KeyCode == GameControls.moveDown) Player.down = true;
            if (e.KeyCode == GameControls.moveLeft) Player.left = true;
            if (e.KeyCode == GameControls.moveRight) Player.right = true;

            if (e.KeyCode == GameControls.camLeft) Player.camLeft = true;
            if (e.KeyCode == GameControls.camRight) Player.camRight = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.KeyCode == GameControls.moveUp) Player.up = false;
            if (e.KeyCode == GameControls.moveDown) Player.down = false;
            if (e.KeyCode == GameControls.moveLeft) Player.left = false;
            if (e.KeyCode == GameControls.moveRight) Player.right = false;

            if (e.KeyCode == GameControls.camLeft) Player.camLeft = false;
            if (e.KeyCode == GameControls.camRight) Player.camRight = false;

            if (e.KeyCode == GameControls.interactKey) Raycaster.InteractWith(currentLevel.mapW);
        }

        // LOAD NEW LEVEL
        private void LoadLevel(Level level)
        {
            // Skloni prosli level sa forme ukoliko postoji (nije null)
            if (currentLevel != null) this.Controls.Remove(currentLevel);

            currentLevel = level; // podesi novi trenutni level
            currentLevel.Dock = DockStyle.Fill; // postavi level da popuni ekran
            this.Controls.Add(currentLevel); // doda level na formu jer smo sklonili prosli

            currentLevel.Refresh();
        }
    }
}
