namespace PreStack
{
    partial class PreStackForm
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
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.HumasonRadioButton = new System.Windows.Forms.RadioButton();
            this.CCDAPRadioButton = new System.Windows.Forms.RadioButton();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.DarksDirectoryListBox = new System.Windows.Forms.CheckedListBox();
            this.BiasDirectoryListBox = new System.Windows.Forms.CheckedListBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.ImageDirectoryListBox = new System.Windows.Forms.CheckedListBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.FlatsDirectoryListBox = new System.Windows.Forms.CheckedListBox();
            this.FilterFlatsButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.TargetLabel = new System.Windows.Forms.Label();
            this.TargetNameText = new System.Windows.Forms.TextBox();
            this.AssembleFilesButton = new System.Windows.Forms.Button();
            this.OpenFitsFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.FlatsFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.DestinationFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.OpenCalibrationFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.FileCopyBar = new System.Windows.Forms.ProgressBar();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.HumasonRadioButton);
            this.GroupBox1.Controls.Add(this.CCDAPRadioButton);
            this.GroupBox1.ForeColor = System.Drawing.Color.MintCream;
            this.GroupBox1.Location = new System.Drawing.Point(217, 379);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(158, 38);
            this.GroupBox1.TabIndex = 47;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Source";
            this.GroupBox1.Enter += new System.EventHandler(this.GroupBox1_Enter);
            // 
            // HumasonRadioButton
            // 
            this.HumasonRadioButton.AutoSize = true;
            this.HumasonRadioButton.Checked = true;
            this.HumasonRadioButton.Location = new System.Drawing.Point(70, 16);
            this.HumasonRadioButton.Name = "HumasonRadioButton";
            this.HumasonRadioButton.Size = new System.Drawing.Size(70, 17);
            this.HumasonRadioButton.TabIndex = 1;
            this.HumasonRadioButton.TabStop = true;
            this.HumasonRadioButton.Text = "Humason";
            this.HumasonRadioButton.UseVisualStyleBackColor = true;
            // 
            // CCDAPRadioButton
            // 
            this.CCDAPRadioButton.AutoSize = true;
            this.CCDAPRadioButton.Location = new System.Drawing.Point(3, 16);
            this.CCDAPRadioButton.Name = "CCDAPRadioButton";
            this.CCDAPRadioButton.Size = new System.Drawing.Size(61, 17);
            this.CCDAPRadioButton.TabIndex = 0;
            this.CCDAPRadioButton.Text = "CCDAP";
            this.CCDAPRadioButton.UseVisualStyleBackColor = true;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(425, 212);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(94, 13);
            this.Label4.TabIndex = 46;
            this.Label4.Text = "Darks Master Files";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(425, 32);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(86, 13);
            this.Label1.TabIndex = 45;
            this.Label1.Text = "Bias Master Files";
            // 
            // DarksDirectoryListBox
            // 
            this.DarksDirectoryListBox.FormattingEnabled = true;
            this.DarksDirectoryListBox.Location = new System.Drawing.Point(428, 231);
            this.DarksDirectoryListBox.Name = "DarksDirectoryListBox";
            this.DarksDirectoryListBox.Size = new System.Drawing.Size(314, 139);
            this.DarksDirectoryListBox.Sorted = true;
            this.DarksDirectoryListBox.TabIndex = 44;
            this.DarksDirectoryListBox.SelectedIndexChanged += new System.EventHandler(this.DarksDirectoryListBox_SelectedIndexChanged);
            // 
            // BiasDirectoryListBox
            // 
            this.BiasDirectoryListBox.FormattingEnabled = true;
            this.BiasDirectoryListBox.Location = new System.Drawing.Point(428, 51);
            this.BiasDirectoryListBox.Name = "BiasDirectoryListBox";
            this.BiasDirectoryListBox.Size = new System.Drawing.Size(314, 139);
            this.BiasDirectoryListBox.Sorted = true;
            this.BiasDirectoryListBox.TabIndex = 43;
            this.BiasDirectoryListBox.SelectedIndexChanged += new System.EventHandler(this.BiasDirectoryListBox_SelectedIndexChanged);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(58, 30);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(81, 13);
            this.Label3.TabIndex = 42;
            this.Label3.Text = "Image Directory";
            // 
            // ImageDirectoryListBox
            // 
            this.ImageDirectoryListBox.FormattingEnabled = true;
            this.ImageDirectoryListBox.Location = new System.Drawing.Point(61, 51);
            this.ImageDirectoryListBox.Name = "ImageDirectoryListBox";
            this.ImageDirectoryListBox.Size = new System.Drawing.Size(167, 319);
            this.ImageDirectoryListBox.Sorted = true;
            this.ImageDirectoryListBox.TabIndex = 41;
            this.ImageDirectoryListBox.SelectedIndexChanged += new System.EventHandler(this.ImageDirectoryListBox_SelectedIndexChanged);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(243, 30);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(74, 13);
            this.Label2.TabIndex = 40;
            this.Label2.Text = "Flats Directory";
            // 
            // FlatsDirectoryListBox
            // 
            this.FlatsDirectoryListBox.FormattingEnabled = true;
            this.FlatsDirectoryListBox.Location = new System.Drawing.Point(246, 51);
            this.FlatsDirectoryListBox.Name = "FlatsDirectoryListBox";
            this.FlatsDirectoryListBox.Size = new System.Drawing.Size(167, 319);
            this.FlatsDirectoryListBox.TabIndex = 39;
            this.FlatsDirectoryListBox.SelectedIndexChanged += new System.EventHandler(this.FlatsDirectoryListBox_SelectedIndexChanged);
            // 
            // FilterFlatsButton
            // 
            this.FilterFlatsButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.FilterFlatsButton.ForeColor = System.Drawing.Color.Black;
            this.FilterFlatsButton.Location = new System.Drawing.Point(476, 385);
            this.FilterFlatsButton.Name = "FilterFlatsButton";
            this.FilterFlatsButton.Size = new System.Drawing.Size(79, 29);
            this.FilterFlatsButton.TabIndex = 38;
            this.FilterFlatsButton.Text = " Filter Flats";
            this.FilterFlatsButton.UseVisualStyleBackColor = false;
            this.FilterFlatsButton.Click += new System.EventHandler(this.FilterFlatsButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.CloseButton.ForeColor = System.Drawing.Color.Black;
            this.CloseButton.Location = new System.Drawing.Point(655, 385);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(87, 29);
            this.CloseButton.TabIndex = 37;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // TargetLabel
            // 
            this.TargetLabel.AutoSize = true;
            this.TargetLabel.Location = new System.Drawing.Point(58, 393);
            this.TargetLabel.Name = "TargetLabel";
            this.TargetLabel.Size = new System.Drawing.Size(38, 13);
            this.TargetLabel.TabIndex = 36;
            this.TargetLabel.Text = "Target";
            // 
            // TargetNameText
            // 
            this.TargetNameText.Location = new System.Drawing.Point(102, 390);
            this.TargetNameText.Name = "TargetNameText";
            this.TargetNameText.Size = new System.Drawing.Size(87, 20);
            this.TargetNameText.TabIndex = 34;
            this.TargetNameText.Text = "NGC ";
            // 
            // AssembleFilesButton
            // 
            this.AssembleFilesButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.AssembleFilesButton.ForeColor = System.Drawing.Color.Black;
            this.AssembleFilesButton.Location = new System.Drawing.Point(392, 385);
            this.AssembleFilesButton.Name = "AssembleFilesButton";
            this.AssembleFilesButton.Size = new System.Drawing.Size(78, 29);
            this.AssembleFilesButton.TabIndex = 35;
            this.AssembleFilesButton.Text = "Assemble";
            this.AssembleFilesButton.UseVisualStyleBackColor = false;
            this.AssembleFilesButton.Click += new System.EventHandler(this.AssembleFilesButton_Click);
            // 
            // OpenFitsFileDialog
            // 
            this.OpenFitsFileDialog.InitialDirectory = "C:\\Users\\Rick\\Documents\\Software Bisque\\TheSkyX Professional Edition\\Camera AutoS" +
    "ave\\Imager";
            this.OpenFitsFileDialog.Multiselect = true;
            this.OpenFitsFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFitsFileDialog_FileOk);
            // 
            // FlatsFolderDialog
            // 
            this.FlatsFolderDialog.Description = "Select date folder containing the flats directory";
            this.FlatsFolderDialog.SelectedPath = "C:\\Users\\Rick\\Documents\\CCDWare\\CCDAutoPilot5\\Images";
            this.FlatsFolderDialog.ShowNewFolderButton = false;
            this.FlatsFolderDialog.HelpRequest += new System.EventHandler(this.FlatsFolderDialog_HelpRequest);
            // 
            // DestinationFolderDialog
            // 
            this.DestinationFolderDialog.RootFolder = System.Environment.SpecialFolder.MyDocuments;
            this.DestinationFolderDialog.SelectedPath = "C:\\Users\\Rick\\Documents\\PreStackPI";
            this.DestinationFolderDialog.HelpRequest += new System.EventHandler(this.DestinationFolderDialog_HelpRequest);
            // 
            // OpenCalibrationFileDialog
            // 
            this.OpenCalibrationFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenCalibrationFileDialog_FileOk);
            // 
            // FileCopyBar
            // 
            this.FileCopyBar.Location = new System.Drawing.Point(65, 433);
            this.FileCopyBar.Name = "FileCopyBar";
            this.FileCopyBar.Size = new System.Drawing.Size(676, 14);
            this.FileCopyBar.TabIndex = 48;
            // 
            // PreStackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(800, 462);
            this.Controls.Add(this.FileCopyBar);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.DarksDirectoryListBox);
            this.Controls.Add(this.BiasDirectoryListBox);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.ImageDirectoryListBox);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.FlatsDirectoryListBox);
            this.Controls.Add(this.FilterFlatsButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.TargetLabel);
            this.Controls.Add(this.TargetNameText);
            this.Controls.Add(this.AssembleFilesButton);
            this.ForeColor = System.Drawing.Color.MintCream;
            this.Name = "PreStackForm";
            this.Text = "PreStack for D:";
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.RadioButton HumasonRadioButton;
        internal System.Windows.Forms.RadioButton CCDAPRadioButton;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.CheckedListBox DarksDirectoryListBox;
        internal System.Windows.Forms.CheckedListBox BiasDirectoryListBox;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.CheckedListBox ImageDirectoryListBox;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.CheckedListBox FlatsDirectoryListBox;
        internal System.Windows.Forms.Button FilterFlatsButton;
        internal System.Windows.Forms.Button CloseButton;
        internal System.Windows.Forms.Label TargetLabel;
        internal System.Windows.Forms.TextBox TargetNameText;
        internal System.Windows.Forms.Button AssembleFilesButton;
        internal System.Windows.Forms.OpenFileDialog OpenFitsFileDialog;
        internal System.Windows.Forms.FolderBrowserDialog FlatsFolderDialog;
        internal System.Windows.Forms.FolderBrowserDialog DestinationFolderDialog;
        internal System.Windows.Forms.OpenFileDialog OpenCalibrationFileDialog;
        private System.Windows.Forms.ProgressBar FileCopyBar;
    }
}

