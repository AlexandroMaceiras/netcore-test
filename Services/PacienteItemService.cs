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

        public async Task<IEnumerable<Consulta>> ConsultaTodasConsultasPorPacienteIdAsync(int id)
        {
            return await _context.Consultas.Where(lambda => lambda.PacienteId == id).ToArrayAsync();
        }
        public async Task<bool> InserirPacienteAsync(Paciente paciente)
        {
            try
            {
                _context.Pacientes.Add(paciente);   
                await _context.SaveChangesAsync();
                return true;

            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<Paciente>> pesquisaPacientePorNomeAsync(string nome)
        {
            return await _context.Pacientes.Where(lambda => lambda.Nome == nome).ToArrayAsync();
        }
        public async Task<IEnumerable<Paciente>> pesquisaPacientePorIdAsync(int id)
        {
            return await _context.Pacientes.Where(lambda => lambda.Id == id).ToArrayAsync();
        }
        
        public async Task<EntityEntry<Paciente>> EditarPacientesAsync(Paciente paciente)
        {
            //Limpa a instância feita pelo pesquisaMedicoPorIdAsync chamado anteriormente em Asincronismo.
            _context.Entry(paciente).State = EntityState.Deleted;
            
            var eep =_context.Pacientes.Update(paciente);            
            await _context.SaveChangesAsync();
            return eep;            
        }

       public async Task<EntityEntry<Paciente>> deletaPacienteModelAsync(Paciente paciente)
       {
           try
           {
               var eep = _context.Pacientes.Remove(paciente);  
               await _context.SaveChangesAsync();
               return eep;
           }
           catch(Exception e)
           {
               throw e;
           }
       }
   }
}