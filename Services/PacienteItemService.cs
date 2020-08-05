using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbertEinstein.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Pacientes.Services
{
    public class PacienteItemService : IPacienteItemService
    {
        private readonly AlbertEinsteinContext _context;

        public PacienteItemService(AlbertEinsteinContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Paciente>> PegaTodosPacientesAsync()
        {        
            return await _context.Pacientes.ToArrayAsync();    
        }
/*         public async Task<IEnumerable<Medico>> pesquisaMedicoPorNomeAsync(string nome)
        {
            return await _context.Medicos.Where(lambda => lambda.Nome == nome).ToArrayAsync();
        }
        public async Task<IEnumerable<Medico>> pesquisaMedicoPorIdAsync(int id)
        {
            return await _context.Medicos.Where(lambda => lambda.Id == id).ToArrayAsync();
        }
        public async Task<bool> InserirMedicoAsync(Medico medico)
        {
            try
            {
                _context.Medicos.Add(medico);   
                await _context.SaveChangesAsync();
                return true;

            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<EntityEntry<Consulta>> InserirConsultaNoModuloMedicoAsync(Consulta consulta)
        {
            var eec = _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();
            return eec;
        }

        public async Task<EntityEntry<Medico>> EditarMedicoAsync(Medico medico)
        {
            //Limpa a instância feita pelo pesquisaMedicoPorIdAsync chamado anteriormente em Asincronismo.
            _context.Entry(medico).State = EntityState.Deleted;
            
            var eem =_context.Medicos.Update(medico);            
            await _context.SaveChangesAsync();
            return eem;            
        }

       public async Task<EntityEntry<Medico>> pesquisaMedicoModelAsync(Medico medico)
       {
           try
           {
               var eem = _context.Medicos.Remove(medico);  
               await _context.SaveChangesAsync();
               return eem;
           }
           catch(Exception e)
           {
               throw e;
           }
       }
 */
   }
}