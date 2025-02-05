namespace AplixEcommerceIntegration.Nidux
{
    partial class Asignar_Atributos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Asignar_Atributos));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSeleccionar = new System.Windows.Forms.Button();
            this.txtAtributos = new System.Windows.Forms.TextBox();
            this.dgvAtributos = new System.Windows.Forms.DataGridView();
            this.CODIGO_NIDUX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DESCRIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ATRIBUTO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activar_atributo = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAtributos)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvAtributos, 0, 1);
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
            this.panel1.Controls.Add(this.txtAtributos);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(637, 34);
            this.panel1.TabIndex = 0;
            // 
            // btnSeleccionar
            // 
            this.btnSeleccionar.Location = new System.Drawing.Point(541, 5);
            this.btnSeleccionar.Name = "btnSeleccionar";
            this.btnSeleccionar.Size = new System.Drawing.Size(93, 23);
            this.btnSeleccionar.TabIndex = 1;
            this.btnSeleccionar.Text = "Seleccionar";
            this.btnSeleccionar.UseVisualStyleBackColor = true;
            this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
            // 
            // txtAtributos
            // 
            this.txtAtributos.Location = new System.Drawing.Point(6, 7);
            this.txtAtributos.Name = "txtAtributos";
            this.txtAtributos.Size = new System.Drawing.Size(212, 20);
            this.txtAtributos.TabIndex = 0;
            this.txtAtributos.TextChanged += new System.EventHandler(this.txtAtributos_TextChanged);
            // 
            // dgvAtributos
            // 
            this.dgvAtributos.AllowUserToAddRows = false;
            this.dgvAtributos.BackgroundColor = System.Drawing.Color.White;
            this.dgvAtributos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAtributos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAtributos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CODIGO_NIDUX,
            this.DESCRIP,
            this.ATRIBUTO,
            this.activar_atributo});
            this.dgvAtributos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAtributos.Location = new System.Drawing.Point(3, 43);
            this.dgvAtributos.Name = "dgvAtributos";
            this.dgvAtributos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvAtributos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAtributos.Size = new System.Drawing.Size(637, 296);
            this.dgvAtributos.TabIndex = 1;
            this.dgvAtributos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAtributos_CellContentClick);
            // 
            // CODIGO_NIDUX
            // 
            this.CODIGO_NIDUX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CODIGO_NIDUX.HeaderText = "Código";
            this.CODIGO_NIDUX.Name = "CODIGO_NIDUX";
            // 
            // DESCRIP
            // 
            this.DESCRIP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DESCRIP.HeaderText = "Descripción";
            this.DESCRIP.Name = "DESCRIP";
            // 
            // ATRIBUTO
            // 
            this.ATRIBUTO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ATRIBUTO.HeaderText = "Atributos";
            this.ATRIBUTO.Name = "ATRIBUTO";
            // 
            // activar_atributo
            // 
            this.activar_atributo.HeaderText = "Activar";
            this.activar_atributo.Name = "activar_atributo";
            this.activar_atributo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.activar_atributo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.activar_atributo.Width = 50;
            // 
            // Asignar_Atributos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(643, 342);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(659, 381);
            this.MinimumSize = new System.Drawing.Size(659, 381);
            this.Name = "Asignar_Atributos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asignar Atributos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Asignar_Atributos_FormClosing);
            this.Load += new System.EventHandler(this.Asignar_Atributos_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAtributos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtAtributos;
        private System.Windows.Forms.DataGridView dgvAtributos;
        private System.Windows.Forms.DataGridViewTextBoxColumn CODIGO_NIDUX;
        private System.Windows.Forms.DataGridViewTextBoxColumn DESCRIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ATRIBUTO;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activar_atributo;
        private System.Windows.Forms.Button btnSeleccionar;
    }
}