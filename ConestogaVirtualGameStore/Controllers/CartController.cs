using ConestogaVirtualGameStore.Classes;
using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly GameStoreContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        public CartController(GameStoreContext context, UserManager<ApplicationUser> uManager)
        {
            _context = context;
            userManager = uManager;
        }
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            var cart = GetCart();
            ViewBag.Cart = cart;
            ViewBag.TotalCost = cart==null ? 0:cart.Sum(a => a.Game.RetailPrice);
            var creditCards = await _context.CreditCards.Where(a => a.UserId == user.Id).ToListAsync();
            ViewBag.CreditCards = creditCards;
            if(cart != null)
            {
                HttpContext.Session.SetString("CartCount", cart.Count.ToString());
            }
            return View();
        }

        public IActionResult AddItem(int id, string type)
        {
            GameModel game = _context.Games.Where(a => a.Id == id).FirstOrDefault();
            if (GetCart()== null)
            {
                List<CartItem> cart = new List<CartItem>();
                cart.Add(new CartItem
                {
                    Game = game,
                    IsPhysicalCopy = type == "Digital" ? false:true
                });
                SessionHelper.SetSessionObject(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<CartItem> cart = GetCart();
                bool IsInCart = FindGameInCart(id);
                if (!IsInCart)
                {
                    cart.Add(new CartItem
                    {
                        Game = game,
                        IsPhysicalCopy = type == "Digital" ? false : true
                    });
                }
                else
                {
                    TempData["CartItemDuplicate"] = "Cannot have duplicate items in the cart";
                }
                SessionHelper.SetSessionObject(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveItem(int id)
        {
            List<CartItem> cart = GetCart();
            int index = FindGameIndex(id);
            if (index != -1)
            {
                cart.RemoveAt(index);
                SessionHelper.SetSessionObject(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CheckOut(CartViewModel card)
        {
            // Still Under Construction//
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index","Home");
            }
            TempData["CardNull"] = "Please select a credit card for payment";
            return RedirectToAction("Index");
        }

        private bool FindGameInCart(int id)
        {
            List<CartItem> cart = GetCart();
            foreach (var item in cart)
            {
                if (item.Game.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        private int FindGameIndex(int id)
        {
            List<CartItem> cart = GetCart();
            for (int x = 0; x<cart.Count; x++)
            {
                if (cart[x].Game.Id == id)
                {
                    return x;
                }
            }
            return -1;
        }

        private List<CartItem> GetCart()
        {
            return SessionHelper.GetSessionObject<List<CartItem>>(HttpContext.Session, "cart");
        }
    }
}
