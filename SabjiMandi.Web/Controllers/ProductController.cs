using SabjiMandi.Entities;
using SabjiMandi.Services;
using SabjiMandi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabjiMandi.Web.Controllers
{
    public class ProductController : Controller
    {

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductTable(string search, int? pageNo)
        {
            ProductSearchViewModel model = new ProductSearchViewModel();
            model.SearchTerm = search;
            int pageSize = ConfigurationsService.Instance.GetPageSize();
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

            var totalRecords = ProductsService.Instance.GetProductsCount(search);
            model.Products = ProductsService.Instance.GetProducts(search, pageNo.Value, pageSize);
            if (model.Products != null)
            {
                model.Pager = new Pager(totalRecords, pageNo, pageSize);
                return PartialView("ProductTable", model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            NewProductViewModel model = new NewProductViewModel();

            model.AvailableCategories = CategoriesService.Instance.GetCategories();

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Create(NewProductViewModel model)
        {
            var newProduct = new Product();
            newProduct.Name = model.Name;
            newProduct.Description = model.Description;
            newProduct.Price = model.Price;
            newProduct.ImageUrl = model.ImageUrl;
            newProduct.Category = CategoriesService.Instance.GetCategory(model.CategoryID);

            ProductsService.Instance.SaveProduct(newProduct);

            return RedirectToAction("ProductTable");
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            EditProductViewModel model = new EditProductViewModel();

            var product = ProductsService.Instance.GetProduct(ID);

            model.ID = product.ID;
            model.Name = product.Name;
            model.Description = product.Description;
            model.Price = product.Price;
            model.ImageUrl = product.ImageUrl;
            model.CategoryID = product.Category != null ? product.Category.ID : 0;

            model.AvailableCategories = CategoriesService.Instance.GetCategories();

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(EditProductViewModel model)
        {
            var existingProduct = ProductsService.Instance.GetProduct(model.ID);
            existingProduct.Name = model.Name;
            existingProduct.Description = model.Description;
            existingProduct.Price = model.Price;
            existingProduct.Category = CategoriesService.Instance.GetCategory(model.CategoryID);

            if(!string.IsNullOrEmpty(model.ImageUrl))
            {
                existingProduct.ImageUrl = model.ImageUrl;
            }
            ProductsService.Instance.UpdateProduct(existingProduct);

            return RedirectToAction("ProductTable");
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            ProductsService.Instance.DeleteProduct(ID);

            return RedirectToAction("ProductTable");
        }

        [HttpGet]
        public ActionResult Details(int ID)
        {
            ProductViewModel model = new ProductViewModel();
            model.Product = ProductsService.Instance.GetProduct(ID);
            return View(model);
        }
    }
}