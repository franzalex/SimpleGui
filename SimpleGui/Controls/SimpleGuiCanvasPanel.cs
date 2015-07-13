using SimpleGui.Drawing;
using System;
using System.Drawing;
using System.Windows.Forms;
using BindingFlags = System.Reflection.BindingFlags;

namespace SimpleGui.Controls
{
    public class SimpleGuiCanvasPanel : SimpleGuiPanel
    {
        private DrawEventHandler _drawHandler;
        private bool _isRunning;
        private KeyEventHandler _keyDownHandler;
        private KeyEventHandler _keyUpHandler;
        private MouseEventHandler _mouseClickHandler;
        private MouseEventHandler _mouseDragHandler;
        private int _targetFps;
        private Canvas canvas;
        private System.Collections.Generic.HashSet<Keys> downKeys;
        private bool formActive;
        private FpsCounter fpsCounter;
        private long lastFrameTime;
        private bool stopRendering;
        private SimpleGuiForm thisForm;

        public SimpleGuiCanvasPanel(SplitterPanel splitterPanel)
            : base(splitterPanel)
        {
            // set thisForm's properties and event handlers
            thisForm = this.Parent.ParentForm as SimpleGuiForm;
            thisForm.KeyPreview = true;

            thisForm.KeyDown += thisForm_KeyDown;
            thisForm.KeyUp += thisForm_KeyUp;
            thisForm.Deactivate += thisForm_Deactivate;
            thisForm.Activated += thisForm_Activated;
            thisForm.FormClosing += (o, e) => this.Stop();

            // set panel properties and event handlers
            Panel.BackColor = Color.White;
            Panel.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                           .SetValue(Panel, true, null);    // make panel double buffered to remove flicker

            Panel.MouseClick += splitterPanel_MouseClick;
            Panel.MouseMove += splitterPanel_MouseMove;
            Panel.Paint += Panel_Paint;

            // set the canvas and frame rate
            canvas = new Canvas() {
                BackgroundColor = Color.Black,
                Owner = splitterPanel,
                Size = new Size(480, 360),
            };

            this.TargetFrameRate = 30;

            downKeys = new System.Collections.Generic.HashSet<Keys>();
        }

        /// <summary>Gets or sets the background color of the canvas.</summary>
        public Color CanvasBackColor
        {
            get { return canvas.BackgroundColor; }
            set { canvas.BackgroundColor = value; }
        }

        /// <summary>Gets the size of the drawing canvas.</summary>
        public Size CanvasSize
        {
            get { return canvas.Size; }
            set { canvas.Size = value; }
        }

        /// <summary>Gets the current frame rate.</summary>
        public double FrameRate { get { return fpsCounter.FPS; } }

        /// <summary>Gets a value indicating whether the canvas is running.</summary>
        /// <value><c>true</c> if the canvas is running; otherwise, <c>false</c>.</value>
        public bool IsRunning { get { return _isRunning; } }

        /// <summary>Gets or sets the target frame rate at which the canvas is to be updated.</summary>
        public int TargetFrameRate
        {
            get { return _targetFps; }
            set
            {
                _targetFps = value;
                fpsCounter = new FpsCounter(value * 2);
                if (_isRunning)
                {
                    this.Stop();
                    this.Start();
                }
            }
        }

        /// <summary>Gets or sets a value indicating whether the panel is visible.</summary>
        /// <value><c>true</c> if the panel is visible; otherwise, <c>false</c>.</value>
        public override bool Visible
        {
            get
            {
                return !thisForm.spltControls.Panel2Collapsed && base.Visible;
            }
            set
            {
                base.Visible = value;

                if (value && thisForm.spltControls.Panel2Collapsed)
                    thisForm.spltControls.Panel2Collapsed = false;
            }
        }

        /// <summary>Gets a screen-shot of the canvas.</summary>
        public Image GetScreenshot()
        {
            if (_drawHandler != null)
                return canvas.CreateScreenshot(_drawHandler);
            else
                return null;
        }

        /// <summary>Sets an event handler for drawing frames to the canvas.</summary>
        /// <param name="drawHandler">The event handler for drawing frames.</param>
        public void SetDrawHandler(DrawEventHandler drawHandler)
        {
            this._drawHandler = drawHandler;
        }

        /// <summary>Sets an event handler for detecting when a key is down.</summary>
        /// <param name="keyDownHandler">The event handler for key down events.</param>
        public void SetKeyDownHandler(KeyEventHandler keyDownHandler)
        {
            this._keyDownHandler = keyDownHandler;
        }

