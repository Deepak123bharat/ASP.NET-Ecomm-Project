using SabjiMandi.Entities;
using SabjiMandi.Services;
using SabjiMandi.Web.Code;
using SabjiMandi.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabjiMandi.Web.Controllers
{
    public class ShopController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult Index(string search, int? maxPrice, int? minPrice, int? categoryID, int? sortBy, int? pageNo)
        {
            ShopViewModel model = new ShopViewModel();
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            model.FeaturedCategories = CategoriesService.Instance.GetFeaturedCategories();
            model.MaximumPrice = ProductsService.Instance.GetMaxPrice();
            model.Products = ProductsService.Instance.SearchProducts(search, maxPrice, minPrice, categoryID, sortBy, pageNo.Value, 6);
            model.sortBy = sortBy;
            model.CategoryID = categoryID;
            int totalCount = ProductsService.Instance.SearchProductsCount(search, maxPrice, minPrice, categoryID, sortBy);
            model.Pager = new Pager(totalCount, pageNo, 6);
            return View(model);
        }
        public ActionResult FilterProducts(string search, int? maxPrice, int? minPrice, int? categoryID, int? sortBy, int? pageNo)
        {
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            FilterProductsViewModel model = new FilterProductsViewModel();
            model.Products = ProductsService.Instance.SearchProducts(search, maxPrice, minPrice, categoryID, sortBy, pageNo.Value, 6);
            int totalCount = ProductsService.Instance.SearchProductsCount(search, maxPrice, minPrice, categoryID, sortBy);
            model.Pager = new Pager(totalCount, pageNo, 6);
            model.MaximumPrice = ProductsService.Instance.GetMaxPrice();
            model.sortBy = sortBy;
            model.CategoryID = categoryID;

            return PartialView(model);
        }

        [Authorize]
        public ActionResult Checkout()
        {
            CheckoutViewModel model = new CheckoutViewModel();

            var CartProductsCookie = Request.Cookies["CartProducts"];

            if (CartProductsCookie != null && !string.IsNullOrEmpty(CartProductsCookie.Value))
            {
                model.CartProductIDs = CartProductsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();

                model.CartProducts = ProductsService.Instance.GetProducts(model.CartProductIDs);
                model.User = UserManager.FindById(User.Identity.GetUserId());
            }

            return View(model);
        }

        public JsonResult PlaceOrder(string productIDs)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            if (!string.IsNullOrEmpty(productIDs))
            {
                var productQuantities = productIDs.Split('-').Select(x => int.Parse(x)).ToList();

                var boughtProducts = ProductsService.Instance.GetProducts(productQuantities.Distinct().ToList());

                Order newOrder = new Order();
                newOrder.UserID = User.Identity.GetUserId();
                newOrder.OrderedAt = DateTime.Now;
                newOrder.Status = "Pending";
                newOrder.TotalAmount = boughtProducts.Sum(x => x.Price * productQuantities.Where(productID => productID == x.ID).Count());

                newOrder.OrderItems = new List<OrderItem>();
                newOrder.OrderItems.AddRange(boughtProducts.Select(x => new OrderItem() { ProductID = x.ID, Quantity = productQuantities.Where(productID => productID == x.ID).Count() }));

                var rowsEffected = ShopService.Instance.SaveOrder(newOrder);

                result.Data = new { Success = true, Rows = rowsEffected };
            }
            else
            {
                result.Data = new { Success = false };
            }

            return result;
        }
    }
}