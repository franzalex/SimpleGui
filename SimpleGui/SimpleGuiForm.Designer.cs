namespace SimpleGui
{
    partial class SimpleGuiForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        internal System.ComponentModel.IContainer Components
        {
            get { return components; }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.spltControls = new System.Windows.Forms.SplitContainer();
            this.tlpControls = new System.Windows.Forms.TableLayoutPanel();
            this.lblPlaceholder = new System.Windows.Forms.Label();
            this.spltCanvas = new System.Windows.Forms.SplitContainer();
            this.txtLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.spltControls)).BeginInit();
            this.spltControls.Panel1.SuspendLayout();
            this.spltControls.Panel2.SuspendLayout();
            this.spltControls.SuspendLayout();
            this.tlpControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltCanvas)).BeginInit();
            this.spltCanvas.Panel2.SuspendLayout();
            this.spltCanvas.SuspendLayout();
            this.SuspendLayout();
            // 
            // spltControls
            // 
            this.spltControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltControls.Location = new System.Drawing.Point(0, 0);
            this.spltControls.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.spltControls.Name = "spltControls";
            // 
            // spltControls.Panel1
            // 
            this.spltControls.Panel1.Controls.Add(this.tlpControls);
            this.spltControls.Panel1.Padding = new System.Windows.Forms.Padding(1);
            // 
            // spltControls.Panel2
            // 
            this.spltControls.Panel2.Controls.Add(this.spltCanvas);
            this.spltControls.Size = new System.Drawing.Size(331, 343);
            this.spltControls.SplitterDistance = 109;
            this.spltControls.SplitterWidth = 12;
            this.spltControls.TabIndex = 0;
            this.spltControls.TabStop = false;
            // 
            // tlpControls
            // 
            this.tlpControls.ColumnCount = 2;
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpControls.Controls.Add(this.lblPlaceholder, 0, 0);
            this.tlpControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpControls.Location = new System.Drawing.Point(1, 1);
            this.tlpControls.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tlpControls.Name = "tlpControls";
            this.tlpControls.RowCount = 1;
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpControls.Size = new System.Drawing.Size(107, 341);
            this.tlpControls.TabIndex = 0;
            // 
            // lblPlaceholder
            // 
            this.lblPlaceholder.AutoSize = true;
            this.tlpControls.SetColumnSpan(this.lblPlaceholder, 2);
            this.lblPlaceholder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPlaceholder.Location = new System.Drawing.Point(3, 0);
            this.lblPlaceholder.Name = "lblPlaceholder";
            this.lblPlaceholder.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.lblPlaceholder.Size = new System.Drawing.Size(101, 341);
            this.lblPlaceholder.TabIndex = 0;
            this.lblPlaceholder.Text = " ";
            // 
            // spltCanvas
            // 
            this.spltCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltCanvas.Location = new System.Drawing.Point(0, 0);
            this.spltCanvas.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.spltCanvas.Name = "spltCanvas";
            this.spltCanvas.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spltCanvas.Panel2
            // 
            this.spltCanvas.Panel2.Controls.Add(this.txtLog);
            this.spltCanvas.Size = new System.Drawing.Size(210, 343);
            this.spltCanvas.SplitterDistance = 261;
            this.spltCanvas.SplitterWidth = 13;
            this.spltCanvas.TabIndex = 0;
            this.spltCanvas.TabStop = false;
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(210, 69);
            this.txtLog.TabIndex = 0;
            // 
            // SimpleGuiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 343);
            this.Controls.Add(this.spltControls);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SimpleGuiForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.spltControls.Panel1.ResumeLayout(false);
            this.spltControls.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltControls)).EndInit();
            this.spltControls.ResumeLayout(false);
            this.tlpControls.ResumeLayout(false);
            this.tlpControls.PerformLayout();
            this.spltCanvas.Panel2.ResumeLayout(false);
            this.spltCanvas.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltCanvas)).EndInit();
            this.spltCanvas.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpControls;
        private System.Windows.Forms.Label lblPlaceholder;
        internal System.Windows.Forms.TextBox txtLog;
        internal System.Windows.Forms.SplitContainer spltControls;
        internal System.Windows.Forms.SplitContainer spltCanvas;
    }
}

