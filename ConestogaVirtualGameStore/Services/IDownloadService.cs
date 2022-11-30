using ConestogaVirtualGameStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Services
{
    public interface IDownloadService
    {
        Task<IEnumerable<GameModel>> GetOwnedGames(string userId);
        bool CheckOwnedGame(string userId,int gameId);
        string GetFileName(int gameId);
    }
}
