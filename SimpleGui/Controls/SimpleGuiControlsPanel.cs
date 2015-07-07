using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleGui.Controls
{
    public class SimpleGuiControlsPanel : SimpleGuiPanel
    {
        private SimpleGuiForm Form;

        public SimpleGuiControlsPanel(SplitterPanel splitterPanel)
            : base(splitterPanel)
        {
            this.Form = this.Parent.ParentForm as SimpleGuiForm;
        }

        /// <summary>Adds a button to the form.</summary>
        /// <param name="caption">The caption to display on the button.</param>
        /// <param name="clickHandler">The click event handler.</param>
        /// <param name="buttonWidth">Width of the button.</param>
        public SimpleButton AddButton(string caption, SimpleEventHandler clickHandler, int buttonWidth = -1)
        {
            return this.AddButton(caption, (sbtn) => clickHandler.Invoke(), buttonWidth);
        }

        /// <summary>Adds a button to the form.</summary>
        /// <param name="caption">The caption to display on the button.</param>
        /// <param name="clickHandler">The click event handler.</param>
        /// <param name="buttonWidth">Width of the button.</param>
        public SimpleButton AddButton(string caption, MultiControlEventHandler clickHandler, int buttonWidth = -1)
        {
            var btn = new Button() {
                Dock = DockStyle.Left,
                Text = caption,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // set the button's width
            if (buttonWidth > 0)
            {
                btn.Width = buttonWidth;
            }
            else
            {
                btn.AutoSize = true;
                btn.AutoSizeMode = AutoSizeMode.GrowOnly;
                btn.MinimumSize = new Size(Math.Max(btn.Size.Width, btn.PreferredSize.Width), 0);
                btn.MaximumSize = new Size(0, Math.Max(btn.Size.Height, btn.PreferredSize.Height));
            }

            this.Form.AddToControls(btn);

            var sbtn = new SimpleButton(btn);
            btn.Click += (o, e) => clickHandler.Invoke(sbtn);

            return sbtn;
        }

        /// <summary>Adds a ComboBox to the form.</summary>
        /// <param name="caption">The ComboBox's caption.</param>
        /// <param name="selectionHandler">The handler for the selection changed event.</param>
        /// <param name="options">The options to display in the ComboBox.</param>
        public SimpleComboBox AddComboBox(string caption, SimpleTextEventHandler selectionHandler, params string[] options)
        {
            return this.AddComboBox(caption, (scb, str) => selectionHandler.Invoke(str), options);
        }

        /// <summary>Adds a ComboBox to the form.</summary>
        /// <param name="caption">The ComboBox's caption.</param>
        /// <param name="selectionHandler">The handler for the selection changed event.</param>
        /// <param name="options">The options to display in the ComboBox.</param>
        public SimpleComboBox AddComboBox(string caption, MultiControlTextEventHandler selectionHandler, params string[] options)
        {
            var lbl = new Label() {
                Text = caption,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            var cbo = new ComboBox() {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            cbo.Items.AddRange(options);
            this.Form.AddToControls(lbl, cbo);

            var scbo = new SimpleComboBox(lbl, cbo);
            cbo.SelectedIndexChanged += (o, e) => selectionHandler.Invoke(scbo, cbo.Items[cbo.SelectedIndex].ToString());

            return scbo;
        }

        /// <summary>Adds a label to the form.</summary>
        /// <param name="caption">The caption to be displayed on the label.</param>
        /// <returns></returns>
        public SimpleLabel AddLabel(string caption)
        {
            var lbl = new Label() {
                Text = caption,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            this.Form.AddToControls(lbl);

            return new SimpleLabel(lbl);
        }

        /// <summary>Adds a text box to the form.</summary>
        /// <param name="caption">The caption for the text box.</param>
        /// <param name="inputHandler">The event handler for text input.</param>
        public SimpleTextBox AddTextBox(string caption, SimpleTextEventHandler inputHandler)
        {
            return this.AddTextBox(caption, (stb, txt) => inputHandler.Invoke(txt));
        }

        /// <summary>Adds a text box to the form.</summary>
        /// <param name="caption">The caption for the text box.</param>
        /// <param name="inputHandler">The event handler for text input.</param>
        public SimpleTextBox AddTextBox(string caption, MultiControlTextEventHandler inputHandler)
        {
            var lbl = new Label() {
                Text = caption,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            var txt = new TextBox() { Dock = DockStyle.Fill };
            this.Form.AddToControls(lbl, txt);

            var stb = new SimpleTextBox(lbl, txt);
            txt.KeyDown += (o, e) => { if (e.KeyCode == Keys.Enter) inputHandler.Invoke(stb, txt.Text); };

            return stb;
        }
    }
}