using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbertEinstein.Models;
using Microsoft.EntityFrameworkCore;

namespace Exames.Services
{
    public class ExameItemService : IExameItemService
    {
        private readonly AlbertEinsteinContext _context;

        public ExameItemService(AlbertEinsteinContext context)
        {
            _context = context;
        }

        public async Task<bool> InserirExameAsync(Exame exame)
        {
            try
            {
                _context.Exames.Add(exame);   
                await _context.SaveChangesAsync();
                return true;

            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<int> EditarExame(Exame exame)
        {
            _context.Exames.Update(exame);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletarExamePorExameAsync(Exame exame)
        {
            try
            {
                _context.Exames.Remove(exame);  
                await _context.SaveChangesAsync();
                return true;

            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<Exame>> pesquisaExamePorIdAsync(int id)
        {
            return await _context.Exames.Where(lambda => lambda.Id == id).ToArrayAsync();
        }

        public async Task<IEnumerable<Exame>> PeagaTodosExamesAsync()
        {        
            return await _context.Exames.ToArrayAsync();    
        }

        public async Task<IEnumerable<Exame>> ExamesPorMedicoIdAsync(int id)
        {        
            return await _context.Exames.Where(lambda => lambda.MedicoId == id).ToArrayAsync();    
        }

        public async Task<IEnumerable<Medico>> pesquisaMedicoPorIdAsync(int id)
        {
            return await _context.Medicos.Where(lambda => lambda.Id == id).ToArrayAsync();
        }
    }
}