using System.Windows.Forms;

namespace SimpleGui.Controls
{
    public abstract class SimpleControl<T> where T : Control
    {
        private T ctrl;

        /// <summary>Initializes a new instance of the <see cref="SimpleControl{T}"/> class.</summary>
        /// <param name="control">The underlying control.</param>
        protected internal SimpleControl(T control)
        {
            this.ctrl = control;
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
        public T StandardControl
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
}