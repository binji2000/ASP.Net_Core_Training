using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EComm.Data;
using EComm.Web.Models;
using EComm.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EComm.Web.Controllers
{
    public class ProductController : Controller
    {
        private ECommContext _context;

        public ProductController(ECommContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        [Authorize(Policy="AdminsOnly")]
        [Route("product/{id:int}")]
        public IActionResult Detail(int id)
        {
            var model = _context.Products.Include(p => p.Supplier).SingleOrDefault(p => p.Id == id);
            if (model == null)
            {
                return NotFound();

            }
            return View(model);
        }

        // GET: /<controller>/
        [Authorize(Policy = "AdminsOnly")]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Include(p => p.Supplier).SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();

            }
            // var person = new { FirstName = "Bill", LastName = "Gates" };
            var model = new ProductEditViewModel(){Product = product, Suppliers = _context.Suppliers };
            return View(model);
        }

        [HttpPost]
        /* id value is retrieved from page url. quantity is retrieved from http post parameter */
        public IActionResult AddToCart(int id, int quantity)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            var totalCost = quantity*product.UnitPrice;
            string message =
                $"you added {product.ProductName} (x {quantity}) to your cart at a total cost of {totalCost:c}";

            var cart = ShoppingCart.GetFromSession(HttpContext.Session);
            var lineItem = cart.LineItems.SingleOrDefault(item =>
                                          item.Product.Id == id);
            if (lineItem != null)
            {
                lineItem.Quantity += quantity;
            }
            else
            {
                cart.LineItems.Add(new ShoppingCart.LineItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            ShoppingCart.StoreInSession(cart, HttpContext.Session);


            return PartialView("_AddedToCart", message);
        }

        public IActionResult Cart()
        {
            var cart = ShoppingCart.GetFromSession(HttpContext.Session);
            var model = new CartViewModel() {Cart = cart};
            return View(model);
        }

        [HttpPost]
        public IActionResult Checkout(CartViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                cvm.Cart = ShoppingCart.GetFromSession(HttpContext.Session);
                return View("Cart", cvm);
            }
            // TODO: Charge the customer's card and record the order
            HttpContext.Session.Clear();
            return View("ThankYou");
        }

        [Authorize(Policy = "AdminsOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(ProductEditViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", cvm);
            }
            var product = _context.Products.SingleOrDefault(p => p.Id == cvm.Product.Id);
            product.ProductName = cvm.Product.ProductName;
            product.UnitPrice = cvm.Product.UnitPrice;
            product.Package = cvm.Product.Package;
            product.SupplierId = cvm.Product.SupplierId;

//            _context.Update(cvm.Product);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
//            return RedirectToAction("Detail");
            //return Detail(cvm.Product.Id);
            //return View("Detail");
        }



    }

    public class ProductList : ViewComponent
    {
        private ECommContext _context;

        public ProductList(ECommContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var model = _context.Products.ToList();
            if (User.IsInRole("Admin"))
            {
                return View("~/Views/Shared/_ProductListAdmin.cshtml", model);
            }
            return View("~/Views/Shared/_ProductList.cshtml", model);
        }
    }
}
