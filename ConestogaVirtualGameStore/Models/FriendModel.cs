namespace ConestogaVirtualGameStore.Models
{
    public class FriendModel
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public bool IsApproved { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser Friend { get; set; }
    }
}
