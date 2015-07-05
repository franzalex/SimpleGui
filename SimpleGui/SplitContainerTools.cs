using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace System.Windows.Forms
{
    internal partial class SplitContainerTools
    {
        private const short ShowSplitterWidth = 8;

        private SplitContainerButton[] _buttons;
        private bool _keepFocus;
        private bool _panel1Border;
        private bool _panel2Border;
        private SplitContainerButtons _showButtons;
        private bool _showGripper;
        private Reflection.FieldInfo _splitRectInternal;
        private ToolTip _tooltip;
        private bool adjustingPanel;    // flag to prevent stack overflow when resizing panels on panelXCollapse
        private Padding p1, p2;         // previous padding values saved for changes in DrawPanelXBorder property
        private Control prevFocused;
        private SplitContainer split;

        /// <summary>Initializes a new instance of the <see cref="SplitContainerTools"/> class.</summary>
        /// <param name="container">
        /// The <see cref="System.Windows.Forms.SplitContainer"/> to which this instance will be bound.
        /// </param>
        public SplitContainerTools(SplitContainer container)
        {
            split = container;
            _keepFocus = false;
            _showGripper = true;
            _showButtons = SplitContainerButtons.None;
            _panel1Border = false;
            _panel2Border = false;
            _buttons = new SplitContainerButton[] { };
            _tooltip = new ToolTip();

            _splitRectInternal = typeof(SplitContainer).GetField("splitterRect", Reflection.BindingFlags.NonPublic |
                                                                                 Reflection.BindingFlags.GetField |
                                                                                 Reflection.BindingFlags.Instance);

            split.Paint += split_Paint;
            split.MouseDown += split_MouseDown;
            split.MouseUp += split_MouseUp;
            split.MouseClick += split_MouseClick;
            split.MouseMove += split_MouseMove;
            split.MouseLeave += (o, e) => split.Cursor = Cursors.Default;
            split.SplitterMoved += split_SplitterMoved;
            split.SizeChanged += split_Resize;
            split.Disposed += (o, e) => _tooltip.Dispose();

            ((Panel)split.Panel1).ClientSizeChanged += Split_Panel_CollapsedChanged;
            ((Panel)split.Panel2).LocationChanged += Split_Panel_CollapsedChanged;
        }

        /// <summary>Gets or sets a the <see cref="SplitContainerButtons"/> to be shown.</summary>
        /// <value>
        /// <c>true</c> if the collapse and restore buttons should be shown; otherwise, <c>false</c>.
        /// </value>
        public SplitContainerButtons Buttons
        {
            get { return _showButtons; }
            set
            {
                if (value != _showButtons)
                {
                    _showButtons = value;
                    UpdateDisplay();
                }
            }
        }

        /// <summary>Gets the container this <see cref="SplitContainer"/> is bound to.</summary>
        public SplitContainer Container
        {
            get { return split; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a 3D border should be drawn for <see cref="SplitContainer.Panel1"/>.
        /// </summary>
        /// <value>
        /// <c>true</c> if a 3D border should be drawn for <see cref="SplitContainer.Panel1"/>;
        /// otherwise, <c>false</c>.
        /// </value>
        public bool DrawPanel1Border
        {
            get { return _panel1Border; }
            set
            {
                if (value != _panel1Border)
                {
                    _panel1Border = value;
                    SetPanelBorderState(split.Panel1, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a 3D border should be drawn for <see cref="SplitContainer.Panel"/>.
        /// </summary>
        /// <value>
        /// <c>true</c> if a 3D border should be drawn for <see cref="SplitContainer.Panel1"/>;
        /// otherwise, <c>false</c>.
        /// </value>
        public bool DrawPanel2Border
        {
            get { return _panel2Border; }
            set
            {
                if (value != _panel2Border)
                {
                    _panel2Border = value;
                    SetPanelBorderState(split.Panel2, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control should keep focus after the splitter
        /// is moved with the mouse.
        /// </summary>
        /// <value>
        /// <c>true</c> if the control should keep focus after dragging the splitter; otherwise, <c>false</c>.
        /// </value>
        public bool KeepFocus
        {
            get { return _keepFocus; }
            set
            {
                _keepFocus = value;
                UpdateDisplay();
            }
        }

        /// <summary>Gets or sets a value indicating whether a gripper should be shown.</summary>
        /// <value><c>true</c> if gripper should be shown; otherwise, <c>false</c>.</value>
        public bool ShowGripper
        {
            get { return _showGripper; }
            set
            {
                _showGripper = value;
                UpdateDisplay();
            }
        }

        private Rectangle SplitRectInternal
        {
            get
            {
                return (Rectangle)_splitRectInternal.GetValue(split);
            }
            set
            {
                _splitRectInternal.SetValue(split, value);
            }
        }

        private Rectangle SplitterRect
        {
            get
            {
                return GetFakedSplitterRectangle();
            }
        }

        /// <summary>Finds the focused control.</summary>
        /// <param name="control">The control whose children are to be searched.</param>
        /// <returns>
        /// A <see cref="T:System.Windows.Forms.Control"/> hosted by <paramref name="control"/> or
        /// one of its child controls that has input focus.
        /// </returns>
        private static Control FindFocusedControl(Control control)
        {
            if (control == null || !control.ContainsFocus)  // control doesn't contain focus
                return null;

            // if control is container control, find focus in the active child control
            if (control is ContainerControl)
                return FindFocusedControl(((ContainerControl)control).ActiveControl);
            else
                return control;
        }

        private void AdjustSplitterRectangle()
        {
            int rectEnd = _buttons.Min(b => split.Orientation == Orientation.Horizontal ?
                                            b.Rect.Left : b.Rect.Top) - 1;

            var rect = this.SplitRectInternal;
            if (split.Orientation == Orientation.Horizontal)
                rect.Width = rectEnd;
            else
                rect.Height = rectEnd;
            this.SplitRectInternal = rect;
        }

        private void DrawButtons(PaintEventArgs e)
        {
            foreach (var button in _buttons)
                e.Graphics.DrawImage(button.GetImage(split), button.Rect);
        }

        private void DrawGripper(PaintEventArgs e)
        {
            float gripWidth, gripX1, gripY1, gripX2, gripY2;
            float maxGripLen = 192;
            var spltRect = this.SplitterRect;

            if (split.Orientation == Orientation.Horizontal)
            {
                // gripper width = two-thirds gripper width but not longer than max gripper length
                gripWidth = Math.Min(spltRect.Width * (2 / 3f), maxGripLen);
                gripX1 = spltRect.Left + ((spltRect.Width - gripWidth) / 2f);
                gripY1 = spltRect.Top + (spltRect.Height / 2f);
                gripX2 = gripX1 + gripWidth;
                gripY2 = gripY1;

                // draw the gripper
                e.Graphics.DrawLine(Drawing.SystemPens.ControlDarkDark, gripX1, gripY1, gripX2, gripY2);
                e.Graphics.DrawLine(Drawing.SystemPens.ControlLightLight, gripX1, gripY1 - 1, gripX2, gripY2 - 1);
            }
            else
            {
                // gripper width = two-thirds gripper height but not longer max gripper length
                gripWidth = Math.Min(spltRect.Height * (2f / 3), maxGripLen);
                gripX1 = spltRect.Left + (spltRect.Width / 2f);
                gripY1 = spltRect.Top + ((spltRect.Height - gripWidth) / 2f);
                gripX2 = gripX1;
                gripY2 = gripY1 + gripWidth;

                // draw the gripper
                e.Graphics.DrawLine(Drawing.SystemPens.ControlDarkDark, gripX1, gripY1, gripX2, gripY2);
                e.Graphics.DrawLine(Drawing.SystemPens.ControlLightLight, gripX1 - 1, gripY1, gripX2 - 1, gripY2);
            }
        }

        private Rectangle GetFakedSplitterRectangle()
        {
            Point pt = split.SplitterRectangle.Location;
            Size sz;

            if (split.Orientation == Orientation.Horizontal)
            {
                if (split.Panel1Collapsed)
                    pt = new Point(0, 0);
                else if (split.Panel2Collapsed)
                    pt = new Point(0, split.ClientRectangle.Bottom - split.SplitterWidth);

                sz = new Size(split.ClientSize.Width, split.SplitterWidth);
            }
            else
            {
                if (split.Panel1Collapsed)
                    pt = new Point(0, 0);
                else if (split.Panel2Collapsed)
                    pt = new Point(split.ClientRectangle.Right - split.SplitterWidth, 0);

                sz = new Size(split.SplitterWidth, split.ClientSize.Height);
            }

            return new Rectangle(pt, sz);
        }

        /// <summary>Refreshes the buttons.</summary>
        private void RefreshButtons()
        {
            if (_showButtons != SplitContainerButtons.None)
                split.SplitterWidth = Math.Max(SplitContainerButton.MinSplitterWidth, split.SplitterWidth);

            var buttons = new List<SplitContainerButton>();
            var btnIds = new[] { SplitContainerButtons.ChangeOrientation,
                                 SplitContainerButtons.Panel2,
                                 SplitContainerButtons.Panel1 };
            var btnSz = SplitContainerButton.ButtonSize;

            // go through the buttons and add each of them that appears in the user's selection.
            foreach (var btnId in btnIds)
            {
                if ((btnId & _showButtons) == btnId)
                {
                    int btnX, btnY;
                    if (split.Orientation == Orientation.Horizontal)
                    {
                        btnX = this.SplitterRect.Right - (buttons.Count * (btnSz.Width + 1)) - (btnSz.Width + 2);
                        btnY = this.SplitterRect.Top + ((this.SplitterRect.Height - btnSz.Height) / 2);
                    }
                    else
                    {
                        btnX = this.SplitterRect.Left + ((this.SplitterRect.Width - btnSz.Width) / 2);
                        btnY = this.SplitterRect.Bottom - (buttons.Count * (btnSz.Height + 1)) - (btnSz.Height + 2);
                    }

                    buttons.Add(new SplitContainerButton() {
                        Type = btnId,
                        Rect = new Rectangle(new Point(btnX, btnY), btnSz)
                    });
                }
            }

            _buttons = buttons.ToArray();
        }

        private void SetButtonToolTips(Control ctrl)
        {
            var pt = split.PointToClient(Cursor.Position);
            var btns = _buttons.Where(b => b.Rect.Contains(pt));

            if (btns.Any())
            {
                ctrl.Cursor = Cursors.Default;     // restore default cursor

                // ensure splitter rectangle doesn't overlap button rectangle
                if (split.SplitterRectangle.Contains(pt))
                    AdjustSplitterRectangle();

                // set the appropriate tool-tip
                var btn = btns.First();
                if (_tooltip.Tag == null || (SplitContainerButtons)_tooltip.Tag != btn.Type)
                {
                    _tooltip.SetToolTip(split, btn.GetToolTip(split));
                    _tooltip.Tag = btn.Type;
                }
            }
            else
            {
                // clear the tool-tip
                _tooltip.SetToolTip(split, "");
                _tooltip.Tag = null;
            }
        }

        private void SetGripCursor(Control ctrl, Point pt)
        {
            var rect = this.SplitterRect;
            rect.Inflate(-1, -1);

            if (rect.Contains(pt))
                ctrl.Cursor = split.Orientation == Orientation.Horizontal ? Cursors.HSplit : Cursors.VSplit;
            else
                ctrl.Cursor = Cursors.Default;
        }

        private void SetPanelBorderState(SplitterPanel splitterPanel, bool paintBorder)
        {
            if (paintBorder)
            {
                // save a copy of the current padding
                if (splitterPanel == split.Panel1)
                    p1 = splitterPanel.Padding;
                else
                    p2 = splitterPanel.Padding;

                // adjust the padding to accommodate the border
                var p = splitterPanel.Padding;
                splitterPanel.Padding = new Padding(Math.Max(p.Left, 1), Math.Min(p.Top, 1),
                                                    Math.Min(p.Right, 1), Math.Min(p.Bottom, 1));

                // hook the paint event handler
                splitterPanel.Paint += splitPanel_Paint;
            }
            else
            {
                // restore the original padding and then unhook the paint event handler
                splitterPanel.Padding = (split.Panel1 == splitterPanel ? p1 : p2);
                splitterPanel.Paint -= splitPanel_Paint;
            }

            splitterPanel.Invalidate();
        }

        private void split_MouseClick(object sender, MouseEventArgs e)
        {
            SplitContainerButtons clickedButton = SplitContainerButtons.None;

            foreach (var btn in _buttons)
                if (btn.Rect.Contains(e.Location))
                    clickedButton = btn.Type;

            switch (clickedButton)
            {
                case SplitContainerButtons.ChangeOrientation:
                    split.Orientation = (Orientation)(((int)split.Orientation + 1) % 2);
                    break;

                case SplitContainerButtons.Panel1:
                    split.Panel1Collapsed = !split.Panel1Collapsed;
                    break;

                case SplitContainerButtons.Panel2:
                    split.Panel2Collapsed = !split.Panel2Collapsed;
                    break;
            }
        }

        private void split_MouseDown(object sender, MouseEventArgs e)
        {
            // save the previously focused control
            if (!_keepFocus)
                prevFocused = FindFocusedControl(split.FindForm());
        }

        private void split_MouseMove(object sender, MouseEventArgs e)
        {
            SetGripCursor((Control)sender, e.Location);
            SetButtonToolTips((Control)sender);

            // allow dragging to restore collapsed panels
            if (e.Button == MouseButtons.Left && (split.Panel1Collapsed || split.Panel2Collapsed))
            {
                var visibleRgn = Tuple.Create(split.Panel1MinSize * 0.9f, split.Panel2MinSize * 0.9f);
                var cursorPos = 0;

                if (split.Orientation == Orientation.Horizontal)
                {
                    cursorPos = e.Location.Y;
                    visibleRgn = Tuple.Create(visibleRgn.Item1, split.ClientRectangle.Bottom - visibleRgn.Item2);
                }
                else
                {
                    cursorPos = e.Location.X;
                    visibleRgn = Tuple.Create(visibleRgn.Item1, split.ClientRectangle.Right - visibleRgn.Item2);
                }

                split.Panel1Collapsed = cursorPos <= visibleRgn.Item1;
                split.Panel2Collapsed = cursorPos >= visibleRgn.Item2;
            }
        }

        private void split_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_keepFocus && prevFocused != null)
                prevFocused.Focus();
        }

        private void split_Paint(object sender, PaintEventArgs e)
        {
            if (_showGripper)
                DrawGripper(e);
            if (_showButtons != SplitContainerButtons.None)
                DrawButtons(e);
        }

        private void Split_Panel_CollapsedChanged(object sender, EventArgs e)
        {
            // this event is triggered when one of the panels is collapsed. the panel that raises
            // this event is the one that isn't collapsed
            var panel = (Panel)sender;

            if (panel.Visible && !adjustingPanel)
            {
                adjustingPanel = true;
                if (split.Orientation == Orientation.Horizontal)
                {
                    if (split.Panel1Collapsed)
                    {
                        panel.Top = split.SplitterWidth;
                        panel.Height -= split.SplitterWidth;
                    }
                    else if (split.Panel2Collapsed)
                        panel.Height -= split.SplitterWidth;
                }
                else
                {
                    if (split.Panel1Collapsed)
                    {
                        panel.Left = split.SplitterWidth;
                        panel.Width -= split.SplitterWidth;
                    }
                    else if (split.Panel2Collapsed)
                        panel.Width -= split.SplitterWidth;
                }

                RefreshButtons();
                adjustingPanel = false;
            }
        }

        private void split_Resize(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void split_SplitterMoved(object sender, SplitterEventArgs e)
        {
            UpdateDisplay();
        }

        private void splitPanel_Paint(object sender, PaintEventArgs e)
        {
            var rect = ((SplitterPanel)sender).ClientRectangle;
            int L = rect.Left, T = rect.Top, R = rect.Right - 1, B = rect.Bottom - 1;
            e.Graphics.DrawLine(Drawing.SystemPens.ControlDarkDark, L, T, R, T);    // TL -> TR
            e.Graphics.DrawLine(Drawing.SystemPens.ControlDarkDark, L, T, L, B);    // TL -> BL
            e.Graphics.DrawLine(Drawing.SystemPens.ControlLightLight, R, T, R, B);  // TR -> BR
            e.Graphics.DrawLine(Drawing.SystemPens.ControlLightLight, L, B, R, B);  // BL -> BR
        }

        /// <summary>Updates the display of the SplitControl.</summary>
        private void UpdateDisplay()
        {
            split.Panel1.SuspendLayout();
            split.Panel2.SuspendLayout();

            // set the minimum splitter width
            if (_showGripper)
                split.SplitterWidth = Math.Max(ShowSplitterWidth, split.SplitterWidth);

            RefreshButtons();
            split.Invalidate();

            split.Panel1.ResumeLayout();
            split.Panel2.ResumeLayout();
        }
    }
}