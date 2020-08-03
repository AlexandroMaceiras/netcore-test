using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AlbertEinstein.Models;
using System;
using Consultas.Services;
using AlbertEinstein.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlbertEinstein.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public IActionResult ConsultaTodasPorMedicoId(int id)
        {
            if(_consultaService.pesquisaMedicoPorId(id).Result.Count() == 0)
                return NotFound("Médico não encontrado, Id inexistente.");

             var consulta = _consultaService.ConsultasPorMedicoIdAsync(id);

             if(consulta.Result.Count() == 0)
                return NotFound("Nenhuma consulta encontrata para este médico.");

             return Ok(consulta);
        }
/*
        [Route("InserirConsulta")]
        [HttpPost]
        public IActionResult Insere([FromBody]Consulta consulta)
        {
            consulta.Id = 0;

            if(consulta.Data < DateTime.Now.Date)
                return BadRequest("Não pode-se cadastrar uma consulta em data inferior a hoje " + DateTime.Now.ToString("dd/MM/yyyy"));

            var pt = PesquisaTudo(consulta);

            if(pt == null)
            {
                var resultado = _context.Consultas.Add(consulta);
                _context.SaveChanges();
                return Ok(resultado.Entity);
            }
            else
            {
                return BadRequest(pt);
            }
        }

        [Route("EditarConsulta")]
        [HttpPut]
        public IActionResult Edita([FromBody]Consulta consulta)
        {
            if(consulta.Id == 0)
                return NotFound("Declare o Id da consulta para poder modificá-la.");    

            var pt = PesquisaTudo(consulta);

            if(pt == null)
            {
                var resultado = _context.Consultas.Update(consulta);

                //Se tentar Editar com um Id inexistente ou errado, causa um erro.
                try
                {
                    _context.SaveChanges();   
                    return Ok(resultado.Entity);                         
                }
                catch(Exception e)
                {
                    if(e.HResult == -2146233088) //-2146233088 é o Erro de Id inexistente.
                        return NotFound(String.Format("Consulta NÃO encontrada para alteração no Id = {0}.", consulta.Id));
                    else //Outro tipo de erro irá aparecer aqui.
                        return NotFound(e.ToString());
                }
            }

            return BadRequest(pt);

        }

        [Route("DeletarConsultaPorId")]
        [HttpDelete]
        public IActionResult Deleta(int id)
        {
            var pesquisa = pesquisaConsultaPorId(id).FirstOrDefault();

            if (pesquisa == null)
            {
                return BadRequest("Id não encontrado.");
            }
            else
            {              
                _context.Consultas.Remove(pesquisa);
                _context.SaveChanges();
                return Ok();
            }
        }
        
        [Route("DeletarTodasConsultasPorMedicoId")]
        [HttpDelete]
        public IActionResult DeletaTodasPorMedicoId(int medicoId)
        {
            var pesquisa = _context.Consultas.Where(lambda => lambda.MedicoId == medicoId);

            if (pesquisa == null)
            {
                return BadRequest("Nenhuma consulta encontrada.");
            }
            else
            {              
                var medico = _context.Medicos.Where(lambda => lambda.Id == medicoId).FirstOrDefault();
                foreach(var cadaConsulta in pesquisa)
                    _context.Consultas.Remove(cadaConsulta);

                _context.SaveChanges();
                return Ok( String.Format("Todas consultas do médico {0} deletadas.",medico.Nome));
            }
        }

*/

/*
        private IQueryable<Consulta> pesquisaConsultaPorId(int id)
        {
            return _context.Consultas.Where(lambda => lambda.Id == id);
        }
        
*/
    }
}