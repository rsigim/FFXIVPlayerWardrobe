namespace FFXIVPlayerWardrobe.Forms
{
    partial class TimeOffsetSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeOffsetSelector));
            this.label1 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.timeUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(323, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select the time in hours you want to offset your current eorzea time.";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(260, 50);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // timeUpDown
            // 
            this.timeUpDown.DecimalPlaces = 1;
            this.timeUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.timeUpDown.Location = new System.Drawing.Point(15, 34);
            this.timeUpDown.Name = "timeUpDown";
            this.timeUpDown.Size = new System.Drawing.Size(176, 20);
            this.timeUpDown.TabIndex = 5;
            this.timeUpDown.ValueChanged += new System.EventHandler(this.timeUpDown_ValueChanged);
            // 
            // TimeOffsetSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 82);
            this.Controls.Add(this.timeUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TimeOffsetSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Time Offset";
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.NumericUpDown timeUpDown;
    }
}