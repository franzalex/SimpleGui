using SimpleGui.Controls;
using System.Windows.Forms;

namespace SimpleGui
{
    public class SimpleGui
    {
        private static SimpleGuiForm Form;
        private static SimpleGuiControlsPanel _controlsPanel;
        private static SimpleGuiCanvasPanel _canvasPanel;
        private static SimpleGuiLogPanel _logPanel;

        /// <summary>Static initializer of the <see cref="SimpleGui"/> class.</summary>
        static SimpleGui()
        {
            Form = new SimpleGuiForm();
            _controlsPanel = new SimpleGuiControlsPanel(Form.spltControls.Panel1);
            _canvasPanel = new SimpleGuiCanvasPanel(Form.spltCanvas.Panel1);
            _logPanel = new SimpleGuiLogPanel(Form.spltCanvas.Panel2);
        }

        /// <summary>Gets the canvas panel of the <see cref="SimpleGuiForm"/>.</summary>
        public static SimpleGuiCanvasPanel CanvasPanel
        {
            get { return _canvasPanel; }
        }

        /// <summary>Gets the controls panel of the <see cref="SimpleGuiForm"/>.</summary>
        public static SimpleGuiControlsPanel ControlsPanel
        {
            get { return _controlsPanel; }
        }

        /// <summary>Gets the output panel of the <see cref="SimpleGUiForm"/>.</summary>
        public static SimpleGuiLogPanel Output
        {
            get { return _logPanel; }
        }

        /// <summary>Gets or sets the title of the SimpleGui form.</summary>
        public static string WindowTitle
        {
            get { return Form.Text; }
            set { Form.Text = value; }
        }

        /// <summary>Starts running the SimpleGui program.</summary>
        public static void Start()
        {
            try
            {
                // try to run the application normally
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(Form);
            }
            catch
            {
                try
                {
                    // if it fails, display the form modally
                    Form.ShowDialog();
                }
                catch
                {
                    MessageBox.Show("Could not run the SimpleGui application.");
                }
            }
        }
    }
}
