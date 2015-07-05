using System.Windows.Forms;

namespace SimpleGui.Controls
{
    /// <summary>
    /// A simplified class for encapsulating the <see cref="System.Windows.Forms.Label"/> control.
    /// </summary>
    public class SimpleLabel : SimpleControl<Label>
    {
        /// <summary>Initializes a new instance of the <see cref="SimpleLabel"/> class.</summary>
        /// <param name="label">The label on which this <see cref="SimpleLabel"/> is based.</param>
        internal SimpleLabel(Label label) : base(label) { }
    }
}
