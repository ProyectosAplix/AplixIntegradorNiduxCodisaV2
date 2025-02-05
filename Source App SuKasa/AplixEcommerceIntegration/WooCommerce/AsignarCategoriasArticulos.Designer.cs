namespace AplixEcommerceIntegration.WooCommerce
{
    partial class AsignarCategoriasArticulos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AsignarCategoriasArticulos));
            this.dt_categorias = new System.Windows.Forms.DataGridView();
            this.codigo_articulo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre_articulo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoria_padre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sincroniza_articulo = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_art = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.dt_categorias)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dt_categorias
            // 
            this.dt_categorias.AllowUserToAddRows = false;
            this.dt_categorias.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dt_categorias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dt_categorias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_categorias.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codigo_articulo,
            this.nombre_articulo,
            this.categoria_padre,
            this.sincroniza_articulo});
            this.dt_categorias.Location = new System.Drawing.Point(2, 30);
            this.dt_categorias.Name = "dt_categorias";
            this.dt_categorias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dt_categorias.Size = new System.Drawing.Size(648, 395);
            this.dt_categorias.TabIndex = 28;
            this.dt_categorias.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dt_categorias_CellContentClick);
            // 
            // codigo_articulo
            // 
            this.codigo_articulo.FillWeight = 64.16989F;
            this.codigo_articulo.HeaderText = "Código ";
            this.codigo_articulo.Name = "codigo_articulo";
            this.codigo_articulo.ReadOnly = true;
            // 
            // nombre_articulo
            // 
            this.nombre_articulo.FillWeight = 141.9789F;
            this.nombre_articulo.HeaderText = "Nombre";
            this.nombre_articulo.Name = "nombre_articulo";
            this.nombre_articulo.ReadOnly = true;
            // 
            // categoria_padre
            // 
            this.categoria_padre.FillWeight = 143.0898F;
            this.categoria_padre.HeaderText = "Categroría Padre";
            this.categoria_padre.Name = "categoria_padre";
            // 
            // sincroniza_articulo
            // 
            this.sincroniza_articulo.FalseValue = "false";
            this.sincroniza_articulo.FillWeight = 50.76142F;
            this.sincroniza_articulo.HeaderText = "Sincronizar";
            this.sincroniza_articulo.Name = "sincroniza_articulo";
            this.sincroniza_articulo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.sincroniza_articulo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.sincroniza_articulo.TrueValue = "true";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.btn_art,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(650, 27);
            this.toolStrip1.TabIndex = 49;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // btn_art
            // 
            this.btn_art.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_art.Enabled = false;
            this.btn_art.Image = ((System.Drawing.Image)(resources.GetObject("btn_art.Image")));
            this.btn_art.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_art.Name = "btn_art";
            this.btn_art.Size = new System.Drawing.Size(24, 24);
            this.btn_art.Text = "&Guardar";
            this.btn_art.Click += new System.EventHandler(this.btn_art_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // AsignarCategoriasArticulos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 425);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dt_categorias);
            this.Name = "AsignarCategoriasArticulos";
            this.Text = "AsignarCategoriasArticulos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AsignarCategoriasArticulos_FormClosing);
            this.Load += new System.EventHandler(this.AsignarCategoriasArticulos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dt_categorias)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dt_categorias;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo_articulo;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre_articulo;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoria_padre;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sincroniza_articulo;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btn_art;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}