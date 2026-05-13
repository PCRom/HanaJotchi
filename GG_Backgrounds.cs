using System.Drawing;

namespace HanaJotchi
{
    public class GG_Backgrounds : GotchaGotchi
    {
        public static void DrawStandardBackground(Graphics g, Rectangle lcdBounds, int width, int height, int size, int petX, int petY)
        {
            // --- PALETTE CONFIGURATION ---
            Color skyColor = Color.FromArgb(184, 216, 248);       // Light Blue
            Color mountainColor = Color.FromArgb(144, 168, 192);  // Muted Slate Blue
            Color grassColor = Color.FromArgb(88, 176, 56);       // Vibrant Grass Green
            Color dirtColor = Color.FromArgb(200, 152, 88);       // Dirt Brown
            Color platformColor = Color.FromArgb(120, 200, 80);   // Lighter Grass Circle

            // Sky & Ground Base Fill
            int horizonY = lcdBounds.Top + (int)(lcdBounds.Height * 0.45);

            using (SolidBrush skyBrush = new SolidBrush(skyColor))
            {
                g.FillRectangle(skyBrush, lcdBounds.X, lcdBounds.Top, lcdBounds.Width, horizonY - lcdBounds.Top);
            }

            using (SolidBrush dirtBrush = new SolidBrush(dirtColor))
            {
                g.FillRectangle(dirtBrush, lcdBounds.X, horizonY, lcdBounds.Width, lcdBounds.Bottom - horizonY);
            }

            //  Distant Mountains (Zigzag horizontal color band)
            using (SolidBrush mtBrush = new SolidBrush(mountainColor))
            {
                Point[] points = {
                  new Point(lcdBounds.Left, horizonY),
                  new Point(lcdBounds.Left + 20, horizonY - 15),
                  new Point(lcdBounds.Left + 45, horizonY - 5),
                  new Point(lcdBounds.Left + 80, horizonY - 22),
                  new Point(lcdBounds.Left + 115, horizonY - 8),
                  new Point(lcdBounds.Right, horizonY - 18),
                  new Point(lcdBounds.Right, horizonY)
                };

                g.FillPolygon(mtBrush, points);
            }

            // Main Foreground Grass Layer
            int grassTopY = horizonY + 12;

            using (SolidBrush grassBrush = new SolidBrush(grassColor))
            {
                g.FillRectangle(grassBrush, lcdBounds.X, grassTopY, lcdBounds.Width, lcdBounds.Bottom - grassTopY);
            }

            // Oval Active Base (Drawn directly under the pet coordinates)
            using (SolidBrush platBrush = new SolidBrush(platformColor))
            {
                using (Pen platPen = new Pen(Color.FromArgb(56, 120, 32), 2)) // Dark green outline
                {
                    // Anchors an oval underneath the pet's floating bounds
                    Rectangle platformBounds = new Rectangle(petX - 10, petY + 40, size + 20, 20);
                    g.FillEllipse(platBrush, platformBounds);
                    g.DrawEllipse(platPen, platformBounds);
                }
            }

        }

