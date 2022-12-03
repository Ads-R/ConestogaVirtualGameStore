using ConestogaVirtualGameStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly GameStoreContext _context;
        public DownloadService(GameStoreContext context)
        {
            _context = context;
        }
        public bool CheckOwnedGame(string userId, int gameId)
        {
            bool result = _context.Orders
                .Any(a => a.UserId == userId && a.GameId == gameId && a.IsPhysicalCopy == false);
            return result;
        }

        public string GetFileName(int gameId)
        {
            var game = _context.Games.Where(a => a.Id == gameId).FirstOrDefault();
            return game.FileName;
        }

        public async Task<IEnumerable<GameModel>> GetOwnedGames(string userId)
        {
            var games = await _context.Orders.Where(a => a.UserId == userId).Where(b => b.IsPhysicalCopy == false).Select(d => d.Game).Distinct().ToListAsync();
            //List<GameModel> gameList = new List<GameModel>();
            //foreach (var i in games)
            //{
            //    gameList.Add(i.Game);
            //}
            return games;
        }
    }
}
