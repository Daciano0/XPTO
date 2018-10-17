using Conexao;
using EmpresaXPTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
	static class Program
	{
		
		static void Main()
		{

			LeitorTxt leitor = new LeitorTxt();


			leitor.salvarClienteBD();
			leitor.SalvarProdutosBD();
			leitor.exbirClienteAtivo();

			int count = 0;

			foreach (Cliente cliente in leitor.clientesAtivos)
			{
				Console.WriteLine("Cliente: "+cliente.Nome);
				if (cliente.produtos != null)
				{

					Console.WriteLine("Teste " + cliente.produtos.Count());
					while (count < cliente.produtos.Count())
					{
						Console.WriteLine("Nome: " + cliente.Nome);
						Console.WriteLine("Produtos: " + cliente.produtos[count].Descricao);
						count++;
					}
				}
				count = 0;
				
			}

		}

		

	}
}
