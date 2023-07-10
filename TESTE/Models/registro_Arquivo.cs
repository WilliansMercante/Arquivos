using Dapper.Contrib.Extensions;

using MySqlConnector;

namespace TESTE.Models
{
    [Table("registro_arquivo")]
    public class registro_Arquivo
    {
        const string connectionString = "Server=localhost; port=3306; User ID=root; Password=123456; Database=arquivo";

        [Key]
        public int id_registro_arquivo { get; set; }
        public string nm_arquivo { get; set; }
        public DateTime dt_cadastro { get; set; }


        public void InserirRegistro(registro_Arquivo registro_Arquivo)
        {
            try
            {
                using (var db = new MySqlConnection(connectionString))
                {
                    db.Open();

                    db.Insert(registro_Arquivo);

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
