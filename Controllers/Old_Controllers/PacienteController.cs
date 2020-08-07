using System.Linq;
using AlbertEinstein.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlbertEinstein.Controllers
{
    //Controller substituido pelo Controler Asincrono e com Ioc por Injeção de Dependência
    //[ApiController]
    //[Route("[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly AlbertEinsteinContext _context;
        public PacienteController(AlbertEinsteinContext context)
        {
            _context = context;
        }

        //[Route("ListarTodosPacientes")]
        //[HttpGet]
        public IActionResult Todos()
        {
            var pacientes = _context.Pacientes;
            
            if (pacientes.Count() == 0) 
                return NotFound("Nenhum paciente cadastrado.");
            else
                return Ok(pacientes);
        }

        //[Route("ListarTodasConsultasPorPacienteId")]
        //[HttpGet]
        public IActionResult ConsultaTodasConsultasPorPacienteId(int id)
        {
             var consulta = _context.Consultas.Where(lambda => lambda.PacienteId == id);

             return Ok(consulta);
        }

        //[Route("InserirPaciente")]
        //[HttpPost]
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

        //[Route("EditarPaciente")]
        //[HttpPut]
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

        //[Route("DeletarPacientePorId")]
        //[HttpDelete]
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

        //[Route("DeletarPacientePorNome")]
        //[HttpDelete]
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
        }
    }
}
