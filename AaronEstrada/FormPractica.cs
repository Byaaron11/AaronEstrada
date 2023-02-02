using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AaronEstrada.Repository;
using AaronEstrada.Model;


namespace AaronEstrada
{
    public partial class FormPractica : Form
    {
        private RepositoryClientePedido repo;
        public FormPractica()
        {
            InitializeComponent();
            this.repo = new RepositoryClientePedido();
            List<string> clientes = new List<string>();
            clientes = this.repo.LoadClientes();
            foreach (string customer in clientes)
            {
                this.cmbclientes.Items.Add(customer);
            }
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string empresa = this.cmbclientes.SelectedItem.ToString();
            Cliente cliente = new Cliente();
            cliente = this.repo.GetCliente(empresa);
            string codCliente = cliente.CodigoCliente.ToString();
            this.txtempresa.Text = cliente.Empresa.ToString();
            this.txtcontacto.Text = cliente.Contacto.ToString();
            this.txtcargo.Text = cliente.Cargo.ToString();
            this.txtciudad.Text = cliente.Ciudad.ToString();
            this.txttelefono.Text = cliente.Telefono.ToString();
            List<string> pedidos = new List<string>();
            pedidos = this.repo.GetAllPedidos(codCliente);
            this.lstpedidos.Items.Clear();
            foreach(string pedido in pedidos)
            {
                this.lstpedidos.Items.Add(pedido);
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pedidoSelected = this.lstpedidos.SelectedItem.ToString();
            Pedido pedido = new Pedido();
            pedido = this.repo.GetPedido(pedidoSelected);
            this.txtcodigopedido.Text = pedido.CodigoPedido.ToString();
            this.txtfechaentrega.Text = pedido.FechaEntrega.ToString();
            this.txtformaenvio.Text = pedido.FormaEnvio.ToString();
            this.txtimporte.Text = pedido.Importe.ToString();
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            string codPedido = this.txtcodigopedido.ToString();
            int eliminados = this.repo.EliminarPedido(codPedido);
            MessageBox.Show("Pedidos eliminados: " + eliminados);
        }

        
    }
}
