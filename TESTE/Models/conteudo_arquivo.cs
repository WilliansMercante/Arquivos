using Dapper.Contrib.Extensions;

using MySqlConnector;

//using System.ComponentModel.DataAnnotations.Schema;

namespace TESTE.Models
{
    [Table("conteudo_arquivo")]
    public class conteudo_arquivo
    {

        const string connectionString = "Server=localhost; port=3306; User ID=root; Password=123456; Database=arquivo";


        [Key]
        public int id_conteudo_arquivo { get; set; }
        public string linha { get; set; }
        public string nm_arquivo { get; set; }
        public DateTime dt_cadastro { get; set; }






        public void InserirLinha(conteudo_arquivo conteudo_arquivo)
        {
            try
            {
                using (var db = new MySqlConnection(connectionString))
                {
                    db.Open();

                    db.Insert(conteudo_arquivo);

                    db.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
            }
        }

    }
}
