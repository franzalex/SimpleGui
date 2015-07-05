using System.Windows.Forms;

namespace SimpleGui.Controls
{
    /// <summary>
    /// A simplified class for encapsulating the <see cref="System.Windows.Forms.Button"/> control.
    /// </summary>
    public class SimpleButton : SimpleControl<Button>
    {
        /// <summary>Initializes a new instance of the <see cref="SimpleButton"/> class.</summary>
        /// <param name="button">The button on which this  <see cref="SimpleButton"/> is based.</param>
        internal SimpleButton(Button button) : base(button) { }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return base.StandardControl.Text + " (Button)";
        }
    }
}
