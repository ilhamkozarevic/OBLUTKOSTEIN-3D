using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace ShooterGame
{
    public static class Raycaster
    {
        public static int FOV = 60;

        public const int WINDOW_WIDTH  = 800;
        public const int WINDOW_HEIGHT = 600;

        public const int TILE_SIZE  = 20;
        public const int MAP_WIDTH  = 40;
        public const int MAP_HEIGHT = 30;
        public const int MAX_DOF    = 64;

        private const int TEX_W = 32;
        private const int TEX_H = 32;

        private const int RAYCASTER_RESOLUTION = 400;

        private const float TWO_PI =  (float)Math.PI * 2;
        private const float DEG_IN_RAD = 0.0174532925f; // vrijednost 1 stepena u radijanima

        // Bitmapa za buffer trenutnog frame-a gdje ce se izracunat pozicije svih piksela kao 32 bitna ARGB vrijednost
        private static Bitmap screenBuffer = new Bitmap(WINDOW_WIDTH, WINDOW_HEIGHT, PixelFormat.Format32bppArgb);

        private static float[] distanceTable;

        // Za spriteove
        private static float[] depthBuffer = new float[RAYCASTER_RESOLUTION];

        public static void DrawRays3D(Graphics g)
        {
            float rayAngle = Player.angle - (DEG_IN_RAD * FOV / 2);

            // pravougaonik dimenzija ekrana za upisivanje vrijednosti na screen buffer
            Rectangle rect = new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);

            BitmapData bmpData = screenBuffer.LockBits(rect, ImageLockMode.WriteOnly, screenBuffer.PixelFormat);

            float rayX, rayY, xOffset, yOffset;

            float px = Player.x;
            float py = Player.y;

            unsafe // unsafe blok zbog pointera i upravljanja memorije kao u C
            {
                // pointer na poziciju 0, 0 u bufferu
                int* screenPtr = (int*)bmpData.Scan0.ToPointer();

                fixed (byte* texPtr = Textures.AllTextures) // gleda teksture preko pointera da bude brze
                fixed (byte* gunPtr = Textures.gunTexture)
                fixed (byte* sprPtr = Textures.SpriteTextures)
                {
                    for (int r = 0; r < RAYCASTER_RESOLUTION; r++)
                    {
                        if (rayAngle < 0) rayAngle += TWO_PI;
                        if (rayAngle >= TWO_PI) rayAngle -= TWO_PI;

                        float sinA = (float)Math.Sin(rayAngle);
                        float cosA = (float)Math.Cos(rayAngle);
                        float nTanA = (float)-Math.Tan(rayAngle); // negative tangent function of the ray angle
                        float nCotA = 1.0f / nTanA; // negative cotangent function of the ray angle

                        float hRayX = px;
                        float hRayY = py;
                        float hDist = float.MaxValue;

                        float vRayX = px;
                        float vRayY = py;
                        float vDist = float.MaxValue;

                        double rDist = double.MaxValue; // ray distance

                        // finalne vrijednosti gdje zraka udari
                        int hitMapX = 0;
                        int hitMapY = 0;

                        // vrijednosti gdje zraka udari
                        int hMapX = 0, hMapY = 0;
                        int vMapX = 0, vMapY = 0;

                        // --- HORIZONTALNA KOMPOMENTA ---
                        if (Math.Abs(sinA) > 0.0001)
                        {
                            if (sinA > 0) // ugao od 0 do pi - gleda gore
                            {
                                rayY = (float)Math.Floor(py / TILE_SIZE) * TILE_SIZE + TILE_SIZE;
                                rayX = (py - rayY) * nCotA + px;
                                yOffset = TILE_SIZE;
                                xOffset = -yOffset * nCotA;
                            }
                            else // ugao od pi do 2pi
                            {
                                rayY = (float)Math.Floor(py / TILE_SIZE) * TILE_SIZE - 0.0001f;
                                rayX = (py - rayY) * nCotA + px;
                                yOffset = -TILE_SIZE;
                                xOffset = -yOffset * nCotA;
                            }

                            for (int dof = 0; dof < MAX_DOF; dof++) // depth of field
                            {
                                // pretvare koordinate ekrana u koordinate niza

                                int mapX = (int)Math.Floor(rayX / TILE_SIZE);
                                int mapY = (int)Math.Floor(rayY / TILE_SIZE);

                                // ako zraka izadje izvan mape prekida se petlja
                                if (mapX < 0 || mapX >= MAP_WIDTH || mapY < 0 || mapY >= MAP_HEIGHT) break;

                                if (GameForm.currentLevel.mapW[mapX, mapY] > 0) // ako je pronadjen zid
                                {
                                    hRayX = rayX;
                                    hRayY = rayY;

                                    double hRayDx = hRayX - px;
                                    double hRayDy = hRayY - py;

                                    hMapX = mapX;
                                    hMapY = mapY;
                                    hDist = (float)Math.Sqrt((hRayDx * hRayDx) + (hRayDy * hRayDy)); // formula za udaljenost od koord. pocetka

                                    break;
                                }

                                rayX += xOffset;
                                rayY += yOffset;
                            }
                        }

                        // --- VERTIKALNA KOMPONENTA ---

                        if (Math.Abs(cosA) > 0.0001)
                        {
                            if (cosA > 0) // ugao od 0 do pi - gleda desno
                            {
                                rayX = (float)Math.Floor(px / TILE_SIZE) * TILE_SIZE + TILE_SIZE;
                                rayY = (px - rayX) * nTanA + py;
                                xOffset = TILE_SIZE;
                                yOffset = -xOffset * nTanA;
                            }
                            else // ugao od pi do 2pi
                            {
                                rayX = (float)Math.Floor(px / TILE_SIZE) * TILE_SIZE - 0.0001f;
                                rayY = (px - rayX) * nTanA + py;
                                xOffset = -TILE_SIZE;
                                yOffset = -xOffset * nTanA;
                            }

                            for (int dof = 0; dof < MAX_DOF; dof++) // depth of field
                            {
                                // pretvare koordinate ekrana u koordinate niza

                                int mapX = (int)Math.Floor(rayX / TILE_SIZE);
                                int mapY = (int)Math.Floor(rayY / TILE_SIZE);

                                // ako zraka izadje izvan mape prekida se petlja
                                if (mapX < 0 || mapX >= MAP_WIDTH || mapY < 0 || mapY >= MAP_HEIGHT) break;

                                if (GameForm.currentLevel.mapW[mapX, mapY] > 0) // ako je pronadjen zid
                                {
                                    vRayX = rayX;
                                    vRayY = rayY;

                                    double vRayDx = vRayX - px;
                                    double vRayDy = vRayY - py;

                                    vMapX = mapX;
                                    vMapY = mapY;

                                    vDist = (float)Math.Sqrt((vRayDx * vRayDx) + (vRayDy * vRayDy)); // formula za udaljenost od koord. pocetka

                                    break;
                                }

                                rayX += xOffset;
                                rayY += yOffset;
                            }
                        }

                        float tx = 0f; // hit pos of ray

                        if (hDist < vDist)
                        {
                            rDist = hDist;
                            hitMapX = hMapX;
                            hitMapY = hMapY;
                            tx = hRayX % TILE_SIZE; // udari horizontalnu osu pa je X i ogranici na tile size
                            if (sinA > 0) tx = TILE_SIZE - tx;
                        }
                        else
                        {
                            rDist = vDist;
                            hitMapX = vMapX;
                            hitMapY = vMapY;
                            tx = vRayY % TILE_SIZE; // udari vertikalnu osu pa je Y i ogranici na tile size
                            if (cosA < 0) tx = TILE_SIZE - tx;
                        }

                        // fisheye fix
                        float raFix = (float)Math.Cos(rayAngle - Player.angle);
                        rDist *= raFix;
                        
                        depthBuffer[r] = (float)rDist;

                        // -- CRTANJE ZIDOVA --

                        // visina jednog isjecka slike
                        double lineHeight = (TILE_SIZE * WINDOW_HEIGHT) / rDist;
                        double lineOffset = (WINDOW_HEIGHT / 2) - (lineHeight / 2);

                        float tx_idx_wall = (tx / TILE_SIZE) * TEX_W;

                        double ty_step = (TEX_H / lineHeight);
                        double ty_curr = 0;

                        // clipping ako tekstura ode van ekrana
                        int drawStart = (int)lineOffset;
                        if (drawStart < 0)
                        {
                            ty_curr = -drawStart * ty_step;
                            drawStart = 0;
                        }
                        int drawEnd = (int)(lineOffset + lineHeight);
                        if (drawEnd >= WINDOW_HEIGHT) drawEnd = WINDOW_HEIGHT - 1;

                        float lineWidth = (float)(WINDOW_WIDTH) / RAYCASTER_RESOLUTION;
                        int screenXStart = (int)(r * lineWidth);

                        int wallID = GameForm.currentLevel.mapW[hitMapX, hitMapY];
                        int texOffset = (wallID - 1) * 3072;

                        for (int y = drawStart; y < drawEnd; y++)
                        {
                            int ty_idx = (int)ty_curr & 31; // osigura da ne predje 32, teksture su 0-31
                            tx_idx_wall = (int)tx_idx_wall & (TEX_W - 1);

                            // indeksiranje prema nizu za teksture (3 bajta za svaki piksel)
                            int pixel_idx = (ty_idx * 32 + (int)tx_idx_wall) * 3 + texOffset;

                            // simulacija osvjetljenja zatamnjenem naleglih zidova
                            float shade = (hDist < vDist) ? 1.0f : 0.7f;
                            byte wall_r = (byte)(texPtr[pixel_idx + 0] * shade);
                            byte wall_g = (byte)(texPtr[pixel_idx + 1] * shade);
                            byte wall_b = (byte)(texPtr[pixel_idx + 2] * shade);

                            int wall_argb = (255 << 24) | (wall_r << 16) | (wall_g << 8) | wall_b;

                            for (int w = 0; w < (int)lineWidth + 1; w++)
                            {
                                int finalX = screenXStart + w;
                                if (finalX < WINDOW_WIDTH)
                                {
                                    screenPtr[y * WINDOW_WIDTH + finalX] = wall_argb;
                                }
                            }

                            ty_curr += ty_step;
                        }

                        // -- CRTANJE PODA I KROVA -- 

                        for (int y = drawEnd; y < WINDOW_HEIGHT; y++)
                        {
                            float dist = distanceTable[y] / raFix; // fisheye fix

                            // koordinate piksela za pod
                            float floorX = px + cosA * dist;
                            float floorY = py + sinA * dist;

                            // mapiranje koordinata piksela na teksture 0-31
                            int tx_idx = (int)((floorX / TILE_SIZE) * TEX_W) & (TEX_W - 1);
                            int ty_idx = (int)((floorY / TILE_SIZE) * TEX_H) & (TEX_H - 1);

                            // mapiranje koordinata prema matrici levela
                            int mapX = (int)(floorX / TILE_SIZE);
                            int mapY = (int)(floorY / TILE_SIZE);

                            // ogranicavanje koordinata da se ne bi desio index out of bounds
                            if (mapX < 0) mapX = 0; if (mapX >= MAP_WIDTH) mapX = MAP_WIDTH - 1;
                            if (mapY < 0) mapY = 0; if (mapY >= MAP_HEIGHT) mapY = MAP_HEIGHT - 1;

                            // uzima ID teksture za pod
                            int floorID = GameForm.currentLevel.mapF[mapX, mapY];
                            int floorTexOffset = (floorID - 1) * 3072;
                            if (floorTexOffset < 0) floorTexOffset = 0;

                            // sprema indeks potrebnog piksela u bufferu
                            int floor_pixel_idx = (ty_idx * TEX_W + tx_idx) * 3 + floorTexOffset;

                            byte floor_r = (byte)(texPtr[floor_pixel_idx + 0] + 20);
                            byte floor_g = (byte)(texPtr[floor_pixel_idx + 0] + 20);
                            byte floor_b = (byte)(texPtr[floor_pixel_idx + 0] + 20);

                            int floor_argb = (255 << 24) | (floor_r << 16) | (floor_g << 8) | floor_b;

                            // za krov

                            int ceilID = GameForm.currentLevel.mapC[mapX, mapY];
                            int ceilTexOffset = (ceilID - 1) * 3072;
                            if (ceilTexOffset < 0) ceilTexOffset = 0;

                            int ceil_pixel_idx = (ty_idx * 32 + tx_idx) * 3 + ceilTexOffset;

                            byte ceil_r = (byte)(texPtr[ceil_pixel_idx + 0]);
                            byte ceil_g = (byte)(texPtr[ceil_pixel_idx + 1]);
                            byte ceil_b = (byte)(texPtr[ceil_pixel_idx + 2]);

                            int ceil_argb = (255 << 24) | (ceil_r << 16) | (ceil_g << 8) | ceil_b;

                            for (int w = 0; w < (int)lineWidth + 1; w++)
                            {
                                int finalX = screenXStart + w;
                                if (finalX < WINDOW_WIDTH)
                                {
                                    screenPtr[y * WINDOW_WIDTH + finalX] = floor_argb;

                                    int ceilY = WINDOW_HEIGHT - y - 1;
                                    screenPtr[ceilY * WINDOW_WIDTH + finalX] = ceil_argb;
                                }
                            }
                        }

                        rayAngle += (FOV / (float)RAYCASTER_RESOLUTION) * DEG_IN_RAD;
                    }

                    List<Sprite> sortedEnemies = EnemyManager.enemies
                        .Where(e => e.state != 0)
                        .OrderByDescending(e =>
                        {
                            float dx = e.x - px;
                            float dy = e.y - py;
                            return dx * dx + dy * dy; // No need for Math.Sqrt just for sorting
                        }).ToList();

                    foreach (Sprite sp in sortedEnemies)
                    {
                        float sx = sp.x - px;
                        float sy = sp.y - py;
                        float dist = (float)Math.Sqrt(sx * sx + sy * sy);

                        float spriteAngle = (float)Math.Atan2(sy, sx) - Player.angle;
                        while (spriteAngle <= -Math.PI) spriteAngle += TWO_PI;
                        while (spriteAngle > Math.PI) spriteAngle -= TWO_PI;

                        if (dist > 0.5f && Math.Abs(spriteAngle) < (FOV * DEG_IN_RAD) / 1.5f)
                        {
                            float baseSize = Math.Abs((64.0f * WINDOW_HEIGHT) / dist);
                            float spriteW = baseSize * sp.w;
                            float spriteH = baseSize * sp.h;
                            float zOffset = (sp.z * WINDOW_HEIGHT) / dist;

                            float screenX = (spriteAngle / (FOV * DEG_IN_RAD)) * WINDOW_WIDTH + (WINDOW_WIDTH / 2.0f);
                            float screenY = (WINDOW_HEIGHT / 2.0f) + zOffset;

                            int startX = (int)(screenX - spriteW / 2);
                            int endX = (int)(screenX + spriteW / 2);
                            int startY = (int)(screenY - spriteH / 2);
                            int endY = (int)(screenY + spriteH / 2);

                            int drawStartX = Math.Max(0, startX);
                            int drawEndX = Math.Min(WINDOW_WIDTH - 1, endX);
                            int drawStartY = Math.Max(0, startY);
                            int drawEndY = Math.Min(WINDOW_HEIGHT - 1, endY);

                            int spriteOffset = sp.map * 3072; 

                            for (int screenX_idx = drawStartX; screenX_idx < drawEndX; screenX_idx++)
                            {
                                int rayIdx = (screenX_idx * RAYCASTER_RESOLUTION) / WINDOW_WIDTH;

                                if (dist < depthBuffer[rayIdx])
                                {
                                    int texX = (int)(((screenX_idx - startX) * 32) / spriteW);

                                    for (int screenY_idx = drawStartY; screenY_idx < drawEndY; screenY_idx++)
                                    {
                                        int texY = (int)(((screenY_idx - startY) * 32) / spriteH);

                                        int pIdx = spriteOffset + (texY * 32 + texX) * 3;
                                        byte sp_r = sprPtr[pIdx + 0];
                                        byte sp_g = sprPtr[pIdx + 1];
                                        byte sp_b = sprPtr[pIdx + 2];

                                        if (sp_r == 255 && sp_g == 0 && sp_b == 255) continue;

                                        if (sp.hitTimer > 0)
                                        {
                                            sp_r = (byte)Math.Min(sp_r + 100, 255);
                                            sp_g = (byte)(sp_g * 0.4);
                                            sp_b = (byte)(sp_b * 0.4);
                                        }

                                        int sp_argb = (255 << 24) | (sp_r << 16) | (sp_g << 8) | sp_b;

                                        screenPtr[screenY_idx * WINDOW_WIDTH + screenX_idx] = sp_argb;
                                    }
                                }
                            }
                        }
                    }

                    // --- CRTANJE METAKA (BULLETS) ---
                    foreach (Bullet b in BulletManager.bullets)
                    {
                        float bX = (float)(b.x - Player.x);
                        float bY = (float)(b.y - Player.y);

                        float bulletDist = (float)Math.Sqrt(bX * bX + bY * bY);

                        float bulletAngle = (float)Math.Atan2(bY, bX) - Player.angle;
                        while (bulletAngle <= -Math.PI) bulletAngle += (float)(2.0 * Math.PI);
                        while (bulletAngle > Math.PI) bulletAngle -= (float)(2.0 * Math.PI);

                        float correctedBulletDist = bulletDist * (float)Math.Cos(bulletAngle);

                        float fovRad = FOV * DEG_IN_RAD;
                        if (correctedBulletDist > 0.1f && Math.Abs(bulletAngle) < fovRad / 1.1f)
                        {
                            float windowH = WINDOW_HEIGHT;
                            float horizon = windowH / 2.0f;
                            float maxBulletDist = bulletDist;
                            float bOffY = (float)b.distanceTraveled;

                            float drawY_mapped = windowH - (bOffY * (windowH - horizon) / maxBulletDist);
                            if (drawY_mapped < horizon) drawY_mapped = horizon;

                            int screenX = (int)((bulletAngle / fovRad) * WINDOW_WIDTH + (WINDOW_WIDTH / 2));
                            int screenY = (int)drawY_mapped;

                            float bulletSizeOnScreen = (5.0f * WINDOW_HEIGHT) / correctedBulletDist;
                            if (bulletSizeOnScreen < 2) bulletSizeOnScreen = 2;
                            if (bulletSizeOnScreen > 40) bulletSizeOnScreen = 40;

                            int radius = (int)(bulletSizeOnScreen / 2);

                            for (int y = -radius; y <= radius; y++)
                            {
                                for (int x = -radius; x <= radius; x++)
                                {
                                    int pX = screenX + x;
                                    int pY = screenY + y;

                                    if (pX >= 0 && pX < WINDOW_WIDTH && pY >= 0 && pY < WINDOW_HEIGHT)
                                    {
                                        int rayIdx = (pX * RAYCASTER_RESOLUTION) / WINDOW_WIDTH;

                                        if (rayIdx >= 0 && rayIdx < depthBuffer.Length)
                                        {
                                            if (true)
                                            {
                                                float distSq = x * x + y * y;
                                                float maxDistSq = radius * radius;
                                                int bulletColor = 0;

                                                if (distSq < maxDistSq * 0.15f) // Core
                                                    bulletColor = unchecked((int)0xFFFFFFFF);
                                                else if (distSq < maxDistSq * 0.45f) // Middle
                                                    bulletColor = unchecked((int)0xFFFFFF00);
                                                else if (distSq < maxDistSq) // Outer
                                                    bulletColor = unchecked((int)0xFFFF4500);

                                                if (bulletColor != 0)
                                                {
                                                    screenPtr[pY * WINDOW_WIDTH + pX] = bulletColor;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // GUN TEXTURING GOES HERE
                    int baseGunY = WINDOW_HEIGHT - 32 * 4 + 3;
                    int baseGunX = WINDOW_WIDTH / 2 - 16 * 4;
                    
                    if (Player.up || Player.down || Player.left || Player.right)
                    {
                        float bobSpeed = (int)(GameForm.stopwatch.ElapsedMilliseconds * 0.015f);

                        baseGunX += (int)(Math.Sin(bobSpeed) * 8);
                        baseGunY -= (int)(Math.Abs(Math.Cos(bobSpeed) * 1));
                    }
                    
                    for (int gy = 0; gy < 32; gy++)
                    {
                        for (int gx = 0; gx < 32; gx++)
                        {
                            int gIdx = (gy * 32 + gx) * 3;

                            byte gun_r = (byte)(gunPtr[gIdx + 0]);
                            byte gun_g = (byte)(gunPtr[gIdx + 1]);
                            byte gun_b = (byte)(gunPtr[gIdx + 2]);

                            if (gun_r == 255 && gun_g == 0 && gun_b == 255) continue;

                            int gun_argb = (255 << 24) | (gun_r << 16) | (gun_g << 8) | gun_b;

                            for (int i = 0; i < 4; i++)
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    int gunX = (baseGunY + gy * 4 - i);
                                    int gunY = (gx * 4 - j + baseGunX);

                                    screenPtr[(gunX * WINDOW_WIDTH) + gunY] = gun_argb;
                                }
                            }
                        }
                    }
                }
            }

            screenBuffer.UnlockBits(bmpData);
            g.DrawImage(screenBuffer, 0, 0);
        }

        public static void InteractWith(byte[,] map)
        {
            float reach = 24.0f;

            // provjerava x i y poziciju gdje igrac gleda pomaknutu za reach piksela
            float checkX = Player.x + (float)Math.Cos(Player.angle) * reach;
            float checkY = Player.y + (float)Math.Sin(Player.angle) * reach;

            int mapX = (int)(checkX / TILE_SIZE);
            int mapY = (int)(checkY / TILE_SIZE);

            if (mapX >= 0 && mapX < MAP_WIDTH && mapY >= 0 && mapY < MAP_HEIGHT)
            {
                if (map[mapX, mapY] == 2)
                {
                    map[mapX, mapY] = 0;

                    // racunanje koordinata gdje se nalazi rectangle za collision
                    int targetX = mapX * TILE_SIZE;
                    int targetY = mapY * TILE_SIZE;

                    // predicate funkcija iz funkcionalnog programiranja prima jedan ili vise ulaza i vraca bool
                    Predicate<Rectangle> match = (rect) => (rect.X == targetX && rect.Y == targetY);

                    // ova funkcija prima predicate funkciju i removea svaki rectangle gdje predicate match vraca true
                    GameForm.currentLevel.collisionRects.RemoveAll(match); 
                }
            }
        }

        public static void PrecalculateDistanceTable() // ovo izracuna svaku mogucu distancu za floor da sacuva performanse
        {
            distanceTable = new float[WINDOW_HEIGHT];

            for (int y = 0; y < WINDOW_HEIGHT; y++)
            {
                float dy = Math.Abs(y - (WINDOW_HEIGHT / 2.0f)); // udaljenost od horizonta
                if (dy == 0) dy = 1; // u slucaju dijeljenja sa 0
                distanceTable[y] = WINDOW_HEIGHT * (TILE_SIZE / 2.0f) / dy; // udaljenost piksela koji se crta na pod
            }
        }
    }
}