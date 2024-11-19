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
    public partial class ListasDoblesControl : UserControl
    {
        private NodoProductoDoble inicio = null;
        private NodoProductoDoble actual = null;
        private int nextID = 1;

        public ListasDoblesControl()
        {
            InitializeComponent();
            btnActualizar.Enabled = false;
        }

        private void ActualizarLista()
        {
            dgvInventario.Rows.Clear();
            NodoProductoDoble temp = inicio;

            while (temp != null)
            {
                dgvInventario.Rows.Add(
                    temp.ID,
                    temp.Descripcion,
                    temp.Categoria,
                    temp.Stock,
                    temp.PrecioVenta,
                    temp.FechaProduccion.ToShortDateString(),
                    temp.FechaVencimiento.ToShortDateString()
                );
                temp = temp.Siguiente;
            }

            ResaltarNodoActual();
        }

        private void ResaltarNodoActual()
        {
            foreach (DataGridViewRow row in dgvInventario.Rows)
            {
                if (row.Cells[0].Value != null && actual != null &&
                    (int)row.Cells[0].Value == actual.ID)
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
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
            txtReferencia.Clear();
            txtReferenciaEliminar.Clear();
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

            NodoProductoDoble nuevoNodo = new NodoProductoDoble
            {
                ID = nextID++,
                Descripcion = txtDescripcion.Text,
                Categoria = txtCategoria.Text,
                Stock = Convert.ToInt32(txtStockInicial.Text),
                PrecioVenta = Convert.ToDecimal(txtPrecioVenta.Text),
                FechaProduccion = dtpFechaProduccion.Value,
                FechaVencimiento = dtpFechaVencimiento.Value,
                Siguiente = inicio,
                Anterior = null
            };

            if (inicio != null)
            {
                inicio.Anterior = nuevoNodo;
            }
            inicio = nuevoNodo;
            actual = inicio;
            ActualizarLista();
            LimpiarCampos();
        }

        private void btnInsertarFinal_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            NodoProductoDoble nuevoNodo = new NodoProductoDoble
            {
                ID = nextID++,
                Descripcion = txtDescripcion.Text,
                Categoria = txtCategoria.Text,
                Stock = Convert.ToInt32(txtStockInicial.Text),
                PrecioVenta = Convert.ToDecimal(txtPrecioVenta.Text),
                FechaProduccion = dtpFechaProduccion.Value,
                FechaVencimiento = dtpFechaVencimiento.Value
            };

            if (inicio == null)
            {
                inicio = nuevoNodo;
            }
            else
            {
                NodoProductoDoble temp = inicio;
                while (temp.Siguiente != null)
                {
                    temp = temp.Siguiente;
                }
                temp.Siguiente = nuevoNodo;
                nuevoNodo.Anterior = temp;
            }
            actual = nuevoNodo;
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

            NodoProductoDoble temp = inicio;
            while (temp != null && temp.ID != referenciaID)
            {
                temp = temp.Siguiente;
            }

            if (temp == null)
            {
                MessageBox.Show("No se encontró un nodo con el ID de referencia.");
                return;
            }

            NodoProductoDoble nuevoNodo = new NodoProductoDoble
            {
                ID = nextID++,
                Descripcion = txtDescripcion.Text,
                Categoria = txtCategoria.Text,
                Stock = Convert.ToInt32(txtStockInicial.Text),
                PrecioVenta = Convert.ToDecimal(txtPrecioVenta.Text),
                FechaProduccion = dtpFechaProduccion.Value,
                FechaVencimiento = dtpFechaVencimiento.Value,
                Siguiente = temp.Siguiente,
                Anterior = temp
            };

            if (temp.Siguiente != null)
            {
                temp.Siguiente.Anterior = nuevoNodo;
            }
            temp.Siguiente = nuevoNodo;
            ActualizarLista();
            LimpiarCampos();
        }


        private void btnEliminarInicio_Click(object sender, EventArgs e)
        {
            if (inicio == null)
            {
                MessageBox.Show("La lista está vacía.");
                return;
            }

            inicio = inicio.Siguiente;
            if (inicio != null)
            {
                inicio.Anterior = null;
            }
            actual = inicio;
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
                NodoProductoDoble temp = inicio;
                while (temp.Siguiente != null)
                {
                    temp = temp.Siguiente;
                }
                temp.Anterior.Siguiente = null;
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

            NodoProductoDoble temp = inicio;
            while (temp != null && temp.ID != referenciaID)
            {
                temp = temp.Siguiente;
            }

            if (temp == null)
            {
                MessageBox.Show("No se encontró un nodo con el ID de referencia.");
                return;
            }

            if (temp.Anterior != null)
            {
                temp.Anterior.Siguiente = temp.Siguiente;
            }
            else
            {
                inicio = temp.Siguiente;
            }

            if (temp.Siguiente != null)
            {
                temp.Siguiente.Anterior = temp.Anterior;
            }
            ActualizarLista();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (actual?.Anterior != null)
            {
                actual = actual.Anterior;
                MostrarNodoActual();
                ResaltarNodoActual();
                btnActualizar.Enabled = true;

            }
            else
            {
                MessageBox.Show("No hay nodos anteriores.");
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (actual?.Siguiente != null)
            {
                actual = actual.Siguiente;
                MostrarNodoActual();
                ResaltarNodoActual();
                btnActualizar.Enabled = true;
            }
            else
            {
                MessageBox.Show("No hay nodos siguientes.");
            }
        }

        private void MostrarNodoActual()
        {
            if (actual != null)
            {
                txtDescripcion.Text = actual.Descripcion;
                txtCategoria.Text = actual.Categoria;
                txtStockInicial.Text = actual.Stock.ToString();
                txtPrecioVenta.Text = actual.PrecioVenta.ToString();
                dtpFechaProduccion.Value = actual.FechaProduccion;
                dtpFechaVencimiento.Value = actual.FechaVencimiento;
            }
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            LimpiarCampos();
            btnActualizar.Enabled = false;

        }

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            if (actual == null)
            {
                MessageBox.Show("No hay un nodo seleccionado para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text) ||
                string.IsNullOrWhiteSpace(txtCategoria.Text) ||
                Convert.ToDecimal(txtPrecioVenta.Text) <= 0 ||
                Convert.ToInt32(txtStockInicial.Text) <= 0)
            {
                MessageBox.Show("Por favor, complete todos los campos antes de actualizar y asegúrese de que el precio y el stock sean mayores a 0.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            actual.Descripcion = txtDescripcion.Text;
            actual.Categoria = txtCategoria.Text;
            actual.Stock = Convert.ToInt32(txtStockInicial.Text);
            actual.PrecioVenta = Convert.ToDecimal(txtPrecioVenta.Text);
            actual.FechaProduccion = dtpFechaProduccion.Value;
            actual.FechaVencimiento = dtpFechaVencimiento.Value;

            ActualizarLista(); 
            MessageBox.Show("Nodo actualizado correctamente.", "Actualizar Nodo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class NodoProductoDoble
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public int Stock { get; set; }
        public decimal PrecioVenta { get; set; }
        public DateTime FechaProduccion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public NodoProductoDoble Siguiente { get; set; }
        public NodoProductoDoble Anterior { get; set; }
    }
}