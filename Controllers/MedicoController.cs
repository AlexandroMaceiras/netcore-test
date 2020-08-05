using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AlbertEinstein.Models;
using System;

namespace AlbertEinstein.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MedicoController: ControllerBase
    {
        private readonly AlbertEinsteinContext _context;

        public MedicoController(AlbertEinsteinContext context)
        {
            _context = context;
        }

        [Route("ListarTodosMedicos")]
        [HttpGet]
        public IActionResult Todos()
        {
            var medicos = _context.Medicos;

            if (medicos.Count() == 0) 
                return NotFound("Nenhum médico cadastrado.");
            else
                return Ok(medicos);
        }

        [Route("InserirMedico")]
        [HttpPost]
        public IActionResult Insere([FromBody]Medico medico)
        {
            if(pesquisaMedicoPorNome(medico.Nome).Count() == 0)
            {                
                medico.Id = 0;

                var resultado = _context.Medicos.Add(medico);
                _context.SaveChanges();
                return Ok(resultado.Entity);
            }
            else
            {
                return NotFound("Médico já cadastrado.");
            }
        }
        
        [Route("InserirConsultaNoModuloMedico")]
        [HttpPost]
        public IActionResult InserirConsultaNoModuloMedico(Consulta consulta)
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

        [Route("EditarMedico")]
        [HttpPut]
        public IActionResult Edita([FromBody]Medico medico)
        {
            if(pesquisaMedicoPorId(medico.Id).Count() == 0)
                return NotFound("Médico não encontrado, Id inexistente.");

            if(pesquisaMedicoPorNome(medico.Nome).Count() > 0)
                return NotFound("Médico já cadastrado.");

            var resultado = _context.Medicos.Update(medico);            
            _context.SaveChanges();            
            return Ok(resultado.Entity);
        }

        [Route("DeletarMedicoPorId")]
        [HttpDelete]
        public IActionResult Deleta(int id)
        {
            var pmpid = pesquisaMedicoPorId(id);

            if (pmpid.Count() == 0)
            {
                return NotFound("Médico não encontrado, Id inexistente.");
            }
            else
            {              
                _context.Medicos.Remove(pmpid.FirstOrDefault());
                _context.SaveChanges();
                return Ok();
            }
        }

        [Route("DeletarMedicoPorNome")]
        [HttpDelete]
        public IActionResult DeletaMedicoPorNome(string nome)
        {
            var pmpn = pesquisaMedicoPorNome(nome);

            if(pmpn.Count() == 0)
            {
                return NotFound("Médico não encontrado, Nome inexistente.");
            }
            else
            {              
                _context.Medicos.Remove(pmpn.FirstOrDefault());
                _context.SaveChanges();
                return Ok();
            }
        }


        private IQueryable<Medico> pesquisaMedicoPorNome(string nome)
        {
            return _context.Medicos.Where(lambda => lambda.Nome == nome);
        }
        private IQueryable<Medico> pesquisaMedicoPorId(int id)
        {
            return _context.Medicos.Where(lambda => lambda.Id == id);
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
