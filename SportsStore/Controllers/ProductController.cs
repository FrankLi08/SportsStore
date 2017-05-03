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
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;


        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ProductController(IProductRepository productrepository)
        {
            this.repository = productrepository;

        }
        public ViewResult List(string category , int page = 1)
        {

            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products.Where(p => category == null || p.Category == category).OrderBy(p => p.ProductID).Skip((page - 1) * PageSize).Take(PageSize),
                //return View(repository.Products.OrderBy(p=>p.ProductID).Skip((page-1)*PageSize).Take(PageSize));
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerpage = PageSize,
                    TotalItems= category==null? repository.Products.Count(): repository.Products.Where(e=> e.Category==category).Count()
                }, CurrentCategory = category
               
            };
            return View(model);
        }

        public FileContentResult GetImage(int productId)
        {
            Product prod = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (prod!=null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
            {
                return null;
            }
           
        }
    }
}