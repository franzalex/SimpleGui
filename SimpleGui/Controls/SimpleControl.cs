using System.Windows.Forms;

namespace SimpleGui.Controls
{
    public abstract class SimpleControl
    {
        private Control ctrl;

        protected internal SimpleControl(Control ctl)
        {
            this.ctrl = ctl;
        }

        /// <summary>Gets or sets the caption.</summary>
        /// <value>The caption to display.</value>
        public virtual string Caption
        {
            get { return ctrl.Text; }
            set { ctrl.Text = value; }
        }

        /// <summary>
        /// Gets the standard <c>System.Windows.Forms</c> control encapsulated by this instance.
        /// </summary>
        public virtual Control StandardControl
        {
            get { return ctrl; }
        }

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ctrl.Text;
        }
    }

    public abstract class SimpleControl<T> : SimpleControl where T : Control
    {
        /// <summary>Initializes a new instance of the <see cref="SimpleControl{T}"/> class.</summary>
        /// <param name="control">The underlying control.</param>
        protected internal SimpleControl(T control) : base(control) { }

        /// <summary>
        /// Gets the standard <c>System.Windows.Forms</c> control encapsulated by this instance.
        /// </summary>
        public new virtual T StandardControl
        {
            get { return (T)base.StandardControl; }
        }
    }
}