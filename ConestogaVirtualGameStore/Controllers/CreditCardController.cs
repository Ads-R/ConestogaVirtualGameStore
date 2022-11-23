using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Models.ViewModels;
using ConestogaVirtualGameStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    [Authorize]
    public class CreditCardController : Controller
    {
        private readonly ICreditCardService _creditCardService;
        private readonly UserManager<ApplicationUser> userManager;
        public CreditCardController(ICreditCardService creditCardService, UserManager<ApplicationUser> uManager)
        {
            _creditCardService = creditCardService;
            userManager = uManager;
        }

        public async Task<IActionResult> PaymentMethod()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            var creditCards = await _creditCardService.GetAllCreditCards(user.Id);
            ViewBag.UserCards = creditCards;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaymentMethod(CreditCardViewModel creditCard)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                if (ModelState.IsValid)
                {
                    bool IsCardValid = _creditCardService.IsDateValid(creditCard);
                    if (!IsCardValid)
                    {
                        ModelState.AddModelError("", "Credit Card has expired");
                    }
                    if (ModelState.IsValid)
                    {
                        _creditCardService.AddCreditCard(creditCard, user.Id);
                        TempData["CCSuccess"] = "Credit Card added successfully";
                        return RedirectToAction("PaymentMethod");
                    }
                }
                var creditCards = await _creditCardService.GetAllCreditCards(user.Id);
                ViewBag.UserCards = creditCards;
                return View(creditCard);
            }
            catch (Exception x)
            {
                TempData["CCException"] = "An error has occurred while trying to add credit card. Pleas try again later.";
                return RedirectToAction("PaymentMethod");
            }
        }

        public async Task<IActionResult> DeleteCard(string userId, long cardNumber)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                if (userId == user.Id)
                {
                    CreditCardModel card = await _creditCardService.GetCreditCard(cardNumber);
                    _creditCardService.DeleteCreditCard(card.CreditCardModelId);
                    TempData["CCSuccess"] = "Credit Card deleted successfully";
                }
                return RedirectToAction("PaymentMethod");
            }
            catch (Exception x)
            {
                TempData["CCException"] = "An error has occurred while trying to delete credit card. Please try again later.";
                return RedirectToAction("PaymentMethod");
            }
        }
    }
}
