using System.ComponentModel;
using Timer = System.Windows.Forms.Timer;

namespace SimpleGui.Controls
{
    /// <summary>A timer calls an event handler repeatedly at a specified interval.</summary>
    /// <remarks>
    /// A program can have an arbitrary number of timers running simultaneously. However, having
    /// many timers running will slow SimpleGui.
    /// </remarks>
    public class SimpleTimer : SimpleControl
    {
        private Timer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTimer"/> class based on the specified <see cref="Timer"/>.
        /// </summary>
        public SimpleTimer(Timer timer)
            : base(null)
        {
            this.timer = timer;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string Caption
        {
            get { return ""; }
            set { ; }
        }

        /// <summary>
        /// Gets or sets the time interval in milliseconds between two consecutive ticks.
        /// </summary>
        public int Interval
        {
            get { return this.StandardControl.Interval; }
            set { this.StandardControl.Interval = value; }
        }

        /// <summary>Gets a value indicating whether the timer is running.</summary>
        /// <value><c>true</c> if the timer is running; otherwise, <c>false</c>.</value>
        public bool IsRunning
        {
            get { return this.StandardControl.Enabled; }
        }

        /// <summary>
        /// Gets the standard <c>System.Windows.Forms</c> control encapsulated by this instance.
        /// </summary>
        public new Timer StandardControl
        {
            get
            {
                return this.timer;
            }
        }

        /// <summary>Starts the timer.</summary>
        public void Start()
        {
            this.StandardControl.Start();
        }

        /// <summary>Stops the timer.</summary>
        public void Stop()
        {
            this.StandardControl.Stop();
        }

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("Timer: {0} ms ({1})", this.Interval, this.IsRunning ? "Running" : "Stopped");
        }
    }
}