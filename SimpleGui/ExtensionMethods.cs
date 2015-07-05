using System.Drawing;
using System.Windows.Forms;

namespace SimpleGui
{
    internal static class ExtensionMethods
    {
        /// <summary>Finds the focused control.</summary>
        /// <param name="control">The control whose children are to be searched.</param>
        /// <returns>
        /// A <see cref="T:System.Windows.Forms.Control"/> hosted by <paramref name="control"/> or
        /// one of its child controls that has input focus.
        /// </returns>
        public static Control FindFocusedControl(this Control control)
        {
            if (!control.ContainsFocus)     // control doesn't contain focus
                return null;

            if (control is ContainerControl)    // check if control is a container control
                return FindFocusedControl(((ContainerControl)control).ActiveControl);
            else
                return control;
        }

        /// <summary>Converts a <see cref="System.Drawing.PointF"/> to a <see cref="System.Drawing.Point"/>.</summary>
        /// <param name="pt">The <see cref="PointF"/> instance to be converted.</param>
        public static Point ToPoint(this PointF pt)
        {
            return new Point((int)pt.X, (int)pt.Y);
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.RectangleF" /> structure to a <see cref="System.Drawing.Rectangle" />.
        /// </summary>
        /// <param name="rect">The <see cref="System.Drawing.RectangleF" /> instance to be converted.</param>
        public static Rectangle ToRectangle(this RectangleF rect)
        {
            return new Rectangle(rect.Location.ToPoint(), rect.Size.ToSize());
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.SizeF" /> structure to a <see cref="System.Drawing.Size" /> structure.
        /// </summary>
        /// <param name="rect">The <see cref="System.Drawing.SizeF" /> instance to be converted.</param>
        public static Size ToSize(this SizeF size)
        {
            return new Size((int)size.Width, (int)size.Height);
        }
    }
}