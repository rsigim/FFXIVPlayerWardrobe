namespace FFXIVPlayerWardrobe
{
    partial class ResidentSelectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResidentSelectForm));
            this.residentGridView = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyCustomizeToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.okButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.searchNextButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.residentGridView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // residentGridView
            // 
            this.residentGridView.AllowUserToAddRows = false;
            this.residentGridView.AllowUserToDeleteRows = false;
            this.residentGridView.AllowUserToResizeColumns = false;
            this.residentGridView.AllowUserToResizeRows = false;
            this.residentGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.residentGridView.ContextMenuStrip = this.contextMenuStrip1;
            this.residentGridView.Location = new System.Drawing.Point(12, 12);
            this.residentGridView.MultiSelect = false;
            this.residentGridView.Name = "residentGridView";
            this.residentGridView.ReadOnly = true;
            this.residentGridView.Size = new System.Drawing.Size(924, 332);
            this.residentGridView.TabIndex = 0;
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
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(861, 350);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(14, 352);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(174, 20);
            this.textBox1.TabIndex = 2;
            // 
            // searchNextButton
            // 
            this.searchNextButton.Location = new System.Drawing.Point(194, 350);
            this.searchNextButton.Name = "searchNextButton";
            this.searchNextButton.Size = new System.Drawing.Size(75, 23);
            this.searchNextButton.TabIndex = 3;
            this.searchNextButton.Text = "Search Next";
            this.searchNextButton.UseVisualStyleBackColor = true;
            this.searchNextButton.Click += new System.EventHandler(this.searchNextButton_Click);
            // 
            // ResidentSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 379);
            this.Controls.Add(this.searchNextButton);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.residentGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResidentSelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Resident";
            ((System.ComponentModel.ISupportInitialize)(this.residentGridView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView residentGridView;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button searchNextButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyCustomizeToClipboardToolStripMenuItem;
    }
}