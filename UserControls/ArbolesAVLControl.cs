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
    public partial class ArbolesAVLControl : UserControl
    {
        private NodoProducto raiz;

        public ArbolesAVLControl()
        {
            InitializeComponent();
            dgvInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventario.MultiSelect = false;
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
            public int Altura { get; set; }

            public NodoProducto(int id, string descripcion, string categoria, DateTime fechaProduccion, DateTime fechaVencimiento, int stock, decimal precio)
            {
                Id = id;
                Descripcion = descripcion;
                Categoria = categoria;
                FechaProduccion = fechaProduccion;
                FechaVencimiento = fechaVencimiento;
                Stock = stock;
                Precio = precio;
                Altura = 1;
            }
        }

        private int ObtenerAltura(NodoProducto nodo)
        {
            return nodo == null ? 0 : nodo.Altura;
        }

        private NodoProducto Insertar(NodoProducto nodo, int id, string descripcion, string categoria, DateTime fechaProduccion, DateTime fechaVencimiento, int stock, decimal precio)
        {
            if (nodo == null)
                return new NodoProducto(id, descripcion, categoria, fechaProduccion, fechaVencimiento, stock, precio);

            if (stock < nodo.Stock)
                nodo.Izquierdo = Insertar(nodo.Izquierdo, id, descripcion, categoria, fechaProduccion, fechaVencimiento, stock, precio);
            else if (stock > nodo.Stock)
                nodo.Derecho = Insertar(nodo.Derecho, id, descripcion, categoria, fechaProduccion, fechaVencimiento, stock, precio);
            else
                return nodo; 

            nodo.Altura = 1 + Math.Max(ObtenerAltura(nodo.Izquierdo), ObtenerAltura(nodo.Derecho));

            return Balancear(nodo);
        }

        private NodoProducto Balancear(NodoProducto nodo)
        {
            int balance = ObtenerAltura(nodo.Izquierdo) - ObtenerAltura(nodo.Derecho);

            if (balance > 1)
            {
                if (ObtenerAltura(nodo.Izquierdo.Izquierdo) >= ObtenerAltura(nodo.Izquierdo.Derecho))
                    return RotarDerecha(nodo);
                else
                {
                    nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo);
                    return RotarDerecha(nodo);
                }
            }
            if (balance < -1)
            {
                if (ObtenerAltura(nodo.Derecho.Derecho) >= ObtenerAltura(nodo.Derecho.Izquierdo))
                    return RotarIzquierda(nodo);
                else
                {
                    nodo.Derecho = RotarDerecha(nodo.Derecho);
                    return RotarIzquierda(nodo);
                }
            }
            return nodo;
        }

        private NodoProducto RotarDerecha(NodoProducto y)
        {
            NodoProducto x = y.Izquierdo;
            NodoProducto T2 = x.Derecho;

            x.Derecho = y;
            y.Izquierdo = T2;

            y.Altura = Math.Max(ObtenerAltura(y.Izquierdo), ObtenerAltura(y.Derecho)) + 1;
            x.Altura = Math.Max(ObtenerAltura(x.Izquierdo), ObtenerAltura(x.Derecho)) + 1;

            return x;
        }

        private NodoProducto RotarIzquierda(NodoProducto x)
        {
            NodoProducto y = x.Derecho;
            NodoProducto T2 = y.Izquierdo;

            y.Izquierdo = x;
            x.Derecho = T2;

            x.Altura = Math.Max(ObtenerAltura(x.Izquierdo), ObtenerAltura(x.Derecho)) + 1;
            y.Altura = Math.Max(ObtenerAltura(y.Izquierdo), ObtenerAltura(y.Derecho)) + 1;

            return y;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text) || string.IsNullOrWhiteSpace(txtCategoria.Text) ||
                txtStockInicial.Value <= 0 || txtPrecioVenta.Value <= 0)
            {
                MessageBox.Show("Por favor, complete todos los campos correctamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = dgvInventario.Rows.Count + 1;
            string descripcion = txtDescripcion.Text;
            string categoria = txtCategoria.Text;
            DateTime fechaProduccion = dtpFechaProduccion.Value;
            DateTime fechaVencimiento = dtpFechaVencimiento.Value;
            int stock = (int)txtStockInicial.Value;
            decimal precio = txtPrecioVenta.Value;

            raiz = Insertar(raiz, id, descripcion, categoria, fechaProduccion, fechaVencimiento, stock, precio);
            MostrarProductos();
            LimpiarCampos();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtDescripcion.Clear();
            txtCategoria.Clear();
            txtStockInicial.Value = 0;
            txtPrecioVenta.Value = 0;
            dtpFechaProduccion.Value = DateTime.Now;
            dtpFechaVencimiento.Value = DateTime.Now;
        }

        private void MostrarProductos()
        {
            dgvInventario.Rows.Clear();
            MostrarProductosEnOrden(raiz);
        }

        private void MostrarProductosEnOrden(NodoProducto nodo)
        {
            if (nodo != null)
            {
                MostrarProductosEnOrden(nodo.Izquierdo);
                dgvInventario.Rows.Add(nodo.Id, nodo.Descripcion, nodo.Categoria, nodo.Stock, nodo.Precio, nodo.FechaProduccion, nodo.FechaVencimiento);
                MostrarProductosEnOrden(nodo.Derecho);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvInventario.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int stockSeleccionado = (int)dgvInventario.SelectedRows[0].Cells["Stock"].Value;
            raiz = Eliminar(raiz, stockSeleccionado);
            MostrarProductos();
        }

        private NodoProducto Eliminar(NodoProducto nodo, int stock)
        {
            if (nodo == null)
                return nodo;

            if (stock < nodo.Stock)
                nodo.Izquierdo = Eliminar(nodo.Izquierdo, stock);
            else if (stock > nodo.Stock)
                nodo.Derecho = Eliminar(nodo.Derecho, stock);
            else
            {
                if (nodo.Izquierdo == null || nodo.Derecho == null)
                {
                    nodo = nodo.Izquierdo ?? nodo.Derecho;
                }
                else
                {
                    NodoProducto temp = ObtenerMinimo(nodo.Derecho);
                    nodo.Stock = temp.Stock;
                    nodo.Descripcion = temp.Descripcion;
                    nodo.Categoria = temp.Categoria;
                    nodo.FechaProduccion = temp.FechaProduccion;
                    nodo.FechaVencimiento = temp.FechaVencimiento;
                    nodo.Precio = temp.Precio;
                    nodo.Derecho = Eliminar(nodo.Derecho, temp.Stock);
                }
            }

            if (nodo == null)
                return nodo;

            nodo.Altura = 1 + Math.Max(ObtenerAltura(nodo.Izquierdo), ObtenerAltura(nodo.Derecho));
            return Balancear(nodo);
        }

        private NodoProducto ObtenerMinimo(NodoProducto nodo)
        {
            while (nodo.Izquierdo != null)
            {
                nodo = nodo.Izquierdo;
            }
            return nodo;
        }
    }
}