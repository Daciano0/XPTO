using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Conexao;
using System.Data.SqlClient;

namespace EmpresaXPTO
{
	public class LeitorTxt
	{
		
		public List<Cliente> clientesAtivos = new List<Cliente>();
		
		public void exbirClienteAtivo()
		{
			
			ConexaoBD conexaoBD = new ConexaoBD();

			using (SqlCommand command = new SqlCommand())
			{
				command.Connection = conexaoBD.conector();
				command.CommandText = $"select * from cliente where Verificador = {1} ";
				SqlDataReader count = command.ExecuteReader();

				while (count.Read())
				{

					Cliente cliente = new Cliente();

					cliente.Id = long.Parse(count["Id"].ToString());
					cliente.Nome = count["Nome"].ToString();
					cliente.Sobrenome = count["Sobrenome"].ToString();
					cliente.Nascimento = DateTime.Parse(count["Nascimento"].ToString());
					cliente.Sexo = count["Sexo"].ToString();
					cliente.Email = count["Email"].ToString();
					cliente.Verificador = Boolean.Parse(count["Verificador"].ToString());

					clientesAtivos.Add(cliente);
				}

				
				int flag = 0;

				foreach(Cliente cliente in clientesAtivos) {

					conexaoBD.desconectar();
					command.Connection = conexaoBD.conector();

					command.CommandText = $"select * from produto where Id_cliente = {cliente.Id}";
				   SqlDataReader countProduto = command.ExecuteReader();

					while (countProduto.Read())
					{
						Produto produto = new Produto();
						Cliente clienteProduto = new Cliente();

						clienteProduto.Id = long.Parse(countProduto["Id_cliente"].ToString());

						produto.Cliente = clienteProduto;
						produto.Descricao = countProduto["Descricao"].ToString();
						produto.Id = long.Parse(countProduto["Id"].ToString());

						clientesAtivos[flag].produtos.Add(produto);

						
					}
					flag++;
				}
				

			}

		}

		
		public void salvarClienteBD()
		{
			

			 List<Cliente> clientes = fileToSalvarClienteBD();

			ConexaoBD conexaoBD = new ConexaoBD();

			int flag = 0;

			using (SqlCommand command = new SqlCommand())
			{
				command.Connection = conexaoBD.conector();

				foreach (Cliente cliente in clientes)
				{

					command.CommandText = $"select * from cliente where Id = @id{flag}";
					command.Parameters.AddWithValue($"@id{flag}", cliente.Id);

					SqlDataReader count = command.ExecuteReader();

				
					flag++;
					

					if (count.Read()) {

						Console.WriteLine("Retornou Cliente");

						return;
					}
					conexaoBD.desconectar();


					Console.WriteLine("Linhas "+count);

						command.Connection = conexaoBD.conector();
						command.CommandText = $"insert into cliente (Id,Nome,Sobrenome,Nascimento,Sexo,Email,Verificador)values(@id{flag},@nome{flag},@sobrenome{flag},@nascimento{flag},@sexo{flag},@email{flag},@verificador{flag})";
						command.Parameters.AddWithValue("@id" + flag, cliente.Id);
						command.Parameters.AddWithValue("@nome" + flag, cliente.Nome);
						command.Parameters.AddWithValue("@sobrenome" + flag, cliente.Sobrenome);
						command.Parameters.AddWithValue("@nascimento" + flag, cliente.Nascimento);
						command.Parameters.AddWithValue("@sexo" + flag, cliente.Sexo);
						command.Parameters.AddWithValue("@email" + flag, cliente.Email);
						command.Parameters.AddWithValue("@verificador" + flag, cliente.Verificador);

						command.ExecuteNonQuery();

						flag++;
					
				}
			}

			conexaoBD.desconectar();

		}

		private  List<Cliente> fileToSalvarClienteBD()
		{
			List<Cliente> clientes = new List<Cliente>();

			String lines = File.ReadAllText("C:/Users/Daciano/Desktop/ARQUIVO1-PLENO.txt");

		
			foreach (string linha in lines.Split(';'))
			{
				string[] clienteRecebe = linha.Split(',');

				Cliente cliente = new Cliente();

				cliente.Id = long.Parse(clienteRecebe[0]);
				cliente.Nome = clienteRecebe[1];
				cliente.Sobrenome = clienteRecebe[2];
				cliente.Nascimento = DateTime.Parse(clienteRecebe[3]);
				cliente.Sexo = clienteRecebe[4];
				cliente.Email = clienteRecebe[5];
				cliente.Verificador = Boolean.Parse(clienteRecebe[6]);

				clientes.Add(cliente);
			}

			return clientes;
		}

		public void SalvarProdutosBD()
		{

			List<Produto> produtos = fileToSalvarProdutosBD(fileToSalvarClienteBD());

			ConexaoBD conexaoBD = new ConexaoBD();

			int flag = 0;

			using (SqlCommand command = new SqlCommand())
			{
				command.Connection = conexaoBD.conector();

				foreach (Produto produto in produtos)
				{
					
					command.CommandText = $"select * from produto where Id = @id{flag}";
					command.Parameters.AddWithValue($"@id{flag}", produto.Id);

					SqlDataReader count = command.ExecuteReader();


					flag++;



					if (count.Read())
					{
						Console.WriteLine("Retornou Produto");

						return;
					}
					conexaoBD.desconectar();


					command.CommandText = $"insert into produto (Id,Descricao,Id_cliente)values(@id{flag},@descricao{flag},@id_cliente{flag})";
					command.Parameters.AddWithValue("@id" + flag, produto.Id);
					command.Parameters.AddWithValue("@descricao" + flag, produto.Descricao);
					command.Parameters.AddWithValue("@id_cliente" + flag, produto.Cliente.Id);
				

					command.ExecuteNonQuery();

					flag++;
					
				}
			}

			conexaoBD.desconectar();


		}

		private List<Produto> fileToSalvarProdutosBD(List<Cliente> clientes)
		{
			Console.WriteLine("-----------");

			List<Produto> produtos = new List<Produto>();

			String lines = File.ReadAllText("C:/Users/Daciano/Desktop/ARQUIVO2-PLENO.txt");

			foreach (string linha in lines.Split(';'))
			{
				string[] produtoRecebe = linha.Split(',');

				Produto produto = new Produto();

				Cliente cliente = new Cliente();

				foreach (Cliente c in clientes)
				{
					if (c.Id == long.Parse(produtoRecebe[1]))
					{
						cliente.Id = c.Id;
					}
				}		

				produto.Cliente = cliente;
				produto.Id = long.Parse(produtoRecebe[0]);
				produto.Descricao = produtoRecebe[2];

				produtos.Add(produto);
			}

			return produtos;
		}
			

	}
}
