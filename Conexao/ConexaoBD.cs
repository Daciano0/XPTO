using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexao
{
    public class ConexaoBD
    {

		 SqlConnection SqlConnection = new SqlConnection();

		public ConexaoBD()
		{
			SqlConnection = new SqlConnection("Server=Localhost;Database=XPTO;User Id=sa;Password=123456;");

		}

		public SqlConnection conector()
		{
			if (SqlConnection.State == System.Data.ConnectionState.Closed)
			{
				SqlConnection.Open();
			}
			return SqlConnection;
		}

		public void desconectar()
		{

			if (SqlConnection.State == System.Data.ConnectionState.Open)
			{
				SqlConnection.Close();
			}

		
		}

    }
}
