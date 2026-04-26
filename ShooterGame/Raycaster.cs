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

        private const int WINDOW_WIDTH  = 800;
        private const int WINDOW_HEIGHT = 600;

        private const int TILE_SIZE  = 20;
        private const int MAP_WIDTH  = 40;
        private const int MAP_HEIGHT = 30;
        private const int MAX_DOF    = 64;

        private const int TEX_W = 32;
        private const int TEX_H = 32;

        private const int RAYCASTER_RESOLUTION = 400;

        private static Color wallColor = Color.Orange;
        private static Color floorColor = Color.Gray;
        private static Color ceilColor = Color.DarkGray;

        private const float TWO_PI =  (float)Math.PI * 2;
        private const float DEG_IN_RAD = 0.0174532925f; // vrijednost 1 stepena u radijanima

        // Bitmapa za buffer trenutnog frame-a gdje ce se izracunat pozicije svih piksela kao 32 bitna ARGB vrijednost
        private static Bitmap screenBuffer = new Bitmap(WINDOW_WIDTH, WINDOW_HEIGHT, PixelFormat.Format32bppArgb);

        public static void DrawRays3D(Graphics g)
        {
            float rayAngle = Player.angle - (DEG_IN_RAD * FOV / 2);

            // pravougaonik dimenzija ekrana za upisivanje vrijednosti na screen buffer
            Rectangle rect = new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);

            BitmapData bmpData = screenBuffer.LockBits(rect, ImageLockMode.WriteOnly, screenBuffer.PixelFormat);

            float px = Player.x;
            float py = Player.y;

            unsafe // unsafe blok zbog pointera i upravljanja memorije kao u C
            {
                // pointer na poziciju 0, 0 u bufferu
                int* screenPtr = (int*)bmpData.Scan0.ToPointer();
                /*
                for (int y = 0; y < WINDOW_HEIGHT; y++)
                {
                    // offset za citav red (pristupamo screenPtr kao 1D nizu)
                    int rowOffset = y * WINDOW_WIDTH; 
                    // pretvaramo boju u ARGB vrijednost koja ce biti spremljena u buffer
                    int color = (y < WINDOW_HEIGHT / 2) ? ceilColor.ToArgb() : floorColor.ToArgb();

                    for (int x = 0; x < WINDOW_WIDTH; x++) // svaki pixel po x osi
                    {
                        // memorijska lokacija [x + rowOffset] u bufferu sprema boju
                        screenPtr[x + rowOffset] = color; 
                    }
                }*/

                for (int r = 0; r < RAYCASTER_RESOLUTION; r++)
                {
                    if (rayAngle < 0) rayAngle += TWO_PI;
                    if (rayAngle >= TWO_PI) rayAngle -= TWO_PI;

                    float sinA  = (float) Math.Sin(rayAngle);
                    float cosA  = (float) Math.Cos(rayAngle);
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
                        float rayX, rayY, xOffset, yOffset;

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
                        float rayX, rayY, xOffset, yOffset;

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

                    // -- CRTANJE ZIDOVA --

                    // visina jednog isjecka slike
                    double lineHeight = (TILE_SIZE * WINDOW_HEIGHT) / rDist;
                    double lineOffset = (WINDOW_HEIGHT / 2) - (lineHeight / 2);

                    float tx_idx_wall  = (tx / TILE_SIZE) * TEX_W;

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
                        float shade = (hDist < vDist) ? 1.0f : 0.8f;
                        byte wall_r = (byte)(Textures.AllTextures[pixel_idx + 0] * shade);
                        byte wall_g = (byte)(Textures.AllTextures[pixel_idx + 1] * shade);
                        byte wall_b = (byte)(Textures.AllTextures[pixel_idx + 2] * shade);

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
                        // udaljenost od horizonta
                        float dy = y - (WINDOW_HEIGHT / 2.0f);

                        // udaljenost piksela koji treba da se nacrta za pod
                        float straightDist = WINDOW_HEIGHT * (TILE_SIZE / 2.0f) / dy;
                        float dist = straightDist / raFix; // fisheye fix

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

                        byte floor_r = Textures.AllTextures[floor_pixel_idx + 0];
                        byte floor_g = Textures.AllTextures[floor_pixel_idx + 1];
                        byte floor_b = Textures.AllTextures[floor_pixel_idx + 2];

                        int floor_argb = (255 << 24) | (floor_r << 16) | (floor_g << 8) | floor_b;

                        // za krov

                        int ceilID = GameForm.currentLevel.mapC[mapX, mapY];
                        int ceilTexOffset = (ceilID - 1) * 3072;
                        if (ceilTexOffset < 0) ceilTexOffset = 0;

                        int ceil_pixel_idx = (ty_idx * 32 + tx_idx) * 3 + ceilTexOffset;

                        byte ceil_r = (byte)(Textures.AllTextures[ceil_pixel_idx + 0]);
                        byte ceil_g = (byte)(Textures.AllTextures[ceil_pixel_idx + 1]);
                        byte ceil_b = (byte)(Textures.AllTextures[ceil_pixel_idx + 2]);

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
            }

            screenBuffer.UnlockBits(bmpData);
            g.DrawImage(screenBuffer, 0, 0);
        }
    }
}