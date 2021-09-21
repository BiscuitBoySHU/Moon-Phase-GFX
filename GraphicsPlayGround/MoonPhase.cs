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
        private readonly int MaxWidth = 300;


        public MoonPhase()
        {
            InitializeComponent();

            Pen = new Pen(Brushes.Yellow);
            ArcA = new Arc(maxWidth: MaxWidth);
            ArcB = new Arc(maxWidth: MaxWidth);

            Text = "Moon Phase";
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            const int formSize = 520;
            Size = new Size(formSize, formSize);
            ImageBox.SetBounds(0, 0, Size.Width, Size.Height);

            Canvas = new Bitmap(Size.Width, Size.Height);
            Graphics = Graphics.FromImage(Canvas);
        }

        private void Updater_Tick(object sender, EventArgs e)
        {
            Graphics.Clear(Color.Black);
            Graphics.DrawEllipse(new Pen(Brushes.DarkGray), new RectangleF(100, 80, MaxWidth, MaxWidth));

            ArcA.Update(halfway: false, decimalPercentage: 0.25d);
            ArcB.Update(halfway: true, decimalPercentage: 0.25d);
            
            ArcA.Draw(Graphics, Pen);
            ArcB.Draw(Graphics, Pen);

            ImageBox.Image = Canvas;
        }

        private void MoonPhase_FormClosing(object sender, FormClosingEventArgs e)
        {
            Pen.Dispose();
            Graphics.Dispose();
            Canvas.Dispose();
        }
    }
}