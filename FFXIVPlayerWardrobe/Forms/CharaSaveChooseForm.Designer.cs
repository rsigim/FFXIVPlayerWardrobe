namespace FFXIVPlayerWardrobe
{
    partial class CharaSaveChooseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CharaSaveChooseForm));
            this.charaSaveListBox = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyCustomizeToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // charaSaveListBox
            // 
            this.charaSaveListBox.ContextMenuStrip = this.contextMenuStrip1;
            this.charaSaveListBox.FormattingEnabled = true;
            this.charaSaveListBox.Location = new System.Drawing.Point(12, 37);
            this.charaSaveListBox.Name = "charaSaveListBox";
            this.charaSaveListBox.Size = new System.Drawing.Size(274, 277);
            this.charaSaveListBox.TabIndex = 0;
            this.charaSaveListBox.DoubleClick += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyCustomizeToClipboardToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(227, 26);
            // 
            // copyCustomizeToClipboardToolStripMenuItem
            // 
            this.copyCustomizeToClipboardToolStripMenuItem.Name = "copyCustomizeToClipboardToolStripMenuItem";
            this.copyCustomizeToClipboardToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.copyCustomizeToClipboardToolStripMenuItem.Text = "Copy customize to clipboard";
            this.copyCustomizeToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyCustomizeToClipboardToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(211, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(9, 9);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(263, 13);
            this.infoLabel.TabIndex = 2;
            this.infoLabel.Text = "Please choose the exact character you are playing as.";
            // 
            // CharaSaveChooseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 349);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.charaSaveListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CharaSaveChooseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose Character Save File";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox charaSaveListBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyCustomizeToClipboardToolStripMenuItem;
    }
}