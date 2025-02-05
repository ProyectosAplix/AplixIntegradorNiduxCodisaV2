namespace AplixEcommerceIntegration.WooCommerce
{
    partial class Categorias_Padre_Woocomerce
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
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtdato = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.combo_opciones = new System.Windows.Forms.ToolStripComboBox();
            this.dt_categorias = new System.Windows.Forms.DataGridView();
            this.codigo_categoria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre_categoria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.agregar = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dt_categorias)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip2
            // 
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator13,
            this.toolStripLabel1,
            this.txtdato,
            this.toolStripSeparator19,
            this.toolStripLabel2,
            this.combo_opciones});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip2.Size = new System.Drawing.Size(465, 25);
            this.toolStrip2.TabIndex = 27;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(65, 22);
            this.toolStripLabel1.Text = "Busqueda :";
            // 
            // txtdato
            // 
            this.txtdato.Name = "txtdato";
            this.txtdato.Size = new System.Drawing.Size(100, 25);
            this.txtdato.Click += new System.EventHandler(this.txtdato_Click);
            this.txtdato.TextChanged += new System.EventHandler(this.toolStripTextBox1_TextChanged);
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            this.toolStripSeparator19.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(25, 22);
            this.toolStripLabel2.Text = "por";
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
            // dt_categorias
            // 
            this.dt_categorias.AllowUserToAddRows = false;
            this.dt_categorias.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dt_categorias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dt_categorias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_categorias.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codigo_categoria,
            this.nombre_categoria,
            this.agregar});
            this.dt_categorias.Location = new System.Drawing.Point(0, 28);
            this.dt_categorias.Name = "dt_categorias";
            this.dt_categorias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dt_categorias.Size = new System.Drawing.Size(465, 250);
            this.dt_categorias.TabIndex = 28;
            this.dt_categorias.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dt_categorias_CellContentClick);
            // 
            // codigo_categoria
            // 
            this.codigo_categoria.FillWeight = 76.14214F;
            this.codigo_categoria.HeaderText = "Código Categoría";
            this.codigo_categoria.Name = "codigo_categoria";
            this.codigo_categoria.ReadOnly = true;
            // 
            // nombre_categoria
            // 
            this.nombre_categoria.FillWeight = 111.9289F;
            this.nombre_categoria.HeaderText = "Nombre Categoría";
            this.nombre_categoria.Name = "nombre_categoria";
            this.nombre_categoria.ReadOnly = true;
            // 
            // agregar
            // 
            this.agregar.FillWeight = 111.9289F;
            this.agregar.HeaderText = "Categoría Padre";
            this.agregar.Name = "agregar";
            // 
            // Categorias_Padre_Woocomerce
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 278);
            this.Controls.Add(this.dt_categorias);
            this.Controls.Add(this.toolStrip2);
            this.Name = "Categorias_Padre_Woocomerce";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Categorias padre";
            this.Load += new System.EventHandler(this.Categorias_Padre_Woocomerce_Load);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dt_categorias)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtdato;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox combo_opciones;
        private System.Windows.Forms.DataGridView dt_categorias;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo_categoria;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre_categoria;
        private System.Windows.Forms.DataGridViewCheckBoxColumn agregar;
    }
}