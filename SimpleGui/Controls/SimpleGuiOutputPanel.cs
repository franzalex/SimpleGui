
namespace SimpleGui.Controls
{
    public class SimpleGuiOutputPanel : SimpleGuiPanel
    {
        private SimpleGuiForm Form;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleGuiOutputPanel"/> class.
        /// </summary>
        /// <param name="splitterPanel">The splitter panel.</param>
        public SimpleGuiOutputPanel(System.Windows.Forms.SplitterPanel splitterPanel)
            : base(splitterPanel)
        {
            this.Form = this.Parent.ParentForm as SimpleGuiForm;
        }

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

        /// <summary>Writes the specified message to the form's output.</summary>
        /// <param name="message">The message to be written to the output.</param>
        public void Write(string message)
        {
            this.Form.txtLog.AppendText(message + "\r\n");
            this.Form.txtLog.Select(this.Form.txtLog.TextLength - 1, 0);
            this.Form.txtLog.ScrollToCaret();
        }

        /// <summary>Writes the string representation of the specified object to the form's output.</summary>
        /// <param name="obj">The object whose string representation is to be written to the output.</param>
        public void Write(object obj)
        {
            this.Write(obj.ToString());
        }

        /// <summary>Clears all text printed on the output.</summary>
        public void Clear()
        {
            this.Form.txtLog.Clear();
            this.Form.txtLog.ClearUndo();
        }

    }
}