        public static void DrawWaterBackground(Graphics g, Rectangle lcdBounds, int width, int height, int size, int petX, int petY)
        {
            // --- PALETTE CONFIGURATION ---
            Color deepWater = Color.FromArgb(16, 88, 168);       // Pure Water Blue
            Color midWater = Color.FromArgb(40, 128, 216);      // Surf Blue
            Color foamColor = Color.FromArgb(248, 248, 248);     // White Foam
            Color sandColor = Color.FromArgb(240, 208, 128);     // Warm Beach Sand
            Color waveColor = Color.FromArgb(40, 168, 216, 248);     // Light accent blue for waves

            // Water Horizon Strips
            int segmentHeight = lcdBounds.Height / 3;

            using (SolidBrush deepBrush = new SolidBrush(deepWater))
            {
                g.FillRectangle(deepBrush, lcdBounds.X, lcdBounds.Top, lcdBounds.Width, segmentHeight);
            }

            using (SolidBrush midBrush = new SolidBrush(midWater))
            {
                g.FillRectangle(midBrush, lcdBounds.X, lcdBounds.Top + segmentHeight, lcdBounds.Width, lcdBounds.Height - segmentHeight);
            }

            // Procedural Wave Lines (Repeated patterns)
            using (Pen wavePen = new Pen(waveColor, 2))
            {
                // Vertical spacing down through the water
                for (int y = lcdBounds.Top + 15; y < lcdBounds.Bottom - 30; y += 25)
                {
                    int rowShift = (y % 2 == 0) ? 35 : 0;
                    int waveWidth = 24;   // Width of the curve bounding box
                    int waveHeight = 8;   // Total height of the curved arc bump
                    int waveGap = 56;     // Distance between wave segments

                    // Horizontal rendering loop
                    for (int x = lcdBounds.Left + 10 + rowShift; x < lcdBounds.Right - waveWidth; x += waveGap)
                    {
                        // Define the bounding rectangle for a single wave arc segment
                        Rectangle arcBounds = new Rectangle(x, y, waveWidth, waveHeight);

                        // Draw a smooth upward arching curve (180 to 360 degrees on the ellipse)
                        g.DrawArc(wavePen, arcBounds, 180, 180);
                    }
                }
            }

            // Sand Island Platform (The pet's safe landing pad)
            using (SolidBrush sandBrush = new SolidBrush(sandColor))
            {
                using (Pen sandPen = new Pen(Color.FromArgb(184, 136, 56), 2))
                {
                    Rectangle islandBounds = new Rectangle(petX - 15, petY + 35, size + 30, 25);
                    g.FillEllipse(sandBrush, islandBounds);
                    g.DrawEllipse(sandPen, islandBounds);
                }
            }

        }

        public static void DrawDungeonBackground(Graphics g, Rectangle lcdBounds, int width, int height, int size, int petX, int petY)
        {
            // --- PALETTE CONFIGURATION ---
            Color caveDarkness = Color.FromArgb(56, 56, 56);     // Dark Gray/Black
            Color rockWall = Color.FromArgb(112, 104, 112);     // Muted Cave Purple/Gray
            Color rockFloor = Color.FromArgb(88, 80, 88);       // Darker Floor Plate
            Color groundSpot = Color.FromArgb(144, 136, 144);   // Highlighted Rock platform

            // Fill Whole Screen with Base Shadow
            using (SolidBrush darkBrush = new SolidBrush(caveDarkness))
            {
                g.FillRectangle(darkBrush, lcdBounds);
            }

            // Render Jagged Cave Ceiling and Side Columns
            using (SolidBrush wallBrush = new SolidBrush(rockWall))
            {
                // Top Ceiling Cave Jagged Outline
                Point[] ceiling = 
                {
                  new Point(lcdBounds.Left, lcdBounds.Top),
                  new Point(lcdBounds.Left + 30, lcdBounds.Top + 25),
                  new Point(lcdBounds.Left + 60, lcdBounds.Top + 15),
                  new Point(lcdBounds.Left + 90, lcdBounds.Top + 30),
                  new Point(lcdBounds.Right - 30, lcdBounds.Top + 10),
                  new Point(lcdBounds.Right, lcdBounds.Top + 25),
                  new Point(lcdBounds.Right, lcdBounds.Top)
                };

                g.FillPolygon(wallBrush, ceiling);
            }

            // Flat Cave Floor
            int caveFloorY = lcdBounds.Bottom - (int)(lcdBounds.Height * 0.35);

            using (SolidBrush floorBrush = new SolidBrush(rockFloor))
            {
                g.FillRectangle(floorBrush, lcdBounds.X, caveFloorY, lcdBounds.Width, lcdBounds.Bottom - caveFloorY);
            }

            // Solid Rock Platform Base underneath Pet
            using (SolidBrush spotBrush = new SolidBrush(groundSpot))
            {
                using (Pen spotPen = new Pen(caveDarkness, 2))
                {
                    Rectangle stoneBounds = new Rectangle(petX - 5, petY + 42, size + 10, 15);
                    g.FillEllipse(spotBrush, stoneBounds);
                    g.DrawEllipse(spotPen, stoneBounds);
                }
            }

        }
    }
}