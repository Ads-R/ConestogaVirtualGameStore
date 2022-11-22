using System.Collections.Generic;
using System.Threading.Tasks;
using ConestogaVirtualGameStore.Models;

namespace ConestogaVirtualGameStore.Services
{
    public class Wish : IWish
    {
        private readonly GameStoreContext _context;
        public Wish(GameStoreContext context)
        {
            _context = context;
        }
        public void AddWishList(string userId, string gameId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<GameModel>> GetAllGames(string userId)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveWishList(string userId, string gameId)
        {
            throw new System.NotImplementedException();
        }
    }
}
