namespace AplixEcommerceIntegration.Shopify
{
    partial class MostrarPedidoLinea
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MostrarPedidoLinea));
            this.dgvPedidoLinea = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ORDER_NUMBER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PRODUCT_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PRICE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PRODUCT_EXISTS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QUANTITY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REQUIRES_SHIPPING = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SKU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TAXABLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOTAL_DISCOUNT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VARIANT_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VARIANT_INVENTORY_MANAGEMENT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VARIANT_TITLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VENDOR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TAX_LINE_PRICE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TAX_LINE_RATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TAX_LINE_TITLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidoLinea)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPedidoLinea
            // 
            this.dgvPedidoLinea.AllowUserToAddRows = false;
            this.dgvPedidoLinea.BackgroundColor = System.Drawing.Color.White;
            this.dgvPedidoLinea.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPedidoLinea.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPedidoLinea.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.ORDER_NUMBER,
            this.PRODUCT_ID,
            this.NAME,
            this.PRICE,
            this.PRODUCT_EXISTS,
            this.QUANTITY,
            this.REQUIRES_SHIPPING,
            this.SKU,
            this.TAXABLE,
            this.TOTAL_DISCOUNT,
            this.VARIANT_ID,
            this.VARIANT_INVENTORY_MANAGEMENT,
            this.VARIANT_TITLE,
            this.VENDOR,
            this.TAX_LINE_PRICE,
            this.TAX_LINE_RATE,
            this.TAX_LINE_TITLE});
            this.dgvPedidoLinea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPedidoLinea.Location = new System.Drawing.Point(0, 0);
            this.dgvPedidoLinea.Name = "dgvPedidoLinea";
            this.dgvPedidoLinea.ReadOnly = true;
            this.dgvPedidoLinea.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPedidoLinea.Size = new System.Drawing.Size(893, 419);
            this.dgvPedidoLinea.TabIndex = 0;
            // 
            // ID
            // 
            this.ID.HeaderText = "Id";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // ORDER_NUMBER
            // 
            this.ORDER_NUMBER.HeaderText = "Número de Orden";
            this.ORDER_NUMBER.Name = "ORDER_NUMBER";
            this.ORDER_NUMBER.ReadOnly = true;
            // 
            // PRODUCT_ID
            // 
            this.PRODUCT_ID.HeaderText = "ID Producto Shopify";
            this.PRODUCT_ID.Name = "PRODUCT_ID";
            this.PRODUCT_ID.ReadOnly = true;
            // 
            // NAME
            // 
            this.NAME.HeaderText = "Nombre";
            this.NAME.Name = "NAME";
            this.NAME.ReadOnly = true;
            // 
            // PRICE
            // 
            this.PRICE.HeaderText = "Precio";
            this.PRICE.Name = "PRICE";
            this.PRICE.ReadOnly = true;
            // 
            // PRODUCT_EXISTS
            // 
            this.PRODUCT_EXISTS.HeaderText = "Producto Existe";
            this.PRODUCT_EXISTS.Name = "PRODUCT_EXISTS";
            this.PRODUCT_EXISTS.ReadOnly = true;
            // 
            // QUANTITY
            // 
            this.QUANTITY.HeaderText = "Cantidad";
            this.QUANTITY.Name = "QUANTITY";
            this.QUANTITY.ReadOnly = true;
            // 
            // REQUIRES_SHIPPING
            // 
            this.REQUIRES_SHIPPING.HeaderText = "Requiere Envío";
            this.REQUIRES_SHIPPING.Name = "REQUIRES_SHIPPING";
            this.REQUIRES_SHIPPING.ReadOnly = true;
            // 
            // SKU
            // 
            this.SKU.HeaderText = "Sku";
            this.SKU.Name = "SKU";
            this.SKU.ReadOnly = true;
            // 
            // TAXABLE
            // 
            this.TAXABLE.HeaderText = "Impuesto";
            this.TAXABLE.Name = "TAXABLE";
            this.TAXABLE.ReadOnly = true;
            // 
            // TOTAL_DISCOUNT
            // 
            this.TOTAL_DISCOUNT.HeaderText = "Total Descuento";
            this.TOTAL_DISCOUNT.Name = "TOTAL_DISCOUNT";
            this.TOTAL_DISCOUNT.ReadOnly = true;
            // 
            // VARIANT_ID
            // 
            this.VARIANT_ID.HeaderText = "ID Variante Producto";
            this.VARIANT_ID.Name = "VARIANT_ID";
            this.VARIANT_ID.ReadOnly = true;
            // 
            // VARIANT_INVENTORY_MANAGEMENT
            // 
            this.VARIANT_INVENTORY_MANAGEMENT.HeaderText = "Administrador de Inventario";
            this.VARIANT_INVENTORY_MANAGEMENT.Name = "VARIANT_INVENTORY_MANAGEMENT";
            this.VARIANT_INVENTORY_MANAGEMENT.ReadOnly = true;
            // 
            // VARIANT_TITLE
            // 
            this.VARIANT_TITLE.HeaderText = "Nombre Variante";
            this.VARIANT_TITLE.Name = "VARIANT_TITLE";
            this.VARIANT_TITLE.ReadOnly = true;
            // 
            // VENDOR
            // 
            this.VENDOR.HeaderText = "Vendedor";
            this.VENDOR.Name = "VENDOR";
            this.VENDOR.ReadOnly = true;
            // 
            // TAX_LINE_PRICE
            // 
            this.TAX_LINE_PRICE.HeaderText = "Impuesto Precio";
            this.TAX_LINE_PRICE.Name = "TAX_LINE_PRICE";
            this.TAX_LINE_PRICE.ReadOnly = true;
            // 
            // TAX_LINE_RATE
            // 
            this.TAX_LINE_RATE.HeaderText = "Porcentaje de Impuesto";
            this.TAX_LINE_RATE.Name = "TAX_LINE_RATE";
            this.TAX_LINE_RATE.ReadOnly = true;
            // 
            // TAX_LINE_TITLE
            // 
            this.TAX_LINE_TITLE.HeaderText = "Nombre del Impuesto";
            this.TAX_LINE_TITLE.Name = "TAX_LINE_TITLE";
            this.TAX_LINE_TITLE.ReadOnly = true;
            // 
            // MostrarPedidoLinea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(893, 419);
            this.Controls.Add(this.dgvPedidoLinea);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MostrarPedidoLinea";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pedido Linea";
            this.Load += new System.EventHandler(this.MostrarPedidoLinea_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidoLinea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPedidoLinea;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ORDER_NUMBER;
        private System.Windows.Forms.DataGridViewTextBoxColumn PRODUCT_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn PRICE;
        private System.Windows.Forms.DataGridViewTextBoxColumn PRODUCT_EXISTS;
        private System.Windows.Forms.DataGridViewTextBoxColumn QUANTITY;
        private System.Windows.Forms.DataGridViewTextBoxColumn REQUIRES_SHIPPING;
        private System.Windows.Forms.DataGridViewTextBoxColumn SKU;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAXABLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOTAL_DISCOUNT;
        private System.Windows.Forms.DataGridViewTextBoxColumn VARIANT_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn VARIANT_INVENTORY_MANAGEMENT;
        private System.Windows.Forms.DataGridViewTextBoxColumn VARIANT_TITLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn VENDOR;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAX_LINE_PRICE;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAX_LINE_RATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAX_LINE_TITLE;
    }
}