namespace Payments.Views
{
    partial class PaymentCaptured
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaymentCaptured));
            this.axAcroPDF2 = new AxAcroPDFLib.AxAcroPDF();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTransNumber = new System.Windows.Forms.Label();
            this.lblTransID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblBussiness = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblNameNewFile = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblNameOldFile = new System.Windows.Forms.Label();
            this.lblNameOldStatic = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.axAcroPDF1 = new AxAcroPDFLib.AxAcroPDF();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF2)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).BeginInit();
            this.SuspendLayout();
            // 
            // axAcroPDF2
            // 
            this.axAcroPDF2.Enabled = true;
            this.axAcroPDF2.Location = new System.Drawing.Point(419, 12);
            this.axAcroPDF2.Name = "axAcroPDF2";
            this.axAcroPDF2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAcroPDF2.OcxState")));
            this.axAcroPDF2.Size = new System.Drawing.Size(327, 577);
            this.axAcroPDF2.TabIndex = 3;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(20, 169);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(380, 23);
            this.button3.TabIndex = 38;
            this.button3.Text = "Split PDF";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.lblTransNumber);
            this.groupBox3.Controls.Add(this.lblTransID);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.lblBussiness);
            this.groupBox3.Location = new System.Drawing.Point(21, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(380, 100);
            this.groupBox3.TabIndex = 37;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "General Info";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Transaction ID:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Transaction number:";
            // 
            // lblTransNumber
            // 
            this.lblTransNumber.AutoSize = true;
            this.lblTransNumber.Location = new System.Drawing.Point(130, 47);
            this.lblTransNumber.Name = "lblTransNumber";
            this.lblTransNumber.Size = new System.Drawing.Size(35, 13);
            this.lblTransNumber.TabIndex = 18;
            this.lblTransNumber.Text = "label8";
            // 
            // lblTransID
            // 
            this.lblTransID.AutoSize = true;
            this.lblTransID.Location = new System.Drawing.Point(130, 20);
            this.lblTransID.Name = "lblTransID";
            this.lblTransID.Size = new System.Drawing.Size(35, 13);
            this.lblTransID.TabIndex = 20;
            this.lblTransID.Text = "label9";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Bussiness:";
            // 
            // lblBussiness
            // 
            this.lblBussiness.AutoSize = true;
            this.lblBussiness.Location = new System.Drawing.Point(130, 74);
            this.lblBussiness.Name = "lblBussiness";
            this.lblBussiness.Size = new System.Drawing.Size(35, 13);
            this.lblBussiness.TabIndex = 23;
            this.lblBussiness.Text = "label8";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(21, 566);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(380, 23);
            this.button2.TabIndex = 35;
            this.button2.Text = "Finish assignation";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblNameNewFile);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(22, 317);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 57);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Info of proof of payment:";
            // 
            // lblNameNewFile
            // 
            this.lblNameNewFile.AutoSize = true;
            this.lblNameNewFile.Location = new System.Drawing.Point(114, 27);
            this.lblNameNewFile.Name = "lblNameNewFile";
            this.lblNameNewFile.Size = new System.Drawing.Size(0, 13);
            this.lblNameNewFile.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Name of the file:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblNameOldFile);
            this.groupBox1.Controls.Add(this.lblNameOldStatic);
            this.groupBox1.Location = new System.Drawing.Point(22, 223);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 62);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info of the paid file:";
            // 
            // lblNameOldFile
            // 
            this.lblNameOldFile.AutoSize = true;
            this.lblNameOldFile.Location = new System.Drawing.Point(115, 26);
            this.lblNameOldFile.Name = "lblNameOldFile";
            this.lblNameOldFile.Size = new System.Drawing.Size(0, 13);
            this.lblNameOldFile.TabIndex = 11;
            // 
            // lblNameOldStatic
            // 
            this.lblNameOldStatic.AutoSize = true;
            this.lblNameOldStatic.Location = new System.Drawing.Point(6, 27);
            this.lblNameOldStatic.Name = "lblNameOldStatic";
            this.lblNameOldStatic.Size = new System.Drawing.Size(84, 13);
            this.lblNameOldStatic.TabIndex = 10;
            this.lblNameOldStatic.Text = "Name of the file:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(379, 26);
            this.button1.TabIndex = 31;
            this.button1.Text = "Add the proof of payment";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 395);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(188, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Sub-Bussiness of this file\'s transaction:";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(21, 422);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(380, 124);
            this.treeView1.TabIndex = 29;
            // 
            // axAcroPDF1
            // 
            this.axAcroPDF1.Enabled = true;
            this.axAcroPDF1.Location = new System.Drawing.Point(752, 12);
            this.axAcroPDF1.Name = "axAcroPDF1";
            this.axAcroPDF1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAcroPDF1.OcxState")));
            this.axAcroPDF1.Size = new System.Drawing.Size(335, 577);
            this.axAcroPDF1.TabIndex = 39;
            // 
            // PaymentCaptured
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 606);
            this.Controls.Add(this.axAcroPDF1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.axAcroPDF2);
            this.Name = "PaymentCaptured";
            this.Text = "PaymentCaptured";
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF2)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private AxAcroPDFLib.AxAcroPDF axAcroPDF2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTransNumber;
        private System.Windows.Forms.Label lblTransID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblBussiness;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblNameNewFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblNameOldFile;
        private System.Windows.Forms.Label lblNameOldStatic;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TreeView treeView1;
        private AxAcroPDFLib.AxAcroPDF axAcroPDF1;
    }
}