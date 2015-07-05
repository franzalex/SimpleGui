using System;
using System.Drawing;
using System.Linq;
using Control = System.Windows.Forms.Control;

namespace SimpleGui.Drawing
{
    public class Canvas
    {
        private SolidBrush _backgroundBrush;
        private Control _owner;
        private float _scale;
        private RectangleF canvasRect;
        private Size desiredSize;

        /// <summary>Initializes a new instance of the <see cref="Canvas"/> class.</summary>
        public Canvas()
        {
            this.BackgroundColor = Color.Black;
        }

        /// <summary>Gets the background brush.</summary>
        /// <value>The background brush.</value>
        public SolidBrush BackgroundBrush
        {
            get { return _backgroundBrush; }
        }

        /// <summary>Gets or sets the background color of the <see cref="Canvas"/>.</summary>
        public Color BackgroundColor
        {
            get
            {
                return _backgroundBrush.Color;
            }
            set
            {
                // dispose the brush if its colour is different from the new one
                if (_backgroundBrush != null && _backgroundBrush.Color != value)
                    _backgroundBrush.Dispose();

                _backgroundBrush = new SolidBrush(value);
            }
        }

        /// <summary>Gets the owner of the <see cref="Canvas"/>.</summary>
        public Control Owner
        {
            get { return _owner; }
            internal set
            {
                if (_owner != null)
                    _owner.SizeChanged -= Owner_SizeChanged;

                if (value != null && value != _owner)
                {
                    _owner = value;
                    _owner.SizeChanged += Owner_SizeChanged;
                    AdjustCanvasRectangle();
                }
            }
        }

        /// <summary>Gets the canvas' rectangle.</summary>
        public Rectangle Rectangle
        {
            get { return canvasRect.ToRectangle(); }
        }

        /// <summary>
        /// Gets the scale factor at which the <see cref="Canvas"/> is drawn to the screen.
        /// </summary>
        public float Scale { get { return _scale; } }

        /// <summary>Gets or sets the size of the <see cref="Canvas"/>.</summary>
        public Size Size
        {
            get
            {
                if (desiredSize.IsEmpty)
                    return Owner.ClientSize;
                else
                    return desiredSize;
            }
            set
            {
                if (desiredSize != value)
                {
                    desiredSize = value;
                    AdjustCanvasRectangle();
                }
            }
        }

        /// <summary>Gets or sets the graphics for drawing to the canvas.</summary>
        internal Graphics Graphics { get; set; }

        /// <summary>Draws a circle with the specified parameters.</summary>
        /// <param name="lineColor">Color of the line along the edge of the circle.</param>
        /// <param name="lineWidth">Width of the line along the edge of the circle.</param>
        /// <param name="location">The coordinates of the upper left corner of the circle.</param>
        /// <param name="size">
        /// The size of the circle. This will be squared to the minimum dimension if the Width and
        /// Height are different.
        /// </param>
        /// <remarks>
        /// If the dimensions of the <see cref="Size.Height"/> and <see cref="Size.Width"/> of the
        /// <paramref name="size"/> parameter are not equal, the minimum of the two will be used to
        /// create a square size for drawing the circle.
        /// </remarks>
        public void DrawCircle(Color lineColor, int lineWidth, Point location, Size size)
        {
            var sideLen = Math.Min(size.Width, size.Height);
            this.DrawEllipse(lineColor, lineWidth, Color.Transparent, location, new Size(sideLen, sideLen));
        }

        /// <summary>Draws a circle with the specified parameters.</summary>
        /// <param name="lineColor">Color of the line along the edge of the circle.</param>
        /// <param name="lineWidth">Width of the line along the edge of the circle.</param>
        /// <param name="fillColor">Color to fill the area enclosed by the circle with.</param>
        /// <param name="location">The coordinates of the upper left corner of the circle.</param>
        /// <param name="size">
        /// The size of the circle. This will be squared to the minimum dimension if the Width and
        /// Height are different.
        /// </param>
        /// <remarks>
        /// If the dimensions of the <see cref="Size.Height"/> and <see cref="Size.Width"/> of the
        /// <paramref name="size"/> parameter are not equal, the minimum of the two will be used to
        /// create a square size for drawing the circle.
        /// </remarks>
        public void DrawCircle(Color lineColor, int lineWidth, Color fillColor, Point location, Size size)
        {
            var sideLen = Math.Min(size.Width, size.Height);
            this.DrawEllipse(lineColor, lineWidth, fillColor, location, new Size(sideLen, sideLen));
        }

