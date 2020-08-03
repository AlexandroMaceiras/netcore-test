using System.Linq;
using AlbertEinstein.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlbertEinstein.Services
{
    public interface IOutrosItemService
    {
        //IEnumerable<Medico> pesquisaMedicoNaConsulta(Consulta consulta);

        string PesquisaTudo(Consulta consulta);
    }
}