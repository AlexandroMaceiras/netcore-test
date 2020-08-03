using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AlbertEinstein.Models;
using System;

namespace AlbertEinstein.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsultaController: ControllerBase
    {
        private readonly AlbertEinsteinContext _context;

        public ConsultaController(AlbertEinsteinContext context)
        {
            _context = context;
        }

        [Route("ListarTodasConsultas")]
        [HttpGet]
        public IActionResult Todos()
        {
            var consultas = _context.Consultas;

            if (consultas.Count() == 0) 
                return NotFound("Nenhuma consulta cadastrada.");
            else
                return Ok(consultas);
        }

        [Route("ListarTodasConsultasPorMedicoId")]
        [HttpGet]
        public IActionResult ConsultaTodasPorMedicoId(int id)
        {
            if(pesquisaMedicoPorId(id).Count() == 0)
                return NotFound("Médico não encontrado, Id inexistente.");

             var consulta = _context.Consultas.Where(lambda => lambda.MedicoId == id);

             if(consulta.Count() == 0)
                return NotFound("Nenhuma consulta encontrata para este médico.");

             return Ok(consulta);
        }

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


        private IQueryable<Medico> pesquisaMedicoPorId(int id)
        {
            return _context.Medicos.Where(lambda => lambda.Id == id);
        }
        private IQueryable<Consulta> pesquisaConsultaPorId(int id)
        {
            return _context.Consultas.Where(lambda => lambda.Id == id);
        }
        private IQueryable<Medico> pesquisaMedicoNaConsulta(Consulta consulta) 
        {
            return _context.Medicos.Where(lambda => lambda.Id == consulta.MedicoId);
        }
        private IQueryable<Paciente> pesquisaPacienteNaConsulta(Consulta consulta) 
        {
            return _context.Pacientes.Where(lambda => lambda.Id == consulta.PacienteId);
        }
        private IQueryable<Consulta> pesquisaConsulta(Consulta consulta) 
        {
            return _context.Consultas.Where(lambda => lambda.MedicoId == consulta.MedicoId && lambda.PacienteId == consulta.PacienteId && lambda.Data == consulta.Data);
        }
        private string PesquisaTudo(Consulta consulta)
        {
            if(pesquisaMedicoNaConsulta(consulta).Count() == 0)
                return "Médico NÃO cadastrado.";                
            if(pesquisaPacienteNaConsulta(consulta).Count() == 0)
                return "Paciente NÃO cadastrado.";
            if(pesquisaConsulta(consulta).Count() != 0)
                return "Consulta JÁ cadastrada, NÃO foi inserida nem alterada, use outro Paciente, Médico ou outra Data e Horário.";

            return null;
        }
    }
}
