using ConestogaVirtualGameStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Services
{
    public interface IWish
    {
        void AddWishList(string userId, string gameId);
        void RemoveWishList(string userId, string gameId);
        Task<IEnumerable<GameModel>> GetAllGames(string userId);
    }
}
