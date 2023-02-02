using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using AaronEstrada.Helper;
using System.Data;
using AaronEstrada.Model;

#region PROCEDIMIENTOS
/*
    CREATE PROCEDURE SP_CLIENTE
    (@EMPRESA NVARCHAR(50))
    AS
	    SELECT * FROM CLIENTES WHERE EMPRESA = @EMPRESA
    GO 

    

    CREATE PROCEDURE SP_PEDIDO_CLIENTE
    (@CODCLIENTE NVARCHAR(10))
    AS
	    SELECT CodigoPedido FROM PEDIDOS WHERE CodigoCliente = @CODCLIENTE
    GO

    ALTER PROCEDURE SP_PEDIDO_CLIENTE
    (@CODPEDIDO NVARCHAR(50))
    AS
	    SELECT CodigoPedido, FechaEntrega, FormaEnvio, Importe
	    FROM PEDIDOS 
	    WHERE CodigoPedido = @CODPEDIDO
    GO



    CREATE PROCEDURE SP_ELIMINAR_PEDIDO
    (@CODPEDIDO NVARCHAR(50))
    AS
	    DELETE FROM PEDIDOS WHERE CodigoPedido = @CODPEDIDO
    GO

 
 
 
 */
#endregion

namespace AaronEstrada.Repository
{
    public class RepositoryClientePedido
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;
        public RepositoryClientePedido()
        {
            string connectionString =
                HelperConfiguration.GetConnectionString();
                //@"Data Source=LOCALHOST\\DESARROLLO;Initial Catalog=PRACTICAADO;User ID=SA;Password=MCSD2023";

            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<string> LoadClientes()
        {
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = "SELECT EMPRESA FROM CLIENTES";
            this.cn.Open();
            List<string> empresas = new List<string>();
            this.reader = this.com.ExecuteReader();
            while (this.reader.Read())
            {
                string empresa = this.reader["Empresa"].ToString();
                empresas.Add(empresa);
            }
            this.reader.Close();
            this.cn.Close();
            return empresas;
        }

        public Cliente GetCliente(string empresa)
        {
            SqlParameter pamempresa = new SqlParameter("@EMPRESA", empresa);
            this.com.Parameters.Add(pamempresa);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTE";
            this.cn.Open();
            Cliente cliente = new Cliente();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();
            cliente.CodigoCliente = this.reader["CodigoCliente"].ToString();
            cliente.Empresa = this.reader["Empresa"].ToString();
            cliente.Contacto = this.reader["Contacto"].ToString();
            cliente.Cargo = this.reader["Cargo"].ToString();
            cliente.Ciudad = this.reader["Ciudad"].ToString();
            cliente.Telefono = int.Parse(this.reader["Telefono"].ToString());
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return cliente;
        }

        public List<string> GetAllPedidos(string codigoCliente)
        {
            SqlParameter pamcodcliente = new SqlParameter("@CODCLIENTE", codigoCliente);
            this.com.Parameters.Add(pamcodcliente);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = "SELECT CodigoPedido FROM PEDIDOS WHERE CodigoCliente = @CODCLIENTE";
            this.cn.Open();
            List<string> pedidos = new List<string>();
            this.reader = this.com.ExecuteReader();
            while (this.reader.Read())
            {
                string codPedido = this.reader["CodigoPedido"].ToString();
                pedidos.Add(codPedido);  
            }
            this.cn.Close();
            this.reader.Close();
            this.com.Parameters.Clear();
            return pedidos;
        }

        public Pedido GetPedido(string codigoPedido)
        {
            SqlParameter pamcodpedido = new SqlParameter("@CODPEDIDO", codigoPedido);
            this.com.Parameters.Add(pamcodpedido);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_PEDIDO_CLIENTE";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();
            string codPedido = this.reader["CodigoPedido"].ToString();
            string fecha = this.reader["FechaEntrega"].ToString();
            string envio = this.reader["FormaEnvio"].ToString();
            int importe = int.Parse(this.reader["Importe"].ToString());
            Pedido pedido = new Pedido();
            pedido.CodigoPedido = codPedido;
            pedido.FechaEntrega = fecha;
            pedido.FormaEnvio = envio;
            pedido.Importe = importe;
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return pedido;
        }

        public int EliminarPedido(string codigoPedido)
        {
            SqlParameter pamcodpedido = new SqlParameter("@CODPEDIDO", codigoPedido);
            this.com.Parameters.Add(pamcodpedido);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_ELIMINAR_PEDIDO";
            this.cn.Open();
            int response = this.com.ExecuteNonQuery();
            this.cn.Close();
            return response;
        }


    }
}
