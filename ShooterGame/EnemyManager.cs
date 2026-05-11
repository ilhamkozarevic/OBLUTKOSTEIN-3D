using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ShooterGame
{
    public static class EnemyManager
    {
        public static Sprite[] enemies = new Sprite[0];
        private static List<Point>[] paths;
        private static double[] pathTimers;

        public static int currentRound = 0;
        private static float baseSpeed = 100f;

        private struct AStarNode
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

        private static double Heuristic(int x1, int y1, int x2, int y2)
        {
            double dx = x2 - x1, dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static void StartNextRound()
        {
            currentRound++;
            int count = 2 + currentRound;
            float speed = 40f + currentRound * 4f;

            List<Point> spawnZones = new List<Point>();
            var map = GameForm.currentLevel.mapW;
            for (int x = 0; x < Raycaster.MAP_WIDTH; x++)
                for (int y = 0; y < Raycaster.MAP_HEIGHT; y++)
                    if (map[x, y] == 10) spawnZones.Add(new Point(x, y));

            if (spawnZones.Count == 0)
            {
                spawnZones.Add(new Point(1, 1));
                spawnZones.Add(new Point(38, 1));
                spawnZones.Add(new Point(1, 28));
                spawnZones.Add(new Point(38, 28));
            }

            enemies = new Sprite[count];
            paths = new List<Point>[count];
            pathTimers = new double[count];

            Random rng = new Random();
            for (int i = 0; i < count; i++)
            {
                Point zone = spawnZones[rng.Next(spawnZones.Count)];
                enemies[i] = new Sprite
                {
                    type = 1,
                    state = 1,
                    health = 3,
                    map = 0,
                    w = 0.4f,
                    h = 0.4f,
                    z = 5,
                    speed = speed,
                    x = zone.X * 20 + 10,
                    y = zone.Y * 20 + 10
                };
            }
        }

        public static bool RoundOver()
        {
            if (enemies.Length == 0) return true;
            foreach (var e in enemies)
                if (e.state == 1) return false;
            return true;
        }

        public static void Update(double dt)
        {
            const int STOP_DIST = 8;
            const double ATTACK_RANGE = 15.0;

            var map = GameForm.currentLevel.mapW;
            float px = Player.x;
            float py = Player.y;

            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].state != 1) continue;

                if (enemies[i].hitTimer > 0) enemies[i].hitTimer -= dt;
                if (enemies[i].damageTimer > 0) enemies[i].damageTimer -= dt;

                double dx = enemies[i].x - px;
                double dy = enemies[i].y - py;
                double dist = Math.Sqrt(dx * dx + dy * dy);

                if (dist < ATTACK_RANGE && enemies[i].damageTimer <= 0)
                {
                    GameState.PlayerHealth -= 2;
                    enemies[i].damageTimer = 1.0;
                    GameState.ScreenFlashTimer = 0.2f;
                }

                pathTimers[i] -= dt;
                if (pathTimers[i] <= 0)
                {
                    pathTimers[i] = 0.3;

                    int ex = (int)(enemies[i].x / 20);
                    int ey = (int)(enemies[i].y / 20);
                    int epx = (int)(px / 20);
                    int epy = (int)(py / 20);

                    paths[i] = FindPath(ex, ey, epx, epy, map);
                }

                if (dist > ATTACK_RANGE && paths[i] != null && paths[i].Count > 0)
                {
                    Point next = paths[i][0];
                    // Target center of the 20x20 tile
                    double tx = next.X * 20 + 10;
                    double ty = next.Y * 20 + 10;
                    double wdx = tx - enemies[i].x;
                    double wdy = ty - enemies[i].y;
                    double wdist = Math.Sqrt(wdx * wdx + wdy * wdy);

                    if (wdist < 5 && paths[i].Count > 1)
                    {
                        paths[i].RemoveAt(0);
                        next = paths[i][0];
                        tx = next.X * 20 + 10;
                        ty = next.Y * 20 + 10;
                        wdx = tx - enemies[i].x;
                        wdy = ty - enemies[i].y;
                        wdist = Math.Sqrt(wdx * wdx + wdy * wdy);
                    }

                    if (wdist > 0)
                    {
                        double moveX = (wdx / wdist) * enemies[i].speed * dt;
                        double moveY = (wdy / wdist) * enemies[i].speed * dt;

                        double newX = enemies[i].x + moveX;
                        double newY = enemies[i].y + moveY;

                        int checkX = Math.Max(0, Math.Min(Raycaster.MAP_WIDTH - 1,
                                     (int)((moveX > 0) ? (newX + STOP_DIST) / 20
                                                        : (newX - STOP_DIST) / 20)));
                        int checkXY = Math.Max(0, Math.Min(Raycaster.MAP_HEIGHT - 1, (int)(enemies[i].y / 20)));

                        if (map[checkX, checkXY] == 0)
                            enemies[i].x = (float)newX;

                        int checkYX = Math.Max(0, Math.Min(Raycaster.MAP_WIDTH - 1, (int)(enemies[i].x / 20)));
                        int checkY = Math.Max(0, Math.Min(Raycaster.MAP_HEIGHT - 1,
                                     (int)((moveY > 0) ? (newY + STOP_DIST) / 20
                                                        : (newY - STOP_DIST) / 20)));

                        if (map[checkYX, checkY] == 0)
                            enemies[i].y = (float)newY;
                    }
                }
            }
        }

        private static List<Point> FindPath(int sx, int sy, int ex, int ey, byte[,] map)
        {
            if (sx == ex && sy == ey) return new List<Point>();

            var openList = new List<AStarNode>();
            var visited = new Dictionary<int, AStarNode>();

            openList.Add(new AStarNode(sx, sy, 0, Heuristic(sx, sy, ex, ey), -1, -1));

            int[] ddx = { 0, 0, 1, -1, 1, 1, -1, -1 };
            int[] ddy = { 1, -1, 0, 0, 1, -1, 1, -1 };
            double[] costs = { 1, 1, 1, 1, 1.414, 1.414, 1.414, 1.414 };

            while (openList.Count > 0)
            {
                int bestIdx = 0;
                for (int i = 1; i < openList.Count; i++)
                    if (openList[i].f < openList[bestIdx].f) bestIdx = i;

                var curr = openList[bestIdx];
                openList.RemoveAt(bestIdx);

                int key = curr.y * Raycaster.MAP_WIDTH + curr.x;
                if (visited.ContainsKey(key)) continue;
                visited[key] = curr;

                if (curr.x == ex && curr.y == ey)
                {
                    var path = new List<Point>();
                    int cx = curr.x, cy = curr.y;
                    while (!(cx == sx && cy == sy))
                    {
                        path.Add(new Point(cx, cy));
                        var node = visited[cy * Raycaster.MAP_WIDTH + cx];
                        cx = node.parentX;
                        cy = node.parentY;
                    }
                    path.Reverse();
                    return path;
                }

                for (int d = 0; d < 8; d++)
                {
                    int nx = curr.x + ddx[d];
                    int ny = curr.y + ddy[d];

                    if (nx < 0 || nx >= Raycaster.MAP_WIDTH || ny < 0 || ny >= Raycaster.MAP_HEIGHT) continue;
                    if (map[nx, ny] > 0) continue;
                    if (d >= 4 && (map[nx, curr.y] > 0 || map[curr.x, ny] > 0)) continue;

                    int nkey = ny * Raycaster.MAP_WIDTH + nx;
                    if (visited.ContainsKey(nkey)) continue;

                    double g = curr.g + costs[d];
                    openList.Add(new AStarNode(nx, ny, g, Heuristic(nx, ny, ex, ey), curr.x, curr.y));
                }
            }
            return null;
        }
    }
}