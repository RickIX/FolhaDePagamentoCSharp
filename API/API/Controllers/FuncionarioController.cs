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
    [Route("api/funcionario")]
    public class FuncionarioController : ControllerBase
    {
        private readonly AppDataContext _ctx;

        public FuncionarioController(AppDataContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            try
            {
                List<Funcionario> funcionarios =
                    _ctx.Funcionarios
                    .ToList();
                return funcionarios.Count == 0 ? NotFound() : Ok(funcionarios);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("cadastrar")]
        public IActionResult Cadastrar([FromBody] Funcionario funcionario)
        {
            try
            {
                _ctx.Funcionarios.Add(funcionario);
                _ctx.SaveChanges();
                return Created("", funcionario);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("buscar/{cpf}")]
        public IActionResult Buscar([FromRoute] string cpf)
        {
            try
            {
                Funcionario? funcionarioCadastrado =
                    _ctx.Funcionarios
                    .FirstOrDefault(x => x.CPF == cpf);
                if (funcionarioCadastrado != null)
                {
                    return Ok(funcionarioCadastrado);
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("deletar/{id}")]
        public IActionResult Deletar([FromRoute] int id)
        {
            try
            {
                Funcionario? funcionarioCadastrado = _ctx.Funcionarios.Find(id);
                if (funcionarioCadastrado != null)
                {
                    _ctx.Funcionarios.Remove(funcionarioCadastrado);
                    _ctx.SaveChanges();
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("alterar/{id}")]
        public IActionResult Alterar([FromRoute] int id,
            [FromBody] Funcionario funcionario)
        {
            try
            {
                Funcionario? funcionarioCadastrado =
                    _ctx.Funcionarios.FirstOrDefault(x => x.Id == id);

                if (funcionarioCadastrado != null)
                {
                    funcionarioCadastrado.Nome = funcionario.Nome;
                    funcionarioCadastrado.CPF = funcionario.CPF;
                    // Outras propriedades de funcionário podem ser atualizadas aqui
                    _ctx.Funcionarios.Update(funcionarioCadastrado);
                    _ctx.SaveChanges();
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
