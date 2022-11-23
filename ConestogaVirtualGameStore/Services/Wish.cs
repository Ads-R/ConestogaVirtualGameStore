using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConestogaVirtualGameStore.Models;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;

namespace ConestogaVirtualGameStore.Services
{
    public class Wish : IWish
    {
        private readonly GameStoreContext _context;
        public Wish(GameStoreContext context)
        {
            _context = context;
        }

        public void AddWishList(string userId, int gameId)
        {
            try
            {
                WishListModel wishList = new WishListModel()
                {
                    UserId = userId,
                    GameId = gameId
                };
                _context.Add(wishList);
                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<GameModel>> GetAllGames(string userId)
        {

            var records = await _context.Wish.Where(a => a.UserId == userId).ToListAsync();
            List<GameModel> wishList = new List<GameModel>();
            foreach (var item in records)
            {
                var games = _context.Games.Where(a => a.Id == item.GameId).FirstOrDefault();
                wishList.Add(games);
            }
            
            return wishList;
        }

        public void RemoveWishList(string userId, int gameId)
        {
            try
            {
                WishListModel wishList = _context.Wish.Where(a => a.UserId == userId && a.GameId == gameId).FirstOrDefault();
                _context.Wish.Remove(wishList);
                _context.SaveChanges();
            }
            catch (Exception x)
            {
                throw;
            }
        }

        public void ShareToSocialMedia(IEnumerable<GameModel> games)
        {
            throw new System.NotImplementedException();
        }
    }
}
