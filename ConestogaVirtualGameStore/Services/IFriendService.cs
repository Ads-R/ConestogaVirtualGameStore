using ConestogaVirtualGameStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Services
{
    public interface IFriendService
    {
        Task<IEnumerable<string>> GetAllFriends(string userId);
        Task<IEnumerable<ApplicationUser>> GetAllAcceptedFriends(string userId);
        Task<IEnumerable<ApplicationUser>> GetPendingApproval(string userId);
        Task<IEnumerable<ApplicationUser>> GetPendingRequest(string userId);

        IQueryable<ApplicationUser> SearchUsers(string searchString, string userName);
        void AddFriend(string userId, string friendId);
        void DeleteFriend(string userId, string friendId);
        void AcceptRequest(string userId, string friendId);
        void DeclineRequest(string userId, string friendId);
        void CancelRequest(string userId, string friendId);

    }
}
