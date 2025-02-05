namespace AplixEcommerceIntegration.WooCommerce
{
    partial class Mostrar_Articulos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mostrar_Articulos));
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_art = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.txt_dato = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel9 = new System.Windows.Forms.ToolStripLabel();
            this.combo_opciones = new System.Windows.Forms.ToolStripComboBox();
            this.dt_atributos = new System.Windows.Forms.DataGridView();
            this.codigo_articulo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre_articulo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sincroniza_articulo = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dt_atributos)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip2
            // 
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.btn_art,
            this.toolStripSeparator1,
            this.toolStripButton1,
            this.toolStripSeparator13,
            this.toolStripLabel7,
            this.txt_dato,
            this.toolStripLabel9,
            this.combo_opciones});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip2.Size = new System.Drawing.Size(709, 27);
            this.toolStrip2.TabIndex = 26;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(59, 24);
            this.toolStripLabel7.Text = "Busqueda";
            // 
            // txt_dato
            // 
            this.txt_dato.Name = "txt_dato";
            this.txt_dato.Size = new System.Drawing.Size(100, 27);
            this.txt_dato.TextChanged += new System.EventHandler(this.txt_dato_TextChanged);
            // 
            // toolStripLabel9
            // 
            this.toolStripLabel9.Name = "toolStripLabel9";
            this.toolStripLabel9.Size = new System.Drawing.Size(28, 24);
            this.toolStripLabel9.Text = "por:";
            // 
            // combo_opciones
            // 
            this.combo_opciones.Items.AddRange(new object[] {
            "Código",
            "Nombre"});
            this.combo_opciones.Name = "combo_opciones";
            this.combo_opciones.Size = new System.Drawing.Size(121, 27);
            this.combo_opciones.Text = "Código";
            // 
            // dt_atributos
            // 
            this.dt_atributos.AllowUserToAddRows = false;
            this.dt_atributos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dt_atributos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dt_atributos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_atributos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codigo_articulo,
            this.nombre_articulo,
            this.sincroniza_articulo});
            this.dt_atributos.Location = new System.Drawing.Point(12, 30);
            this.dt_atributos.Name = "dt_atributos";
            this.dt_atributos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dt_atributos.Size = new System.Drawing.Size(685, 429);
            this.dt_atributos.TabIndex = 27;
            this.dt_atributos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dt_atributos_CellContentClick);
            // 
            // codigo_articulo
            // 
            this.codigo_articulo.FillWeight = 76.14214F;
            this.codigo_articulo.HeaderText = "Código Articulo";
            this.codigo_articulo.Name = "codigo_articulo";
            this.codigo_articulo.ReadOnly = true;
            // 
            // nombre_articulo
            // 
            this.nombre_articulo.FillWeight = 173.4394F;
            this.nombre_articulo.HeaderText = "Nombre Articulo";
            this.nombre_articulo.Name = "nombre_articulo";
            this.nombre_articulo.ReadOnly = true;
            // 
            // sincroniza_articulo
            // 
            this.sincroniza_articulo.FalseValue = "false";
            this.sincroniza_articulo.FillWeight = 50.41844F;
            this.sincroniza_articulo.HeaderText = "Sincronizar";
            this.sincroniza_articulo.Name = "sincroniza_articulo";
            this.sincroniza_articulo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.sincroniza_articulo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.sincroniza_articulo.TrueValue = "true";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton1.Text = "Cargar articulos";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // Mostrar_Articulos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 471);
            this.Controls.Add(this.dt_atributos);
            this.Controls.Add(this.toolStrip2);
            this.Name = "Mostrar_Articulos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Articulos";
            this.Load += new System.EventHandler(this.Mostrar_Articulos_Load);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dt_atributos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btn_art;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.DataGridView dt_atributos;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripTextBox txt_dato;
        private System.Windows.Forms.ToolStripLabel toolStripLabel9;
        private System.Windows.Forms.ToolStripComboBox combo_opciones;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo_articulo;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre_articulo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sincroniza_articulo;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}