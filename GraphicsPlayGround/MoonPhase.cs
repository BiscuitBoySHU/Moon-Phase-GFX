using System;
using System.Drawing;
using System.Windows.Forms;

namespace MoonPhaseSpace
{
    public partial class MoonPhase : Form
    {
        private readonly Bitmap Canvas;
        private readonly Graphics Graphics;
        private readonly Pen Pen;

        private Arc ArcA;
        private Arc ArcB;
        private readonly int RightAngleStart = -90;
        private readonly int LeftAngleStart = 90;
        private readonly int AngleEnd = 180;
        private readonly int MaxWidth = 300;
        private readonly int DistanceFromTop = 80;
        private readonly int ChangeRate = 3;
        private readonly Rectangle RightArcStart;
        private readonly Rectangle LeftArcStart;


        public MoonPhase()
        {
            InitializeComponent();

            Pen = new Pen(Brushes.Yellow);
            RightArcStart = new Rectangle(100, DistanceFromTop, MaxWidth, MaxWidth);
            LeftArcStart = new Rectangle(250, DistanceFromTop, 0, MaxWidth);
            ArcA = new Arc(RightArcStart, RightAngleStart, AngleEnd);
            ArcB = new Arc(RightArcStart, RightAngleStart, AngleEnd);

            Text = "Moon Phase";
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            const int formSize = 520;
            Size = new Size(formSize, formSize);
            pictureBox1.SetBounds(0, 0, Size.Width, Size.Height);

            Canvas = new Bitmap(Size.Width, Size.Height);
            Graphics = Graphics.FromImage(Canvas);
        }

        private void Updater_Tick(object sender, EventArgs e)
        {
            Graphics.Clear(Color.Black);
            Graphics.DrawEllipse(new Pen(Brushes.DarkGray), new RectangleF(100, DistanceFromTop, MaxWidth, MaxWidth));

            HandleArc(ArcA);
            if (ArcA.IsComplete)
                HandleArc(ArcB);

            ArcA.Draw(Graphics, Pen);
            ArcB.Draw(Graphics, Pen);

            pictureBox1.Image = Canvas;

            if (ArcA.IsComplete && ArcB.IsComplete)
            {
                ResetArc(ArcA);
                ResetArc(ArcB);
            }


            void HandleArc(Arc arc)
            {
                if (!arc.IsComplete)
                    arc.Update(increase: arc.IsHalfway, quantity: ChangeRate);

                if (arc.Bounds.Width <= 0)
                {
                    Graphics.DrawLine(Pen, 250, DistanceFromTop, 250, MaxWidth + 80);
                    arc.IsHalfway = true;
                    arc.Bounds = LeftArcStart;
                    arc.StartAngle = LeftAngleStart;
                }
                else if (arc.Bounds.Width >= MaxWidth)
                    arc.IsComplete = true;
            }

            void ResetArc(Arc arc)
            {
                arc.Bounds = RightArcStart;
                arc.StartAngle = RightAngleStart;
                arc.IsComplete = false;
                arc.IsHalfway = false;
            }
        }

        private void MoonPhase_FormClosing(object sender, FormClosingEventArgs e)
        {
            Pen.Dispose();
            Graphics.Dispose();
            Canvas.Dispose();
        }
    }
}