namespace AplixEcommerceIntegration.Shopify
{
    partial class MantenimientoEditarArticulos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MantenimientoEditarArticulos));
            this.dgvColecciones = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvVariantes = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.chbImpuesto = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbbEstado = new System.Windows.Forms.ComboBox();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.toolStripMantenimiento = new System.Windows.Forms.ToolStrip();
            this.btnGuardarArticulos = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColecciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariantes)).BeginInit();
            this.toolStripMantenimiento.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvColecciones
            // 
            this.dgvColecciones.BackgroundColor = System.Drawing.Color.White;
            this.dgvColecciones.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvColecciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvColecciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column4,
            this.Column7});
            this.dgvColecciones.Location = new System.Drawing.Point(332, 54);
            this.dgvColecciones.Name = "dgvColecciones";
            this.dgvColecciones.Size = new System.Drawing.Size(423, 126);
            this.dgvColecciones.TabIndex = 58;
            this.dgvColecciones.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvColecciones_CellMouseDoubleClick);
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Colecciones";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 130;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.HeaderText = "Nombre";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Activo";
            this.Column7.Name = "Column7";
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column7.Width = 50;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(334, 183);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 15);
            this.label6.TabIndex = 57;
            this.label6.Text = "Variantes de Artículos";
            // 
            // dgvVariantes
            // 
            this.dgvVariantes.BackgroundColor = System.Drawing.Color.White;
            this.dgvVariantes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvVariantes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVariantes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column5,
            this.Column6,
            this.Column8});
            this.dgvVariantes.Location = new System.Drawing.Point(332, 203);
            this.dgvVariantes.Name = "dgvVariantes";
            this.dgvVariantes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVariantes.Size = new System.Drawing.Size(423, 164);
            this.dgvVariantes.TabIndex = 56;
            this.dgvVariantes.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvVariantes_CellMouseDoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Artículos";
            this.Column1.Name = "Column1";
            this.Column1.Width = 90;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Variantes";
            this.Column2.Name = "Column2";
            this.Column2.Width = 80;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Cantidad";
            this.Column5.Name = "Column5";
            this.Column5.Width = 80;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Precio";
            this.Column6.Name = "Column6";
            this.Column6.Width = 80;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Activo";
            this.Column8.Name = "Column8";
            this.Column8.Width = 50;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(334, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 15);
            this.label5.TabIndex = 55;
            this.label5.Text = "Colección";
            // 
            // chbImpuesto
            // 
            this.chbImpuesto.AutoSize = true;
            this.chbImpuesto.Enabled = false;
            this.chbImpuesto.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbImpuesto.Location = new System.Drawing.Point(37, 334);
            this.chbImpuesto.Name = "chbImpuesto";
            this.chbImpuesto.Size = new System.Drawing.Size(165, 19);
            this.chbImpuesto.TabIndex = 49;
            this.chbImpuesto.Text = "Impuesto sobre el Artículo";
            this.chbImpuesto.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(34, 282);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 15);
            this.label4.TabIndex = 54;
            this.label4.Text = "Estado del Artículo";
            // 
            // cbbEstado
            // 
            this.cbbEstado.FormattingEnabled = true;
            this.cbbEstado.Location = new System.Drawing.Point(34, 300);
            this.cbbEstado.Name = "cbbEstado";
            this.cbbEstado.Size = new System.Drawing.Size(275, 21);
            this.cbbEstado.TabIndex = 48;
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(31, 55);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(275, 20);
            this.txtCodigo.TabIndex = 45;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(28, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 15);
            this.label3.TabIndex = 53;
            this.label3.Text = "Código Artículo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(31, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 15);
            this.label2.TabIndex = 52;
            this.label2.Text = "Descripción en Tienda Shopify";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 15);
            this.label1.TabIndex = 51;
            this.label1.Text = "Nombre en Tienda Shopify";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(31, 150);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(275, 122);
            this.txtDescripcion.TabIndex = 47;
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(31, 101);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(275, 20);
            this.txtNombre.TabIndex = 46;
            // 
            // toolStripMantenimiento
            // 
            this.toolStripMantenimiento.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMantenimiento.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnGuardarArticulos});
            this.toolStripMantenimiento.Location = new System.Drawing.Point(0, 0);
            this.toolStripMantenimiento.Name = "toolStripMantenimiento";
            this.toolStripMantenimiento.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripMantenimiento.Size = new System.Drawing.Size(781, 27);
            this.toolStripMantenimiento.TabIndex = 50;
            this.toolStripMantenimiento.Text = "toolStrip2";
            // 
            // btnGuardarArticulos
            // 
            this.btnGuardarArticulos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGuardarArticulos.Image = ((System.Drawing.Image)(resources.GetObject("btnGuardarArticulos.Image")));
            this.btnGuardarArticulos.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGuardarArticulos.Name = "btnGuardarArticulos";
            this.btnGuardarArticulos.Size = new System.Drawing.Size(24, 24);
            this.btnGuardarArticulos.ToolTipText = "Guardar Artículos";
            this.btnGuardarArticulos.Click += new System.EventHandler(this.btnGuardarArticulos_Click);
            // 
            // MantenimientoEditarArticulos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(781, 379);
            this.Controls.Add(this.dgvColecciones);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dgvVariantes);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chbImpuesto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbbEstado);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.toolStripMantenimiento);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(797, 418);
            this.MinimumSize = new System.Drawing.Size(797, 418);
            this.Name = "MantenimientoEditarArticulos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mantenimiento Articulos";
            this.Load += new System.EventHandler(this.MantenimientoEditarArticulos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvColecciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariantes)).EndInit();
            this.toolStripMantenimiento.ResumeLayout(false);
            this.toolStripMantenimiento.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvColecciones;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvVariantes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chbImpuesto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbbEstado;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.ToolStrip toolStripMantenimiento;
        private System.Windows.Forms.ToolStripButton btnGuardarArticulos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column8;
    }
}