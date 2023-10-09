using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/folha")]
    public class FolhaPagamentoController : ControllerBase
    {
        private readonly AppDataContext _ctx;

        public FolhaPagamentoController(AppDataContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost]
        [Route("cadastrar")]
        public IActionResult Cadastrar([FromBody] FolhaPagamento folhaPagamento)
        {
            try
            {
                // Verifique se o funcionário com o ID fornecido existe
                Funcionario funcionario = _ctx.Funcionarios.FirstOrDefault(f => f.Id == folhaPagamento.FuncionarioId);
                if (funcionario == null)
                {
                    return NotFound("Funcionário não encontrado.");
                }

                // Realize os cálculos necessários para a folha de pagamento
                CalcularFolhaPagamento(folhaPagamento, funcionario);

                // Adicione a folha de pagamento ao contexto e salve as alterações
                _ctx.FolhasPagamento.Add(folhaPagamento);
                _ctx.SaveChanges();

                return Created("", folhaPagamento);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
[Route("listar")]
public IActionResult Listar()
{
    try
    {
        List<FolhaPagamento> folhasPagamento =
            _ctx.FolhasPagamento
            .Include(fp => fp.Funcionario) // Inclua as informações do funcionário
            .ToList();

        return folhasPagamento.Count == 0 ? NotFound() : Ok(folhasPagamento);
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}


        [HttpGet]
[Route("buscar/{cpf}/{mes}/{ano}")]
public IActionResult Buscar([FromRoute] string cpf, [FromRoute] int mes, [FromRoute] int ano)
{
    try
    {
        // Encontre a folha de pagamento com base no CPF do funcionário, mês e ano
        FolhaPagamento folhaPagamento =
            _ctx.FolhasPagamento
            .Include(fp => fp.Funcionario)
            .FirstOrDefault(fp => fp.Funcionario.CPF == cpf && 
                                  fp.MesAno.Month == mes && 
                                  fp.MesAno.Year == ano);

        if (folhaPagamento != null)
        {
            return Ok(folhaPagamento);
        }

        return NotFound();
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}



        // Função para calcular os valores da folha de pagamento
        private void CalcularFolhaPagamento(FolhaPagamento folhaPagamento, Funcionario funcionario)
        {
            // Realize os cálculos necessários com base nas regras fornecidas

            // Cálculo do Salário Bruto (número de horas trabalhadas * valor da hora)
            decimal salarioBruto = folhaPagamento.Quantidade * folhaPagamento.Valor;

            // Cálculo do Imposto de Renda
            decimal impostoRenda = CalcularImpostoRenda(salarioBruto);

            // Cálculo do INSS
            decimal inss = CalcularINSS(salarioBruto);

            // Cálculo do FGTS
            decimal fgts = CalcularFGTS(salarioBruto);

            // Cálculo do Salário Líquido
            decimal salarioLiquido = salarioBruto - impostoRenda - inss;

            // Defina os valores calculados nos campos apropriados da folha de pagamento
            folhaPagamento.SalarioBruto = salarioBruto;
            folhaPagamento.ImpostoRenda = impostoRenda;
            folhaPagamento.INSS = inss;
            folhaPagamento.FGTS = fgts;
            folhaPagamento.SalarioLiquido = salarioLiquido;
        }

        // Função para calcular o Imposto de Renda
        private decimal CalcularImpostoRenda(decimal salarioBruto)
        {
            if (salarioBruto <= 1903.98m)
            {
                return 0;
            }
            else if (salarioBruto <= 2826.65m)
            {
                return (salarioBruto * 0.075m) - 142.80m;
            }
            else if (salarioBruto <= 3751.05m)
            {
                return (salarioBruto * 0.15m) - 354.80m;
            }
            else if (salarioBruto <= 4664.68m)
            {
                return (salarioBruto * 0.225m) - 636.13m;
            }
            else
            {
                return (salarioBruto * 0.275m) - 869.36m;
            }
        }

        // Função para calcular o INSS
        private decimal CalcularINSS(decimal salarioBruto)
        {
            if (salarioBruto <= 1693.72m)
            {
                return salarioBruto * 0.08m;
            }
            else if (salarioBruto <= 2822.90m)
            {
                return salarioBruto * 0.09m;
            }
            else if (salarioBruto <= 5645.80m)
            {
                return salarioBruto * 0.11m;
            }
            else
            {
                return 621.03m; // Valor fixo para salários acima de 5645.80m
            }
        }

        // Função para calcular o FGTS
        private decimal CalcularFGTS(decimal salarioBruto)
        {
            return salarioBruto * 0.08m; // FGTS é 8% do salário bruto
        }
    }
}
