using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations.Schema;

namespace TESTE.Models
{
    [Table("conteudo_arquivo")]
    public class conteudo_arquivo
    {
        public int id_conteudo_arquivo { get; set; }
        public string linha { get; set; }
        public string nm_arquivo { get; set; }
        public DateTime dt_cadastro { get; set; }
    }
}
