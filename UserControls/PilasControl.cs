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
    public partial class PilasControl : UserControl
    {
        private Stack<ProductoPila> pilaInventario = new Stack<ProductoPila>();
        private const int MaxPilaSize = 10;

        public PilasControl()
        {
            InitializeComponent();
        }

        private void ActualizarLista()
        {
            dgvInventario.Rows.Clear();

            foreach (var producto in pilaInventario)
            {
                dgvInventario.Rows.Add(
                    producto.ID,
                    producto.Descripcion,
                    producto.Categoria,
                    producto.Stock,
                    producto.PrecioVenta,
                    producto.FechaProduccion.ToShortDateString(),
                    producto.FechaVencimiento.ToShortDateString()
                );
            }
        }

        private void LimpiarCampos()
        {
            txtDescripcion.Clear();
            txtCategoria.Clear();
            txtStockInicial.Text = "0";
            txtPrecioVenta.Text = "0";
            dtpFechaProduccion.Value = DateTime.Now;
            dtpFechaVencimiento.Value = DateTime.Now;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text) ||
                string.IsNullOrWhiteSpace(txtCategoria.Text) ||
                string.IsNullOrWhiteSpace(txtStockInicial.Text) ||
                string.IsNullOrWhiteSpace(txtPrecioVenta.Text) ||
                !int.TryParse(txtStockInicial.Text, out int stock) || stock <= 0 ||
                !decimal.TryParse(txtPrecioVenta.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("Por favor, complete todos los campos correctamente antes de agregar.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return; 

            if (pilaInventario.Count >= MaxPilaSize)
            {
                MessageBox.Show("La pila de inventario ha alcanzado su capacidad máxima.", "Límite de Pila", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ProductoPila nuevoProducto = new ProductoPila
            {
                ID = pilaInventario.Count + 1,
                Descripcion = txtDescripcion.Text,
                Categoria = txtCategoria.Text,
                Stock = Convert.ToInt32(txtStockInicial.Text),
                PrecioVenta = Convert.ToDecimal(txtPrecioVenta.Text),
                FechaProduccion = dtpFechaProduccion.Value,
                FechaVencimiento = dtpFechaVencimiento.Value
            };

            pilaInventario.Push(nuevoProducto);
            ActualizarLista();
            LimpiarCampos();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (pilaInventario.Count > 0)
            {
                pilaInventario.Pop(); 
                ActualizarLista(); 
            }
            else
            {
                MessageBox.Show("La pila de inventario está vacía.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    public class ProductoPila
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public int Stock { get; set; }
        public decimal PrecioVenta { get; set; }
        public DateTime FechaProduccion { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }
}
