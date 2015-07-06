using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleGui.Controls
{
    /// <summary>Represents the method that will handle canvas drawing events.</summary>
    /// <param name="canvas">The canvas to be drawn.</param>
    public delegate void DrawEventHandler(Drawing.Canvas canvas);

    /// <summary>Represents the method that will handle keyboard events.</summary>
    /// <param name="key">The key that triggered the event.</param>
    public delegate void KeyEventHandler(Keys key);

    /// <summary>Represents the method that will handle mouse events.</summary>
    /// <param name="location">The location at which the event occurred.</param>
    /// <param name="button">The mouse button that triggered the event.</param>
    public delegate void MouseEventHandler(Point location, MouseButtons button);

    /// <summary>
    /// Represents the method that will handle events raised by one or more instances of the
    /// <see cref="SimpleControl"/> class.
    /// </summary>
    /// <param name="control">The <see cref="SimpleControl"/> that raised the event.</param>
    public delegate void MultiControlEventHandler(SimpleControl control);

    /// <summary>
    /// Represents the method that will handle events raised by one or more instances of the
    /// <see cref="SimpleControl" /> class that involves text.
    /// </summary>
    /// <param name="control">The <see cref="SimpleControl" /> that raised the event.</param>
    /// <param name="text">The text associated with the event.</param>
    public delegate void MultiControlTextEventHandler(SimpleControl control, string text);

    /// <summary>Represents the method that will handle simple events raised by a <see cref="SimpleControl"/>.</summary>
    public delegate void SimpleEventHandler();

    /// <summary>
    /// Represents the method that will handle simple events raised by a <see cref="SimpleControl"/>
    /// that involves text.
    /// </summary>
    /// <param name="text">The text associated with the event.</param>
    public delegate void SimpleTextEventHandler(string text);
}
