using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AlbertEinstein.Models;
using System;
using Consultas.Services;
using AlbertEinstein.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AlbertEinstein.Controllers
{
    //[ApiController]
    [Route("/Async/IoC/[controller]")]
    public class ConsultasController: ControllerBase
    {
        IConsultaItemService _consultaService;
        IOutrosItemService _outrosService;

        public ConsultasController(IConsultaItemService consultaService, IOutrosItemService outrosService)
        {
            _consultaService    = consultaService;
            _outrosService      = outrosService;
        }

        [Route("ListarTodasConsultas")]
        [HttpGet]
        public async Task<IActionResult> TodosAsync()
        {
            var consultas = await _consultaService.PeagaTodasConsultasAsync();

            if (consultas.Count() == 0) 
                return NotFound("Nenhuma consulta cadastrada.");
            else
                return Ok(consultas);
        }
 
        [Route("ListarTodasConsultasPorMedicoId")]
        [HttpGet]
        public async Task<IActionResult> ConsultaTodasPorMedicoId(int id)
        {
            var medico = await _consultaService.pesquisaMedicoPorIdAsync(id);

            if(medico.Count() == 0)
                return NotFound("Médico não encontrado, Id inexistente.");

             var consulta = await _consultaService.ConsultasPorMedicoIdAsync(id);

             if(consulta.Count() == 0)
                return NotFound($"Nenhuma consulta encontrata para o médico {medico.FirstOrDefault().Nome}.");

             return Ok(consulta);
        }

        [Route("InserirConsulta")]
        [HttpPost]
        public async Task<IActionResult> Insere([FromBody]Consulta consulta)
        {
            consulta.Id = 0;

            if(consulta.Data < DateTime.Now.Date)
                return BadRequest("Não pode-se cadastrar uma consulta em data inferior a hoje " + DateTime.Now.ToString("dd/MM/yyyy"));

            var pt = await _outrosService.PesquisaTudoAsync(consulta);

            if(pt == null)
                return Ok(await _consultaService.InserirConsultaAsync(consulta));
            else
                return BadRequest(pt);
        }

        [Route("EditarConsulta")]
        [HttpPut]
        public async Task<IActionResult> Edita([FromBody]Consulta consulta)
        {
            if(consulta.Id == 0)
                return NotFound("Declare o Id da consulta para poder modificá-la.");    

            var pt = await _outrosService.PesquisaTudoAsync(consulta);

            if(pt == null)
            {
                //Se tentar Editar com um Id inexistente ou errado, causa um erro.
                try
                {
                    var resultado = await _consultaService.EditarConsulta(consulta);
                    return Ok(resultado);                         
                }
                catch(DbUpdateConcurrencyException)
                {
                    return NotFound(String.Format("Consulta NÃO encontrada para alteração no Id = {0}.", consulta.Id));
                }
            } 
            return BadRequest(pt);
        }

        [Route("DeletarConsultaPorId")]
        [HttpDelete]
        public async Task<IActionResult> Deleta(int id)
        {
            var pesquisa = await _consultaService.pesquisaConsultaPorIdAsync(id);

            if (pesquisa.Count() == 0)
            {
                return BadRequest("Id não encontrado.");
            }
            else
            {              
                await _consultaService.DeletarConsultaPorConsultaAsync(pesquisa.FirstOrDefault());
                return Ok();
            }
        }
        
        [Route("DeletarTodasConsultasPorMedicoId")]
        [HttpDelete]
        public async Task<IActionResult> DeletaTodasPorMedicoId(int medicoId)
        {
            var pesquisa = await _consultaService.ConsultasPorMedicoIdAsync(medicoId);

            if (pesquisa.Count() == 0)
                return BadRequest("Nenhuma consulta encontrada.");
              
            var medico = await _consultaService.pesquisaMedicoPorIdAsync(medicoId);
            foreach(var cadaConsulta in pesquisa)
                await _consultaService.DeletarConsultaPorConsultaAsync(cadaConsulta);

            return Ok($"Todas consultas do médico {medico.FirstOrDefault().Nome} deletadas.");

        }
    }
}