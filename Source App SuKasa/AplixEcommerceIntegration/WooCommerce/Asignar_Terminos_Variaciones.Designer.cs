namespace AplixEcommerceIntegration.WooCommerce
{
    partial class Asignar_Terminos_Variaciones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Asignar_Terminos_Variaciones));
            this.dt_variaciones = new System.Windows.Forms.DataGridView();
            this.cod_atributo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codigo_termio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre_atributo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estado = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_art = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.dt_variaciones)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dt_variaciones
            // 
            this.dt_variaciones.AllowUserToAddRows = false;
            this.dt_variaciones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dt_variaciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dt_variaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_variaciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cod_atributo,
            this.codigo_termio,
            this.nombre_atributo,
            this.estado});
            this.dt_variaciones.Location = new System.Drawing.Point(0, 30);
            this.dt_variaciones.Name = "dt_variaciones";
            this.dt_variaciones.Size = new System.Drawing.Size(389, 334);
            this.dt_variaciones.TabIndex = 46;
            this.dt_variaciones.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dt_variaciones_CellContentClick);
            // 
            // cod_atributo
            // 
            this.cod_atributo.HeaderText = "Codigo Atributo";
            this.cod_atributo.Name = "cod_atributo";
            this.cod_atributo.Visible = false;
            // 
            // codigo_termio
            // 
            this.codigo_termio.HeaderText = "Codigo Termino";
            this.codigo_termio.Name = "codigo_termio";
            this.codigo_termio.Visible = false;
            // 
            // nombre_atributo
            // 
            this.nombre_atributo.FillWeight = 169.5432F;
            this.nombre_atributo.HeaderText = "Nombre";
            this.nombre_atributo.Name = "nombre_atributo";
            // 
            // estado
            // 
            this.estado.FillWeight = 30.45685F;
            this.estado.HeaderText = "Asignar";
            this.estado.Name = "estado";
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
            this.toolStrip1.Size = new System.Drawing.Size(389, 27);
            this.toolStrip1.TabIndex = 48;
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
            // Asignar_Terminos_Variaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 365);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dt_variaciones);
            this.Name = "Asignar_Terminos_Variaciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asignar Valores de Variacion";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Asignar_Terminos_Variaciones_FormClosing);
            this.Load += new System.EventHandler(this.Asignar_Terminos_Variaciones_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dt_variaciones)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dt_variaciones;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btn_art;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.DataGridViewTextBoxColumn cod_atributo;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo_termio;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre_atributo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn estado;
    }
}