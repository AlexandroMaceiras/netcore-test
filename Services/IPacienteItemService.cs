using AlbertEinstein.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Pacientes.Services
{
    public interface IPacienteItemService
    {
        Task<IEnumerable<Paciente>> PegaTodosPacientesAsync();
/*         Task<IEnumerable<Paciente>> pesquisaPacientePorNomeAsync(string nome);
        Task<bool> InserirPacienteAsync(Paciente paciente);
        Task<EntityEntry<Consulta>> InserirConsultaNoModuloPacienteAsync(Consulta consulta);
        Task<IEnumerable<Paciente>> pesquisaPacientePorIdAsync(int id);
        Task<EntityEntry<Paciente>> EditarPacienteAsync(Paciente paciente);
        Task<EntityEntry<Paciente>> pesquisaPacienteModelAsync(Paciente paciente); */
   }
}