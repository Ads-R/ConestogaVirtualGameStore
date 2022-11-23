using ConestogaVirtualGameStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Services
{
    public interface IWish
    {
        void AddWishList(string userId, int gameId);
        void RemoveWishList(string userId, int gameId);
        void ShareToSocialMedia(IEnumerable<GameModel> games);
        Task<IEnumerable<GameModel>> GetAllGames(string userId);
    }
}