        /// <summary>Draws a circle with the specified parameters.</summary>
        /// <param name="lineColor">Color of the line along the edge of the circle.</param>
        /// <param name="lineWidth">Width of the line along the edge of the circle.</param>
        /// <param name="center">The coordinates of the center of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        public void DrawCircle(Color lineColor, int lineWidth, Point center, int radius)
        {
            this.DrawEllipse(lineColor, lineWidth, Color.Transparent, center, radius, radius);
        }

        /// <summary>Draws a circle with the specified parameters.</summary>
        /// <param name="lineColor">Color of the line along the edge of the circle.</param>
        /// <param name="lineWidth">Width of the line along the edge of the circle.</param>
        /// <param name="fillColor">Color to fill the area enclosed by the circle with.</param>
        /// <param name="center">The coordinates of the center of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        public void DrawCircle(Color lineColor, int lineWidth, Color fillColor, Point center, int radius)
        {
            this.DrawEllipse(lineColor, lineWidth, fillColor, center, radius, radius);
        }

        /// <summary>Draws an ellipse with the specified parameters.</summary>
        /// <param name="lineColor">Color of the line along the edge of the ellipse.</param>
        /// <param name="lineWidth">Width of the line along the edge of the ellipse.</param>
        /// <param name="center">The coordinates of the center of the ellipse.</param>
        /// <param name="xRadius">The radius along the X-axis.</param>
        /// <param name="yRadius">The radius along the Y-axis.</param>
        public void DrawEllipse(Color lineColor, int lineWidth, Point center, int xRadius, int yRadius)
        {
            this.DrawEllipse(lineColor, lineWidth, Color.Transparent, center, xRadius, yRadius);
        }

        /// <summary>Draws an ellipse with the specified parameters.</summary>
        /// <param name="lineColor">Color of the line along the edge of the ellipse.</param>
        /// <param name="lineWidth">Width of the line along the edge of the ellipse.</param>
        /// <param name="fillColor">Color to fill the area enclosed by the ellipse with.</param>
        /// <param name="center">The coordinates of the center of the ellipse.</param>
        /// <param name="xRadius">The radius along the X-axis.</param>
        /// <param name="yRadius">The radius along the Y-axis.</param>
        public void DrawEllipse(Color lineColor, int lineWidth, Color fillColor, Point center, int xRadius, int yRadius)
        {
            this.DrawEllipse(lineColor, lineWidth, fillColor,
                             new Point(center.X - xRadius, center.Y - yRadius),
                             new Size(xRadius * 2, yRadius * 2));
        }

        /// <summary>Draws an ellipse with the specified parameters.</summary>
        /// <param name="lineColor">Color of the line along the edge of the ellipse.</param>
        /// <param name="lineWidth">Width of the line along the edge of the ellipse.</param>
        /// <param name="location">The upper left location of the ellipse.</param>
        /// <param name="size">The size of the ellipse.</param>
        public void DrawEllipse(Color lineColor, int lineWidth, Point location, Size size)
        {
            this.DrawEllipse(lineColor, lineWidth, Color.Transparent, location, size);
        }

        /// <summary>Draws an ellipse with the specified parameters.</summary>
        /// <param name="lineColor">Color of the line along the edge of the ellipse.</param>
        /// <param name="lineWidth">Width of the line along the edge of the ellipse.</param>
        /// <param name="fillColor">Color to fill the area enclosed by the ellipse with.</param>
        /// <param name="location">The upper left location of the ellipse.</param>
        /// <param name="size">The size of the ellipse.</param>
        public void DrawEllipse(Color lineColor, int lineWidth, Color fillColor, Point location, Size size)
        {
            if (Graphics != null)
            {
                var ellipseRect = new RectangleF(CanvasToClient(location),
                                                 new SizeF(size.Width * _scale, size.Height * _scale));

                if (fillColor != Color.Transparent)
                    using (var brush = new SolidBrush(fillColor))
                        Graphics.FillEllipse(brush, ellipseRect);

                using (var pen = new Pen(lineColor, lineWidth * _scale))
                    Graphics.DrawEllipse(pen, ellipseRect);
            }
        }

