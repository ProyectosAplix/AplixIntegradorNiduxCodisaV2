namespace AplixEcommerceIntegration.Nidux
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pedido_Linea));
            this.dgvPedidoLinea = new System.Windows.Forms.DataGridView();
            this.ORDERID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID_PRODUCTO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID_VARIACION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SKU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NOMBRE_PRODUCTO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PRECIO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CANTIDAD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PORCENTAJE_DESCUENTO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SUBTOTAL_DESCUENTO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SUBTOTAL_LINEA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IMPUESTO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SUBTOTAL_IMPUESTOS = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.ORDERID,
            this.ID_PRODUCTO,
            this.ID_VARIACION,
            this.SKU,
            this.NOMBRE_PRODUCTO,
            this.PRECIO,
            this.CANTIDAD,
            this.PORCENTAJE_DESCUENTO,
            this.SUBTOTAL_DESCUENTO,
            this.SUBTOTAL_LINEA,
            this.IMPUESTO,
            this.SUBTOTAL_IMPUESTOS});
            this.dgvPedidoLinea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPedidoLinea.Location = new System.Drawing.Point(0, 0);
            this.dgvPedidoLinea.Name = "dgvPedidoLinea";
            this.dgvPedidoLinea.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPedidoLinea.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPedidoLinea.Size = new System.Drawing.Size(742, 354);
            this.dgvPedidoLinea.TabIndex = 0;
            // 
            // ORDERID
            // 
            this.ORDERID.HeaderText = "Número de Orden";
            this.ORDERID.Name = "ORDERID";
            // 
            // ID_PRODUCTO
            // 
            this.ID_PRODUCTO.HeaderText = "Código Artículo";
            this.ID_PRODUCTO.Name = "ID_PRODUCTO";
            // 
            // ID_VARIACION
            // 
            this.ID_VARIACION.HeaderText = "Código Variación Artículo";
            this.ID_VARIACION.Name = "ID_VARIACION";
            // 
            // SKU
            // 
            this.SKU.HeaderText = "Sku";
            this.SKU.Name = "SKU";
            // 
            // NOMBRE_PRODUCTO
            // 
            this.NOMBRE_PRODUCTO.HeaderText = "Nombre";
            this.NOMBRE_PRODUCTO.Name = "NOMBRE_PRODUCTO";
            // 
            // PRECIO
            // 
            this.PRECIO.HeaderText = "Precio";
            this.PRECIO.Name = "PRECIO";
            // 
            // CANTIDAD
            // 
            this.CANTIDAD.HeaderText = "Cantidad";
            this.CANTIDAD.Name = "CANTIDAD";
            // 
            // PORCENTAJE_DESCUENTO
            // 
            this.PORCENTAJE_DESCUENTO.HeaderText = "Porcentaje Descuento";
            this.PORCENTAJE_DESCUENTO.Name = "PORCENTAJE_DESCUENTO";
            // 
            // SUBTOTAL_DESCUENTO
            // 
            this.SUBTOTAL_DESCUENTO.HeaderText = "SubTotal Descuento";
            this.SUBTOTAL_DESCUENTO.Name = "SUBTOTAL_DESCUENTO";
            // 
            // SUBTOTAL_LINEA
            // 
            this.SUBTOTAL_LINEA.HeaderText = "SubTotal Línea";
            this.SUBTOTAL_LINEA.Name = "SUBTOTAL_LINEA";
            // 
            // IMPUESTO
            // 
            this.IMPUESTO.HeaderText = "Impuesto";
            this.IMPUESTO.Name = "IMPUESTO";
            // 
            // SUBTOTAL_IMPUESTOS
            // 
            this.SUBTOTAL_IMPUESTOS.HeaderText = "SubTotal Impuestos";
            this.SUBTOTAL_IMPUESTOS.Name = "SUBTOTAL_IMPUESTOS";
            // 
            // Pedido_Linea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(742, 354);
            this.Controls.Add(this.dgvPedidoLinea);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Pedido_Linea";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detalles del Pedido";
            this.Load += new System.EventHandler(this.Pedido_Linea_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidoLinea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPedidoLinea;
        private System.Windows.Forms.DataGridViewTextBoxColumn ORDERID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID_PRODUCTO;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID_VARIACION;
        private System.Windows.Forms.DataGridViewTextBoxColumn SKU;
        private System.Windows.Forms.DataGridViewTextBoxColumn NOMBRE_PRODUCTO;
        private System.Windows.Forms.DataGridViewTextBoxColumn PRECIO;
        private System.Windows.Forms.DataGridViewTextBoxColumn CANTIDAD;
        private System.Windows.Forms.DataGridViewTextBoxColumn PORCENTAJE_DESCUENTO;
        private System.Windows.Forms.DataGridViewTextBoxColumn SUBTOTAL_DESCUENTO;
        private System.Windows.Forms.DataGridViewTextBoxColumn SUBTOTAL_LINEA;
        private System.Windows.Forms.DataGridViewTextBoxColumn IMPUESTO;
        private System.Windows.Forms.DataGridViewTextBoxColumn SUBTOTAL_IMPUESTOS;
    }
}