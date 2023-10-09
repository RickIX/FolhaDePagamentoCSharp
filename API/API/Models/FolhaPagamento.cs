namespace API.Models
{
    public class FolhaPagamento
    {
        public FolhaPagamento() => MesAno = DateTime.Now;
        public int Id { get; set; }
        public DateTime MesAno { get; set; }
        public decimal SalarioBruto { get; set; }
        public decimal ImpostoRenda { get; set; }
        public decimal INSS { get; set; }
        public decimal FGTS { get; set; }
        public decimal SalarioLiquido { get; set; }

        // Adicione os campos Quantidade e Valor
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }

        public int FuncionarioId { get; set; }
        public Funcionario? Funcionario { get; set; }
    }
}