        /// <summary>Sets an event handler for detecting when a key is released.</summary>
        /// <param name="keyUpHandler">The event handler for key up events.</param>
        public void SetKeyUpHandler(KeyEventHandler keyUpHandler)
        {
            this._keyUpHandler = keyUpHandler;
        }

        /// <summary>Sets an event handler to listen for mouse clicks.</summary>
        /// <param name="mouseClickHandler">The mouse click event handler.</param>
        public void SetMouseClickHandler(MouseEventHandler mouseClickHandler)
        {
            this._mouseClickHandler = mouseClickHandler;
        }

        /// <summary>Sets an event handler to listen for dragging the mouse.</summary>
        /// <param name="mouseClickHandler">The mouse drag event handler.</param>
        /// <remarks>
        /// <para>A mouse drag event occurs when the mouse is moved while a button is held down.</para>
        /// <para>
        /// This event will be raised once for each new position the mouse moves to while the button
        /// is held down.
        /// </para>
        /// </remarks>
        public void SetMouseDragHandler(MouseEventHandler mouseDragHandler)
        {
            this._mouseDragHandler = mouseDragHandler;
        }

        /// <summary>Starts drawing the canvas at the specified FPS.</summary>
        /// <param name="fps">The number of frames to be drawn per second.</param>
        internal void Start()
        {
            var fpsDelta = 1000 / _targetFps;  // time delta between two frames at specified FPS
            var sleepTime = 1;                 // the time to sleep while waiting for next frame

            stopRendering = false;  // make sure the canvas isn't in paused state
            _isRunning = true;      // frames are being rendered

            do
            {
                var frameTime = Environment.TickCount;
                if (frameTime >= lastFrameTime + fpsDelta)
                {
                    lastFrameTime = frameTime;
                    Panel.Invalidate(canvas.Rectangle);
                    Application.DoEvents();
                }
                else
                {
                    System.Threading.Thread.Sleep(sleepTime);
                }
            } while (CanRenderNextFrame());

            _isRunning = false;
            stopRendering = false;
        }

        /// <summary>Stops rendering the canvas.</summary>
        internal void Stop()
        {
            stopRendering = true;
            fpsCounter.SkipNextFrame();
        }

        /// <summary>Determines whether this instance can render the next frame.</summary>
        /// <returns><c>true</c> if the next frame can be rendered; otherwise <c>false</c>.</returns>
        private bool CanRenderNextFrame()
        {
            return thisForm.Visible && formActive &&
                   !stopRendering && this.Visible;
        }

        /// <summary>Handles the Paint event of the Panel control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(canvas.BackgroundBrush, canvas.Rectangle);

            if (_drawHandler != null)
            {
                e.Graphics.SetClip(canvas.Rectangle);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                try
                {
                    canvas.Graphics = e.Graphics;
                    _drawHandler.Invoke(canvas);
                }
                finally
                {
                    canvas.Graphics = null;
                }
            }

            fpsCounter.MarkFrame();
        }

        /// <summary>Handles the MouseClick event of the splitterPanel control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void splitterPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (_mouseClickHandler != null)
            {
                Point pos = canvas.ClientToCanvas(e.Location).ToPoint();
                _mouseClickHandler(pos, e.Button);
            }
        }

        /// <summary>Handles the MouseMove event of the splitterPanel control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void splitterPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None && _mouseDragHandler != null)
            {
                Point pos = canvas.ClientToCanvas(e.Location).ToPoint();
                _mouseDragHandler(pos, e.Button);
            }
        }

        private void StartStop_Canvas(object sender, EventArgs e)
        {
            if (CanRenderNextFrame())
            {
                if (!_isRunning)
                    this.Start();
            }
            else
            {
                this.Stop();
            }
        }

        private void thisForm_Activated(object sender, EventArgs e)
        {
            formActive = true;
            StartStop_Canvas(sender, e);
        }

        private void thisForm_Deactivate(object sender, EventArgs e)
        {
            formActive = false;
            StartStop_Canvas(sender, e);
        }

        private void thisForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (downKeys.Add(e.KeyCode) && _keyUpHandler != null)
                _keyDownHandler.Invoke(e.KeyCode);
        }

        private void thisForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (downKeys.Remove(e.KeyCode) && _keyDownHandler != null)
                _keyUpHandler.Invoke(e.KeyCode);
        }
    }
}