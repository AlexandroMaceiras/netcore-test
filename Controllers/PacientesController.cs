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

        [Route("ListarTodasConsultasPorPacienteId")]
        [HttpGet]
        public async Task<IActionResult> ConsultaTodasConsultasPorPacienteId(int id)
        {
             var consulta = await _pacienteService.ConsultaTodasConsultasPorPacienteIdAsync(id);

             if(consulta.Count() == 0)
                return NotFound("Nenhuma consula encontrada para o paciente deste Id.");

             return Ok(consulta);
        }

        [Route("InserirPaciente")]
        [HttpPost]
        public async Task<IActionResult> Insere([FromBody]Paciente paciente)
        {
            paciente.Id = 0;

            var pm = await _pacienteService.pesquisaPacientePorNomeAsync(paciente.Nome);

            if(pm.Count() != 0)
                return NotFound("Paciente já cadastrado.");
            else
            {
                await _pacienteService.InserirPacienteAsync(paciente);
                return Ok(paciente);
            }

        }

        [Route("EditarPaciente")]
        [HttpPut]
        public async Task<IActionResult> Edita([FromBody]Paciente paciente)
        {
            if(_pacienteService.pesquisaPacientePorIdAsync(paciente.Id).Result.Count() == 0)
                return NotFound("Paciente não encontrado, Id inexistente.");

            var resultado = await _pacienteService.EditarPacientesAsync(paciente); 
            return Ok(resultado.Entity);
        }

        [Route("DeletarPacientePorId")]
        [HttpDelete]
        public IActionResult Deleta(int id)
        {
            var pppid = _pacienteService.pesquisaPacientePorIdAsync(id);

            if (pppid.Result.Count() == 0)
                return NotFound("Paciente não encontrado, Id inexistente.");

            _pacienteService.deletaPacienteModelAsync(pppid.Result.FirstOrDefault());
            return Ok();
        }

        [Route("DeletarPacientePorNome")]
        [HttpDelete]
        public async Task<IActionResult> DeletaPorNome(string nome)
        {
            var pppn = _pacienteService.pesquisaPacientePorNomeAsync(nome);

            if(pppn.Result.Count() == 0)
            {
                return NotFound("Paciente não encontrado, Nome inexistente.");
            }
            else
            {              
                await _pacienteService.deletaPacienteModelAsync(pppn.Result.FirstOrDefault());
                return Ok();
            }
        }
    }
}
