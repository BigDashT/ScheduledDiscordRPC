// ================================================
// UITHEME.cs
//
// Centralized "look" for the app: a dark, Windows 11 / Discord-inspired flat palette, plus small
// helpers that apply it to the stock WinForms controls already used by MainForm and
// EditScheduleForm (buttons, textboxes, combo/date pickers, checkboxes, the schedule grid, tabs,
// and the window chrome itself).
//
// Deliberately dependency-free — see CLAUDE.md's "no bloat" guidance — everything here is
// System.Drawing/WinForms plus two documented DWM/UxTheme P/Invoke calls (dark title bar, rounded
// corners, and disabling native visual-style chrome on the couple of controls that otherwise
// ignore BackColor/ForeColor). No new NuGet packages, no new architectural layers.
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScheduledDiscordRPC
{
    internal static class UiTheme
    {
        // === Palette (dark, blurple-accented — matches Discord's own dark theme + Win11's flat,
        // rounded aesthetic) ===
        public static readonly Color WindowBackground = ColorTranslator.FromHtml("#1E1F22");
        public static readonly Color Surface = ColorTranslator.FromHtml("#2B2D31");
        public static readonly Color SurfaceRaised = ColorTranslator.FromHtml("#313338");
        public static readonly Color Border = ColorTranslator.FromHtml("#3F4147");
        public static readonly Color Accent = ColorTranslator.FromHtml("#5865F2");
        public static readonly Color AccentHover = ColorTranslator.FromHtml("#4752C4");
        public static readonly Color TextPrimary = ColorTranslator.FromHtml("#F2F3F5");
        public static readonly Color TextSecondary = ColorTranslator.FromHtml("#B5BAC1");
        public static readonly Color Danger = ColorTranslator.FromHtml("#DA373C");
        public static readonly Color DangerHover = ColorTranslator.FromHtml("#A12D2F");

        public static Font BaseFont { get; } = CreatePreferredFont(9.5f);
        public static Font BoldFont { get; } = CreateBoldFont(9.5f);
        public static Font HeaderFont { get; } = CreateBoldFont(11.5f);

        /// <summary>Prefers Windows 11's "Segoe UI Variable"; falls back gracefully on Windows 10
        /// or any system missing it, down to whatever the runtime considers its default font.</summary>
        private static Font CreatePreferredFont(float size)
        {
            foreach (var name in new[] { "Segoe UI Variable Display", "Segoe UI Variable", "Segoe UI" })
            {
                try
                {
                    using var probe = new FontFamily(name);
                    return new Font(name, size, FontStyle.Regular, GraphicsUnit.Point);
                }
                catch (ArgumentException)
                {
                    // Family isn't installed on this OS — try the next candidate.
                }
            }
            return new Font(SystemFonts.DefaultFont.FontFamily, size);
        }

        /// <summary>"Segoe UI Variable" — the preferred UI family — doesn't expose a classic Bold
        /// GDI style (it's a true variable-weight font), so asking for FontStyle.Bold on it throws
        /// ArgumentException("Parameter is not valid") rather than silently substituting a weight.
        /// Falls back to plain "Segoe UI", which every supported Windows version ships with Bold
        /// support for, whenever the preferred family can't do it.</summary>
        private static Font CreateBoldFont(float size)
        {
            try
            {
                if (BaseFont.FontFamily.IsStyleAvailable(FontStyle.Bold))
                    return new Font(BaseFont.FontFamily, size, FontStyle.Bold, GraphicsUnit.Point);
            }
            catch (ArgumentException)
            {
                // Fall through to the guaranteed-safe family below.
            }
            return new Font("Segoe UI", size, FontStyle.Bold, GraphicsUnit.Point);
        }

        // === Win32 interop — both APIs are documented (not the undocumented ordinal-based dark
        // mode hacks some apps use), and failures here are purely cosmetic, so every call site
        // wraps them in try/catch rather than letting a theming issue affect the app itself. ===
        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int SetWindowTheme(IntPtr hWnd, string? pszSubAppName, string? pszSubIdList);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWCP_ROUND = 2;

        /// <summary>Applies a dark title bar and (on Windows 11) rounded window corners. Safe to
        /// call on any Windows version — unsupported attributes are simply ignored by DWM.</summary>
        public static void ApplyWindowChrome(Form form)
        {
            try
            {
                if (!OperatingSystem.IsWindows()) return;
                int enabled = 1;
                DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref enabled, sizeof(int));
                int corner = DWMWCP_ROUND;
                DwmSetWindowAttribute(form.Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref corner, sizeof(int));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UiTheme: window chrome not applied → {ex.Message}");
            }
        }

        /// <summary>Strips native visual-style rendering from a single control (the documented
        /// SetWindowTheme API) so its BackColor/ForeColor actually take effect. ComboBox and
        /// DateTimePicker otherwise ignore those properties for their button/border chrome.</summary>
        private static void Flatten(Control control)
        {
            try
            {
                if (!OperatingSystem.IsWindows()) return;
                SetWindowTheme(control.Handle, "", "");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UiTheme: could not flatten {control.Name} → {ex.Message}");
            }
        }

        /// <summary>Walks a form's entire control tree and applies the theme to every recognized
        /// control type, based on runtime type — so styling stays out of the Designer files and
        /// applying it to a new control is just "add it to the form", not "remember to style it".</summary>
        public static void ApplyTheme(Form form)
        {
            form.BackColor = WindowBackground;
            form.ForeColor = TextPrimary;
            form.Font = BaseFont;
            ApplyWindowChrome(form);
            ApplyToChildren(form.Controls);
        }

        private static void ApplyToChildren(Control.ControlCollection controls)
        {
            // Snapshotted rather than iterated live: StyleTabControl adds new sibling panels to
            // this exact collection (a form's Controls) while it may still be the one being walked
            // here, and mutating a collection mid-enumeration is not something to rely on being safe.
            foreach (Control control in controls.Cast<Control>().ToList())
            {
                try
                {
                    StyleOne(control);
                }
                catch (Exception ex)
                {
                    // Cosmetic only — one control failing to theme (e.g. an unusual font/style
                    // combination) must never take down the whole dialog with it.
                    System.Diagnostics.Debug.WriteLine($"UiTheme: failed to style {control.Name} ({control.GetType().Name}) → {ex.Message}");
                }

                // Recurse regardless of type/outcome so nested containers (panels, tab pages,
                // card bodies) still get their children styled even if the parent itself didn't.
                if (control.HasChildren)
                    ApplyToChildren(control.Controls);
            }
        }

        private static void StyleOne(Control control)
        {
            switch (control)
            {
                case Button button:
                    StyleButton(button);
                    break;
                case CheckBox checkBox:
                    StyleCheckBox(checkBox);
                    break;
                case RadioButton radioButton:
                    StyleRadioButton(radioButton);
                    break;
                case TextBox textBox:
                    StyleTextBox(textBox);
                    break;
                case ComboBox comboBox:
                    StyleComboBox(comboBox);
                    break;
                case DateTimePicker dateTimePicker:
                    StyleDateTimePicker(dateTimePicker);
                    break;
                case NumericUpDown numericUpDown:
                    StyleNumericUpDown(numericUpDown);
                    break;
                case DataGridView grid:
                    StyleDataGridView(grid);
                    break;
                case TabControl tabControl:
                    StyleTabControl(tabControl);
                    break;
                case Card card:
                    StyleCard(card);
                    break;
                case Label label:
                    StyleLabel(label);
                    break;
                case Panel panel:
                    panel.BackColor = panel.Parent?.BackColor ?? WindowBackground;
                    break;
            }
        }

        private static void StyleLabel(Label label)
        {
            label.ForeColor = label.Tag as string == "secondary" ? TextSecondary : TextPrimary;
            label.BackColor = Color.Transparent;
        }

        /// <summary>Flat, rounded, accent-colored primary buttons by default. Set
        /// <c>button.Tag = "secondary"</c> or <c>"danger"</c> in the Designer for the other two
        /// variants used in this app (Cancel, destructive actions).</summary>
        public static void StyleButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;

            bool danger = button.Tag as string == "danger";
            bool secondary = button.Tag as string == "secondary";

            if (danger)
            {
                button.BackColor = Danger;
                button.ForeColor = Color.White;
                button.FlatAppearance.MouseOverBackColor = DangerHover;
                button.FlatAppearance.MouseDownBackColor = DangerHover;
            }
            else if (secondary)
            {
                button.BackColor = SurfaceRaised;
                button.ForeColor = TextPrimary;
                button.FlatAppearance.MouseOverBackColor = Border;
                button.FlatAppearance.MouseDownBackColor = Border;
            }
            else
            {
                button.BackColor = Accent;
                button.ForeColor = Color.White;
                button.FlatAppearance.MouseOverBackColor = AccentHover;
                button.FlatAppearance.MouseDownBackColor = AccentHover;
            }

            ApplyRoundedRegion(button, 8);
            button.Resize -= ButtonOnResize;
            button.Resize += ButtonOnResize;
        }

        private static void ButtonOnResize(object? sender, EventArgs e)
        {
            if (sender is Button button) ApplyRoundedRegion(button, 8);
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0) return;
            var path = RoundedRect(new Rectangle(0, 0, control.Width, control.Height), radius);
            control.Region?.Dispose();
            control.Region = new Region(path);
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

        private static void StyleCheckBox(CheckBox checkBox)
        {
            checkBox.FlatStyle = FlatStyle.Flat;
            checkBox.FlatAppearance.BorderSize = 0;
            checkBox.ForeColor = TextPrimary;
            checkBox.BackColor = Color.Transparent;
            checkBox.Cursor = Cursors.Hand;

            // "Chip" toggles (the weekday-of-week pickers) are marked via Tag in the Designer so
            // they render as pill-shaped filled/outline buttons instead of a checkbox+label —
            // matches the Windows 11 "repeat on" day picker used in Clock/Calendar.
            if (checkBox.Tag as string == "chip")
            {
                checkBox.Appearance = Appearance.Button;
                checkBox.TextAlign = ContentAlignment.MiddleCenter;
                checkBox.FlatAppearance.CheckedBackColor = Accent;
                checkBox.FlatAppearance.MouseOverBackColor = Border;
                checkBox.BackColor = SurfaceRaised;
                checkBox.UseVisualStyleBackColor = false;
                ApplyRoundedRegion(checkBox, checkBox.Height / 2);
                checkBox.Resize -= ChipOnResize;
                checkBox.Resize += ChipOnResize;
                checkBox.CheckedChanged -= ChipCheckedChanged;
                checkBox.CheckedChanged += ChipCheckedChanged;
                ChipCheckedChanged(checkBox, EventArgs.Empty);
            }
        }

        private static void ChipOnResize(object? sender, EventArgs e)
        {
            if (sender is CheckBox chip) ApplyRoundedRegion(chip, chip.Height / 2);
        }

        private static void ChipCheckedChanged(object? sender, EventArgs e)
        {
            if (sender is CheckBox chip)
                chip.ForeColor = chip.Checked ? Color.White : TextSecondary;
        }

        private static void StyleRadioButton(RadioButton radioButton)
        {
            radioButton.FlatStyle = FlatStyle.Flat;
            radioButton.FlatAppearance.BorderSize = 0;
            radioButton.ForeColor = TextPrimary;
            radioButton.BackColor = Color.Transparent;
            radioButton.Cursor = Cursors.Hand;
        }

        private static void StyleTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = SurfaceRaised;
            textBox.ForeColor = TextPrimary;
        }

        private static void StyleComboBox(ComboBox comboBox)
        {
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.BackColor = SurfaceRaised;
            comboBox.ForeColor = TextPrimary;
            if (comboBox.IsHandleCreated) Flatten(comboBox);
            else comboBox.HandleCreated += (s, e) => Flatten(comboBox);
        }

        private static void StyleDateTimePicker(DateTimePicker picker)
        {
            picker.CalendarForeColor = TextPrimary;
            picker.CalendarMonthBackground = Surface;
            picker.CalendarTitleBackColor = SurfaceRaised;
            picker.CalendarTitleForeColor = TextPrimary;
            picker.CalendarTrailingForeColor = TextSecondary;
            picker.CalendarFont = BaseFont;

            // Flattening (same SetWindowTheme trick as ComboBox) has to happen before the color
            // assignments below, not after — otherwise it resets BackColor/ForeColor right back to
            // the native-themed values it was just given.
            if (picker.IsHandleCreated) Flatten(picker);
            else picker.HandleCreated += (s, e) => Flatten(picker);

            picker.BackColor = SurfaceRaised;
            picker.ForeColor = TextPrimary;
        }

        private static void StyleNumericUpDown(NumericUpDown numericUpDown)
        {
            numericUpDown.BorderStyle = BorderStyle.FixedSingle;
            numericUpDown.BackColor = SurfaceRaised;
            numericUpDown.ForeColor = TextPrimary;
        }

        public static void StyleDataGridView(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = WindowBackground;
            grid.GridColor = Border;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.RowHeadersVisible = false;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 40;
            grid.RowTemplate.Height = 34;
            grid.Font = BaseFont;

            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Surface,
                ForeColor = TextSecondary,
                SelectionBackColor = Surface,
                SelectionForeColor = TextSecondary,
                Font = BoldFont,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
            };
            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = WindowBackground,
                ForeColor = TextPrimary,
                SelectionBackColor = Accent,
                SelectionForeColor = Color.White,
                Padding = new Padding(8, 4, 8, 4),
            };
            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Surface,
                ForeColor = TextPrimary,
                SelectionBackColor = Accent,
                SelectionForeColor = Color.White,
                Padding = new Padding(8, 4, 8, 4),
            };
        }

        /// <summary>Per-TabControl map of TabPage → the sibling panel now hosting that page's real
        /// content, so <see cref="TabControlOnSelectedIndexChanged"/> knows which one to reveal.
        /// See the long comment in <see cref="StyleTabControl"/> for why this exists.</summary>
        private static readonly System.Collections.Generic.Dictionary<TabControl, System.Collections.Generic.Dictionary<TabPage, Panel>> TabContentPanels = new();

        /// <summary>Owner-draws the tab strip as flat, underline-accented segments instead of the
        /// default beveled Win32 tabs, and — because TabPage's own content area is painted natively
        /// by the tab control (DrawThemeBackground for the tab body) and stays the default light
        /// gray no matter what's tried against it (BackColor, SetWindowTheme flattening, an opaque
        /// child panel docked to fill + sent to back, reparenting everything into that panel, even
        /// an explicit OnPaintBackground override forcing a fill) — sidesteps TabPage entirely for
        /// hosting content. Each page's controls move into a plain panel that's a sibling of the
        /// TabControl (a direct child of the form, which paints correctly), sized to the tab's
        /// display area and shown/hidden as the selected tab changes.</summary>
        private static void StyleTabControl(TabControl tabControl)
        {
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.ItemSize = new Size(140, 38);
            tabControl.Padding = new Point(16, 6);
            tabControl.BackColor = WindowBackground;

            if (tabControl.Parent == null) return;

            var displayRect = tabControl.DisplayRectangle;
            var contentPanels = new System.Collections.Generic.Dictionary<TabPage, Panel>();

            foreach (TabPage page in tabControl.TabPages)
            {
                page.BackColor = WindowBackground;

                var content = new Panel
                {
                    BackColor = WindowBackground,
                    Location = new Point(tabControl.Left + displayRect.Left, tabControl.Top + displayRect.Top),
                    Size = displayRect.Size,
                    Visible = page == tabControl.SelectedTab,
                };

                foreach (var child in page.Controls.Cast<Control>().ToList())
                    content.Controls.Add(child);

                tabControl.Parent.Controls.Add(content);
                content.BringToFront();
                contentPanels[page] = content;

                // The moved-in controls are no longer reachable from the normal ApplyTheme walk
                // (they used to be under the TabPage, which the walk still visits, but they now
                // live under this new form-level sibling instead) — style them explicitly rather
                // than relying on the outer recursion to happen to reach a panel just added to the
                // very collection it's currently iterating.
                ApplyToChildren(content.Controls);
            }

            TabContentPanels[tabControl] = contentPanels;
            tabControl.SelectedIndexChanged -= TabControlOnSelectedIndexChanged;
            tabControl.SelectedIndexChanged += TabControlOnSelectedIndexChanged;

            tabControl.DrawItem -= TabControlOnDrawItem;
            tabControl.DrawItem += TabControlOnDrawItem;

            // A new EditScheduleForm (and thus a new TabControl) is created every time Add/Edit is
            // clicked — without this, the static map below would keep growing for as long as the
            // app runs, holding each old dialog's entire control tree alive.
            tabControl.Disposed -= TabControlOnDisposed;
            tabControl.Disposed += TabControlOnDisposed;
        }

        private static void TabControlOnDisposed(object? sender, EventArgs e)
        {
            if (sender is TabControl tabControl)
                TabContentPanels.Remove(tabControl);
        }

        private static void TabControlOnSelectedIndexChanged(object? sender, EventArgs e)
        {
            if (sender is not TabControl tabControl) return;
            if (!TabContentPanels.TryGetValue(tabControl, out var panels)) return;
            foreach (var (page, panel) in panels)
                panel.Visible = page == tabControl.SelectedTab;
        }

        private static void TabControlOnDrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not TabControl tabControl) return;
            var tabRect = tabControl.GetTabRect(e.Index);
            bool selected = e.Index == tabControl.SelectedIndex;

            using var backBrush = new SolidBrush(WindowBackground);
            e.Graphics.FillRectangle(backBrush, tabRect);

            using var textBrush = new SolidBrush(selected ? TextPrimary : TextSecondary);
            using var textFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            // BoldFont/BaseFont are shared static instances used across the whole app — must not
            // be wrapped in `using` here, or this disposes them for every other consumer after the
            // first tab repaint.
            var font = selected ? BoldFont : BaseFont;
            e.Graphics.DrawString(tabControl.TabPages[e.Index].Text, font, textBrush, tabRect, textFormat);

            if (selected)
            {
                using var accentBrush = new SolidBrush(Accent);
                e.Graphics.FillRectangle(accentBrush, tabRect.Left + 12, tabRect.Bottom - 3, tabRect.Width - 24, 3);
            }
        }

        /// <summary>Draws the accent top-border + header text for a <see cref="Card"/> (this app's
        /// replacement for GroupBox, which can't be recolored without full owner-draw).</summary>
        private static void StyleCard(Card card)
        {
            card.BackColor = Surface;
        }
    }
}
