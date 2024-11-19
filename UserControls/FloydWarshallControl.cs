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
    public partial class FloydWarshallControl : UserControl
    {
        private int[,] precios;
        private List<string> productos;

        public FloydWarshallControl()
        {
            InitializeComponent();
            productos = new List<string>();
            dgvInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventario.MultiSelect = false;
            cmbProductoOrigen.Items.Clear();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string descripcion = txtDescripcion.Text;
            string categoria = txtCategoria.Text;
            int stock = (int)numStockInicial.Value;
            decimal precio = numPrecioVenta.Value;
            DateTime fechaProduccion = dtpFechaProduccion.Value;
            DateTime fechaVencimiento = dtpFechaVencimiento.Value;

            if (string.IsNullOrWhiteSpace(descripcion) || stock <= 0 || precio <= 0)
            {
                MessageBox.Show("Por favor, complete todos los campos correctamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvInventario.Rows.Add(dgvInventario.Rows.Count + 1, descripcion, categoria, stock, precio, fechaProduccion, fechaVencimiento);
            productos.Add(descripcion);
            cmbProductoOrigen.Items.Add(descripcion);
            ActualizarMatrizPrecios();
            LimpiarCampos();
        }

        private void ActualizarMatrizPrecios()
        {
            int n = productos.Count;
            precios = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        precios[i, j] = 0; 
                    }
                    else
                    {
                        int precio1 = ObtenerPrecioProducto(productos[i]);
                        int precio2 = ObtenerPrecioProducto(productos[j]);
                        precios[i, j] = Math.Abs(precio1 - precio2); 
                    }
                }
            }

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (precios[i, k] != int.MaxValue && precios[k, j] != int.MaxValue
                            && precios[i, j] > precios[i, k] + precios[k, j])
                        {
                            precios[i, j] = precios[i, k] + precios[k, j];
                        }
                    }
                }
            }
        }

        private int ObtenerPrecioProducto(string descripcion)
        {
            foreach (DataGridViewRow row in dgvInventario.Rows)
            {
                if (row.Cells["Descripcion"].Value?.ToString() == descripcion)
                {
                    return Convert.ToInt32(row.Cells["Precio"].Value);
                }
            }
            return 0;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvInventario.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string descripcion = dgvInventario.SelectedRows[0].Cells["Descripcion"].Value.ToString();
            dgvInventario.Rows.RemoveAt(dgvInventario.SelectedRows[0].Index);
            productos.Remove(descripcion);
            cmbProductoOrigen.Items.Remove(descripcion);
            ActualizarMatrizPrecios();
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

        private void btnEjecutar_Click_1(object sender, EventArgs e)
        {
            if (cmbProductoOrigen.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un producto de origen.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string origen = cmbProductoOrigen.SelectedItem.ToString();
            int indiceOrigen = productos.IndexOf(origen);

            lstResultados.Items.Clear();
            lstResultados.Items.Add("Productos ordenados por proximidad de precio:");

            var productosOrdenados = productos
                .Select((p, i) => new { Producto = p, Precio = ObtenerPrecioProducto(p), Distancia = precios[indiceOrigen, i] })
                .Where(p => p.Producto != origen) 
                .OrderBy(p => p.Distancia) 
                .ToList();

            foreach (var item in productosOrdenados)
            {
                lstResultados.Items.Add($"Producto {item.Producto}: Precio {item.Precio}");
            }
        }
    }
}
