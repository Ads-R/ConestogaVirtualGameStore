namespace ConestogaVirtualGameStore.Models
{
    public class PreferencesModel
    {
        public int PreferencesModelId { get; set; }
        public string UserId { get; set; }
        public string Platform { get; set; }
        public string Category { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
