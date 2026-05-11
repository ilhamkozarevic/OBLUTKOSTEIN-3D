using System;
using System.Collections.Generic;

namespace ShooterGame
{
    public static class BulletManager
    {
        public static List<Bullet> bullets = new List<Bullet>();

        public static void Shoot()
        {
            if (GameState.ShootTimer > 0) return;

            // Pushing the spawn point out 12 units. 
            // This ensures the bullet starts outside the player's own 
            // collision box even if you are running forward.
            double spawnOffset = 12.0;
            double spawnX = Player.x + Math.Cos(Player.angle) * spawnOffset;
            double spawnY = Player.y + Math.Sin(Player.angle) * spawnOffset;

            bullets.Add(new Bullet
            {
                active = true,
                angle = Player.angle,
                speed = 1000.0,
                x = spawnX,
                y = spawnY,
                distanceTraveled = 0
            });

            GameState.ShootTimer = 0.2;
        }

        public static void Update(double dt)
        {
            float TILE_SIZE = Raycaster.TILE_SIZE;
            byte[,] map = GameForm.currentLevel.mapW;

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                Bullet b = bullets[i];

                double totalDist = b.speed * dt;
                double dirX = Math.Cos(b.angle);
                double dirY = Math.Sin(b.angle);

                int steps = (int)Math.Ceiling(totalDist / 4.0);
                double stepDist = totalDist / steps;
                bool destroyed = false;

                for (int s = 0; s < steps; s++)
                {
                    b.x += dirX * stepDist;
                    b.y += dirY * stepDist;
                    b.distanceTraveled += stepDist;

                    // --- WALL COLLISION ---
                    int mx = (int)(b.x / TILE_SIZE);
                    int my = (int)(b.y / TILE_SIZE);

                    if (mx < 0 || mx >= Raycaster.MAP_WIDTH || my < 0 || my >= Raycaster.MAP_HEIGHT)
                    {
                        destroyed = true;
                        break;
                    }

                    if (map[mx, my] > 0 && map[mx, my] != 10)
                    {
                        destroyed = true;
                        break;
                    }

                    // --- ENEMY COLLISION ---
                    for (int e = 0; e < EnemyManager.enemies.Length; e++)
                    {
                        if (EnemyManager.enemies[e].state != 1) continue;

                        double dx = b.x - EnemyManager.enemies[e].x;
                        double dy = b.y - EnemyManager.enemies[e].y;

                        if ((dx * dx + dy * dy) < 400)
                        {
                            EnemyManager.enemies[e].health -= 1;
                            EnemyManager.enemies[e].hitTimer = 0.15;

                            if (EnemyManager.enemies[e].health <= 0)
                            {
                                EnemyManager.enemies[e].state = 0;
                                GameState.Score += 20;
                            }

                            destroyed = true;
                            break;
                        }
                    }

                    if (destroyed) break;
                }

                if (destroyed || b.distanceTraveled > 4000)
                {
                    bullets.RemoveAt(i);
                }
                else
                {
                    bullets[i] = b;
                }
            }
        }
    }
}