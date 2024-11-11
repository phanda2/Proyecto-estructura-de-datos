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
    public partial class OrdenamientosControl : UserControl
    {
        private List<ProductoOrdenamientos> productos = new List<ProductoOrdenamientos>();
        private int nextID = 1;

        public OrdenamientosControl()
        {
            InitializeComponent();
            CargarCriteriosOrdenacion();
            ActualizarTabla();
        }

        private void CargarCriteriosOrdenacion()
        {
            comboBoxCriterios.Items.Add("Precio (Mayor a Menor)");
            comboBoxCriterios.Items.Add("Precio (Menor a Mayor)");
            comboBoxCriterios.Items.Add("Stock (Mayor a Menor)");
            comboBoxCriterios.Items.Add("Stock (Menor a Mayor)");
            comboBoxCriterios.SelectedIndex = 0; 
        }

        private void ActualizarTabla()
        {
            dgvInventario.Rows.Clear();
            foreach (var producto in productos)
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

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text) || string.IsNullOrWhiteSpace(txtCategoria.Text) ||
                string.IsNullOrWhiteSpace(txtStockInicial.Text) || string.IsNullOrWhiteSpace(txtPrecioVenta.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos antes de guardar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ProductoOrdenamientos nuevoProducto = new ProductoOrdenamientos
            {
                ID = nextID++,
                Descripcion = txtDescripcion.Text,
                Categoria = txtCategoria.Text,
                Stock = int.Parse(txtStockInicial.Text),
                PrecioVenta = decimal.Parse(txtPrecioVenta.Text),
                FechaProduccion = dtpFechaProduccion.Value,
                FechaVencimiento = dtpFechaVencimiento.Value
            };

            productos.Add(nuevoProducto);
            ActualizarTabla();
            LimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvInventario.SelectedRows.Count > 0)
            {
                int idProducto = Convert.ToInt32(dgvInventario.SelectedRows[0].Cells[0].Value);

                // Eliminar el  de la lista de productos
                productos.RemoveAll(p => p.ID == idProducto);

                ActualizarTabla();
            }
            else
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            string criterio = comboBoxCriterios.SelectedItem?.ToString();
            if (criterio == null)
            {
                MessageBox.Show("Seleccione un criterio de ordenación.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rbtnBubbleSort.Checked)
            {
                BubbleSort(criterio);
            }
            else if (rbtnQuickSort.Checked)
            {
                QuickSort(criterio, 0, productos.Count - 1);
            }
            else if (rbtnHeapSort.Checked)
            {
                HeapSort(criterio);
            }
            else
            {
                MessageBox.Show("Seleccione un algoritmo de ordenación.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ActualizarTabla();
        }

        private void BubbleSort(string criterio)
        {
            for (int i = 0; i < productos.Count - 1; i++)
            {
                for (int j = 0; j < productos.Count - i - 1; j++)
                {
                    bool shouldSwap = false;
                    if (criterio == "Precio (Mayor a Menor)")
                    {
                        shouldSwap = productos[j].PrecioVenta < productos[j + 1].PrecioVenta;
                    }
                    else if (criterio == "Precio (Menor a Mayor)")
                    {
                        shouldSwap = productos[j].PrecioVenta > productos[j + 1].PrecioVenta;
                    }
                    else if (criterio == "Stock (Mayor a Menor)")
                    {
                        shouldSwap = productos[j].Stock < productos[j + 1].Stock;
                    }
                    else if (criterio == "Stock (Menor a Mayor)")
                    {
                        shouldSwap = productos[j].Stock > productos[j + 1].Stock;
                    }

                    if (shouldSwap)
                    {
                        var temp = productos[j];
                        productos[j] = productos[j + 1];
                        productos[j + 1] = temp;
                    }
                }
            }
        }

        private void QuickSort(string criterio, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(criterio, low, high);
                QuickSort(criterio, low, pi - 1);
                QuickSort(criterio, pi + 1, high);
            }
        }

        private int Partition(string criterio, int low, int high)
        {
            var pivot = productos[high];
            int i = (low - 1);
            for (int j = low; j <= high - 1; j++)
            {
                bool shouldSwap = false;
                if (criterio == "Precio (Mayor a Menor)")
                {
                    shouldSwap = productos[j].PrecioVenta > pivot.PrecioVenta;
                }
                else if (criterio == "Precio (Menor a Mayor)")
                {
                    shouldSwap = productos[j].PrecioVenta < pivot.PrecioVenta;
                }
                else if (criterio == "Stock (Mayor a Menor)")
                {
                    shouldSwap = productos[j].Stock > pivot.Stock;
                }
                else if (criterio == "Stock (Menor a Mayor)")
                {
                    shouldSwap = productos[j].Stock < pivot.Stock;
                }

                if (shouldSwap)
                {
                    i++;
                    var temp = productos[i];
                    productos[i] = productos[j];
                    productos[j] = temp;
                }
            }
            var swapTemp = productos[i + 1];
            productos[i + 1] = productos[high];
            productos[high] = swapTemp;
            return i + 1;
        }

        private void HeapSort(string criterio)
        {
            int n = productos.Count;
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(criterio, n, i);

            for (int i = n - 1; i > 0; i--)
            {
                var temp = productos[0];
                productos[0] = productos[i];
                productos[i] = temp;
                Heapify(criterio, i, 0);
            }
        }

        private void Heapify(string criterio, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && Comparar(productos[left], productos[largest], criterio))
                largest = left;

            if (right < n && Comparar(productos[right], productos[largest], criterio))
                largest = right;

            if (largest != i)
            {
                var swap = productos[i];
                productos[i] = productos[largest];
                productos[largest] = swap;
                Heapify(criterio, n, largest);
            }
        }

        private bool Comparar(ProductoOrdenamientos a, ProductoOrdenamientos b, string criterio)
        {
            if (criterio == "Precio (Mayor a Menor)")
                return a.PrecioVenta < b.PrecioVenta;
            else if (criterio == "Precio (Menor a Mayor)")
                return a.PrecioVenta > b.PrecioVenta;
            else if (criterio == "Stock (Mayor a Menor)")
                return a.Stock < b.Stock;
            else if (criterio == "Stock (Menor a Mayor)")
                return a.Stock > b.Stock;
            return false;
        }

        private void btnReiniciarOrden_Click(object sender, EventArgs e)
        {
            productos.Sort((x, y) => x.ID.CompareTo(y.ID)); 
            ActualizarTabla();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

    public class ProductoOrdenamientos
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
