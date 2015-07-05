
namespace SimpleGui.Controls
{
    public class SimpleGuiLogPanel : SimpleGuiPanel
    {
        private SimpleGuiForm Form;

        public SimpleGuiLogPanel(System.Windows.Forms.SplitterPanel splitterPanel)
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

        /// <summary>Prints the specified message to the form's output.</summary>
        /// <param name="message">The message to be printed.</param>
        public void Print(string message)
        {
            this.Form.txtLog.AppendText(message + "\r\n");
            this.Form.txtLog.Select(this.Form.txtLog.TextLength - 1, 0);
            this.Form.txtLog.ScrollToCaret();
        }

        /// <summary>Clears all text printed on the output.</summary>
        public void Clear()
        {
            this.Form.txtLog.Clear();
            this.Form.txtLog.ClearUndo();
        }

    }
}
