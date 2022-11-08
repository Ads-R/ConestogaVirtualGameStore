using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Services
{
    public interface ICreditCardService
    {
        Task<IEnumerable<CreditCardModel>> GetAllCreditCards(string userId);
        Task<CreditCardModel> GetCreditCard(long cardNumber);
        void DeleteCreditCard(int cardId);
        void AddCreditCard(CreditCardViewModel creditCard, string userId);
        bool IsDateValid(CreditCardViewModel card);
    }
}
