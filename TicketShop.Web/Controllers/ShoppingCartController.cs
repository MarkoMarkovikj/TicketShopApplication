using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketShop.Service.Interface;

namespace TicketShop.Web.Controllers {
    public class ShoppingCartController: Controller {

        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService) {
            _shoppingCartService = shoppingCartService;
        }
        public IActionResult Index() {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(this._shoppingCartService.GetShoppingCartInfo(userId));
        }

        public IActionResult DeleteFromShoppingCart(Guid id) {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            this._shoppingCartService.DeleteProductFromShoppingCart(userId, id);

            return RedirectToAction("Index", "ShoppingCart");
        }

        public IActionResult Order() {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            this._shoppingCartService.OrderNow(userId);

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
