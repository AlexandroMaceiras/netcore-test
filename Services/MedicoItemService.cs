using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbertEinstein.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Medicos.Services
{
    public class MedicoItemService : IMedicoItemService
    {
        private readonly AlbertEinsteinContext _context;

        public MedicoItemService(AlbertEinsteinContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medico>> PegaTodosMedicosAsync()
        {        
            return await _context.Medicos.ToArrayAsync();    
        }
        public async Task<IEnumerable<Medico>> pesquisaMedicoPorNomeAsync(string nome)
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

       public async Task<EntityEntry<Medico>> deletaMedicoModelAsync(Medico medico)
       {
           try
           {
               var eem = _context.Medicos.Remove(medico);

               //Deletando todas as consultas deste médico junto com ele.
               var iqc = _context.Consultas.Where(lambda => lambda.MedicoId == medico.Id);
               foreach(var item in iqc)
                    _context.Consultas.Remove(item);
                
               //Zerando todos os exames que este médico fazia parte junto com ele.
               var iqe = _context.Exames.Where(lambda => lambda.MedicoId == medico.Id);
               foreach(var item in iqe)
               {
                   item.MedicoId = 0;
                   _context.Exames.Update(item); 
               }

               await _context.SaveChangesAsync();
               return eem;
           }
           catch(Exception e)
           {
               throw e;
           }
       }
    }
}