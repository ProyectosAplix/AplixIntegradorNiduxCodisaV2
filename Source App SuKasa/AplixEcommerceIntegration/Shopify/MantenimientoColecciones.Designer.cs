namespace AplixEcommerceIntegration.Shopify
{
    partial class MantenimientoColecciones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MantenimientoColecciones));
            this.toolStripMantenimiento = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtColecciones = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cbbColeccion = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGuardarColeccion = new System.Windows.Forms.ToolStripButton();
            this.dgvColeccion = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.toolStripMantenimiento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColeccion)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMantenimiento
            // 
            this.toolStripMantenimiento.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMantenimiento.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtColecciones,
            this.toolStripSeparator1,
            this.cbbColeccion,
            this.toolStripSeparator2,
            this.btnGuardarColeccion});
            this.toolStripMantenimiento.Location = new System.Drawing.Point(0, 0);
            this.toolStripMantenimiento.Name = "toolStripMantenimiento";
            this.toolStripMantenimiento.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripMantenimiento.Size = new System.Drawing.Size(545, 27);
            this.toolStripMantenimiento.TabIndex = 31;
            this.toolStripMantenimiento.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(109, 24);
            this.toolStripLabel1.Text = "Buscar Colecciones";
            // 
            // txtColecciones
            // 
            this.txtColecciones.Name = "txtColecciones";
            this.txtColecciones.Size = new System.Drawing.Size(170, 27);
            this.txtColecciones.TextChanged += new System.EventHandler(this.txtColecciones_TextChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // cbbColeccion
            // 
            this.cbbColeccion.Items.AddRange(new object[] {
            "Código",
            "Nombre"});
            this.cbbColeccion.Name = "cbbColeccion";
            this.cbbColeccion.Size = new System.Drawing.Size(121, 27);
            this.cbbColeccion.Text = "Código";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // btnGuardarColeccion
            // 
            this.btnGuardarColeccion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGuardarColeccion.Image = ((System.Drawing.Image)(resources.GetObject("btnGuardarColeccion.Image")));
            this.btnGuardarColeccion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGuardarColeccion.Name = "btnGuardarColeccion";
            this.btnGuardarColeccion.Size = new System.Drawing.Size(24, 24);
            this.btnGuardarColeccion.ToolTipText = "Seleccionar Colecciones";
            this.btnGuardarColeccion.Click += new System.EventHandler(this.btnGuardarColeccion_Click);
            // 
            // dgvColeccion
            // 
            this.dgvColeccion.AllowUserToAddRows = false;
            this.dgvColeccion.BackgroundColor = System.Drawing.Color.White;
            this.dgvColeccion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvColeccion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvColeccion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dgvColeccion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvColeccion.Location = new System.Drawing.Point(0, 27);
            this.dgvColeccion.Name = "dgvColeccion";
            this.dgvColeccion.Size = new System.Drawing.Size(545, 285);
            this.dgvColeccion.TabIndex = 32;
            this.dgvColeccion.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvColeccion_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Código Colección";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 170;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "Nombre";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Seleccionar";
            this.Column3.Name = "Column3";
            this.Column3.Width = 80;
            // 
            // MantenimientoColecciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(545, 312);
            this.Controls.Add(this.dgvColeccion);
            this.Controls.Add(this.toolStripMantenimiento);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MantenimientoColecciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asignar Colecciones";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MantenimientoColecciones_FormClosing);
            this.Load += new System.EventHandler(this.MantenimientoColecciones_Load);
            this.toolStripMantenimiento.ResumeLayout(false);
            this.toolStripMantenimiento.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColeccion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMantenimiento;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtColecciones;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox cbbColeccion;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnGuardarColeccion;
        private System.Windows.Forms.DataGridView dgvColeccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
    }
}