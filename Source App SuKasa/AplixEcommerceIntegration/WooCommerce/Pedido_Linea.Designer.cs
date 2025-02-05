namespace AplixEcommerceIntegration.WooCommerce
{
    partial class Pedido_Linea
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dt_lineas = new System.Windows.Forms.DataGridView();
            this.id_pedido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.variation_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tax_class = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subtotal_tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shipping_tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sku = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.linea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createddate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recorddate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dt_metodos = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id_envio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.method_tittle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.method_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.instance_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toatal_envio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_tax_envio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineas_envio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn42 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn43 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dt_lineas)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dt_metodos)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(925, 462);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dt_lineas);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(917, 436);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Líneas de pedidos";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dt_lineas
            // 
            this.dt_lineas.AllowUserToAddRows = false;
            this.dt_lineas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dt_lineas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_lineas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_pedido,
            this.id,
            this.name,
            this.product_id,
            this.variation_id,
            this.quantity,
            this.tax_class,
            this.subtotal_tax,
            this.subtotal,
            this.total,
            this.total_tax,
            this.shipping_tax,
            this.sku,
            this.price,
            this.linea,
            this.createddate,
            this.recorddate});
            this.dt_lineas.Location = new System.Drawing.Point(6, 6);
            this.dt_lineas.Name = "dt_lineas";
            this.dt_lineas.Size = new System.Drawing.Size(905, 424);
            this.dt_lineas.TabIndex = 34;
            // 
            // id_pedido
            // 
            this.id_pedido.HeaderText = "Id Pedido";
            this.id_pedido.Name = "id_pedido";
            // 
            // id
            // 
            this.id.HeaderText = "Id";
            this.id.Name = "id";
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            // 
            // product_id
            // 
            this.product_id.HeaderText = "Product Id";
            this.product_id.Name = "product_id";
            // 
            // variation_id
            // 
            this.variation_id.HeaderText = "Variation Id";
            this.variation_id.Name = "variation_id";
            // 
            // quantity
            // 
            this.quantity.HeaderText = "Quantity";
            this.quantity.Name = "quantity";
            // 
            // tax_class
            // 
            this.tax_class.HeaderText = "Tax Class";
            this.tax_class.Name = "tax_class";
            // 
            // subtotal_tax
            // 
            this.subtotal_tax.HeaderText = "Sub Total Tax";
            this.subtotal_tax.Name = "subtotal_tax";
            // 
            // subtotal
            // 
            this.subtotal.HeaderText = "Subtotal";
            this.subtotal.Name = "subtotal";
            // 
            // total
            // 
            this.total.HeaderText = "Total";
            this.total.Name = "total";
            // 
            // total_tax
            // 
            this.total_tax.HeaderText = "Total Tax";
            this.total_tax.Name = "total_tax";
            // 
            // shipping_tax
            // 
            this.shipping_tax.HeaderText = "Shipping Tax";
            this.shipping_tax.Name = "shipping_tax";
            // 
            // sku
            // 
            this.sku.HeaderText = "Sku";
            this.sku.Name = "sku";
            // 
            // price
            // 
            this.price.HeaderText = "Price";
            this.price.Name = "price";
            // 
            // linea
            // 
            this.linea.HeaderText = "Linea";
            this.linea.Name = "linea";
            // 
            // createddate
            // 
            this.createddate.HeaderText = "Created Date";
            this.createddate.Name = "createddate";
            // 
            // recorddate
            // 
            this.recorddate.HeaderText = "Record Date";
            this.recorddate.Name = "recorddate";
            // 
            // tabPage2
            // 
            this.tabPage2.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.tabPage2.Controls.Add(this.dt_metodos);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(917, 436);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Métodos de envío";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dt_metodos
            // 
            this.dt_metodos.AllowUserToAddRows = false;
            this.dt_metodos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dt_metodos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_metodos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.id_envio,
            this.method_tittle,
            this.method_id,
            this.instance_id,
            this.toatal_envio,
            this.total_tax_envio,
            this.lineas_envio,
            this.dataGridViewTextBoxColumn42,
            this.dataGridViewTextBoxColumn43});
            this.dt_metodos.Location = new System.Drawing.Point(4, 6);
            this.dt_metodos.Name = "dt_metodos";
            this.dt_metodos.Size = new System.Drawing.Size(904, 424);
            this.dt_metodos.TabIndex = 35;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Id Pedido";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // id_envio
            // 
            this.id_envio.HeaderText = "Id";
            this.id_envio.Name = "id_envio";
            // 
            // method_tittle
            // 
            this.method_tittle.HeaderText = "Method Tittle";
            this.method_tittle.Name = "method_tittle";
            // 
            // method_id
            // 
            this.method_id.HeaderText = "Method Id";
            this.method_id.Name = "method_id";
            // 
            // instance_id
            // 
            this.instance_id.HeaderText = "Instance Id";
            this.instance_id.Name = "instance_id";
            // 
            // toatal_envio
            // 
            this.toatal_envio.HeaderText = "Total";
            this.toatal_envio.Name = "toatal_envio";
            // 
            // total_tax_envio
            // 
            this.total_tax_envio.HeaderText = "Total Tax";
            this.total_tax_envio.Name = "total_tax_envio";
            // 
            // lineas_envio
            // 
            this.lineas_envio.HeaderText = "Líneas ";
            this.lineas_envio.Name = "lineas_envio";
            // 
            // dataGridViewTextBoxColumn42
            // 
            this.dataGridViewTextBoxColumn42.HeaderText = "Created Date";
            this.dataGridViewTextBoxColumn42.Name = "dataGridViewTextBoxColumn42";
            // 
            // dataGridViewTextBoxColumn43
            // 
            this.dataGridViewTextBoxColumn43.HeaderText = "Record Date";
            this.dataGridViewTextBoxColumn43.Name = "dataGridViewTextBoxColumn43";
            // 
            // Pedido_Linea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 465);
            this.Controls.Add(this.tabControl1);
            this.MaximumSize = new System.Drawing.Size(944, 504);
            this.Name = "Pedido_Linea";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Líneas del pedido";
            this.Load += new System.EventHandler(this.Pedido_Linea_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dt_lineas)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dt_metodos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dt_lineas;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_pedido;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn variation_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn tax_class;
        private System.Windows.Forms.DataGridViewTextBoxColumn subtotal_tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn subtotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipping_tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn sku;
        private System.Windows.Forms.DataGridViewTextBoxColumn price;
        private System.Windows.Forms.DataGridViewTextBoxColumn linea;
        private System.Windows.Forms.DataGridViewTextBoxColumn createddate;
        private System.Windows.Forms.DataGridViewTextBoxColumn recorddate;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dt_metodos;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_envio;
        private System.Windows.Forms.DataGridViewTextBoxColumn method_tittle;
        private System.Windows.Forms.DataGridViewTextBoxColumn method_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn instance_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn toatal_envio;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_tax_envio;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineas_envio;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn42;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn43;
    }
}