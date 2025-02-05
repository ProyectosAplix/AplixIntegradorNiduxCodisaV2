namespace AplixEcommerceIntegration.WooCommerce
{
    partial class Envios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Envios));
            this.combo_zonas = new System.Windows.Forms.ComboBox();
            this.combo_metodos = new System.Windows.Forms.ComboBox();
            this.txt_articulos = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_descri = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_precio = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // combo_zonas
            // 
            this.combo_zonas.FormattingEnabled = true;
            this.combo_zonas.Location = new System.Drawing.Point(25, 58);
            this.combo_zonas.Name = "combo_zonas";
            this.combo_zonas.Size = new System.Drawing.Size(311, 21);
            this.combo_zonas.TabIndex = 0;
            this.combo_zonas.SelectedIndexChanged += new System.EventHandler(this.combo_zonas_SelectedIndexChanged);
            // 
            // combo_metodos
            // 
            this.combo_metodos.FormattingEnabled = true;
            this.combo_metodos.Location = new System.Drawing.Point(25, 104);
            this.combo_metodos.Name = "combo_metodos";
            this.combo_metodos.Size = new System.Drawing.Size(311, 21);
            this.combo_metodos.TabIndex = 1;
            // 
            // txt_articulos
            // 
            this.txt_articulos.Location = new System.Drawing.Point(25, 149);
            this.txt_articulos.Name = "txt_articulos";
            this.txt_articulos.ReadOnly = true;
            this.txt_articulos.Size = new System.Drawing.Size(311, 20);
            this.txt_articulos.TabIndex = 2;
            this.txt_articulos.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txt_articulos_MouseClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripButton3,
            this.toolStripSeparator2,
            this.toolStripSeparator3,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(362, 27);
            this.toolStrip1.TabIndex = 27;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton3.Text = "&Guardar";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(159, 24);
            this.toolStripLabel1.Text = "Agregar un método de envío";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Seleccionar una zona:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Seleccionar método de pago";
            // 
            // lb_descri
            // 
            this.lb_descri.AutoSize = true;
            this.lb_descri.Location = new System.Drawing.Point(25, 133);
            this.lb_descri.Name = "lb_descri";
            this.lb_descri.Size = new System.Drawing.Size(146, 13);
            this.lb_descri.TabIndex = 30;
            this.lb_descri.Text = "Seleccionar árticulo de envío";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "Precio con impuesto:";
            // 
            // txt_precio
            // 
            this.txt_precio.Location = new System.Drawing.Point(25, 198);
            this.txt_precio.Name = "txt_precio";
            this.txt_precio.ReadOnly = true;
            this.txt_precio.Size = new System.Drawing.Size(311, 20);
            this.txt_precio.TabIndex = 31;
            // 
            // Envios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 247);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_precio);
            this.Controls.Add(this.lb_descri);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.txt_articulos);
            this.Controls.Add(this.combo_metodos);
            this.Controls.Add(this.combo_zonas);
            this.MaximumSize = new System.Drawing.Size(378, 286);
            this.MinimumSize = new System.Drawing.Size(378, 276);
            this.Name = "Envios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Envios";
            this.Load += new System.EventHandler(this.Envios_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox combo_zonas;
        private System.Windows.Forms.ComboBox combo_metodos;
        private System.Windows.Forms.TextBox txt_articulos;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_descri;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_precio;
    }
}