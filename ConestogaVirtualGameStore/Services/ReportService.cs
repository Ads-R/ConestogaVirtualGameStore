using ConestogaVirtualGameStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Services
{
    public class ReportService : IReportService
    {
        private readonly GameStoreContext _context;
        public ReportService(GameStoreContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GameModel>> GetAllGames()
        {
            var games = await _context.Games.ToListAsync();
            return games;
        }
    }
}
