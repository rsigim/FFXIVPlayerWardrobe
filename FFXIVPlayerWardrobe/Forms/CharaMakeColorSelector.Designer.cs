namespace FFXIVPlayerWardrobe.Forms
{
    partial class CharaMakeColorSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CharaMakeColorSelector));
            this.colorListView = new System.Windows.Forms.ListView();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // colorListView
            // 
            this.colorListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorListView.Location = new System.Drawing.Point(12, 12);
            this.colorListView.MultiSelect = false;
            this.colorListView.Name = "colorListView";
            this.colorListView.Size = new System.Drawing.Size(691, 401);
            this.colorListView.TabIndex = 0;
            this.colorListView.UseCompatibleStateImageBehavior = false;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(628, 419);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // CharaMakeColorSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 450);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.colorListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CharaMakeColorSelector";
            this.Text = "Select Color";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView colorListView;
        private System.Windows.Forms.Button okButton;
    }
}