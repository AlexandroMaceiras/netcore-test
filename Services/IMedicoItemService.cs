using AlbertEinstein.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Medicos.Services
{
    public interface IMedicoItemService
    {
        Task<IEnumerable<Medico>> PegaTodosMedicosAsync();
        Task<IEnumerable<Medico>> pesquisaMedicoPorNomeAsync(string nome);
        Task<bool> InserirMedicoAsync(Medico medico);
        Task<EntityEntry<Consulta>> InserirConsultaNoModuloMedicoAsync(Consulta consulta);
        Task<IEnumerable<Medico>> pesquisaMedicoPorIdAsync(int id);
        Task<EntityEntry<Medico>> EditarMedicoAsync(Medico medico);
        Task<EntityEntry<Medico>> pesquisaMedicoModelAsync(Medico medico);
   }
}