using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpresaXPTO
{
    public class Cliente
    {

		public long Id { get; set; }
		public string Nome { get; set; }
		public string Sobrenome { get; set; }
		public DateTime Nascimento { get; set; }
		public string Sexo { get; set; }
		public string Email { get; set; }
		public Boolean Verificador { get; set; }


		public List<Produto> produtos = new List<Produto>();
	}
}
