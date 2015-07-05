using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleGui
{
    internal partial class SimpleGuiForm : Form
    {
        public SimpleGuiForm()
        {
            InitializeComponent();

            // set the sizes of the form
            this.ClientSize = new Size(640, 480);
            this.MinimumSize = this.Size;

            // set the splitter positions
            spltControls.SplitterDistance = 230;
            spltControls.Panel1MinSize = 200;
            spltControls.Panel2MinSize = 128;
            spltCanvas.SplitterDistance = 300;
            spltCanvas.Panel2MinSize = 96;

            var spltExtra1 = new SplitContainerTools(spltControls) {
                KeepFocus = false,
                ShowGripper = true,
                DrawPanel1Border = true,
                Buttons = SplitContainerButtons.PanelControls
            };
            var spltExtra2 = new SplitContainerTools(spltCanvas) {
                KeepFocus = false,
                ShowGripper = true,
                DrawPanel1Border = true,
                Buttons = SplitContainerButtons.PanelControls
            };
        }

        internal void AddToControls(Label captionLabel, Control inputControl = null)
        {
            tlpControls.RowStyles.Insert(tlpControls.RowCount - 1, new RowStyle(SizeType.AutoSize));
            var row = tlpControls.RowStyles.Count - 2;

            tlpControls.Controls.Add(captionLabel, 0, row);
            if (inputControl == null)
                tlpControls.SetColumnSpan(captionLabel, 2);
            else
                tlpControls.Controls.Add(inputControl, 1, row);

            tlpControls.SetRow(lblPlaceholder, tlpControls.RowStyles.Count - 1); // move the placeholder to last row
        }

        internal void AddToControls(Control control)
        {
            tlpControls.RowStyles.Insert(tlpControls.RowCount - 1, new RowStyle(SizeType.AutoSize));
            var row = tlpControls.RowStyles.Count - 2;

            tlpControls.Controls.Add(control, 0, row);
            tlpControls.SetColumnSpan(control, 2);

            tlpControls.SetRow(lblPlaceholder, tlpControls.RowStyles.Count - 1); // move the placeholder to last row
        }

        private void DisplayPanelSize(object sender, EventArgs e)
        {
            txtLog.Text = string.Join(Environment.NewLine,
                            new[] { 
                                "Controls: " + spltControls.Panel1.ClientSize.ToString() ,
                                "Log     : " + spltCanvas.Panel2.ClientSize.ToString(),
                                "Canvas  : " + spltCanvas.Panel1.ClientSize.ToString()
                            }
                );
            txtLog.Select(0, 0);
        }
    }
}
