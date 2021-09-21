using System.Drawing;

namespace MoonPhaseSpace
{
    public class Arc
    {
        public Rectangle Bounds { get; set; }
        public int StartAngle { get; set; }
        public int EndAngle { get; set; }
        public bool IsHalfway { get; set; } = false;
        public bool IsComplete { get; set; } = false;

        public Arc(Rectangle bounds, int startAngle, int endAngle)
        {
            Bounds = bounds;
            StartAngle = startAngle;
            EndAngle = endAngle;
        }

        public void Draw(Graphics graphics, Pen pen)
        {
            if (Bounds.Width > 0)
                graphics.DrawArc(pen, Bounds, StartAngle, EndAngle);
        }
        public void Update(bool increase, int quantity) => Bounds = (increase ?
            new Rectangle(Bounds.X - quantity, Bounds.Y, Bounds.Width + (quantity * 2), Bounds.Height) :
            new Rectangle(Bounds.X + quantity, Bounds.Y, Bounds.Width - (quantity * 2), Bounds.Height));      
    }
}