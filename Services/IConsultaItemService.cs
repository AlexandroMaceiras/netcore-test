using System.Linq;
using AlbertEinstein.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Consultas.Services
{
    public interface IConsultaItemService
    {
        Task<IEnumerable<Consulta>> PeagaTodasConsultasAsync();

        Task<IEnumerable<Medico>> pesquisaMedicoPorId(int id);

        Task<IEnumerable<Consulta>> ConsultasPorMedicoIdAsync(int id);

    }
}