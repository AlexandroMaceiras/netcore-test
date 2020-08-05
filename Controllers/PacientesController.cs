using System.Linq;
using System.Threading.Tasks;
using AlbertEinstein.Models;
using AlbertEinstein.Services;
using Microsoft.AspNetCore.Mvc;
using Pacientes.Services;

namespace AlbertEinstein.Controllers
{
    [ApiController]
    [Route("/Async/IoC/[controller]")]
    public class PacientesController : ControllerBase
    {
        IPacienteItemService _pacienteService;
        IOutrosItemService _outrosService;
        public PacientesController(IPacienteItemService pacienteService, IOutrosItemService outrosService)
        {
            _pacienteService    = pacienteService;
            _outrosService      = outrosService;
        }

        [Route("ListarTodosPacientes")]
        [HttpGet]
        public async Task<IActionResult> Todos()
        {
            var pacientes = await _pacienteService.PegaTodosPacientesAsync();
            
            if (pacientes.Count() == 0) 
                return NotFound("Nenhum paciente cadastrado.");
            else
                return Ok(pacientes);
        }
/* 
        [Route("ListarTodasConsultasPorPacienteId")]
        [HttpGet]
        public IActionResult ConsultaTodasConsultasPorPacienteId(int id)
        {
             var consulta = _context.Consultas.Where(lambda => lambda.PacienteId == id);

             return Ok(consulta);
        }

        [Route("InserirPaciente")]
        [HttpPost]
        public IActionResult Insere([FromBody]Paciente paciente)
        {
            if(pesquisaPacientePorNome(paciente.Nome).Count() == 0)
            {
                if(paciente.Id != 0)
                    paciente.Id = 0;
                if(paciente.CPF.Length > 11)
                    return BadRequest("CPF do paciente está ultrapassando o limite.");

                var resultado = _context.Pacientes.Add(paciente);
                _context.SaveChanges();
                return Ok(resultado.Entity);
            }
            else
            {
                return NotFound("Paciente já cadastrado.");
            }
        }

        [Route("EditarPaciente")]
        [HttpPut]
        public IActionResult Edita([FromBody]Paciente paciente)
        {
            if(pesquisaPacientePorId(paciente.Id).Count() == 0)
                return NotFound("Paciente não encontrado, Id inexistente.");

            if(pesquisaPacientePorNome(paciente.Nome).Count() > 0)
                return NotFound("Paciente já cadastrado.");

            var resultado = _context.Pacientes.Update(paciente);            
            _context.SaveChanges();            
            return Ok(resultado.Entity);
        }

        [Route("DeletarPacientePorId")]
        [HttpDelete]
        public IActionResult Deleta(int id)
        {
            var pppid = pesquisaPacientePorId(id);

            if (pppid.Count() == 0)
            {
                return NotFound("Paciente não encontrado, Id inexistente.");
            }
            else
            {              
                _context.Pacientes.Remove(pppid.FirstOrDefault());
                _context.SaveChanges();
                return Ok();
            }
        }                

        [Route("DeletarPacientePorNome")]
        [HttpDelete]
        public IActionResult DeletaPorNome(string nome)
        {
            var pppn = pesquisaPacientePorNome(nome);

            if(pppn.Count() == 0)
            {
                return NotFound("Paciente não encontrado, Nome inexistente.");
            }
            else
            {              
                _context.Pacientes.Remove(pppn.FirstOrDefault());
                _context.SaveChanges();
                return Ok();
            }
        }


        private IQueryable<Paciente> pesquisaPacientePorNome(string nome)
        {
            return _context.Pacientes.Where(lambda => lambda.Nome == nome);
        }
        private IQueryable<Paciente> pesquisaPacientePorId(int id)
        {
            return _context.Pacientes.Where(lambda => lambda.Id == id);
        } */
    }
}
