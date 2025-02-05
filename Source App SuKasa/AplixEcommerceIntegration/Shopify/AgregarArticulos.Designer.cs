namespace AplixEcommerceIntegration.Shopify.Metodos
{
    partial class AgregarArticulos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgregarArticulos));
            this.toolStripMantenimiento = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtArticulo = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator31 = new System.Windows.Forms.ToolStripSeparator();
            this.cbbArticulo = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGuardarArticulos = new System.Windows.Forms.ToolStripButton();
            this.dgvArticulos = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripMantenimiento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArticulos)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMantenimiento
            // 
            this.toolStripMantenimiento.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMantenimiento.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtArticulo,
            this.toolStripSeparator31,
            this.cbbArticulo,
            this.toolStripSeparator1,
            this.btnGuardarArticulos});
            this.toolStripMantenimiento.Location = new System.Drawing.Point(0, 0);
            this.toolStripMantenimiento.Name = "toolStripMantenimiento";
            this.toolStripMantenimiento.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripMantenimiento.Size = new System.Drawing.Size(467, 27);
            this.toolStripMantenimiento.TabIndex = 31;
            this.toolStripMantenimiento.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(91, 24);
            this.toolStripLabel1.Text = "Buscar artículo: ";
            // 
            // txtArticulo
            // 
            this.txtArticulo.Name = "txtArticulo";
            this.txtArticulo.Size = new System.Drawing.Size(170, 27);
            this.txtArticulo.TextChanged += new System.EventHandler(this.txtArticulo_TextChanged);
            // 
            // toolStripSeparator31
            // 
            this.toolStripSeparator31.Name = "toolStripSeparator31";
            this.toolStripSeparator31.Size = new System.Drawing.Size(6, 27);
            // 
            // cbbArticulo
            // 
            this.cbbArticulo.Items.AddRange(new object[] {
            "Nombre",
            "Código"});
            this.cbbArticulo.Name = "cbbArticulo";
            this.cbbArticulo.Size = new System.Drawing.Size(121, 27);
            this.cbbArticulo.Text = "Nombre";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // btnGuardarArticulos
            // 
            this.btnGuardarArticulos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGuardarArticulos.Image = ((System.Drawing.Image)(resources.GetObject("btnGuardarArticulos.Image")));
            this.btnGuardarArticulos.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGuardarArticulos.Name = "btnGuardarArticulos";
            this.btnGuardarArticulos.Size = new System.Drawing.Size(24, 24);
            this.btnGuardarArticulos.ToolTipText = "Seleccionar Artículo";
            this.btnGuardarArticulos.Click += new System.EventHandler(this.btnGuardarArticulos_Click);
            // 
            // dgvArticulos
            // 
            this.dgvArticulos.AllowUserToAddRows = false;
            this.dgvArticulos.BackgroundColor = System.Drawing.Color.White;
            this.dgvArticulos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvArticulos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArticulos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dgvArticulos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvArticulos.Location = new System.Drawing.Point(0, 27);
            this.dgvArticulos.MultiSelect = false;
            this.dgvArticulos.Name = "dgvArticulos";
            this.dgvArticulos.ReadOnly = true;
            this.dgvArticulos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvArticulos.Size = new System.Drawing.Size(467, 210);
            this.dgvArticulos.TabIndex = 32;
            this.dgvArticulos.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvArticulos_CellMouseDoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Sku";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 140;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "Nombre";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // AgregarArticulos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(467, 237);
            this.Controls.Add(this.dgvArticulos);
            this.Controls.Add(this.toolStripMantenimiento);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AgregarArticulos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agregar Artículos";
            this.Load += new System.EventHandler(this.AgregarArticulos_Load);
            this.toolStripMantenimiento.ResumeLayout(false);
            this.toolStripMantenimiento.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArticulos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMantenimiento;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtArticulo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator31;
        private System.Windows.Forms.ToolStripComboBox cbbArticulo;
        private System.Windows.Forms.ToolStripButton btnGuardarArticulos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.DataGridView dgvArticulos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}