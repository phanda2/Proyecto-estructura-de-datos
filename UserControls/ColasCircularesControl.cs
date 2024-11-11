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
    public partial class ColasCircularesControl : UserControl
    {
        private ProductosColasCirculares[] colaInventario;
        private int front;
        private int rear;
        private int count;
        private int nextID = 1;
        private const int MaxColaSize = 5;

        public ColasCircularesControl()
        {
            InitializeComponent();
            colaInventario = new ProductosColasCirculares[MaxColaSize];
            front = 0;
            rear = -1;
            count = 0;
        }

        private void ActualizarLista()
        {
            dgvInventario.Rows.Clear();
            int current = front;
            for (int i = 0; i < count; i++)
            {
                var producto = colaInventario[current];
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
                current = (current + 1) % MaxColaSize;
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

            if (count >= MaxColaSize)
            {
                MessageBox.Show("La cola de inventario ha alcanzado su capacidad máxima.", "Límite de Cola", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rear = (rear + 1) % MaxColaSize;
            colaInventario[rear] = new ProductosColasCirculares
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
            front = (front + 1) % MaxColaSize;
            count--;
            ActualizarLista();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }

    public class ProductosColasCirculares
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
