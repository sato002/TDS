namespace BVH.TDS
{
    partial class Form1
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
            this.grdAccount = new System.Windows.Forms.DataGridView();
            this.btnGetToken = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbTotalCoin = new System.Windows.Forms.Label();
            this.btnGetCoin = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbCountAll = new System.Windows.Forms.Label();
            this.lbCountSelected = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccount)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdAccount
            // 
            this.grdAccount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdAccount.Location = new System.Drawing.Point(2, 100);
            this.grdAccount.Name = "grdAccount";
            this.grdAccount.Size = new System.Drawing.Size(840, 438);
            this.grdAccount.TabIndex = 0;
            this.grdAccount.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grdAccount_CellPainting);
            this.grdAccount.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.grdAccount_RowPostPaint);
            this.grdAccount.SelectionChanged += new System.EventHandler(this.grdAccount_SelectionChanged);
            this.grdAccount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdAccount_KeyDown);
            this.grdAccount.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdAccount_MouseClick);
            // 
            // btnGetToken
            // 
            this.btnGetToken.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetToken.Location = new System.Drawing.Point(16, 54);
            this.btnGetToken.Name = "btnGetToken";
            this.btnGetToken.Size = new System.Drawing.Size(104, 30);
            this.btnGetToken.TabIndex = 1;
            this.btnGetToken.Text = "Check Token";
            this.btnGetToken.UseVisualStyleBackColor = true;
            this.btnGetToken.Click += new System.EventHandler(this.btnGetToken_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(779, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tổng xu";
            // 
            // lbTotalCoin
            // 
            this.lbTotalCoin.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTotalCoin.ForeColor = System.Drawing.Color.Red;
            this.lbTotalCoin.Location = new System.Drawing.Point(652, 27);
            this.lbTotalCoin.Name = "lbTotalCoin";
            this.lbTotalCoin.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbTotalCoin.Size = new System.Drawing.Size(190, 29);
            this.lbTotalCoin.TabIndex = 3;
            this.lbTotalCoin.Text = "0";
            // 
            // btnGetCoin
            // 
            this.btnGetCoin.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetCoin.Location = new System.Drawing.Point(16, 18);
            this.btnGetCoin.Name = "btnGetCoin";
            this.btnGetCoin.Size = new System.Drawing.Size(104, 30);
            this.btnGetCoin.TabIndex = 4;
            this.btnGetCoin.Text = "Check Xu";
            this.btnGetCoin.UseVisualStyleBackColor = true;
            this.btnGetCoin.Click += new System.EventHandler(this.btnGetCoin_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(2, 545);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Tổng:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(133, 546);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Đã chọn:";
            // 
            // lbCountAll
            // 
            this.lbCountAll.AutoSize = true;
            this.lbCountAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbCountAll.ForeColor = System.Drawing.Color.DarkOrange;
            this.lbCountAll.Location = new System.Drawing.Point(48, 541);
            this.lbCountAll.Name = "lbCountAll";
            this.lbCountAll.Size = new System.Drawing.Size(18, 20);
            this.lbCountAll.TabIndex = 7;
            this.lbCountAll.Text = "0";
            // 
            // lbCountSelected
            // 
            this.lbCountSelected.AutoSize = true;
            this.lbCountSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCountSelected.ForeColor = System.Drawing.Color.Green;
            this.lbCountSelected.Location = new System.Drawing.Point(198, 541);
            this.lbCountSelected.Name = "lbCountSelected";
            this.lbCountSelected.Size = new System.Drawing.Size(18, 20);
            this.lbCountSelected.TabIndex = 8;
            this.lbCountSelected.Text = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGetToken);
            this.groupBox1.Controls.Add(this.btnGetCoin);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 90);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kiểm tra";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 563);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbCountSelected);
            this.Controls.Add(this.lbCountAll);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbTotalCoin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grdAccount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TDS by HỘI TAY VỊN";
            ((System.ComponentModel.ISupportInitialize)(this.grdAccount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdAccount;
        private System.Windows.Forms.Button btnGetToken;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbTotalCoin;
        private System.Windows.Forms.Button btnGetCoin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbCountAll;
        private System.Windows.Forms.Label lbCountSelected;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

