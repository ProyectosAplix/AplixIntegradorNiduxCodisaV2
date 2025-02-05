namespace AplixEcommerceIntegration.WooCommerce
{
    partial class Mostrar_Articulos_Envio
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
            this.dt_articulos = new System.Windows.Forms.DataGridView();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.txt_dato = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel9 = new System.Windows.Forms.ToolStripLabel();
            this.combo_opciones = new System.Windows.Forms.ToolStripComboBox();
            this.codigo_articulo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre_articulo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sincroniza_articulo = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dt_articulos)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dt_articulos
            // 
            this.dt_articulos.AllowUserToAddRows = false;
            this.dt_articulos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dt_articulos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dt_articulos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_articulos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codigo_articulo,
            this.nombre_articulo,
            this.Column1,
            this.Column2,
            this.Column3,
            this.sincroniza_articulo});
            this.dt_articulos.Location = new System.Drawing.Point(2, 33);
            this.dt_articulos.Name = "dt_articulos";
            this.dt_articulos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dt_articulos.Size = new System.Drawing.Size(884, 445);
            this.dt_articulos.TabIndex = 28;
            this.dt_articulos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dt_articulos_CellContentClick);
            // 
            // toolStrip2
            // 
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.toolStripLabel7,
            this.txt_dato,
            this.toolStripLabel9,
            this.combo_opciones});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip2.Size = new System.Drawing.Size(887, 25);
            this.toolStrip2.TabIndex = 29;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(59, 22);
            this.toolStripLabel7.Text = "Busqueda";
            // 
            // txt_dato
            // 
            this.txt_dato.Name = "txt_dato";
            this.txt_dato.Size = new System.Drawing.Size(100, 25);
            this.txt_dato.TextChanged += new System.EventHandler(this.txt_dato_TextChanged);
            // 
            // toolStripLabel9
            // 
            this.toolStripLabel9.Name = "toolStripLabel9";
            this.toolStripLabel9.Size = new System.Drawing.Size(28, 22);
            this.toolStripLabel9.Text = "por:";
            // 
            // combo_opciones
            // 
            this.combo_opciones.Items.AddRange(new object[] {
            "Código",
            "Nombre"});
            this.combo_opciones.Name = "combo_opciones";
            this.combo_opciones.Size = new System.Drawing.Size(121, 25);
            this.combo_opciones.Text = "Código";
            // 
            // codigo_articulo
            // 
            this.codigo_articulo.FillWeight = 121.8274F;
            this.codigo_articulo.HeaderText = "Articulo";
            this.codigo_articulo.Name = "codigo_articulo";
            this.codigo_articulo.ReadOnly = true;
            // 
            // nombre_articulo
            // 
            this.nombre_articulo.FillWeight = 158.3139F;
            this.nombre_articulo.HeaderText = "Descripción";
            this.nombre_articulo.Name = "nombre_articulo";
            this.nombre_articulo.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.FillWeight = 91.27907F;
            this.Column1.HeaderText = "Precio con Impuesto";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.FillWeight = 91.27907F;
            this.Column2.HeaderText = "Impuesto";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.FillWeight = 91.27907F;
            this.Column3.HeaderText = "Precio con Impuesto";
            this.Column3.Name = "Column3";
            // 
            // sincroniza_articulo
            // 
            this.sincroniza_articulo.FalseValue = "false";
            this.sincroniza_articulo.FillWeight = 46.02148F;
            this.sincroniza_articulo.HeaderText = "Estado";
            this.sincroniza_articulo.Name = "sincroniza_articulo";
            this.sincroniza_articulo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.sincroniza_articulo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.sincroniza_articulo.TrueValue = "true";
            // 
            // Mostrar_Articulos_Envio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 482);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.dt_articulos);
            this.Name = "Mostrar_Articulos_Envio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Articulos de Envío";
            this.Load += new System.EventHandler(this.Mostrar_Articulos_Envio_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dt_articulos)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dt_articulos;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripTextBox txt_dato;
        private System.Windows.Forms.ToolStripLabel toolStripLabel9;
        private System.Windows.Forms.ToolStripComboBox combo_opciones;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo_articulo;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre_articulo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sincroniza_articulo;
    }
}