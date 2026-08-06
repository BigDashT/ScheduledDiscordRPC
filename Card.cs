// ================================================
// CARD.cs
//
// A drop-in replacement for GroupBox: a flat, rounded, dark "card" container with a bold header
// caption, used on EditScheduleForm's Presence tab (Large Image / Small Image / Buttons
// sections). GroupBox's beveled caption chrome is rendered by Windows' visual-style engine and
// can't be recolored without full owner-draw, so rather than fight it, this is a small Panel
// subclass that paints its own header + border. Child controls are added exactly as they were to
// a GroupBox — same Location/Size, same Controls.Add calls — only the container's declared type
// changes.
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ScheduledDiscordRPC
{
    internal class Card : Panel
    {
        public Card()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            Padding = new Padding(0, 30, 0, 0); // room for the header caption drawn in OnPaint
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var borderPath = RoundedRect(bounds, 8))
            using (var borderPen = new Pen(UiTheme.Border))
                e.Graphics.DrawPath(borderPen, borderPath);

            if (!string.IsNullOrEmpty(Text))
            {
                using var headerBrush = new SolidBrush(UiTheme.TextPrimary);
                e.Graphics.DrawString(Text, UiTheme.BoldFont, headerBrush, new PointF(12, 8));
            }
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
