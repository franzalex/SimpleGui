using SimpleGui.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ProjectResources = SimpleGui.Properties.Resources;

namespace SimpleGui
{
    public class SimpleGui
    {
        private SimpleGuiCanvasPanel _canvasPanel;
        private SimpleGuiControlsPanel _controlsPanel;
        private SimpleGuiLogPanel _logPanel;
        private SimpleGuiForm Form;
        private List<Timer> timers;

        /// <summary>Creates a new instance of the <see cref="SimpleGui"/> class.</summary>
        public SimpleGui()
        {
            timers = new List<Timer>();

            Form = new SimpleGuiForm() {
                Text = "Simple GUI",
                Icon = ProjectResources.AppIcon
            };
            Form.FormClosed += Form_FormClosed;

            _controlsPanel = new SimpleGuiControlsPanel(Form.spltControls.Panel1);
            _canvasPanel = new SimpleGuiCanvasPanel(Form.spltCanvas.Panel1);
            _logPanel = new SimpleGuiLogPanel(Form.spltCanvas.Panel2);
        }

        /// <summary>Gets the canvas panel of the <see cref="SimpleGuiForm"/>.</summary>
        public SimpleGuiCanvasPanel CanvasPanel
        {
            get { return _canvasPanel; }
        }

        /// <summary>Gets the controls panel of the <see cref="SimpleGuiForm"/>.</summary>
        public SimpleGuiControlsPanel ControlsPanel
        {
            get { return _controlsPanel; }
        }

        /// <summary>Gets the output panel of the <see cref="SimpleGuiForm"/>.</summary>
        public SimpleGuiLogPanel Output
        {
            get { return _logPanel; }
        }

        /// <summary>Gets or sets the title of the SimpleGui form.</summary>
        public string WindowTitle
        {
            get { return Form.Text; }
            set { Form.Text = value; }
        }

        /// <summary>Creates timer associated with the form.</summary>
        /// <param name="interval">
        /// The interval in milliseconds between consecutive ticks of the timer.
        /// </param>
        /// <param name="tickHandler">The event handler for the timer's tick.</param>
        public SimpleTimer CreateTimer(int interval, SimpleEventHandler tickHandler)
        {
            return this.CreateTimer(interval, (tmr) => tickHandler.Invoke());
        }

        /// <summary>Starts running the SimpleGui program.</summary>
        public void Start()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.Run(Form);
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof(ObjectDisposedException))
                    MessageBox.Show("Could not run the SimpleGui application.");
            }
        }

        /// <summary>Creates timer associated with the form.</summary>
        /// <param name="interval">
        /// The interval in milliseconds between consecutive ticks of the timer.
        /// </param>
        /// <param name="tickHandler">The event handler for the timer's tick.</param>
        /// <returns></returns>
        private SimpleTimer CreateTimer(int interval, MultiControlEventHandler tickHandler)
        {
            var tmr = new Timer() { Interval = interval };
            var sTmr = new SimpleTimer(tmr);

            timers.Add(tmr);
            tmr.Tick += (o, e) => tickHandler(sTmr);

            return sTmr;
        }

        /// <summary>Handles the FormClosed event of the Form control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            // stop and dispose all timers that have been created
            foreach (var tmr in timers)
            {
                tmr.Stop();
                tmr.Dispose();
            }
        }
    }
}