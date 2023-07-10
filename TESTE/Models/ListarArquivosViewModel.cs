namespace TESTE.Models
{
    public class ListarArquivosViewModel
    {
        public IEnumerable<conteudo_arquivo> Arquivos { get; set; }
        public IEnumerable<registro_Arquivo> ArquivosFisicos { get; set; }
    }
}
