﻿namespace Localizationeer
{
    partial class frmMain
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
			this.oflSelectFile = new System.Windows.Forms.OpenFileDialog();
			this.btnSelectFile = new System.Windows.Forms.Button();
			this.txtOutput = new System.Windows.Forms.TextBox();
			this.btnSelectFolder = new System.Windows.Forms.Button();
			this.fbdSelectFolder = new System.Windows.Forms.FolderBrowserDialog();
			this.tbxSelectedFolder = new System.Windows.Forms.TextBox();
			this.tbxSelectedFile = new System.Windows.Forms.TextBox();
			this.btnApply = new System.Windows.Forms.Button();
			this.lblSelectedFolder = new System.Windows.Forms.Label();
			this.lblSelectedFile = new System.Windows.Forms.Label();
			this.lblOutput = new System.Windows.Forms.Label();
			this.btnAbout = new System.Windows.Forms.Button();
			this.NudEnglish = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.NudID = new System.Windows.Forms.NumericUpDown();
			this.cbxOption = new System.Windows.Forms.ComboBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.btnReset = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.NudEnglish)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.NudID)).BeginInit();
			this.SuspendLayout();
			// 
			// btnSelectFile
			// 
			this.btnSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectFile.Location = new System.Drawing.Point(487, 110);
			this.btnSelectFile.Name = "btnSelectFile";
			this.btnSelectFile.Size = new System.Drawing.Size(123, 28);
			this.btnSelectFile.TabIndex = 7;
			this.btnSelectFile.Text = "Select File";
			this.btnSelectFile.UseVisualStyleBackColor = true;
			this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
			// 
			// txtOutput
			// 
			this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtOutput.Location = new System.Drawing.Point(12, 222);
			this.txtOutput.Multiline = true;
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.ReadOnly = true;
			this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtOutput.Size = new System.Drawing.Size(598, 296);
			this.txtOutput.TabIndex = 15;
			// 
			// btnSelectFolder
			// 
			this.btnSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectFolder.Location = new System.Drawing.Point(487, 61);
			this.btnSelectFolder.Name = "btnSelectFolder";
			this.btnSelectFolder.Size = new System.Drawing.Size(123, 28);
			this.btnSelectFolder.TabIndex = 4;
			this.btnSelectFolder.Text = "Select Folder";
			this.btnSelectFolder.UseVisualStyleBackColor = true;
			this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
			// 
			// tbxSelectedFolder
			// 
			this.tbxSelectedFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxSelectedFolder.Location = new System.Drawing.Point(12, 64);
			this.tbxSelectedFolder.Name = "tbxSelectedFolder";
			this.tbxSelectedFolder.Size = new System.Drawing.Size(467, 22);
			this.tbxSelectedFolder.TabIndex = 3;
			this.tbxSelectedFolder.TextChanged += new System.EventHandler(this.tbxSelectedFolder_TextChanged);
			// 
			// tbxSelectedFile
			// 
			this.tbxSelectedFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxSelectedFile.Location = new System.Drawing.Point(12, 113);
			this.tbxSelectedFile.Name = "tbxSelectedFile";
			this.tbxSelectedFile.Size = new System.Drawing.Size(467, 22);
			this.tbxSelectedFile.TabIndex = 6;
			this.tbxSelectedFile.TextChanged += new System.EventHandler(this.tbxSelectedFile_TextChanged);
			// 
			// btnApply
			// 
			this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnApply.Location = new System.Drawing.Point(486, 553);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(123, 28);
			this.btnApply.TabIndex = 19;
			this.btnApply.Text = "Apply";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// lblSelectedFolder
			// 
			this.lblSelectedFolder.AutoSize = true;
			this.lblSelectedFolder.Location = new System.Drawing.Point(12, 44);
			this.lblSelectedFolder.Name = "lblSelectedFolder";
			this.lblSelectedFolder.Size = new System.Drawing.Size(463, 17);
			this.lblSelectedFolder.TabIndex = 2;
			this.lblSelectedFolder.Text = "The app/src/main/res folder which contains your values*/strings.xml files:";
			// 
			// lblSelectedFile
			// 
			this.lblSelectedFile.AutoSize = true;
			this.lblSelectedFile.Location = new System.Drawing.Point(12, 93);
			this.lblSelectedFile.Name = "lblSelectedFile";
			this.lblSelectedFile.Size = new System.Drawing.Size(318, 17);
			this.lblSelectedFile.TabIndex = 5;
			this.lblSelectedFile.Text = "The Excel file with string ids and localized strings:";
			// 
			// lblOutput
			// 
			this.lblOutput.AutoSize = true;
			this.lblOutput.Location = new System.Drawing.Point(12, 202);
			this.lblOutput.Name = "lblOutput";
			this.lblOutput.Size = new System.Drawing.Size(55, 17);
			this.lblOutput.TabIndex = 14;
			this.lblOutput.Text = "Output:";
			// 
			// btnAbout
			// 
			this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAbout.Location = new System.Drawing.Point(12, 553);
			this.btnAbout.Name = "btnAbout";
			this.btnAbout.Size = new System.Drawing.Size(123, 28);
			this.btnAbout.TabIndex = 17;
			this.btnAbout.Text = "About";
			this.btnAbout.UseVisualStyleBackColor = true;
			this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
			// 
			// NudEnglish
			// 
			this.NudEnglish.Location = new System.Drawing.Point(177, 171);
			this.NudEnglish.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.NudEnglish.Name = "NudEnglish";
			this.NudEnglish.Size = new System.Drawing.Size(120, 22);
			this.NudEnglish.TabIndex = 11;
			this.NudEnglish.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.NudEnglish.ValueChanged += new System.EventHandler(this.NudEnglish_ValueChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 173);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(140, 17);
			this.label2.TabIndex = 10;
			this.label2.Text = "English column index";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 144);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(107, 17);
			this.label1.TabIndex = 8;
			this.label1.Text = "ID column index";
			// 
			// NudID
			// 
			this.NudID.Location = new System.Drawing.Point(177, 142);
			this.NudID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.NudID.Name = "NudID";
			this.NudID.Size = new System.Drawing.Size(120, 22);
			this.NudID.TabIndex = 9;
			this.NudID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.NudID.ValueChanged += new System.EventHandler(this.NudID_ValueChanged);
			// 
			// cbxOption
			// 
			this.cbxOption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbxOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxOption.FormattingEnabled = true;
			this.cbxOption.Items.AddRange(new object[] {
            "Excel to Android xml files",
            "Xliff iOS files to Excel"});
			this.cbxOption.Location = new System.Drawing.Point(12, 13);
			this.cbxOption.Name = "cbxOption";
			this.cbxOption.Size = new System.Drawing.Size(597, 24);
			this.cbxOption.TabIndex = 1;
			this.cbxOption.SelectedIndexChanged += new System.EventHandler(this.cbxOption_SelectedIndexChanged);
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.Location = new System.Drawing.Point(12, 524);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(598, 23);
			this.progressBar.TabIndex = 16;
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(141, 553);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(123, 28);
			this.btnReset.TabIndex = 18;
			this.btnReset.Text = "Reset";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(622, 593);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.cbxOption);
			this.Controls.Add(this.NudEnglish);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.NudID);
			this.Controls.Add(this.btnAbout);
			this.Controls.Add(this.lblOutput);
			this.Controls.Add(this.lblSelectedFile);
			this.Controls.Add(this.lblSelectedFolder);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.tbxSelectedFile);
			this.Controls.Add(this.tbxSelectedFolder);
			this.Controls.Add(this.btnSelectFolder);
			this.Controls.Add(this.txtOutput);
			this.Controls.Add(this.btnSelectFile);
			this.MinimumSize = new System.Drawing.Size(640, 640);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Localizationeer";
			((System.ComponentModel.ISupportInitialize)(this.NudEnglish)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.NudID)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog oflSelectFile;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.FolderBrowserDialog fbdSelectFolder;
        private System.Windows.Forms.TextBox tbxSelectedFolder;
        private System.Windows.Forms.TextBox tbxSelectedFile;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lblSelectedFolder;
        private System.Windows.Forms.Label lblSelectedFile;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.Button btnAbout;
		private System.Windows.Forms.NumericUpDown NudEnglish;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown NudID;
		private System.Windows.Forms.ComboBox cbxOption;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button btnReset;
	}
}

