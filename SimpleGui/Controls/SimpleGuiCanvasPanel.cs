using System;
using System.Drawing;
using System.Windows.Forms;
using Canvas = SimpleGui.Drawing.Canvas;

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
        private Canvas canvas;
        private SimpleGuiForm Form;
        private long lastFrame;
        private bool stopRendering;

        public SimpleGuiCanvasPanel(SplitterPanel splitterPanel)
            : base(splitterPanel)
        {
            this.Form = this.Parent.ParentForm as SimpleGuiForm;
            this.Form.KeyPreview = true;
            this.Form.KeyDown += (o, e) => { if (_keyDownHandler != null) _keyDownHandler(e.KeyCode); };
            this.Form.KeyUp += (o, e) => { if (_keyUpHandler != null) _keyUpHandler(e.KeyCode); };

            Panel.BackColor = Color.White;

            Panel.MouseClick += splitterPanel_MouseClick;
            Panel.MouseMove += splitterPanel_MouseMove;
            Panel.Paint += Panel_Paint;

            canvas = new Canvas() {
                BackgroundColor = Color.Black,
                Owner = splitterPanel,
                Size = new Size(480, 360),
            };
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

        /// <summary>Gets a value indicating whether the canvas is running.</summary>
        /// <value><c>true</c> if the canvas is running; otherwise, <c>false</c>.</value>
        public bool IsRunning { get { return _isRunning; } }

        /// <summary>Gets or sets a value indicating whether the panel is visible.</summary>
        /// <value><c>true</c> if the panel is visible; otherwise, <c>false</c>.</value>
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;

                if (value && Form.spltControls.Panel2Collapsed)
                    Form.spltControls.Panel2Collapsed = false;
            }
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
        public void Start(int fps = 30)
        {
            var fpsDelta = 1000 / fps;                  // time delta between two frames at specified FPS
            var sleepTime = Math.Max(fpsDelta / 10, 1); // the time to sleep while waiting for next frame; 1/10 of frame rate

            stopRendering = false;  // make sure the canvas isn't in paused state
            _isRunning = true;      // frames are being rendered

            do
            {
                var frameTime = Environment.TickCount;
                if (frameTime >= lastFrame + fpsDelta)
                {
                    lastFrame = frameTime;
                    Form.spltCanvas.Panel1.Invalidate();
                    Application.DoEvents();
                }
                else
                {
                    System.Threading.Thread.Sleep(sleepTime);
                }
            } while (!stopRendering);

            _isRunning = false;
        }

        /// <summary>Stops rendering the canvas.</summary>
        public void Stop()
        {
            stopRendering = true;
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

                canvas.Graphics = e.Graphics;
                _drawHandler.Invoke(canvas);
                canvas.Graphics = null;
            }
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
            if (e.Button != MouseButtons.None && _mouseDragHandler != null &&
                canvas.Rectangle.Contains(e.Location))
            {
                Point pos = canvas.ClientToCanvas(e.Location).ToPoint();
                _mouseDragHandler(pos, e.Button);
            }
        }
    }
}