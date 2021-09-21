using System;
using System.Drawing;

namespace MoonPhaseSpace
{
    public class Arc
    {
        private static readonly int LeftAngleStart = 90;
        private static readonly int RightAngleStart = -90;
        private static readonly Rectangle LeftArcStart = new Rectangle(250, 80, 0, 300);
        private static readonly Rectangle RightArcStart = new Rectangle(100, 80, 300, 300);


        public Rectangle Bounds { get; private set; }
        private Rectangle DrawingBounds { get; set; }

        public int StartAngle { get; private set; }
        public int EndAngle { get; } = 180;
        public int MaxWidth { get; }


        public Arc(int maxWidth)
        {
            MaxWidth = maxWidth;
            Update(halfway: false, decimalPercentage: 0.0d);
        }

        public void Update(bool halfway, double decimalPercentage)
        {
            if (decimalPercentage >= .5d) decimalPercentage = .4999d;
            var newWidth = (int)(Math.Floor(MaxWidth * decimalPercentage));

            if (halfway)
            {
                Bounds = LeftArcStart;
                DrawingBounds = new Rectangle(Bounds.X - newWidth, Bounds.Y, Bounds.Width + (newWidth * 2), Bounds.Height);
                StartAngle = LeftAngleStart;
            }
            else
            {
                Bounds = RightArcStart;
                DrawingBounds = new Rectangle(Bounds.X + newWidth, Bounds.Y, Bounds.Width - (newWidth * 2), Bounds.Height);
                StartAngle = RightAngleStart;
            }
        }
        public void Draw(Graphics graphics, Pen pen) => graphics.DrawArc(pen, DrawingBounds, StartAngle, EndAngle);
    }
}