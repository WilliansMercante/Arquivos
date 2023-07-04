using Dapper.Contrib.Extensions;

using Microsoft.AspNetCore.Mvc;

using MySqlConnector;

using TESTE.Models;

namespace TESTE.Controllers
{
    public class ArquivoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Gravar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LerArquivo(IFormFile file)
        {

            try
            {
                var nome = file.FileName.Trim();
                var leitura = file.OpenReadStream();
                using (var reader = new StreamReader(leitura))
                {
                    while (!reader.EndOfStream)
                    {
                        string linha = reader.ReadLine();

                        conteudo_arquivo conteudo_arquivo = new conteudo_arquivo() { linha = linha, nm_arquivo = nome, dt_cadastro = DateTime.Now };
                        InserirLinha(conteudo_arquivo);
                    }

                }

                Console.WriteLine("teste");
            }
            catch (Exception e)
            {

                Console.WriteLine("Erro: " + e.Message);
            }

            return View("Index");
        }

        public void InserirLinha(conteudo_arquivo conteudo_arquivo)
        {
            try
            {

                string connectionString = "Server=localhost; port=3306; User ID=root; Password=123456; Database=arquivo";



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

        [HttpPost]
        public void GravarArquivo(IFormFile file)
        {
            string caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\arquivos\\teste");

            using (FileStream fs = System.IO.File.Create(Path.Combine(caminho, file.FileName)))
            {
                file.CopyTo(fs);
            }
        }



        //comandos


        //            var query = "Select * from conteudo_arquivo";
        //            var clientes = db.Query<conteudo_arquivo>(query);

        //var dados = db.GetAll<conteudo_arquivo>();







    }
}
