using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AlbertEinstein.Models;
using System;
using Exames.Services;
using AlbertEinstein.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AlbertEinstein.Controllers
{
    [ApiController]
    [Route("/Async/IoC/[controller]")]
    public class ExamesController: ControllerBase
    {
        IExameItemService _examesService;
        IOutrosItemService _outrosService;

        public ExamesController(IExameItemService examesService, IOutrosItemService outrosService)
        {
            _examesService    = examesService;
            _outrosService      = outrosService;
        }

        [Route("ListarTodosExames")]
        [HttpGet]
        public async Task<IActionResult> TodosAsync()
        {
            var examess = await _examesService.PeagaTodosExamesAsync();

            if (examess.Count() == 0) 
                return NotFound("Nenhuma exame cadastrado.");
            else
                return Ok(examess);
        }
 
        [Route("ListarTodosExamesPorMedicoId")]
        [HttpGet]
        public async Task<IActionResult> ExameTodasPorMedicoId(int id)
        {
            var medico = await _examesService.pesquisaMedicoPorIdAsync(id);

            if(medico.Count() == 0)
                return NotFound("Exame não encontrado, Id inexistente.");

             var exames = await _examesService.ExamesPorMedicoIdAsync(id);

             if(exames.Count() == 0)
                return NotFound($"Nenhuma exame encontrato para o médico {medico.FirstOrDefault().Nome}.");

             return Ok(exames);
        }

        [Route("InserirExame")]
        [HttpPost]
        public async Task<IActionResult> Insere([FromBody]Exame exames)
        {
            exames.Id = 0;

            if(exames.TipoExame == 0)
                return BadRequest("Definir o tipo de exame é obrigatório.");

            Consulta consulta = new Consulta()
            {
                Id          = exames.Id,
                MedicoId    = exames.MedicoId,
                PacienteId  = exames.PacienteId,
                Data        = exames.Data
            };

            var pt = await _outrosService.PesquisaTudoAsync(consulta);

            if(pt == null)
                return Ok(await _examesService.InserirExameAsync(exames));
            else
                return BadRequest(pt);
        }

        [Route("EditarExame")]
        [HttpPut]
        public async Task<IActionResult> Edita([FromBody]Exame exames)
        {
            if(exames.Id == 0)
                return NotFound("Declare o Id da exames para poder modificá-la.");    

                Consulta cc = new Consulta()
                {
                    Id = exames.Id,
                    MedicoId = exames.MedicoId,
                    PacienteId = exames.PacienteId
                };

            var pt = await _outrosService.PesquisaTudoAsync(cc);

            if(pt == null)
            {
                //Se tentar Editar com um Id inexistente ou errado, causa um erro.
                try
                {
                    var resultado = await _examesService.EditarExame(exames);
                    return Ok(resultado);                         
                }
                catch(DbUpdateConcurrencyException)
                {
                    return NotFound(String.Format("Exame NÃO encontrada para alteração no Id = {0}.", exames.Id));
                }
            } 
            return BadRequest(pt);
        }

        [Route("DeletarExamePorId")]
        [HttpDelete]
        public async Task<IActionResult> Deleta(int id)
        {
            var pesquisa = await _examesService.pesquisaExamePorIdAsync(id);

            if (pesquisa.Count() == 0)
            {
                return BadRequest("Id não encontrado.");
            }
            else
            {              
                await _examesService.DeletarExamePorExameAsync(pesquisa.FirstOrDefault());
                return Ok();
            }
        }
        
        [Route("DeletarTodasExamesPorMedicoId")]
        [HttpDelete]
        public async Task<IActionResult> DeletaTodasPorMedicoId(int medicoId)
        {
            var pesquisa = await _examesService.ExamesPorMedicoIdAsync(medicoId);

            if (pesquisa.Count() == 0)
                return BadRequest("Nenhuma exames encontrada.");
              
            var medico = await _examesService.pesquisaMedicoPorIdAsync(medicoId);
            foreach(var cadaExame in pesquisa)
                await _examesService.DeletarExamePorExameAsync(cadaExame);

            return Ok($"Todas examess do médico {medico.FirstOrDefault().Nome} deletadas.");

        }
    }
}