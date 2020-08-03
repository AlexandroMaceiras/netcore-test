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
            return await _context.Consultas.Where(lambda => lambda.MedicoId == consulta.MedicoId && lambda.PacienteId == consulta.PacienteId && lambda.Data == consulta.Data).ToArrayAsync();
        }
        public string PesquisaTudo(Consulta consulta)
        {
            if(pesquisaMedicoNaConsulta(consulta).Result.Count() == 0)
                return "Médico NÃO cadastrado.";                
            if(pesquisaPacienteNaConsulta(consulta).Result.Count() == 0)
                return "Paciente NÃO cadastrado.";
            if(pesquisaConsulta(consulta).Result.Count() != 0)
                return "Consulta JÁ cadastrada, NÃO foi inserida nem alterada, use outro Paciente, Médico ou outra Data e Horário.";

            return null;
        }

    }
}