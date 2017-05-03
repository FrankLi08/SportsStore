using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportsStore.Models;


namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IProductRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }
        public  ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }



        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnURl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product !=null)
            {
                GetCart().AddItem(product, 1);

            }
            return RedirectToAction("Index", new { returnURl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
              cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
                                              
        }


        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart== null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public PartialViewResult Summary (Cart cart)
        {
            return PartialView(cart);
        }
        

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }


        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count()==0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
                    //why we use modelstate
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
                //page 249
            }

        }



        // GET: Cart
        //public ActionResult Index()
        //{
        //    return View();
        //}
    }
}