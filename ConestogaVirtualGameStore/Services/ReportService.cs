using ConestogaVirtualGameStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<ApplicationUser>> GellAllUsers()
        {
            var users = await _context.Users.Where(i=>i.UserName !="Admin").Include(a =>a.Profile)
                                            .Include(b=>b.Preference)
                                            .Include(c=>c.Address).ToArrayAsync();
            return users;
        }

        public async Task<IEnumerable<GameModel>> GetAllGames()
        {
            var games = await _context.Games.ToListAsync();
            return games;
        }

        public async Task<IEnumerable<Orders>> GetAllOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders;
        }

        public async Task<IEnumerable<WishListModel>> GetAllWishes()
        {
            var wishes = await _context.Wish.Include(a =>a.Game).ToListAsync();
            return wishes;        
        }
    }
}
