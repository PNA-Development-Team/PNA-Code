namespace ViewUtility
{
    partial class SetGridForm
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
            m_Instance = null;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.cbIsShow = new System.Windows.Forms.CheckBox();
            this.lbAccuracy = new System.Windows.Forms.Label();
            this.nUPAccuracy = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nUPAccuracy)).BeginInit();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(34, 58);
            this.btOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(62, 33);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(128, 58);
            this.btCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(62, 33);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // cbIsShow
            // 
            this.cbIsShow.AutoSize = true;
            this.cbIsShow.Checked = true;
            this.cbIsShow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIsShow.Location = new System.Drawing.Point(16, 18);
            this.cbIsShow.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbIsShow.Name = "cbIsShow";
            this.cbIsShow.Size = new System.Drawing.Size(52, 21);
            this.cbIsShow.TabIndex = 2;
            this.cbIsShow.Text = "Grid";
            this.cbIsShow.UseVisualStyleBackColor = true;
            this.cbIsShow.CheckedChanged += new System.EventHandler(this.cbIsShow_CheckedChanged);
            // 
            // lbAccuracy
            // 
            this.lbAccuracy.AutoSize = true;
            this.lbAccuracy.Location = new System.Drawing.Point(81, 18);
            this.lbAccuracy.Name = "lbAccuracy";
            this.lbAccuracy.Size = new System.Drawing.Size(66, 17);
            this.lbAccuracy.TabIndex = 3;
            this.lbAccuracy.Text = "Accuracy :";
            // 
            // nUPAccuracy
            // 
            this.nUPAccuracy.Location = new System.Drawing.Point(153, 16);
            this.nUPAccuracy.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nUPAccuracy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nUPAccuracy.Name = "nUPAccuracy";
            this.nUPAccuracy.Size = new System.Drawing.Size(49, 23);
            this.nUPAccuracy.TabIndex = 4;
            this.nUPAccuracy.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // SetGridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 104);
            this.Controls.Add(this.nUPAccuracy);
            this.Controls.Add(this.lbAccuracy);
            this.Controls.Add(this.cbIsShow);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetGridForm";
            this.Text = "Set Grid";
            ((System.ComponentModel.ISupportInitialize)(this.nUPAccuracy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.CheckBox cbIsShow;
        private System.Windows.Forms.Label lbAccuracy;
        private System.Windows.Forms.NumericUpDown nUPAccuracy;
    }
}