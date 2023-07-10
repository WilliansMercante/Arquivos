using Dapper.Contrib.Extensions;

using Microsoft.AspNetCore.Mvc;

using MySqlConnector;

using System.Text;

using TESTE.Models;

namespace TESTE.Controllers
{
    public class ArquivoController : Controller
    {
        const string connectionString = "Server=localhost; port=3306; User ID=root; Password=123456; Database=arquivo";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Gravar()
        {
            return View();
        }

        public IActionResult ListarGravadosFisicamente()
        {
            try
            {
                ListarArquivosViewModel ListarArquivosVM = new ListarArquivosViewModel();

                ListarArquivosVM.ArquivosFisicos = ListarArquivosFisicos();
                return View(ListarArquivosVM);
            }
            catch (Exception)
            {
                throw;
            }
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
                        conteudo_arquivo.InserirLinha(conteudo_arquivo);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
            }

            return View("Index");
        }

        [HttpPost]
        public void GravarArquivo(IFormFile file)
        {
            try
            {
                registro_Arquivo registro_Arquivo = new registro_Arquivo() { nm_arquivo = file.FileName, dt_cadastro = DateTime.Now };

                string caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\arquivos\\teste");

                using (FileStream fs = System.IO.File.Create(Path.Combine(caminho, file.FileName)))
                {
                    file.CopyTo(fs);
                }

                registro_Arquivo.InserirRegistro(registro_Arquivo);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult ListarArquivosTela()
        {
            try
            {
                ListarArquivosViewModel ListarArquivosVM = new ListarArquivosViewModel();

                ListarArquivosVM.Arquivos = ListarArquivos();
                return View(ListarArquivosVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<conteudo_arquivo> ListarArquivos()
        {
            IEnumerable<conteudo_arquivo> retorno = new List<conteudo_arquivo>();

            try
            {
                using (var db = new MySqlConnection(connectionString))
                {
                    db.Open();

                    retorno = db.GetAll<conteudo_arquivo>().DistinctBy(p => p.nm_arquivo);

                    db.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
            }

            return retorno;
        }

        public IEnumerable<registro_Arquivo> ListarArquivosFisicos()
        {
            IEnumerable<registro_Arquivo> retorno = new List<registro_Arquivo>();

            try
            {
                using (var db = new MySqlConnection(connectionString))
                {
                    db.Open();

                    retorno = db.GetAll<registro_Arquivo>().DistinctBy(p => p.id_registro_arquivo);

                    db.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
            }

            return retorno;
        }

        [HttpPost]
        public JsonResult Excluir(int id)
        {
            bool deletado = false;

            try
            {
                using (var db = new MySqlConnection(connectionString))
                {
                    db.Open();

                    var arquivo = db.Get<conteudo_arquivo>(id);
                    var todosRegistros = db.GetAll<conteudo_arquivo>().Where(p => p.nm_arquivo == arquivo.nm_arquivo);
                    deletado = db.Delete(todosRegistros);

                    db.Close();
                }

                if (deletado)
                {
                    return Json(new { flSucesso = true, arquivoEncontrado = true });
                }
                else
                {
                    return Json(new { flSucesso = true, arquivoEncontrado = false });
                }
            }
            catch (Exception e)
            {
                return Json(new { flSussesso = false, arquivoEncontrado = false, mensagem = e.Message });
            }
        }

        [HttpGet]
        public IActionResult Gerar(int id)
        {          
            try
            {
                using (var db = new MySqlConnection(connectionString))
                {
                    //db.Open();

                    var arquivo = db.Get<conteudo_arquivo>(id);
                    var todosRegistros = db.GetAll<conteudo_arquivo>().Where(p => p.nm_arquivo == arquivo.nm_arquivo);

                    //db.Close();

                    var arquivoString = new StringBuilder();

                    foreach (var item in todosRegistros)
                    {
                        arquivoString.AppendLine(item.linha);
                    }

                    byte[] bytes = Encoding.Unicode.GetBytes(arquivoString.ToString());

                    return File(bytes, "text/plain", arquivo.nm_arquivo);
                }
            }
            catch (Exception e)
            {
                return Ok("Gay");
            }
        }

        [HttpGet]
        public IActionResult Gerar2(int id)
        {
            try
            {
                using (var db = new MySqlConnection(connectionString))
                {
                    db.Open();

                    var arquivo = db.Get<conteudo_arquivo>(id);
                    var caminhoImagem = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\arquivos\\teste", arquivo.nm_arquivo);

                    var will = System.IO.File.Open(caminhoImagem, FileMode.Open);

                    return File(will, "text/plain", arquivo.nm_arquivo);
                }
            }
            catch (Exception e)
            {
                return Ok("Gay");
            }
        }


        //comandos


        //            var query = "Select * from conteudo_arquivo";
        //            var clientes = db.Query<conteudo_arquivo>(query);

        //var dados = db.GetAll<conteudo_arquivo>();

    }
}
