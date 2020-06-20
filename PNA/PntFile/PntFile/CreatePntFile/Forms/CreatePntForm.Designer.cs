namespace PntFile.Forms
{
    partial class CreatePntForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btImportPnt = new System.Windows.Forms.Button();
            this.btExportPnt = new System.Windows.Forms.Button();
            this.dgvTable = new System.Windows.Forms.DataGridView();
            this.Place = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Token = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pre_Transition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pre_Weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Post_Transition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Post_Weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btAddPlace = new System.Windows.Forms.Button();
            this.btDeletePlace = new System.Windows.Forms.Button();
            this.lbNotice = new System.Windows.Forms.Label();
            this.cbExportMatrix = new System.Windows.Forms.ComboBox();
            this.btExportGra = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTable)).BeginInit();
            this.SuspendLayout();
            // 
            // btImportPnt
            // 
            this.btImportPnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btImportPnt.Location = new System.Drawing.Point(538, 64);
            this.btImportPnt.Name = "btImportPnt";
            this.btImportPnt.Size = new System.Drawing.Size(110, 20);
            this.btImportPnt.TabIndex = 0;
            this.btImportPnt.TabStop = false;
            this.btImportPnt.Text = "Import Pnt(&I)";
            this.btImportPnt.UseVisualStyleBackColor = true;
            this.btImportPnt.Click += new System.EventHandler(this.btImportPnt_Click);
            // 
            // btExportPnt
            // 
            this.btExportPnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExportPnt.Location = new System.Drawing.Point(538, 90);
            this.btExportPnt.Name = "btExportPnt";
            this.btExportPnt.Size = new System.Drawing.Size(110, 20);
            this.btExportPnt.TabIndex = 0;
            this.btExportPnt.TabStop = false;
            this.btExportPnt.Text = "Export Pnt(&E)";
            this.btExportPnt.UseVisualStyleBackColor = true;
            this.btExportPnt.Click += new System.EventHandler(this.btExportPnt_Click);
            // 
            // dgvTable
            // 
            this.dgvTable.AllowUserToAddRows = false;
            this.dgvTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Place,
            this.Token,
            this.Pre_Transition,
            this.Pre_Weight,
            this.Post_Transition,
            this.Post_Weight});
            this.dgvTable.Location = new System.Drawing.Point(12, 12);
            this.dgvTable.Name = "dgvTable";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvTable.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTable.RowTemplate.Height = 23;
            this.dgvTable.Size = new System.Drawing.Size(505, 188);
            this.dgvTable.TabIndex = 1;
            // 
            // Place
            // 
            this.Place.HeaderText = "Place";
            this.Place.Name = "Place";
            this.Place.Width = 50;
            // 
            // Token
            // 
            this.Token.HeaderText = "Token";
            this.Token.Name = "Token";
            this.Token.Width = 50;
            // 
            // Pre_Transition
            // 
            this.Pre_Transition.HeaderText = "Pre_Transition";
            this.Pre_Transition.Name = "Pre_Transition";
            // 
            // Pre_Weight
            // 
            this.Pre_Weight.HeaderText = "Pre_Weight";
            this.Pre_Weight.Name = "Pre_Weight";
            this.Pre_Weight.Width = 80;
            // 
            // Post_Transition
            // 
            this.Post_Transition.HeaderText = "Post_Transition";
            this.Post_Transition.Name = "Post_Transition";
            // 
            // Post_Weight
            // 
            this.Post_Weight.HeaderText = "Post_Weight";
            this.Post_Weight.Name = "Post_Weight";
            this.Post_Weight.Width = 80;
            // 
            // btAddPlace
            // 
            this.btAddPlace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddPlace.Location = new System.Drawing.Point(538, 12);
            this.btAddPlace.Name = "btAddPlace";
            this.btAddPlace.Size = new System.Drawing.Size(110, 20);
            this.btAddPlace.TabIndex = 2;
            this.btAddPlace.TabStop = false;
            this.btAddPlace.Text = "Add(&A)";
            this.btAddPlace.UseVisualStyleBackColor = true;
            this.btAddPlace.Click += new System.EventHandler(this.btAddPlace_Click);
            // 
            // btDeletePlace
            // 
            this.btDeletePlace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDeletePlace.Location = new System.Drawing.Point(538, 38);
            this.btDeletePlace.Name = "btDeletePlace";
            this.btDeletePlace.Size = new System.Drawing.Size(110, 20);
            this.btDeletePlace.TabIndex = 2;
            this.btDeletePlace.TabStop = false;
            this.btDeletePlace.Text = "Delete(&D)";
            this.btDeletePlace.UseVisualStyleBackColor = true;
            this.btDeletePlace.Click += new System.EventHandler(this.btDeletePlace_Click);
            // 
            // lbNotice
            // 
            this.lbNotice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbNotice.AutoSize = true;
            this.lbNotice.Location = new System.Drawing.Point(12, 220);
            this.lbNotice.Name = "lbNotice";
            this.lbNotice.Size = new System.Drawing.Size(53, 12);
            this.lbNotice.TabIndex = 3;
            this.lbNotice.Text = "Notice: ";
            // 
            // cbExportMatrix
            // 
            this.cbExportMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbExportMatrix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbExportMatrix.FormattingEnabled = true;
            this.cbExportMatrix.Items.AddRange(new object[] {
            "Pre Incidence Matrix",
            "Post Incidence Matrix",
            "Inceidence Matrix",
            "Export Matrix"});
            this.cbExportMatrix.Location = new System.Drawing.Point(538, 142);
            this.cbExportMatrix.Name = "cbExportMatrix";
            this.cbExportMatrix.Size = new System.Drawing.Size(110, 20);
            this.cbExportMatrix.TabIndex = 5;
            this.cbExportMatrix.TabStop = false;
            // 
            // btExportGra
            // 
            this.btExportGra.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExportGra.Location = new System.Drawing.Point(538, 116);
            this.btExportGra.Name = "btExportGra";
            this.btExportGra.Size = new System.Drawing.Size(110, 20);
            this.btExportGra.TabIndex = 0;
            this.btExportGra.TabStop = false;
            this.btExportGra.Text = "Export Gra(&G)";
            this.btExportGra.UseVisualStyleBackColor = true;
            this.btExportGra.Click += new System.EventHandler(this.btExportGra_Click);
            // 
            // CreatePntForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 241);
            this.Controls.Add(this.cbExportMatrix);
            this.Controls.Add(this.lbNotice);
            this.Controls.Add(this.btDeletePlace);
            this.Controls.Add(this.btAddPlace);
            this.Controls.Add(this.dgvTable);
            this.Controls.Add(this.btExportGra);
            this.Controls.Add(this.btExportPnt);
            this.Controls.Add(this.btImportPnt);
            this.MaximumSize = new System.Drawing.Size(685, 800);
            this.MinimumSize = new System.Drawing.Size(685, 280);
            this.Name = "CreatePntForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Create Pnt File";
            this.Load += new System.EventHandler(this.CreatePntForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btImportPnt;
        private System.Windows.Forms.Button btExportPnt;
        private System.Windows.Forms.DataGridView dgvTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Place;
        private System.Windows.Forms.DataGridViewTextBoxColumn Token;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pre_Transition;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pre_Weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn Post_Transition;
        private System.Windows.Forms.DataGridViewTextBoxColumn Post_Weight;
        private System.Windows.Forms.Button btAddPlace;
        private System.Windows.Forms.Button btDeletePlace;
        private System.Windows.Forms.Label lbNotice;
        private System.Windows.Forms.ComboBox cbExportMatrix;
        private System.Windows.Forms.Button btExportGra;
    }
}