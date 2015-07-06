using SimpleGui.Controls;
using System;
using System.Windows.Forms;
using ProjectResources = SimpleGui.Properties.Resources;

namespace SimpleGui
{
    public class SimpleGui
    {
        private SimpleGuiForm Form;
        private SimpleGuiControlsPanel _controlsPanel;
        private SimpleGuiCanvasPanel _canvasPanel;
        private SimpleGuiLogPanel _logPanel;

        /// <summary>Creates a new instance of the <see cref="SimpleGui"/> class.</summary>
        public SimpleGui()
        {
            Form = new SimpleGuiForm() {
                Text = "Simple GUI",
                Icon = ProjectResources.AppIcon
            };

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

        /// <summary>Gets the output panel of the <see cref="SimpleGUiForm"/>.</summary>
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
    }
}
