using System.Windows.Forms;

namespace SimpleGui.Controls
{
    /// <summary>
    /// A simple class encapsulating the <see cref="System.Windows.Forms.ComboBox"/> control.
    /// </summary>
    public class SimpleComboBox : SimpleControl<ComboBox>
    {
        private Label _label;

        public SimpleComboBox(Label captionLabel, ComboBox cbo)
            : base(cbo)
        {
            _label = captionLabel;
        }

        /// <summary>Gets or sets the caption.</summary>
        /// <value>The caption.</value>
        public override string Caption
        {
            get { return _label.Text; }
            set { _label.Text = value; }
        }

        /// <summary>Gets or sets the input text of this <see cref="SimpleTextBox"/>.</summary>
        public string Text
        {
            get
            {
                return this.StandardControl.SelectedIndex >= 0 ?
                       this.StandardControl.SelectedItem.ToString() : "";
            }
            set
            {
                for (int i = 0; i < this.StandardControl.Items.Count; i++)
                {
                    if (this.StandardControl.Items[i].ToString().Equals(value, System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.StandardControl.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return (!string.IsNullOrEmpty(this.Caption) ? this.Caption + " :" : "") + this.Text;
        }
    }
}