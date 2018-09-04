namespace Localizationeer
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
            this.SuspendLayout();
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFile.Location = new System.Drawing.Point(487, 75);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(123, 28);
            this.btnSelectFile.TabIndex = 5;
            this.btnSelectFile.Text = "Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(12, 127);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(598, 420);
            this.txtOutput.TabIndex = 7;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFolder.Location = new System.Drawing.Point(487, 26);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(123, 28);
            this.btnSelectFolder.TabIndex = 2;
            this.btnSelectFolder.Text = "Select Folder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // tbxSelectedFolder
            // 
            this.tbxSelectedFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxSelectedFolder.Location = new System.Drawing.Point(12, 29);
            this.tbxSelectedFolder.Name = "tbxSelectedFolder";
            this.tbxSelectedFolder.ReadOnly = true;
            this.tbxSelectedFolder.Size = new System.Drawing.Size(467, 22);
            this.tbxSelectedFolder.TabIndex = 1;
            // 
            // tbxSelectedFile
            // 
            this.tbxSelectedFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxSelectedFile.Location = new System.Drawing.Point(12, 78);
            this.tbxSelectedFile.Name = "tbxSelectedFile";
            this.tbxSelectedFile.ReadOnly = true;
            this.tbxSelectedFile.Size = new System.Drawing.Size(467, 22);
            this.tbxSelectedFile.TabIndex = 4;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(486, 553);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(123, 28);
            this.btnApply.TabIndex = 9;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // lblSelectedFolder
            // 
            this.lblSelectedFolder.AutoSize = true;
            this.lblSelectedFolder.Location = new System.Drawing.Point(12, 9);
            this.lblSelectedFolder.Name = "lblSelectedFolder";
            this.lblSelectedFolder.Size = new System.Drawing.Size(463, 17);
            this.lblSelectedFolder.TabIndex = 0;
            this.lblSelectedFolder.Text = "The app/src/main/res folder which contains your values*/strings.xml files:";
            // 
            // lblSelectedFile
            // 
            this.lblSelectedFile.AutoSize = true;
            this.lblSelectedFile.Location = new System.Drawing.Point(12, 58);
            this.lblSelectedFile.Name = "lblSelectedFile";
            this.lblSelectedFile.Size = new System.Drawing.Size(318, 17);
            this.lblSelectedFile.TabIndex = 3;
            this.lblSelectedFile.Text = "The Excel file with string ids and localized strings:";
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(12, 107);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(55, 17);
            this.lblOutput.TabIndex = 6;
            this.lblOutput.Text = "Output:";
            // 
            // btnAbout
            // 
            this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAbout.Location = new System.Drawing.Point(12, 553);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(123, 28);
            this.btnAbout.TabIndex = 8;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 593);
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
    }
}

