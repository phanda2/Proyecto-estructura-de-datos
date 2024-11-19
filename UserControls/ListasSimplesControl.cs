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
    public partial class ListasSimplesControl : UserControl
    {
        private NodoProducto inicio = null;
        private int nextID = 1;

        public ListasSimplesControl()
        {
            InitializeComponent();
        }

        private void ActualizarLista()
        {
            dgvInventario.Rows.Clear();
            NodoProducto actual = inicio;

            while (actual != null)
            {
                dgvInventario.Rows.Add(
                    actual.ID,
                    actual.Descripcion,
                    actual.Categoria,
                    actual.Stock,
                    actual.PrecioVenta,
                    actual.FechaProduccion.ToShortDateString(),
                    actual.FechaVencimiento.ToShortDateString()
                );
                actual = actual.Siguiente;
            }
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
                MessageBox.Show("Por favor, complete todos los campos correctamente y asegúrese de que los valores numéricos sean mayores a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnInsertarInicio_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            NodoProducto nuevoNodo = new NodoProducto
            {
                ID = nextID++,
                Descripcion = txtDescripcion.Text,
                Categoria = txtCategoria.Text,
                Stock = Convert.ToInt32(txtStockInicial.Text),
                PrecioVenta = Convert.ToDecimal(txtPrecioVenta.Text),
                FechaProduccion = dtpFechaProduccion.Value,
                FechaVencimiento = dtpFechaVencimiento.Value,
                Siguiente = inicio
            };

            inicio = nuevoNodo;
            ActualizarLista();
            LimpiarCampos();
        }

        private void btnInsertarFinal_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            NodoProducto nuevoNodo = new NodoProducto
            {
                ID = nextID++,
                Descripcion = txtDescripcion.Text,
                Categoria = txtCategoria.Text,
                Stock = Convert.ToInt32(txtStockInicial.Text),
                PrecioVenta = Convert.ToDecimal(txtPrecioVenta.Text),
                FechaProduccion = dtpFechaProduccion.Value,
                FechaVencimiento = dtpFechaVencimiento.Value,
                Siguiente = null
            };

            if (inicio == null)
            {
                inicio = nuevoNodo;
            }
            else
            {
                NodoProducto actual = inicio;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevoNodo;
            }
            ActualizarLista();
            LimpiarCampos();
        }

        private void btnInsertarporReferencia_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            int referenciaID;
            if (!int.TryParse(txtReferencia.Text, out referenciaID))
            {
                MessageBox.Show("Por favor, ingrese un ID de referencia válido.");
                return;
            }

            NodoProducto actual = inicio;
            while (actual != null && actual.ID != referenciaID)
            {
                actual = actual.Siguiente;
            }

            if (actual == null)
            {
                MessageBox.Show("No se encontró un nodo con el ID de referencia.");
                return;
            }

            NodoProducto nuevoNodo = new NodoProducto
            {
                ID = nextID++,
                Descripcion = txtDescripcion.Text,
                Categoria = txtCategoria.Text,
                Stock = Convert.ToInt32(txtStockInicial.Text),
                PrecioVenta = Convert.ToDecimal(txtPrecioVenta.Text),
                FechaProduccion = dtpFechaProduccion.Value,
                FechaVencimiento = dtpFechaVencimiento.Value,
                Siguiente = actual.Siguiente
            };

            actual.Siguiente = nuevoNodo;
            ActualizarLista();
            LimpiarCampos();
            txtReferencia.Clear();
        }


        private void btnEliminarInicio_Click(object sender, EventArgs e)
        {
            if (inicio == null)
            {
                MessageBox.Show("La lista está vacía.");
                return;
            }

            inicio = inicio.Siguiente;
            ActualizarLista();
        }

        private void btnEliminarFinal_Click(object sender, EventArgs e)
        {
            if (inicio == null)
            {
                MessageBox.Show("La lista está vacía.");
                return;
            }

            if (inicio.Siguiente == null)
            {
                inicio = null;
            }
            else
            {
                NodoProducto actual = inicio;
                while (actual.Siguiente.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = null;
            }
            ActualizarLista();
        }

        private void btnEliminarporReferencia_Click(object sender, EventArgs e)
        {
            int referenciaID;
            if (!int.TryParse(txtReferenciaEliminar.Text, out referenciaID))
            {
                MessageBox.Show("Por favor, ingrese un ID de referencia válido.");
                return;
            }

            if (inicio == null)
            {
                MessageBox.Show("La lista está vacía.");
                return;
            }

            if (inicio.ID == referenciaID)
            {
                inicio = inicio.Siguiente;
            }
            else
            {
                NodoProducto actual = inicio;
                while (actual.Siguiente != null && actual.Siguiente.ID != referenciaID)
                {
                    actual = actual.Siguiente;
                }

                if (actual.Siguiente == null)
                {
                    MessageBox.Show("No se encontró un nodo con el ID de referencia.");
                    return;
                }

                actual.Siguiente = actual.Siguiente.Siguiente;
            }
            ActualizarLista();
            txtReferenciaEliminar.Clear();
        }

        private void LimpiarCampos()
        {
            txtDescripcion.Clear();
            txtCategoria.Clear();
            txtStockInicial.Text = "0";
            txtPrecioVenta.Text = "0";
            dtpFechaProduccion.Value = DateTime.Now;
            dtpFechaVencimiento.Value = DateTime.Now;
            txtReferencia.Clear();
            txtReferenciaEliminar.Clear();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }

    public class NodoProducto
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public int Stock { get; set; }
        public decimal PrecioVenta { get; set; }
        public DateTime FechaProduccion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public NodoProducto Siguiente { get; set; } 
    }

}
