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
            this.gridAccInfor = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.lbTotalCoin = new System.Windows.Forms.Label();
            this.btnGetCoin = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbCountAll = new System.Windows.Forms.Label();
            this.lbCountSelected = new System.Windows.Forms.Label();
            this.txtUserNhanXu = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbAllXu = new System.Windows.Forms.CheckBox();
            this.numSoXuTang = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numLuong = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.btnNhanXuTim = new System.Windows.Forms.Button();
            this.lbTotalCoinSell = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblCountCanSell = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbLastCheck = new System.Windows.Forms.Label();
            this.lbDiffXu = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccInfor)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoXuTang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLuong)).BeginInit();
            this.SuspendLayout();
            // 
            // gridAccInfor
            // 
            this.gridAccInfor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAccInfor.Location = new System.Drawing.Point(2, 100);
            this.gridAccInfor.Name = "gridAccInfor";
            this.gridAccInfor.Size = new System.Drawing.Size(840, 438);
            this.gridAccInfor.TabIndex = 0;
            this.gridAccInfor.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grdAccount_CellPainting);
            this.gridAccInfor.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.grdAccount_RowPostPaint);
            this.gridAccInfor.SelectionChanged += new System.EventHandler(this.grdAccount_SelectionChanged);
            this.gridAccInfor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdAccount_KeyDown);
            this.gridAccInfor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdAccount_MouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(561, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tổng xu";
            // 
            // lbTotalCoin
            // 
            this.lbTotalCoin.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTotalCoin.ForeColor = System.Drawing.Color.Red;
            this.lbTotalCoin.Location = new System.Drawing.Point(640, 41);
            this.lbTotalCoin.Name = "lbTotalCoin";
            this.lbTotalCoin.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbTotalCoin.Size = new System.Drawing.Size(202, 29);
            this.lbTotalCoin.TabIndex = 3;
            this.lbTotalCoin.Text = "0";
            // 
            // btnGetCoin
            // 
            this.btnGetCoin.Font = new System.Drawing.Font("Roboto", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetCoin.Location = new System.Drawing.Point(742, 2);
            this.btnGetCoin.Name = "btnGetCoin";
            this.btnGetCoin.Size = new System.Drawing.Size(95, 39);
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
            // txtUserNhanXu
            // 
            this.txtUserNhanXu.Location = new System.Drawing.Point(82, 18);
            this.txtUserNhanXu.Name = "txtUserNhanXu";
            this.txtUserNhanXu.Size = new System.Drawing.Size(115, 20);
            this.txtUserNhanXu.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "User Nhận Xu";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckbAllXu);
            this.groupBox1.Controls.Add(this.numSoXuTang);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtUserNhanXu);
            this.groupBox1.Location = new System.Drawing.Point(19, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 83);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tặng xu";
            // 
            // ckbAllXu
            // 
            this.ckbAllXu.AutoSize = true;
            this.ckbAllXu.Location = new System.Drawing.Point(170, 44);
            this.ckbAllXu.Name = "ckbAllXu";
            this.ckbAllXu.Size = new System.Drawing.Size(51, 17);
            this.ckbAllXu.TabIndex = 13;
            this.ckbAllXu.Text = "All xu";
            this.ckbAllXu.UseVisualStyleBackColor = true;
            this.ckbAllXu.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // numSoXuTang
            // 
            this.numSoXuTang.Increment = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numSoXuTang.Location = new System.Drawing.Point(82, 43);
            this.numSoXuTang.Maximum = new decimal(new int[] {
            5000000,
            0,
            0,
            0});
            this.numSoXuTang.Name = "numSoXuTang";
            this.numSoXuTang.Size = new System.Drawing.Size(82, 20);
            this.numSoXuTang.TabIndex = 12;
            this.numSoXuTang.ThousandsSeparator = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Số xu";
            // 
            // numLuong
            // 
            this.numLuong.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numLuong.Location = new System.Drawing.Point(628, 13);
            this.numLuong.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numLuong.Name = "numLuong";
            this.numLuong.Size = new System.Drawing.Size(54, 20);
            this.numLuong.TabIndex = 12;
            this.numLuong.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(588, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Luồng";
            // 
            // btnNhanXuTim
            // 
            this.btnNhanXuTim.Location = new System.Drawing.Point(258, 12);
            this.btnNhanXuTim.Name = "btnNhanXuTim";
            this.btnNhanXuTim.Size = new System.Drawing.Size(77, 23);
            this.btnNhanXuTim.TabIndex = 14;
            this.btnNhanXuTim.Text = "Nhận Xu Tim";
            this.btnNhanXuTim.UseVisualStyleBackColor = true;
            this.btnNhanXuTim.Click += new System.EventHandler(this.btnNhanXuTim_Click);
            // 
            // lbTotalCoinSell
            // 
            this.lbTotalCoinSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTotalCoinSell.ForeColor = System.Drawing.Color.ForestGreen;
            this.lbTotalCoinSell.Location = new System.Drawing.Point(655, 69);
            this.lbTotalCoinSell.Name = "lbTotalCoinSell";
            this.lbTotalCoinSell.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbTotalCoinSell.Size = new System.Drawing.Size(187, 29);
            this.lbTotalCoinSell.TabIndex = 15;
            this.lbTotalCoinSell.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(561, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 18);
            this.label7.TabIndex = 16;
            this.label7.Text = "> 1.1M :";
            // 
            // lblCountCanSell
            // 
            this.lblCountCanSell.AutoSize = true;
            this.lblCountCanSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountCanSell.ForeColor = System.Drawing.Color.Green;
            this.lblCountCanSell.Location = new System.Drawing.Point(634, 73);
            this.lblCountCanSell.Name = "lblCountCanSell";
            this.lblCountCanSell.Size = new System.Drawing.Size(20, 24);
            this.lblCountCanSell.TabIndex = 17;
            this.lblCountCanSell.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(516, 545);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Last check:";
            // 
            // lbLastCheck
            // 
            this.lbLastCheck.AutoSize = true;
            this.lbLastCheck.Location = new System.Drawing.Point(581, 545);
            this.lbLastCheck.Name = "lbLastCheck";
            this.lbLastCheck.Size = new System.Drawing.Size(13, 13);
            this.lbLastCheck.TabIndex = 19;
            this.lbLastCheck.Text = "0";
            // 
            // lbDiffXu
            // 
            this.lbDiffXu.AutoSize = true;
            this.lbDiffXu.Location = new System.Drawing.Point(739, 545);
            this.lbDiffXu.Name = "lbDiffXu";
            this.lbDiffXu.Size = new System.Drawing.Size(13, 13);
            this.lbDiffXu.TabIndex = 20;
            this.lbDiffXu.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 563);
            this.Controls.Add(this.lbDiffXu);
            this.Controls.Add(this.lbLastCheck);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblCountCanSell);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbTotalCoinSell);
            this.Controls.Add(this.btnNhanXuTim);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numLuong);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGetCoin);
            this.Controls.Add(this.lbCountSelected);
            this.Controls.Add(this.lbCountAll);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbTotalCoin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridAccInfor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TDS by HỘI TAY VỊN";
            ((System.ComponentModel.ISupportInitialize)(this.gridAccInfor)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoXuTang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLuong)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridAccInfor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGetCoin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbCountAll;
        private System.Windows.Forms.Label lbCountSelected;
        private System.Windows.Forms.TextBox txtUserNhanXu;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numSoXuTang;
        private System.Windows.Forms.NumericUpDown numLuong;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox ckbAllXu;
        private System.Windows.Forms.Button btnNhanXuTim;
        private System.Windows.Forms.Label lbTotalCoinSell;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblCountCanSell;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbLastCheck;
        private System.Windows.Forms.Label lbDiffXu;
        private System.Windows.Forms.Label lbTotalCoin;
    }
}

