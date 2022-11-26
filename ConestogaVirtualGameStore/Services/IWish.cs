using ConestogaVirtualGameStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Services
{
    public interface IWish
    {
        void AddWishList(string userId, int gameId);
        void RemoveWishList(string userId, int gameId);
        Task<IEnumerable<GameModel>> GetAllGames(string userId);
        Task<IEnumerable<GameModel>> GetFriendGames(string userName);
    }
}
