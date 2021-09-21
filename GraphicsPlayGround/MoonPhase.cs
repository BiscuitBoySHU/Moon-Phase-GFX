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
        private readonly Font TextFont;

        private readonly TimeSpan TimeBetweenNewMoons;
        private readonly DateTime NewMoonReferenceTime;
        private readonly Arc ArcA;
        private readonly Arc ArcB;
        public static readonly int MaxWidth = 300;


        public MoonPhase()
        {
            InitializeComponent();

            Pen = new Pen(Brushes.Yellow, 2f);
            TextFont = new Font(SystemFonts.DefaultFont.FontFamily, 12, FontStyle.Regular);
            ArcA = new Arc();
            ArcB = new Arc();

            Text = "Moon Phase";
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            const int formSize = 520;
            Size = new Size(formSize, formSize);
            ImageBox.SetBounds(0, 0, Size.Width, Size.Height);

            Canvas = new Bitmap(Size.Width, Size.Height);
            Graphics = Graphics.FromImage(Canvas);

            TimeBetweenNewMoons = new TimeSpan(days: 29, hours: 12, minutes: 44, seconds: 3); // Average time between new moon phases.
            NewMoonReferenceTime = new DateTime(year: 2021, month: 1, day: 13, hour: 5, minute: 0, second: 0); // Time of new moon phase, Jan, 2021.
        }


        private void Updater_Tick(object sender, EventArgs e)
        {
            const double phaseQuarter = 0.25d;
            Graphics.Clear(Color.Black);
            Graphics.DrawImage(Properties.Resources.Moon, new RectangleF(100, 80, MaxWidth + 4, MaxWidth));
            Graphics.DrawEllipse(new Pen(Brushes.DarkGray), new RectangleF(100, 82, MaxWidth, MaxWidth - 4));

            double moonAgePercentage = CalculateMoonAgePercentage();
            bool reachedFirstQuarter = moonAgePercentage >= phaseQuarter;
            bool reachedFullMoon = moonAgePercentage >= 2 * phaseQuarter;
            bool reachedLastQuarter = moonAgePercentage >= 3 * phaseQuarter;
            double arcAPercent = reachedFullMoon ? 1.0d : moonAgePercentage % phaseQuarter / phaseQuarter;
            double arcBPercent = reachedFullMoon ? moonAgePercentage % phaseQuarter / phaseQuarter : 0.0d;

            bool isFirstQuarter = (moonAgePercentage == 0.25);
            bool isLastQuarter = (moonAgePercentage == 0.75);
            bool isExactNew = (moonAgePercentage == 0.0d || moonAgePercentage == 1.0d);

            var phase = "Waxing Crescent";
            if (reachedLastQuarter) phase = "Waning Crescent";
            else if (reachedFullMoon) phase = "Waning Gibbous";
            else if (reachedFirstQuarter) phase = "Waxing Gibbous";

            if (isFirstQuarter || isLastQuarter)
                Graphics.DrawLine(Pen, MaxWidth - 50, 80, MaxWidth - 50, MaxWidth + 80);
            if (!isExactNew)
            {
                if (!isFirstQuarter)
                {
                    ArcA.Update(halfway: reachedFirstQuarter, decimalPercentage: 0.5d * arcAPercent);
                    ArcA.Draw(Graphics, Pen);
                }
                else
                    phase = "First Quarter";
                if (!isLastQuarter)
                {
                    ArcB.Update(halfway: reachedLastQuarter, decimalPercentage: 0.5d * arcBPercent);
                    ArcB.Draw(Graphics, Pen);
                }
                else
                    phase = "Last Quarter";
            }
            else
                phase = "New Moon";

            Graphics.DrawString($"Phase: {phase}\n{DateTime.UtcNow} (UTC)\nAge: {moonAgePercentage * 100}%", TextFont, Brushes.White, 10f, 10f);
            ImageBox.Image = Canvas;
        }

        private double CalculateMoonAgePercentage()
        {
            var utcNow = DateTime.UtcNow;
            double timeSinceSeconds = (utcNow - NewMoonReferenceTime).TotalSeconds;
            double newMoonCount = timeSinceSeconds / TimeBetweenNewMoons.TotalSeconds;

            double agePercentage = (double)decimal.Round((decimal)newMoonCount % 1, 5);
            return agePercentage;
        }

        private void MoonPhase_FormClosing(object sender, FormClosingEventArgs e)
        {
            Pen.Dispose();
            Font.Dispose();
            Graphics.Dispose();
            Canvas.Dispose();
        }
    }
}