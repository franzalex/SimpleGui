using System.Windows.Forms;

namespace SimpleGui.Controls
{
    /// <summary>
    /// A simple class encapsulating the <see cref="System.Windows.Forms.TextBox"/> control.
    /// </summary>
    public class SimpleTextBox : SimpleControl<TextBox>
    {
        private Label _lbl;

        /// <summary>Initializes a new instance of the <see cref="SimpleTextBox"/> class.</summary>
        /// <param name="captionLabel">The caption label.</param>
        /// <param name="textBox">The text box.</param>
        public SimpleTextBox(Label captionLabel, TextBox textBox)
            : base(textBox)
        {
            this._lbl = captionLabel;
        }

        /// <summary>Gets or sets the caption.</summary>
        /// <value>The caption.</value>
        public override string Caption
        {
            get { return _lbl.Text; }
            set { _lbl.Text = value; }
        }

        /// <summary>Gets or sets the input text of this <see cref="SimpleTextBox"/>.</summary>
        public string Text
        {
            get { return this.StandardControl.Text; }
            set { this.StandardControl.Text = value; }
        }

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return (!string.IsNullOrEmpty(this.Caption) ? this.Caption + " :" : "") + this.Text;
        }
    }
}