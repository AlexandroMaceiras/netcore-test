using System.Linq;
using AlbertEinstein.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exames.Services
{
    public interface IExameItemService
    {
        Task<IEnumerable<Exame>> pesquisaExamePorIdAsync(int id);
        Task<IEnumerable<Exame>> PeagaTodosExamesAsync();
        Task<IEnumerable<Medico>> pesquisaMedicoPorIdAsync(int id);
        Task<IEnumerable<Exame>> ExamesPorMedicoIdAsync(int id);
        Task<bool> InserirExameAsync(Exame consulta);
        Task<int> EditarExame(Exame consulta);
        Task<bool> DeletarExamePorExameAsync(Exame consulta);

    }
}