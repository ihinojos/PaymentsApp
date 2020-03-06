namespace Payments.Views
{
    partial class NewMain
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lblTitleResult = new System.Windows.Forms.Label();
            this.lblSelectedFileStatic = new System.Windows.Forms.Label();
            this.lblSelectedFile = new System.Windows.Forms.Label();
            this.btnViewPDF = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNameBuss = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPaymentCaptured = new System.Windows.Forms.Button();
            this.btnMakePayment = new System.Windows.Forms.Button();
            this.btnSigned = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.btnChangeFileOfBussiness = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setRootPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subBussinessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.businesssToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.rootButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.subBussinessLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.filePathLabel = new System.Windows.Forms.Label();
            this.lblAmuont = new System.Windows.Forms.Label();
            this.lblDateModified = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(21, 188);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(158, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(12, 69);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(96, 13);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "Incoming Invoices:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bussiness";
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.Location = new System.Drawing.Point(191, 137);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(509, 370);
            this.gridControl1.TabIndex = 3;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridView1_RowStyle);
            // 
            // lblTitleResult
            // 
            this.lblTitleResult.AutoSize = true;
            this.lblTitleResult.Location = new System.Drawing.Point(104, 69);
            this.lblTitleResult.Name = "lblTitleResult";
            this.lblTitleResult.Size = new System.Drawing.Size(0, 13);
            this.lblTitleResult.TabIndex = 5;
            // 
            // lblSelectedFileStatic
            // 
            this.lblSelectedFileStatic.AutoSize = true;
            this.lblSelectedFileStatic.Location = new System.Drawing.Point(178, 22);
            this.lblSelectedFileStatic.Name = "lblSelectedFileStatic";
            this.lblSelectedFileStatic.Size = new System.Drawing.Size(57, 13);
            this.lblSelectedFileStatic.TabIndex = 6;
            this.lblSelectedFileStatic.Text = "File Name:";
            // 
            // lblSelectedFile
            // 
            this.lblSelectedFile.AutoSize = true;
            this.lblSelectedFile.Location = new System.Drawing.Point(234, 22);
            this.lblSelectedFile.Name = "lblSelectedFile";
            this.lblSelectedFile.Size = new System.Drawing.Size(65, 13);
            this.lblSelectedFile.TabIndex = 7;
            this.lblSelectedFile.Text = "Select a file.";
            // 
            // btnViewPDF
            // 
            this.btnViewPDF.Location = new System.Drawing.Point(15, 438);
            this.btnViewPDF.Name = "btnViewPDF";
            this.btnViewPDF.Size = new System.Drawing.Size(80, 32);
            this.btnViewPDF.TabIndex = 9;
            this.btnViewPDF.Text = "View PDF";
            this.btnViewPDF.UseVisualStyleBackColor = true;
            this.btnViewPDF.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(101, 438);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(84, 32);
            this.btnReload.TabIndex = 12;
            this.btnReload.Text = "Show on Disk";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Bussiness:";
            // 
            // lblNameBuss
            // 
            this.lblNameBuss.AutoSize = true;
            this.lblNameBuss.Location = new System.Drawing.Point(68, 22);
            this.lblNameBuss.Name = "lblNameBuss";
            this.lblNameBuss.Size = new System.Drawing.Size(99, 13);
            this.lblNameBuss.TabIndex = 14;
            this.lblNameBuss.Text = "Select a Bussiness.";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblDateModified);
            this.groupBox1.Controls.Add(this.lblAmuont);
            this.groupBox1.Controls.Add(this.filePathLabel);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.subBussinessLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblSelectedFileStatic);
            this.groupBox1.Controls.Add(this.lblNameBuss);
            this.groupBox1.Controls.Add(this.labelTitle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblTitleResult);
            this.groupBox1.Controls.Add(this.lblSelectedFile);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(688, 97);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Invoice Information";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPaymentCaptured);
            this.groupBox2.Controls.Add(this.btnMakePayment);
            this.groupBox2.Controls.Add(this.btnSigned);
            this.groupBox2.Controls.Add(this.btnChangeFileOfBussiness);
            this.groupBox2.Controls.Add(this.btnCapture);
            this.groupBox2.Location = new System.Drawing.Point(15, 215);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(170, 217);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Actions";
            // 
            // btnPaymentCaptured
            // 
            this.btnPaymentCaptured.Location = new System.Drawing.Point(6, 140);
            this.btnPaymentCaptured.Name = "btnPaymentCaptured";
            this.btnPaymentCaptured.Size = new System.Drawing.Size(158, 32);
            this.btnPaymentCaptured.TabIndex = 15;
            this.btnPaymentCaptured.Text = "Capture proof of payment";
            this.btnPaymentCaptured.UseVisualStyleBackColor = true;
            this.btnPaymentCaptured.Click += new System.EventHandler(this.btnPaymentCaptured_Click);
            // 
            // btnMakePayment
            // 
            this.btnMakePayment.Location = new System.Drawing.Point(6, 102);
            this.btnMakePayment.Name = "btnMakePayment";
            this.btnMakePayment.Size = new System.Drawing.Size(158, 32);
            this.btnMakePayment.TabIndex = 14;
            this.btnMakePayment.Text = "Make payment";
            this.btnMakePayment.UseVisualStyleBackColor = true;
            this.btnMakePayment.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSigned
            // 
            this.btnSigned.Location = new System.Drawing.Point(6, 64);
            this.btnSigned.Name = "btnSigned";
            this.btnSigned.Size = new System.Drawing.Size(158, 32);
            this.btnSigned.TabIndex = 13;
            this.btnSigned.Text = "Sing invoice";
            this.btnSigned.UseVisualStyleBackColor = true;
            this.btnSigned.Click += new System.EventHandler(this.btnSigned_Click_1);
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(6, 26);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(158, 32);
            this.btnCapture.TabIndex = 12;
            this.btnCapture.Text = "Capture invoice";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // btnChangeFileOfBussiness
            // 
            this.btnChangeFileOfBussiness.Location = new System.Drawing.Point(6, 178);
            this.btnChangeFileOfBussiness.Name = "btnChangeFileOfBussiness";
            this.btnChangeFileOfBussiness.Size = new System.Drawing.Size(158, 32);
            this.btnChangeFileOfBussiness.TabIndex = 18;
            this.btnChangeFileOfBussiness.Text = "Change file of bussiness";
            this.btnChangeFileOfBussiness.UseVisualStyleBackColor = true;
            this.btnChangeFileOfBussiness.Click += new System.EventHandler(this.button5_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 476);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(170, 32);
            this.button1.TabIndex = 19;
            this.button1.Text = "View completed transactions";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.createToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(717, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setRootPathToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // setRootPathToolStripMenuItem
            // 
            this.setRootPathToolStripMenuItem.Name = "setRootPathToolStripMenuItem";
            this.setRootPathToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.setRootPathToolStripMenuItem.Text = "Set root path";
            this.setRootPathToolStripMenuItem.Click += new System.EventHandler(this.setRootPathToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usersToolStripMenuItem,
            this.subBussinessToolStripMenuItem,
            this.businesssToolStripMenuItem});
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.createToolStripMenuItem.Text = "Create";
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.usersToolStripMenuItem.Text = "Users";
            this.usersToolStripMenuItem.Click += new System.EventHandler(this.usersToolStripMenuItem_Click);
            // 
            // subBussinessToolStripMenuItem
            // 
            this.subBussinessToolStripMenuItem.Name = "subBussinessToolStripMenuItem";
            this.subBussinessToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.subBussinessToolStripMenuItem.Text = "Sub-Bussiness";
            this.subBussinessToolStripMenuItem.Click += new System.EventHandler(this.subBussinessToolStripMenuItem_Click);
            // 
            // businesssToolStripMenuItem
            // 
            this.businesssToolStripMenuItem.Name = "businesssToolStripMenuItem";
            this.businesssToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.businesssToolStripMenuItem.Text = "Business";
            this.businesssToolStripMenuItem.Click += new System.EventHandler(this.businesssToolStripMenuItem_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Bussiness:";
            // 
            // rootButton
            // 
            this.rootButton.Location = new System.Drawing.Point(21, 137);
            this.rootButton.Name = "rootButton";
            this.rootButton.Size = new System.Drawing.Size(158, 32);
            this.rootButton.TabIndex = 22;
            this.rootButton.Text = "Go back to root";
            this.rootButton.UseVisualStyleBackColor = true;
            this.rootButton.Click += new System.EventHandler(this.rootButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Sub Bussiness:";
            // 
            // subBussinessLabel
            // 
            this.subBussinessLabel.AutoSize = true;
            this.subBussinessLabel.Location = new System.Drawing.Point(86, 45);
            this.subBussinessLabel.Name = "subBussinessLabel";
            this.subBussinessLabel.Size = new System.Drawing.Size(72, 13);
            this.subBussinessLabel.TabIndex = 16;
            this.subBussinessLabel.Text = "Not assigned.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(178, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "File path:";
            // 
            // filePathLabel
            // 
            this.filePathLabel.AutoSize = true;
            this.filePathLabel.Location = new System.Drawing.Point(227, 45);
            this.filePathLabel.Name = "filePathLabel";
            this.filePathLabel.Size = new System.Drawing.Size(65, 13);
            this.filePathLabel.TabIndex = 18;
            this.filePathLabel.Text = "Select a file.";
            // 
            // lblAmuont
            // 
            this.lblAmuont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAmuont.AutoSize = true;
            this.lblAmuont.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F);
            this.lblAmuont.Location = new System.Drawing.Point(477, 53);
            this.lblAmuont.Name = "lblAmuont";
            this.lblAmuont.Size = new System.Drawing.Size(32, 36);
            this.lblAmuont.TabIndex = 19;
            this.lblAmuont.Text = "$";
            // 
            // lblDateModified
            // 
            this.lblDateModified.AutoSize = true;
            this.lblDateModified.Location = new System.Drawing.Point(253, 69);
            this.lblDateModified.Name = "lblDateModified";
            this.lblDateModified.Size = new System.Drawing.Size(0, 13);
            this.lblDateModified.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(178, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Date modified:";
            // 
            // NewMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 519);
            this.Controls.Add(this.rootButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnViewPDF);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "NewMain";
            this.Text = "newMain";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label label2;
        public DevExpress.XtraGrid.GridControl gridControl1;
        public DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Label lblTitleResult;
        private System.Windows.Forms.Label lblSelectedFileStatic;
        private System.Windows.Forms.Label lblSelectedFile;
        private System.Windows.Forms.Button btnViewPDF;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNameBuss;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnPaymentCaptured;
        private System.Windows.Forms.Button btnMakePayment;
        private System.Windows.Forms.Button btnSigned;
        public System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Button btnChangeFileOfBussiness;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subBussinessToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem businesssToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setRootPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button rootButton;
        private System.Windows.Forms.Label filePathLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label subBussinessLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDateModified;
        private System.Windows.Forms.Label lblAmuont;
        private System.Windows.Forms.Label label6;
    }
}