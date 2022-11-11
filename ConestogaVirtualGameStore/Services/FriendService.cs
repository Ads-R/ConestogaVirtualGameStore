using ConestogaVirtualGameStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Services
{
    public class FriendService : IFriendService
    {
        private readonly GameStoreContext _context;
        public FriendService(GameStoreContext context)
        {
            _context = context;
        }

        public void CancelRequest(string userId, string friendId)
        {
            var friendRecord = _context.Friends.Where(a => a.UserId == userId && a.FriendId == friendId).Where(c => c.IsApproved == false).FirstOrDefault();
            if (friendRecord == null)
            {
                throw new Exception("The friend request is missing from the database. The user may have already declined/approved the friend request.");
            }
            _context.Friends.Remove(friendRecord);
            _context.SaveChanges();
        }

        public void DeclineRequest(string userId, string friendId)
        {
            var friendRecord = _context.Friends.Where(a => a.UserId == friendId && a.FriendId == userId).Where(c => c.IsApproved == false).FirstOrDefault();
            if (friendRecord == null)
            {
                throw new Exception("The friend request is missing from the database. The user may have canceled the friend request.");
            }
            _context.Friends.Remove(friendRecord);
            _context.SaveChanges();
        }

        public void AcceptRequest(string userId, string friendId)
        {
            var friendRecord = _context.Friends.Where(a => a.UserId == friendId && a.FriendId == userId).Where(c => c.IsApproved == false).FirstOrDefault();
            if(friendRecord == null)
            {
                throw new Exception("The friend request is missing from the database. The user may have canceled the friend request.");
            }
            friendRecord.IsApproved = true;
            _context.Friends.Update(friendRecord);
            _context.SaveChanges();
        }

        public void AddFriend(string userId, string friendId)
        {
            var friendRecord = _context.Friends.Where(a => (a.UserId == userId && a.FriendId == friendId) || (a.UserId == friendId && a.FriendId == userId)).FirstOrDefault();
            if (friendRecord != null)
            {
                throw new Exception("User has already sent you a friend request. Refresh your page if you don't see it.");
            }

            FriendModel friend = new FriendModel()
            {
                UserId = userId,
                FriendId = friendId,
                IsApproved = false
            };
            _context.Add(friend);
            _context.SaveChanges();
        }

        public void DeleteFriend(string userId, string friendId)
        {
            var friendRecord = _context.Friends.Where(a => (a.UserId == userId && a.FriendId == friendId) || (a.UserId == friendId && a.FriendId == userId)).FirstOrDefault();
            if (friendRecord == null)
            {
                throw new Exception("User has already unfriended you. Refresh your page to see the recent changes.");
            }
            _context.Friends.Remove(friendRecord);
            _context.SaveChanges();
        }

        public IQueryable<ApplicationUser> SearchUsers(string searchString, string userName)
        {
            return _context.Users.Where(a => a.UserName.Contains(searchString)).Where(b => b.UserName != userName).Where(c => c.UserName != "Admin");
        }
        //////////////////////////////////////////////////////////////////////////////////////////
        public async Task<IEnumerable<ApplicationUser>> GetAllAcceptedFriends(string userId)
        {
            var friends = await GetAcceptedFriends(userId);
            var friendsOf = await GetAcceptedFriendsOf(userId);
            var result = friends.Union(friendsOf);
            return result;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAcceptedFriends(string userId)
        {
            var friends = (await _context.Friends.Include(b => b.Friend).Where(a => a.UserId == userId && a.IsApproved == true).ToListAsync()).Select(c => c.Friend);
            return friends;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAcceptedFriendsOf(string userId)
        {
            var friends =  (await _context.Friends.Include(b => b.User).Where(a => a.FriendId == userId && a.IsApproved == true).ToListAsync()).Select(c => c.User);
            return friends;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        public async Task<IEnumerable<string>> GetAllFriends(string userId)
        {
            var friends = await GetFriends(userId);
            var friendsOf = await GetFriendsOf(userId);
            var result = friends.Union(friendsOf);
            return result;
        }

        public async Task<IEnumerable<string>> GetFriends(string userId)
        {
            var friendIds = (await _context.Friends.Where(a => a.UserId == userId).ToListAsync()).Select(c => c.FriendId);
            return friendIds;
        }
        public async Task<IEnumerable<string>> GetFriendsOf(string userId)
        {
            var friendIds = (await _context.Friends.Where(a => a.FriendId == userId).ToListAsync()).Select(c => c.UserId);
            return friendIds;
        }

        ////////////////////////////////////////////////////////////////////////////////////

        public async Task<IEnumerable<ApplicationUser>> GetPendingApproval(string userId)
        {
            var friends = (await _context.Friends.Include(b => b.User).Where(a => a.FriendId== userId && a.IsApproved == false).ToListAsync()).Select(c => c.User);
            return friends;
        }

        public async Task<IEnumerable<ApplicationUser>> GetPendingRequest(string userId)
        {
            var friends = (await _context.Friends.Include(b => b.Friend).Where(a => a.UserId == userId && a.IsApproved == false).ToListAsync()).Select(c => c.Friend);
            return friends;
        }
        
        ////////////////////////////////////////////////////////////////////////////
        
        public void AcceptFriend()
        {

        }
        
    }
}
