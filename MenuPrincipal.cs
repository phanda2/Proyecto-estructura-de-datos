using Proyecto_Estructura_de_datos.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Estructura_de_datos
{
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        private void MenuPrincipal_Load(object sender, EventArgs e)
        {
        }

      
        private void LoadUserControl(UserControl control)
        {
            panelPrincipal.Controls.Clear();
            control.Dock = DockStyle.Fill; 
            panelPrincipal.Controls.Add(control); 
        }

        // Eventos para cada opción del menú

        private void pilasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PilasControl pilasControl = new PilasControl();
            LoadUserControl(pilasControl);
        }

        private void colasSimplesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColasSimplesControl colasSimplesControl = new ColasSimplesControl();
            LoadUserControl(colasSimplesControl);
        }

        private void colasCircularesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColasCircularesControl colasCircularesControl = new ColasCircularesControl();
            LoadUserControl(colasCircularesControl);
        }

        private void listasSimplesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListasSimplesControl listasSimplesControl = new ListasSimplesControl();
            LoadUserControl(listasSimplesControl);
        }

        private void listasDoblesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListasDoblesControl listasDoblesControl = new ListasDoblesControl();
            LoadUserControl(listasDoblesControl);
        }

        private void arbolesBinariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArbolesBinariosControl arbolesBinariosControl = new ArbolesBinariosControl();
            LoadUserControl(arbolesBinariosControl);
        }

        private void arbolesAVLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArbolesAVLControl arbolesAVLControl = new ArbolesAVLControl();
            LoadUserControl(arbolesAVLControl);
        }

        private void dijkstraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DijkstraControl dijkstraControl = new DijkstraControl();
            LoadUserControl(dijkstraControl);
        }

     
        private void ordenamientoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrdenamientosControl OrdenamientosControl = new OrdenamientosControl();
            LoadUserControl(OrdenamientosControl);
        }

        private void algoritmoDeFLOYDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FloydControl FloydControl = new FloydControl();
            LoadUserControl(FloydControl);
        }

        private void algoritmoDeMARSHALLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarshallControl MarshallControl = new MarshallControl();
            LoadUserControl(MarshallControl);
        }
    }
}
