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
    public partial class ColasSimplesControl : UserControl
    {
        private ProductosColasSimples[] colaInventario;
        private int front;
        private int rear;
        private int count;
        private int nextID = 1;
        private const int MaxColaSize = 5;

        public ColasSimplesControl()
        {
            InitializeComponent();
            colaInventario = new ProductosColasSimples[MaxColaSize];
            front = 0;
            rear = -1;
            count = 0;
        }

        private void ActualizarLista()
        {
            dgvInventario.Rows.Clear();
            for (int i = front; i <= rear; i++)
            {
                var producto = colaInventario[i];
                if (producto != null)
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
            // Verifica que los campos de texto y los valores no estén vacíos o en cero
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
            if (!ValidarCampos()) return; // Si la validación falla, se detiene el proceso

            if (count >= MaxColaSize || rear == MaxColaSize - 1)
            {
                MessageBox.Show("La cola de inventario ha alcanzado su capacidad máxima.", "Límite de Cola", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rear++;
            colaInventario[rear] = new ProductosColasSimples
            {
                ID = nextID++,
                Descripcion = txtDescripcion.Text,
                Categoria = txtCategoria.Text,
                Stock = Convert.ToInt32(txtStockInicial.Text),
                PrecioVenta = Convert.ToDecimal(txtPrecioVenta.Text),
                FechaProduccion = dtpFechaProduccion.Value,
                FechaVencimiento = dtpFechaVencimiento.Value
            };
            count++;
            ActualizarLista();
            LimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (count == 0)
            {
                MessageBox.Show("La cola de inventario está vacía.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            colaInventario[front] = null;
            front++;
            count--;
            ActualizarLista();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }

    public class ProductosColasSimples
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