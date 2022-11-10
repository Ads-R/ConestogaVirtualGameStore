using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Models.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly GameStoreContext _context;
        public CreditCardService(GameStoreContext context)
        {
            _context = context;
        }

        public void AddCreditCard(CreditCardViewModel creditCard, string userId)
        {
            try
            {
                //set up values
                CreditCardModel credit = new CreditCardModel()
                {
                    CreditCardNumber = creditCard.CreditCardNumber,
                    UserId = userId,
                    FirstName = creditCard.FirstName,
                    LastName = creditCard.LastName,
                    ExpiryDate = SetExpiryDate(creditCard.ExpiryMonth, creditCard.ExpiryYear),
                    SecurityCode = creditCard.SecurityCode
                };
                //after setting up
                _context.Add(credit);
                _context.SaveChanges();
            }
            catch (Exception x)
            {
                throw;
            }
        }

        public void DeleteCreditCard(int cardId)
        {
            try
            {
                CreditCardModel creditCard = _context.CreditCards.Where(a => a.CreditCardModelId == cardId).FirstOrDefault();
                _context.CreditCards.Remove(creditCard);
                _context.SaveChanges();
            }
            catch (Exception x)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CreditCardModel>> GetAllCreditCards(string userId)
        {
            return await _context.CreditCards.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<CreditCardModel> GetCreditCard(long cardNumber)
        {
            try
            {
                CreditCardModel creditCard = await _context.CreditCards
                    .Where(a => a.CreditCardNumber == cardNumber).FirstOrDefaultAsync();
                return creditCard;
            }
            catch (Exception x)
            {
                throw;
            }
        }

        private DateTime SetExpiryDate(string month, string year)
        {
            DateTime expiryDate = DateTime.ParseExact($"01/{month}/{year}", "d/M/yyyy", CultureInfo.InvariantCulture);
            return expiryDate;
        }

        public bool IsDateValid(CreditCardViewModel card)
        {
            DateTime expiryDate = SetExpiryDate(card.ExpiryMonth, card.ExpiryYear);
            if (expiryDate > DateTime.Today)
            {
                return true;
            }
            return false;
        }
    }
}
