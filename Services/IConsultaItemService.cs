using System.Linq;
using AlbertEinstein.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Consultas.Services
{
    public interface IConsultaItemService
    {
        Task<IEnumerable<Consulta>> pesquisaConsultaPorIdAsync(int id);
        Task<IEnumerable<Consulta>> PeagaTodasConsultasAsync();
        Task<IEnumerable<Medico>> pesquisaMedicoPorIdAsync(int id);
        Task<IEnumerable<Consulta>> ConsultasPorMedicoIdAsync(int id);
        Task<bool> InserirConsultaAsync(Consulta consulta);
        Task<int> EditarConsulta(Consulta consulta);
        Task<bool> DeletarConsultaPorConsultaAsync(Consulta consulta);

    }
}