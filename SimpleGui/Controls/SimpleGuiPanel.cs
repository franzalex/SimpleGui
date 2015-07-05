using System.Windows.Forms;

namespace SimpleGui.Controls
{
    public abstract class SimpleGuiPanel
    {
        private bool isPanel1;
        private SplitterPanel panel;
        private SplitContainer split;

        /// <summary>Initializes a new instance of the <see cref="SimpleGuiPanel"/> class.</summary>
        /// <param name="panel">The panel to which this <see cref="SimpleGuiPanel"/> is bound.</param>
        internal SimpleGuiPanel(SplitterPanel panel)
        {
            this.panel = panel;
            this.split = (SplitContainer)((Control)panel).Parent;
            this.isPanel1 = split.Panel1.Equals(panel);
        }

        /// <summary>Gets the panel to which this <see cref="SimpleGuiPanel"/> is bound.</summary>
        protected internal SplitterPanel Panel { get { return panel; } }

        /// <summary>Gets or sets a value indicating whether the panel is visible.</summary>
        /// <value><c>true</c> if the panel is visible; otherwise, <c>false</c>.</value>
        public virtual bool Visible
        {
            get
            {
                if (isPanel1)
                    return !this.Parent.Panel1Collapsed;
                else
                    return !this.Parent.Panel2Collapsed;
            }
            set
            {
                if (isPanel1)
                    this.Parent.Panel1Collapsed = !value;
                else
                    this.Parent.Panel2Collapsed = !value;
            }
        }

        /// <summary>Gets a value indicating whether this is the Panel1 of the <see cref="SimpleGuiPanel.Parent"/>.</summary>
        protected bool IsPanel1 { get { return isPanel1; } }

        /// <summary>Gets the parent.</summary>
        protected SplitContainer Parent { get { return split; } }
    }
}