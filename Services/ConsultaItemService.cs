using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbertEinstein.Models;
using Microsoft.EntityFrameworkCore;

namespace Consultas.Services
{
    public class ConsultaItemService : IConsultaItemService
    {
        private readonly AlbertEinsteinContext _context;

        public ConsultaItemService(AlbertEinsteinContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Consulta>> PeagaTodasConsultasAsync()
        {        
            return await _context.Consultas.ToArrayAsync();    
        }
        public async Task<IEnumerable<Consulta>> ConsultasPorMedicoIdAsync(int id)
        {        
            return await _context.Consultas.Where(lambda => lambda.MedicoId == id).ToArrayAsync();    
        }

        public async Task<IEnumerable<Medico>> pesquisaMedicoPorId(int id)
        {
            return await _context.Medicos.Where(lambda => lambda.Id == id).ToArrayAsync();
        }
    }
}