namespace AplixEcommerceIntegration.Nidux
{
    partial class Asignar_Categorias
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Asignar_Categorias));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSeleccionar = new System.Windows.Forms.Button();
            this.txtCategorias = new System.Windows.Forms.TextBox();
            this.dgvCategoriasSelect = new System.Windows.Forms.DataGridView();
            this.NOMBRE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CODIGO_CATEGORIAS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cod_sistema_cat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activar = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategoriasSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvCategoriasSelect, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(643, 342);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSeleccionar);
            this.panel1.Controls.Add(this.txtCategorias);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(637, 34);
            this.panel1.TabIndex = 0;
            // 
            // btnSeleccionar
            // 
            this.btnSeleccionar.Location = new System.Drawing.Point(533, 5);
            this.btnSeleccionar.Name = "btnSeleccionar";
            this.btnSeleccionar.Size = new System.Drawing.Size(99, 23);
            this.btnSeleccionar.TabIndex = 1;
            this.btnSeleccionar.Text = "Seleccionar";
            this.btnSeleccionar.UseVisualStyleBackColor = true;
            this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
            // 
            // txtCategorias
            // 
            this.txtCategorias.Location = new System.Drawing.Point(7, 7);
            this.txtCategorias.Name = "txtCategorias";
            this.txtCategorias.Size = new System.Drawing.Size(207, 20);
            this.txtCategorias.TabIndex = 0;
            this.txtCategorias.TextChanged += new System.EventHandler(this.txtCategorias_TextChanged);
            // 
            // dgvCategoriasSelect
            // 
            this.dgvCategoriasSelect.AllowUserToAddRows = false;
            this.dgvCategoriasSelect.BackgroundColor = System.Drawing.Color.White;
            this.dgvCategoriasSelect.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCategoriasSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategoriasSelect.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NOMBRE,
            this.CODIGO_CATEGORIAS,
            this.Cod_sistema_cat,
            this.activar});
            this.dgvCategoriasSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCategoriasSelect.Location = new System.Drawing.Point(3, 43);
            this.dgvCategoriasSelect.Name = "dgvCategoriasSelect";
            this.dgvCategoriasSelect.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCategoriasSelect.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCategoriasSelect.Size = new System.Drawing.Size(637, 296);
            this.dgvCategoriasSelect.TabIndex = 1;
            this.dgvCategoriasSelect.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCategoriasSelect_CellContentClick);
            // 
            // NOMBRE
            // 
            this.NOMBRE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NOMBRE.HeaderText = "Nombre";
            this.NOMBRE.Name = "NOMBRE";
            // 
            // CODIGO_CATEGORIAS
            // 
            this.CODIGO_CATEGORIAS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CODIGO_CATEGORIAS.HeaderText = "Código";
            this.CODIGO_CATEGORIAS.Name = "CODIGO_CATEGORIAS";
            // 
            // Cod_sistema_cat
            // 
            this.Cod_sistema_cat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Cod_sistema_cat.HeaderText = "SubCódigo";
            this.Cod_sistema_cat.Name = "Cod_sistema_cat";
            // 
            // activar
            // 
            this.activar.HeaderText = "Activar";
            this.activar.Name = "activar";
            this.activar.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.activar.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.activar.Width = 50;
            // 
            // Asignar_Categorias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(643, 342);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(659, 381);
            this.MinimumSize = new System.Drawing.Size(659, 381);
            this.Name = "Asignar_Categorias";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asignar Categorias";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Asignar_Categorias_FormClosing);
            this.Load += new System.EventHandler(this.Asignar_Categorias_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategoriasSelect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvCategoriasSelect;
        private System.Windows.Forms.TextBox txtCategorias;
        private System.Windows.Forms.DataGridViewTextBoxColumn NOMBRE;
        private System.Windows.Forms.DataGridViewTextBoxColumn CODIGO_CATEGORIAS;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cod_sistema_cat;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activar;
        private System.Windows.Forms.Button btnSeleccionar;
    }
}