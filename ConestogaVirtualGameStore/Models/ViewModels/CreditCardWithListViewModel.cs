using System.Collections.Generic;

namespace ConestogaVirtualGameStore.Models.ViewModels
{
    public class CreditCardWithListViewModel
    {
        public CreditCardViewModel creditCardViewModel { get; set; }
        public IEnumerable<CreditCardModel> creditCards { get; set; }
    }
}
