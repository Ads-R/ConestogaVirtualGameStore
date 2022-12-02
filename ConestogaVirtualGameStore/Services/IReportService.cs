using ConestogaVirtualGameStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Services
{
    public interface IReportService
    {
        Task<IEnumerable<GameModel>> GetAllGames();
    }
}
