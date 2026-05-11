using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShooterGame
{
    public static class GameState
    {
        public static int PlayerHealth = 100;
        public static int Score = 0;
        public static float ScreenFlashTimer = 0f;
        public static bool IsGameOver = false;
        public static double ShootTimer = 0;

        public static void Reset()
        {
            PlayerHealth = 100;
            Score = 0;
            ScreenFlashTimer = 0;
            IsGameOver = false;
            ShootTimer = 0;
        }
    }
}