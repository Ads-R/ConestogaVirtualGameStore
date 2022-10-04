using System.ComponentModel;

namespace ConestogaVirtualGameStore.Models.ViewModels
{
    public class PreferenceViewModel
    {
        public int PreferencesModelId { get; set; }
        public string UserId { get; set; }
        public string[] Platform { get; set; }
        public string[] Category { get; set; }
    }
}