        /// <summary>Draws the specified image at the given location.</summary>
        /// <param name="image">The image to be drawn.</param>
        /// <param name="location">
        /// The upper left coordinates of the location the image will be drawn.
        /// </param>
        public void DrawImage(Image image, Point location)
        {
            this.DrawImage(image, location, image.Size);
        }

        /// <summary>Draws the specified image at the given location with the specified size.</summary>
        /// <param name="image">The image to be drawn.</param>
        /// <param name="location">
        /// The upper left coordinates of the location the image will be drawn.
        /// </param>
        /// <param name="size">The size of the drawn image.</param>
        public void DrawImage(Image image, Point location, Size size)
        {
            if (Graphics != null)
            {
                Graphics.DrawImage(image, new RectangleF(CanvasToClient(location),
                                   new SizeF(size.Width * _scale, size.Height * _scale)));
            }
        }

        /// <summary>Draws a line connecting the two points.</summary>
        /// <param name="color">The color of the line to be drawn.</param>
        /// <param name="lineWidth">Width of the line.</param>
        /// <param name="pt1">The starting point of the line.</param>
        /// <param name="pt2">The end point of the line.</param>
        public void DrawLine(Color color, int lineWidth, Point pt1, Point pt2)
        {
            this.DrawLines(color, lineWidth, pt1, pt2);
        }

        /// <summary>Draws a series of line segments to connect the specified points.</summary>
        /// <param name="color">The color.</param>
        /// <param name="lineWidth">Width of the line.</param>
        /// <param name="points">The points to be connected with line segments.</param>
        public void DrawLines(Color color, int lineWidth, params Point[] points)
        {
            if (Graphics != null)
            {
                using (var pen = new Pen(color, lineWidth * _scale))
                    Graphics.DrawLines(pen, points.Select(pt => CanvasToClient(pt)).ToArray());
            }
        }

        /// <summary>Draws the polygon with the specified parameters.</summary>
        /// <param name="lineColor">Color of the line along the polygon's edge.</param>
        /// <param name="lineWidth">Width of the line.</param>
        /// <param name="points">The points at the corners of the polygon.</param>
        public void DrawPolygon(Color lineColor, int lineWidth, params Point[] points)
        {
            this.DrawPolygon(lineColor, lineWidth, Color.Transparent, points);
        }

        /// <summary>Draws the polygon with the specified parameters.</summary>
        /// <param name="lineColor">Color of the line along the polygon's edge.</param>
        /// <param name="fillColor">Colour to fill the polygon with.</param>
        /// <param name="lineWidth">Width of the line.</param>
        /// <param name="points">The points at the corners of the polygon.</param>
        public void DrawPolygon(Color lineColor, int lineWidth, Color fillColor, params Point[] points)
        {
            if (Graphics != null)
            {
                if (fillColor != Color.Transparent)
                    using (var fill = new SolidBrush(fillColor))
                        Graphics.FillPolygon(fill, points);

                using (var pen = new Pen(lineColor, lineWidth * _scale))
                    Graphics.DrawPolygon(pen, points);
            }
        }

        /// <summary>Draws a rectangle with the specified parameters.</summary>
        /// <param name="lineColor">Color of the line along the edge of the rectangle.</param>
        /// <param name="lineWidth">Width of the line along the edge of the rectangle.</param>
        /// <param name="location">The upper left location of the rectangle.</param>
        /// <param name="size">The size of the rectangle.</param>
        public void DrawRectangle(Color lineColor, int lineWidth, Point location, Size size)
        {
            this.DrawRectangle(lineColor, lineWidth, Color.Transparent, location, size);
        }

        /// <summary>Draws a rectangle with the specified parameters.</summary>
        /// <param name="lineColor">Color of the line along the edge of the rectangle.</param>
        /// <param name="lineWidth">Width of the line along the edge of the rectangle.</param>
        /// <param name="fillColor">Color to fill the area enclosed by the rectangle with.</param>
        /// <param name="location">The upper left location of the rectangle.</param>
        /// <param name="size">The size of the rectangle.</param>
        public void DrawRectangle(Color lineColor, int lineWidth, Color fillColor, Point location, Size size)
        {
            this.DrawPolygon(lineColor, lineWidth, fillColor, location,
                            new Point(location.X + size.Width, location.Y),
                            new Point(location.X + size.Width, location.Y + size.Height),
                            new Point(location.X, location.Y + size.Height));
        }

