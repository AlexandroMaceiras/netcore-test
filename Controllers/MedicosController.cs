using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AlbertEinstein.Models;
using System;
using AlbertEinstein.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Medicos.Services;

namespace AlbertEinstein.Controllers
{
    [ApiController]
    [Route("/Async/IoC/[controller]")]
    public class MedicosController: ControllerBase
    {
        IMedicoItemService _medicoService;
        IOutrosItemService _outrosService;

        public MedicosController(IMedicoItemService medicoService, IOutrosItemService outrosService)
        {
            _medicoService      = medicoService;
            _outrosService      = outrosService;
        }

        [Route("ListarTodosMedicos")]
        [HttpGet]
        public async Task<IActionResult> TodosAsync()
        {
            var consultas = await _medicoService.PegaTodosMedicosAsync();

            if (consultas.Count() == 0) 
                return NotFound("Nenhum médico cadastrada.");
            else
                return Ok(consultas);
        }

        [Route("InserirMedico")]
        [HttpPost]
        public async Task<IActionResult> Insere([FromBody]Medico medico)
        {
            medico.Id = 0;

            var pm = await _medicoService.pesquisaMedicoPorNomeAsync(medico.Nome);

            if(pm.Count() != 0)
                return NotFound("Médico já cadastrado.");
            else
            {
                await _medicoService.InserirMedicoAsync(medico);
                return Ok(medico);
            }

        }

        [Route("InserirConsultaNoModuloMedico")]
        [HttpPost]
        public async Task<IActionResult> InserirConsultaNoModuloMedicoAsync(Consulta consulta)
        {
            consulta.Id = 0;

            if(consulta.Data < DateTime.Now.Date)
                return BadRequest("Não pode-se cadastrar uma consulta em data inferior a hoje " + DateTime.Now.ToString("dd/MM/yyyy"));

            var pt = await _outrosService.PesquisaTudoAsync(consulta);

            if(pt == null)
            {
                var eec = await _medicoService.InserirConsultaNoModuloMedicoAsync(consulta);
                return Ok(eec.Entity);
            }
            else
            {
                return BadRequest(pt);
            }
        }

        [Route("EditarMedico")]
        [HttpPut]
        public async Task<IActionResult> Edita([FromBody]Medico medico)
        {
            if(_medicoService.pesquisaMedicoPorIdAsync(medico.Id).Result.Count() == 0)
                return NotFound("Médico não encontrado, Id inexistente.");

            if(_medicoService.pesquisaMedicoPorNomeAsync(medico.Nome).Result.Count() > 0)
                return NotFound("Médico já cadastrado.");

            var eem = await _medicoService.EditarMedicoAsync(medico);            
            return Ok(eem.Entity);
        }

        [Route("DeletarMedicoPorId")]
        [HttpDelete]
        public async Task<IActionResult> Deleta(int id)
        {
            var pmpi = await _medicoService.pesquisaMedicoPorIdAsync(id);
            if (pmpi.Count() == 0)
                return NotFound("Médico não encontrado, Id inexistente.");

                      
            var retorno = await _medicoService.pesquisaMedicoModelAsync(pmpi.FirstOrDefault());
            return Ok(retorno.Entity);
        }
        
        [Route("DeletarMedicoPorNome")]
        [HttpDelete]
        public async Task<IActionResult> DeletaMedicoPorNomeAsync(string nome)
        {
            var pmpn = await _medicoService.pesquisaMedicoPorNomeAsync(nome);

            if(pmpn.Count() == 0)
                return NotFound("Médico não encontrado, Nome inexistente.");

            var retorno =_medicoService.pesquisaMedicoModelAsync(pmpn.FirstOrDefault());
            return Ok($"O médico {pmpn.FirstOrDefault().Nome} foi deletado.");
        }
    }
}