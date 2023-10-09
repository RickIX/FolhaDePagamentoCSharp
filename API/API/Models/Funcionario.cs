namespace API.Models
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string CPF { get; set; }
        // Outras propriedades relacionadas ao funcionário podem ser adicionadas aqui

        // public List<FolhaPagamento>? FolhasPagamento { get; set; }
    }
}
