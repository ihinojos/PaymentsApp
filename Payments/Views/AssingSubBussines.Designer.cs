namespace Payments.Views
{
    partial class AssingSubBussines
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
        public void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssingSubBussines));
            this.axAcroPDF1 = new AxAcroPDFLib.AxAcroPDF();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFileSelected = new System.Windows.Forms.Label();
            this.lblSubSelected = new System.Windows.Forms.Label();
            this.btnAssingTo = new System.Windows.Forms.Button();
            this.comboBoxSubBussiness = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxTransaction = new System.Windows.Forms.TextBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxAmount = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // axAcroPDF1
            // 
            this.axAcroPDF1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axAcroPDF1.Enabled = true;
            this.axAcroPDF1.Location = new System.Drawing.Point(224, 12);
            this.axAcroPDF1.Name = "axAcroPDF1";
            this.axAcroPDF1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAcroPDF1.OcxState")));
            this.axAcroPDF1.Size = new System.Drawing.Size(252, 331);
            this.axAcroPDF1.TabIndex = 0;
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(15, 124);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(203, 93);
            this.gridControl1.TabIndex = 1;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "File Selected:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sub-Bussiness Selected:";
            // 
            // lblFileSelected
            // 
            this.lblFileSelected.AutoSize = true;
            this.lblFileSelected.Location = new System.Drawing.Point(138, 9);
            this.lblFileSelected.Name = "lblFileSelected";
            this.lblFileSelected.Size = new System.Drawing.Size(0, 13);
            this.lblFileSelected.TabIndex = 5;
            // 
            // lblSubSelected
            // 
            this.lblSubSelected.AutoSize = true;
            this.lblSubSelected.Location = new System.Drawing.Point(138, 37);
            this.lblSubSelected.Name = "lblSubSelected";
            this.lblSubSelected.Size = new System.Drawing.Size(0, 13);
            this.lblSubSelected.TabIndex = 6;
            // 
            // btnAssingTo
            // 
            this.btnAssingTo.Location = new System.Drawing.Point(141, 68);
            this.btnAssingTo.Name = "btnAssingTo";
            this.btnAssingTo.Size = new System.Drawing.Size(75, 22);
            this.btnAssingTo.TabIndex = 10;
            this.btnAssingTo.Text = "+";
            this.btnAssingTo.UseVisualStyleBackColor = true;
            this.btnAssingTo.Click += new System.EventHandler(this.btnAssingTo_Click);
            // 
            // comboBoxSubBussiness
            // 
            this.comboBoxSubBussiness.FormattingEnabled = true;
            this.comboBoxSubBussiness.Location = new System.Drawing.Point(15, 68);
            this.comboBoxSubBussiness.Name = "comboBoxSubBussiness";
            this.comboBoxSubBussiness.Size = new System.Drawing.Size(123, 21);
            this.comboBoxSubBussiness.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 271);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Number of Transaction:";
            // 
            // textBoxTransaction
            // 
            this.textBoxTransaction.Location = new System.Drawing.Point(15, 287);
            this.textBoxTransaction.Name = "textBoxTransaction";
            this.textBoxTransaction.Size = new System.Drawing.Size(201, 20);
            this.textBoxTransaction.TabIndex = 14;
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(12, 320);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(204, 23);
            this.btnCapture.TabIndex = 15;
            this.btnCapture.Text = "Capture";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(204, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Currents sub-bussiness of the file selected";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 232);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Invoice Amount";
            // 
            // textBoxAmount
            // 
            this.textBoxAmount.Location = new System.Drawing.Point(15, 248);
            this.textBoxAmount.Name = "textBoxAmount";
            this.textBoxAmount.Size = new System.Drawing.Size(201, 20);
            this.textBoxAmount.TabIndex = 18;
            // 
            // AssingSubBussines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 355);
            this.Controls.Add(this.textBoxAmount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.textBoxTransaction);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxSubBussiness);
            this.Controls.Add(this.btnAssingTo);
            this.Controls.Add(this.lblSubSelected);
            this.Controls.Add(this.lblFileSelected);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.axAcroPDF1);
            this.Name = "AssingSubBussines";
            this.Text = "AssingSubBussines";
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxAcroPDFLib.AxAcroPDF axAcroPDF1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFileSelected;
        private System.Windows.Forms.Label lblSubSelected;
        private System.Windows.Forms.Button btnAssingTo;
        private System.Windows.Forms.ComboBox comboBoxSubBussiness;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxTransaction;
        public  System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxAmount;
    }
}