using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbertEinstein.Models;
using Microsoft.EntityFrameworkCore;

namespace AlbertEinstein.Services
{
    public class OutrosItemService : IOutrosItemService
    {
        private readonly AlbertEinsteinContext _context;

        public OutrosItemService(AlbertEinsteinContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medico>> pesquisaMedicoNaConsulta(Consulta consulta)
        {
            return await _context.Medicos.Where(lambda => lambda.Id == consulta.MedicoId).ToArrayAsync();
        }

        private async Task<IEnumerable<Paciente>> pesquisaPacienteNaConsulta(Consulta consulta) 
        {
            return await _context.Pacientes.Where(lambda => lambda.Id == consulta.PacienteId).ToArrayAsync();
        }
        private async Task<IEnumerable<Consulta>> pesquisaConsulta(Consulta consulta) 
        {
            var teste = await _context.Consultas.Where
            (lambda =>   
                           (lambda.MedicoId   == consulta.MedicoId
                        && lambda.Data        == consulta.Data)
                        || (lambda.PacienteId == consulta.PacienteId 
                        && lambda.Data        == consulta.Data)
            ).ToArrayAsync();
            return teste;
        }
        private async Task<IEnumerable<Exame>> pesquisaExame(Consulta consulta) 
        {
            Exame exame = new Exame()
            {
                Id          = consulta.Id,
                PacienteId  = consulta.PacienteId,
                MedicoId    = consulta.MedicoId,
                Data        = consulta.Data
            };

            return await _context.Exames.Where
            (lambda =>   
                           (lambda.MedicoId   == exame.MedicoId
                        && lambda.Data        == exame.Data)
                        || (lambda.PacienteId == exame.PacienteId 
                        && lambda.Data        == exame.Data)
            ).ToArrayAsync();
        }
        public async Task<string> PesquisaTudoAsync(Consulta consulta)
        {
            if(pesquisaMedicoNaConsulta(consulta).Result.Count() == 0)
                return await Task.FromResult<string>("Médico NÃO cadastrado.");                
            if(pesquisaPacienteNaConsulta(consulta).Result.Count() == 0)
                return await Task.FromResult<string>("Paciente NÃO cadastrado.");
            if(pesquisaConsulta(consulta).Result.Count() != 0)
                return await Task.FromResult<string>("Consulta JÁ cadastrada nessa Data e Horário, para o mesmo Paciente e/ou Médico.");
            if(pesquisaExame(consulta).Result.Count() != 0)
                return await Task.FromResult<string>("Exame JÁ cadastrado nessa Data e Horário, para o mesmo Paciente e/ou Médico.");

            return await Task.FromResult<string>(null);
        }

    }
}