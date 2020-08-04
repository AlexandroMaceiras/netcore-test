using System;
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

        public async Task<bool> InserirConsultaAsync(Consulta consulta)
        {
            try
            {
                _context.Consultas.Add(consulta);   
                await _context.SaveChangesAsync();
                return true;

            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<int> EditarConsulta(Consulta consulta)
        {
            _context.Consultas.Update(consulta);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletarConsultaPorConsultaAsync(Consulta consulta)
        {
            try
            {
                _context.Consultas.Remove(consulta);  
                await _context.SaveChangesAsync();
                return true;

            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<Consulta>> pesquisaConsultaPorIdAsync(int id)
        {
            return await _context.Consultas.Where(lambda => lambda.Id == id).ToArrayAsync();
        }

        public async Task<IEnumerable<Consulta>> PeagaTodasConsultasAsync()
        {        
            return await _context.Consultas.ToArrayAsync();    
        }

        public async Task<IEnumerable<Consulta>> ConsultasPorMedicoIdAsync(int id)
        {        
            return await _context.Consultas.Where(lambda => lambda.MedicoId == id).ToArrayAsync();    
        }

        public async Task<IEnumerable<Medico>> pesquisaMedicoPorIdAsync(int id)
        {
            return await _context.Medicos.Where(lambda => lambda.Id == id).ToArrayAsync();
        }
    }
}