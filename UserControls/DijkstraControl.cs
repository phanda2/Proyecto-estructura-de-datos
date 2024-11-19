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
    public partial class DijkstraControl : UserControl
    {
        private Dictionary<string, List<Tuple<string, int>>> grafo; 

        public DijkstraControl()
        {
            InitializeComponent();
            grafo = new Dictionary<string, List<Tuple<string, int>>>();
            dgvInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventario.MultiSelect = false;
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

            if (!cmbProductoOrigen.Items.Contains(descripcion))
            {
                cmbProductoOrigen.Items.Add(descripcion);
            }

            if (!grafo.ContainsKey(descripcion))
            {
                grafo[descripcion] = new List<Tuple<string, int>>();
            }

            foreach (var prod in grafo.Keys)
            {
                if (prod != descripcion)
                {
                    int peso = Math.Abs(stock - ObtenerStockProducto(prod));
                    grafo[descripcion].Add(new Tuple<string, int>(prod, peso));
                    grafo[prod].Add(new Tuple<string, int>(descripcion, peso));
                }
            }

            dgvInventario.Rows.Add(dgvInventario.Rows.Count + 1, descripcion, categoria, stock, precio, fechaProduccion, fechaVencimiento);
            LimpiarCampos();
        }

        private void btnEjecutarDijkstra_Click(object sender, EventArgs e)
        {
            if (cmbProductoOrigen.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un producto de origen.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string origen = cmbProductoOrigen.SelectedItem.ToString();
            var stockOrigen = ObtenerStockProducto(origen);

            if (stockOrigen == 0)
            {
                MessageBox.Show("El producto seleccionado no tiene un stock válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var distancias = Dijkstra(origen);
            MostrarStocksOrdenados(distancias, origen, stockOrigen);
        }

        private Dictionary<string, int> Dijkstra(string origen)
        {
            var distancias = new Dictionary<string, int>();
            var visitados = new HashSet<string>();
            var colaPrioridad = new SortedSet<Tuple<int, string>>(Comparer<Tuple<int, string>>.Create((a, b) =>
            {
                int result = a.Item1.CompareTo(b.Item1);
                return result == 0 ? a.Item2.CompareTo(b.Item2) : result;
            }));

            foreach (var producto in grafo.Keys)
            {
                distancias[producto] = int.MaxValue;
            }
            distancias[origen] = 0;
            colaPrioridad.Add(new Tuple<int, string>(0, origen));

            while (colaPrioridad.Count > 0)
            {
                var nodoActual = colaPrioridad.Min;
                colaPrioridad.Remove(nodoActual);
                int distanciaActual = nodoActual.Item1;
                string productoActual = nodoActual.Item2;

                if (visitados.Contains(productoActual)) continue;
                visitados.Add(productoActual);

                foreach (var vecino in grafo[productoActual])
                {
                    string productoVecino = vecino.Item1;
                    int peso = vecino.Item2;
                    int nuevaDistancia = distanciaActual + peso;

                    if (nuevaDistancia < distancias[productoVecino])
                    {
                        colaPrioridad.Remove(new Tuple<int, string>(distancias[productoVecino], productoVecino));
                        distancias[productoVecino] = nuevaDistancia;
                        colaPrioridad.Add(new Tuple<int, string>(nuevaDistancia, productoVecino));
                    }
                }
            }

            return distancias;
        }

        private void MostrarStocksOrdenados(Dictionary<string, int> distancias, string origen, int stockOrigen)
        {
            lstResultados.Items.Clear();
            lstResultados.Items.Add("Productos ordenados por proximidad en stock:");

            var productosOrdenados = distancias
                .Where(d => d.Key != origen) // Excluir el producto de origen
                .Select(d => new
                {
                    Producto = d.Key,
                    Stock = ObtenerStockProducto(d.Key),
                    Distancia = Math.Abs(stockOrigen - ObtenerStockProducto(d.Key)) // Calcular la diferencia en stock
                })
                .OrderBy(p => p.Distancia) // Ordenar por la proximidad al stock del producto seleccionado
                .ThenBy(p => p.Stock) // En caso de empate, ordenar por stock
                .ToList();

            foreach (var item in productosOrdenados)
            {
                lstResultados.Items.Add($"Producto {item.Producto}: Stock {item.Stock}");
            }
        }


        private int ObtenerStockProducto(string descripcion)
        {
            foreach (DataGridViewRow row in dgvInventario.Rows)
            {
                if (row.Cells["Descripcion"].Value?.ToString() == descripcion)
                {
                    return Convert.ToInt32(row.Cells["Stock"].Value);
                }
            }
            return 0;
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

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (dgvInventario.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string descripcion = dgvInventario.SelectedRows[0].Cells["Descripcion"].Value.ToString();
            dgvInventario.Rows.RemoveAt(dgvInventario.SelectedRows[0].Index);

            if (grafo.ContainsKey(descripcion))
            {
                grafo.Remove(descripcion);
                foreach (var producto in grafo.Keys)
                {
                    grafo[producto].RemoveAll(t => t.Item1 == descripcion);
                }
            }

            if (!dgvInventario.Rows.Cast<DataGridViewRow>().Any(row => row.Cells["Descripcion"].Value?.ToString() == descripcion))
            {
                cmbProductoOrigen.Items.Remove(descripcion);
            }
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
           LimpiarCampos();       
        }
    }
}