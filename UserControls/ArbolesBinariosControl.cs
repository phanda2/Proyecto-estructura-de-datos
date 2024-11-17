using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Estructura_de_datos.UserControls
{
    public partial class ArbolesBinariosControl : UserControl
    {
        private BinarySearchTreeProductos bst;

        public ArbolesBinariosControl()
        {
            InitializeComponent();
            bst = new BinarySearchTreeProductos();
            dgvInventario.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvInventario.CellClick += DgvInventario_CellClick;
        }

        private void DgvInventario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvInventario.Columns[e.ColumnIndex].Name == "Precio")
            {
                foreach (DataGridViewRow row in dgvInventario.Rows)
                {
                    row.Cells["Precio"].Style.BackColor = dgvInventario.DefaultCellStyle.BackColor;
                }

                dgvInventario.Rows[e.RowIndex].Cells["Precio"].Style.BackColor = Color.LightBlue;
            }
        }

        public class NodoProducto
        {
            public int Id { get; set; }
            public string Descripcion { get; set; }
            public string Categoria { get; set; }
            public DateTime FechaProduccion { get; set; }
            public DateTime FechaVencimiento { get; set; }
            public int Stock { get; set; }
            public decimal Precio { get; set; }
            public NodoProducto Izquierdo { get; set; }
            public NodoProducto Derecho { get; set; }

            public NodoProducto(int id, string descripcion, string categoria, DateTime fechaProduccion, DateTime fechaVencimiento, int stock, decimal precio)
            {
                Id = id;
                Descripcion = descripcion;
                Categoria = categoria;
                FechaProduccion = fechaProduccion;
                FechaVencimiento = fechaVencimiento;
                Stock = stock;
                Precio = precio;
                Izquierdo = null;
                Derecho = null;
            }
        }

        public class BinarySearchTreeProductos
        {
            public NodoProducto Root;

            public BinarySearchTreeProductos()
            {
                Root = null;
            }

            public void Insert(NodoProducto producto)
            {
                Root = InsertRecursive(Root, producto);
            }

            private NodoProducto InsertRecursive(NodoProducto node, NodoProducto producto)
            {
                if (node == null)
                {
                    return producto;
                }

                if (producto.Precio < node.Precio)
                {
                    node.Izquierdo = InsertRecursive(node.Izquierdo, producto);
                }
                else if (producto.Precio > node.Precio)
                {
                    node.Derecho = InsertRecursive(node.Derecho, producto);
                }
                else
                {
                    MessageBox.Show("Ya existe un producto con el mismo precio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return node;
            }

            public void Remove(decimal precio)
            {
                Root = RemoveRecursive(Root, precio);
            }

            private NodoProducto RemoveRecursive(NodoProducto node, decimal precio)
            {
                if (node == null) return node;

                if (precio < node.Precio)
                {
                    node.Izquierdo = RemoveRecursive(node.Izquierdo, precio);
                }
                else if (precio > node.Precio)
                {
                    node.Derecho = RemoveRecursive(node.Derecho, precio);
                }
                else
                {
                    if (node.Izquierdo == null) return node.Derecho;
                    if (node.Derecho == null) return node.Izquierdo;

                    NodoProducto minNode = FindMin(node.Derecho);
                    node.Precio = minNode.Precio;
                    node.Id = minNode.Id;
                    node.Descripcion = minNode.Descripcion;
                    node.Categoria = minNode.Categoria;
                    node.Stock = minNode.Stock;
                    node.FechaProduccion = minNode.FechaProduccion;
                    node.FechaVencimiento = minNode.FechaVencimiento;
                    node.Derecho = RemoveRecursive(node.Derecho, minNode.Precio);
                }

                return node;
            }

            private NodoProducto FindMin(NodoProducto node)
            {
                while (node.Izquierdo != null)
                {
                    node = node.Izquierdo;
                }
                return node;
            }

            public void InOrderTraversal(NodoProducto node, DataGridView dgv)
            {
                if (node != null)
                {
                    InOrderTraversal(node.Izquierdo, dgv);
                    dgv.Rows.Add(node.Id, node.Descripcion, node.Categoria, node.Stock, node.Precio, node.FechaProduccion, node.FechaVencimiento);
                    InOrderTraversal(node.Derecho, dgv);
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text) || string.IsNullOrWhiteSpace(txtCategoria.Text) ||
                numStockInicial.Value <= 0 || numPrecioVenta.Value <= 0)
            {
                MessageBox.Show("Por favor, complete todos los campos correctamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = dgvInventario.Rows.Count + 1;
            string descripcion = txtDescripcion.Text;
            string categoria = txtCategoria.Text;
            DateTime fechaProduccion = dtpFechaProduccion.Value;
            DateTime fechaVencimiento = dtpFechaVencimiento.Value;
            int stock = (int)numStockInicial.Value;
            decimal precio = numPrecioVenta.Value;

            NodoProducto nuevoProducto = new NodoProducto(id, descripcion, categoria, fechaProduccion, fechaVencimiento, stock, precio);
            bst.Insert(nuevoProducto);
            MostrarProductos();
            LimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvInventario.CurrentCell != null && dgvInventario.CurrentCell.OwningColumn.Name == "Precio" && dgvInventario.CurrentCell.RowIndex >= 0)
            {
                int rowIndex = dgvInventario.CurrentCell.RowIndex;
                decimal precio = Convert.ToDecimal(dgvInventario.Rows[rowIndex].Cells["Precio"].Value);

                bst.Remove(precio);
                MostrarProductos();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione la celda del precio en la fila del producto que desea eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtDescripcion.Clear();
            txtCategoria.Clear();
            numStockInicial.Value = 0;
            numPrecioVenta.Value = 0;
            dtpFechaProduccion.Value = DateTime.Now;
            dtpFechaVencimiento.Value = DateTime.Now;
        }

        private void MostrarProductos()
        {
            dgvInventario.Rows.Clear();
            bst.InOrderTraversal(bst.Root, dgvInventario);
        }
    }
}
