namespace FFXIVPlayerWardrobe.Forms
{
    partial class EditCustomizeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditCustomizeForm));
            this.raceComboBox = new System.Windows.Forms.ComboBox();
            this.tribeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.applyButton = new System.Windows.Forms.Button();
            this.legacyMarkCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.genderComboBox = new System.Windows.Forms.ComboBox();
            this.heightUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bustSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.raceFeatureSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.hairTypeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.raceFeatureTypeUpDown = new System.Windows.Forms.NumericUpDown();
            this.selectNpcButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bustSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.raceFeatureSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hairTypeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.raceFeatureTypeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // raceComboBox
            // 
            this.raceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.raceComboBox.FormattingEnabled = true;
            this.raceComboBox.Location = new System.Drawing.Point(12, 32);
            this.raceComboBox.Name = "raceComboBox";
            this.raceComboBox.Size = new System.Drawing.Size(121, 21);
            this.raceComboBox.TabIndex = 0;
            this.raceComboBox.SelectedIndexChanged += new System.EventHandler(this.raceComboBox_SelectedIndexChanged);
            // 
            // tribeComboBox
            // 
            this.tribeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tribeComboBox.FormattingEnabled = true;
            this.tribeComboBox.Location = new System.Drawing.Point(12, 76);
            this.tribeComboBox.Name = "tribeComboBox";
            this.tribeComboBox.Size = new System.Drawing.Size(121, 21);
            this.tribeComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Race";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tribe";
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(304, 251);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 4;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // legacyMarkCheckBox
            // 
            this.legacyMarkCheckBox.AutoSize = true;
            this.legacyMarkCheckBox.Location = new System.Drawing.Point(159, 121);
            this.legacyMarkCheckBox.Name = "legacyMarkCheckBox";
            this.legacyMarkCheckBox.Size = new System.Drawing.Size(88, 17);
            this.legacyMarkCheckBox.TabIndex = 5;
            this.legacyMarkCheckBox.Text = "Legacy Mark";
            this.legacyMarkCheckBox.UseVisualStyleBackColor = true;
            this.legacyMarkCheckBox.CheckedChanged += new System.EventHandler(this.legacyMarkCheckBox_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Gender";
            // 
            // genderComboBox
            // 
            this.genderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.genderComboBox.FormattingEnabled = true;
            this.genderComboBox.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.genderComboBox.Location = new System.Drawing.Point(12, 119);
            this.genderComboBox.Name = "genderComboBox";
            this.genderComboBox.Size = new System.Drawing.Size(121, 21);
            this.genderComboBox.TabIndex = 6;
            // 
            // heightUpDown
            // 
            this.heightUpDown.Location = new System.Drawing.Point(12, 163);
            this.heightUpDown.Name = "heightUpDown";
            this.heightUpDown.Size = new System.Drawing.Size(120, 20);
            this.heightUpDown.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Height";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 190);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Bust Size";
            // 
            // bustSizeUpDown
            // 
            this.bustSizeUpDown.Location = new System.Drawing.Point(12, 206);
            this.bustSizeUpDown.Name = "bustSizeUpDown";
            this.bustSizeUpDown.Size = new System.Drawing.Size(120, 20);
            this.bustSizeUpDown.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 233);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Race Feature Size";
            // 
            // raceFeatureSizeUpDown
            // 
            this.raceFeatureSizeUpDown.Location = new System.Drawing.Point(13, 249);
            this.raceFeatureSizeUpDown.Name = "raceFeatureSizeUpDown";
            this.raceFeatureSizeUpDown.Size = new System.Drawing.Size(120, 20);
            this.raceFeatureSizeUpDown.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(156, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Hair Style";
            // 
            // hairTypeUpDown
            // 
            this.hairTypeUpDown.Location = new System.Drawing.Point(159, 33);
            this.hairTypeUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.hairTypeUpDown.Name = "hairTypeUpDown";
            this.hairTypeUpDown.Size = new System.Drawing.Size(120, 20);
            this.hairTypeUpDown.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(156, 233);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Race Feature Type";
            // 
            // raceFeatureTypeUpDown
            // 
            this.raceFeatureTypeUpDown.Location = new System.Drawing.Point(159, 249);
            this.raceFeatureTypeUpDown.Name = "raceFeatureTypeUpDown";
            this.raceFeatureTypeUpDown.Size = new System.Drawing.Size(120, 20);
            this.raceFeatureTypeUpDown.TabIndex = 16;
            // 
            // selectNpcButton
            // 
            this.selectNpcButton.Location = new System.Drawing.Point(172, 175);
            this.selectNpcButton.Name = "selectNpcButton";
            this.selectNpcButton.Size = new System.Drawing.Size(75, 23);
            this.selectNpcButton.TabIndex = 18;
            this.selectNpcButton.Text = "Select NPC";
            this.selectNpcButton.UseVisualStyleBackColor = true;
            this.selectNpcButton.Click += new System.EventHandler(this.selectNpcButton_Click);
            // 
            // EditCustomizeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 286);
            this.Controls.Add(this.selectNpcButton);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.raceFeatureTypeUpDown);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.hairTypeUpDown);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.raceFeatureSizeUpDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bustSizeUpDown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.heightUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.genderComboBox);
            this.Controls.Add(this.legacyMarkCheckBox);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tribeComboBox);
            this.Controls.Add(this.raceComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditCustomizeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Body";
            this.Load += new System.EventHandler(this.EditCustomizeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bustSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.raceFeatureSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hairTypeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.raceFeatureTypeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox raceComboBox;
        private System.Windows.Forms.ComboBox tribeComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.CheckBox legacyMarkCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox genderComboBox;
        private System.Windows.Forms.NumericUpDown heightUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown bustSizeUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown raceFeatureSizeUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown hairTypeUpDown;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown raceFeatureTypeUpDown;
        private System.Windows.Forms.Button selectNpcButton;
    }
}