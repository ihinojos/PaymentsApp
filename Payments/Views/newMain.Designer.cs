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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(20, 172);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(165, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(12, 68);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(92, 13);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "Pending Invoices:";
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
            this.gridControl1.Location = new System.Drawing.Point(191, 154);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(458, 364);
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
            // 
            // lblTitleResult
            // 
            this.lblTitleResult.AutoSize = true;
            this.lblTitleResult.Location = new System.Drawing.Point(128, 69);
            this.lblTitleResult.Name = "lblTitleResult";
            this.lblTitleResult.Size = new System.Drawing.Size(0, 13);
            this.lblTitleResult.TabIndex = 5;
            // 
            // lblSelectedFileStatic
            // 
            this.lblSelectedFileStatic.AutoSize = true;
            this.lblSelectedFileStatic.Location = new System.Drawing.Point(12, 45);
            this.lblSelectedFileStatic.Name = "lblSelectedFileStatic";
            this.lblSelectedFileStatic.Size = new System.Drawing.Size(71, 13);
            this.lblSelectedFileStatic.TabIndex = 6;
            this.lblSelectedFileStatic.Text = "Selected File:";
            // 
            // lblSelectedFile
            // 
            this.lblSelectedFile.AutoSize = true;
            this.lblSelectedFile.Location = new System.Drawing.Point(128, 45);
            this.lblSelectedFile.Name = "lblSelectedFile";
            this.lblSelectedFile.Size = new System.Drawing.Size(65, 13);
            this.lblSelectedFile.TabIndex = 7;
            this.lblSelectedFile.Text = "Select a file.";
            // 
            // btnViewPDF
            // 
            this.btnViewPDF.Location = new System.Drawing.Point(15, 449);
            this.btnViewPDF.Name = "btnViewPDF";
            this.btnViewPDF.Size = new System.Drawing.Size(80, 32);
            this.btnViewPDF.TabIndex = 9;
            this.btnViewPDF.Text = "View PDF";
            this.btnViewPDF.UseVisualStyleBackColor = true;
            this.btnViewPDF.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(101, 449);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(84, 32);
            this.btnReload.TabIndex = 12;
            this.btnReload.Text = "Reload Table";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Bussiness Selected:";
            // 
            // lblNameBuss
            // 
            this.lblNameBuss.AutoSize = true;
            this.lblNameBuss.Location = new System.Drawing.Point(128, 22);
            this.lblNameBuss.Name = "lblNameBuss";
            this.lblNameBuss.Size = new System.Drawing.Size(99, 13);
            this.lblNameBuss.TabIndex = 14;
            this.lblNameBuss.Text = "Select a Bussiness.";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblSelectedFileStatic);
            this.groupBox1.Controls.Add(this.lblNameBuss);
            this.groupBox1.Controls.Add(this.labelTitle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblTitleResult);
            this.groupBox1.Controls.Add(this.lblSelectedFile);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(537, 96);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Info";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Payments.Properties.Resources.icons8_payment_history_100;
            this.pictureBox1.Location = new System.Drawing.Point(555, 41);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(94, 100);
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPaymentCaptured);
            this.groupBox2.Controls.Add(this.btnMakePayment);
            this.groupBox2.Controls.Add(this.btnSigned);
            this.groupBox2.Controls.Add(this.btnCapture);
            this.groupBox2.Location = new System.Drawing.Point(15, 199);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(170, 190);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Assing a state";
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
            this.btnChangeFileOfBussiness.Location = new System.Drawing.Point(15, 411);
            this.btnChangeFileOfBussiness.Name = "btnChangeFileOfBussiness";
            this.btnChangeFileOfBussiness.Size = new System.Drawing.Size(170, 32);
            this.btnChangeFileOfBussiness.TabIndex = 18;
            this.btnChangeFileOfBussiness.Text = "Change file of bussiness";
            this.btnChangeFileOfBussiness.UseVisualStyleBackColor = true;
            this.btnChangeFileOfBussiness.Click += new System.EventHandler(this.button5_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 486);
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
            this.menuStrip1.Size = new System.Drawing.Size(666, 24);
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
            this.setRootPathToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.setRootPathToolStripMenuItem.Text = "Set root path";
            this.setRootPathToolStripMenuItem.Click += new System.EventHandler(this.setRootPathToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
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
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.usersToolStripMenuItem.Text = "Users";
            this.usersToolStripMenuItem.Click += new System.EventHandler(this.usersToolStripMenuItem_Click);
            // 
            // subBussinessToolStripMenuItem
            // 
            this.subBussinessToolStripMenuItem.Name = "subBussinessToolStripMenuItem";
            this.subBussinessToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.subBussinessToolStripMenuItem.Text = "Sub-Bussiness";
            this.subBussinessToolStripMenuItem.Click += new System.EventHandler(this.subBussinessToolStripMenuItem_Click);
            // 
            // businesssToolStripMenuItem
            // 
            this.businesssToolStripMenuItem.Name = "businesssToolStripMenuItem";
            this.businesssToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.businesssToolStripMenuItem.Text = "Business";
            this.businesssToolStripMenuItem.Click += new System.EventHandler(this.businesssToolStripMenuItem_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Bussiness:";
            // 
            // NewMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 533);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnChangeFileOfBussiness);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pictureBox1);
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.PictureBox pictureBox1;
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
    }
}