        /// <summary>
        /// Draws the given text onto the <see cref="Canvas"/> at the specified location with the
        /// given font size and colour.
        /// </summary>
        /// <param name="text">The text to be drawn to the <see cref="Canvas"/>.</param>
        /// <param name="location">The lower upper left position at which the text will be drawn.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="textColor">Colour of the text.</param>
        public void DrawText(string text, Point location, float fontSize, Color textColor)
        {
            this.DrawText(text, location, fontSize, textColor, "sans serif");
        }

        /// <summary>
        /// Draws the given text onto the <see cref="Canvas"/> at the specified location with the
        /// given font face, font size and colour.
        /// </summary>
        /// <param name="text">The text to be drawn to the <see cref="Canvas"/>.</param>
        /// <param name="location">The lower upper left position at which the text will be drawn.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="textColor">Colour of the text.</param>
        /// <param name="fontName">Name of the font face to use in drawing the text.</param>
        /// <remarks>
        /// <para>
        /// Any valid font name can be specified as <paramref name="fontName"/>. Additionally,
        /// 'sans-serif', 'serif' and 'monospace' can be specified in order to draw the text with
        /// the default sans serif, serif or monospace font faces respectively.
        /// </para>
        /// </remarks>
        public void DrawText(string text, Point location, float fontSize, Color textColor, string fontName)
        {
            if (Graphics != null)
            {
                var fontIndex = "sans-serif|serif|monospace".Split('|').ToList().IndexOf(fontName.ToLower());
                if (fontIndex > -1)
                    fontName = new[] { "Segoe UI", "Constantia", "Consolas" }[fontIndex];
                var fontFamily = FontFamily.Families.FirstOrDefault(f => f.Name.ToLower() == fontName.ToLower());

                using (var font = new Font(fontFamily, fontSize * _scale))
                using (var brush = new SolidBrush(textColor))
                {
                    Graphics.DrawString(text, font, brush, CanvasToClient(location));
                }
            }
        }

        /// <summary>Computes the location of the specified canvas point into client coordinates.</summary>
        /// <param name="point">The client coordinate <see cref="Point"/> to convert.</param>
        internal PointF CanvasToClient(Point point)
        {
            //+ HIGH: Debug canvas to point code
            var ptX = point.X / _scale;
            var ptY = point.Y / _scale;

            ptX += canvasRect.X;
            ptY += canvasRect.Y;

            return new Point((int)ptX, (int)ptY);
        }

        /// <summary>Computes the location of the specified client point into canvas coordinates.</summary>
        /// <param name="point">The client coordinate <see cref="Point"/> to convert.</param>
        internal PointF ClientToCanvas(Point point)
        {
            //+ HIGH: Debug the point to canvas code
            var ptX = point.X - canvasRect.X;
            var ptY = point.Y - canvasRect.Y;

            ptX *= _scale;
            ptY *= _scale;

            return new Point((int)ptX, (int)ptY);
        }

        /// <summary>Adjusts the canvas rectangle.</summary>
        private void AdjustCanvasRectangle()
        {
            if (this.Owner == null)
            {
                _scale = 1;
                canvasRect = new RectangleF(new Point(0, 0), desiredSize);
            }
            else
            {
                var clientSz = this.Owner.ClientSize;

                if (desiredSize.IsEmpty)   // desired size = 0p x 0p
                {
                    _scale = 1;
                    canvasRect = Owner.ClientRectangle;
                }
                else
                {
                    _scale = new float[] { (float)clientSz.Width / desiredSize.Width,
                                           (float)clientSz.Height / desiredSize.Height,
                                           1f}.Min();

                    // calculate canvas rectangle
                    var newW = desiredSize.Width * _scale;
                    var newH = desiredSize.Height * _scale;
                    canvasRect = new RectangleF((clientSz.Width - newW) / 2,
                                                (clientSz.Height - newH) / 2,
                                                newW, newH);
                }
            }
        }

        /// <summary>Handles the SizeChanged event of the Owner control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Owner_SizeChanged(object sender, EventArgs e)
        {
            AdjustCanvasRectangle();
        }
    }
}