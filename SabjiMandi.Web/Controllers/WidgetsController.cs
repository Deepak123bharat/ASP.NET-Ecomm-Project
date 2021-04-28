using SabjiMandi.Services;
using SabjiMandi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabjiMandi.Web.Controllers
{
    public class WidgetsController : Controller
    {
        // GET: Widgets
        public ActionResult Products(bool isLatestProduct, bool? ShowCategory, int? CategoryID)
        {
            ProductsWidgetViewModel model = new ProductsWidgetViewModel();
            model.IsLatestProduct = isLatestProduct;
            model.ShowCategory = ShowCategory.HasValue ? ShowCategory.Value : false;
            if (isLatestProduct)
            {
                model.Products = ProductsService.Instance.GetLatestProducts(4);
            }
            else if(CategoryID.HasValue && CategoryID.Value > 0)
            {
                model.Products = ProductsService.Instance.GetProductsByCategory(CategoryID.Value, 1, 4);
            }
            else
            {
                model.ShowCategory = true;
                model.Products = ProductsService.Instance.GetProducts(1, 8);
            }
            return PartialView(model);
        }
    }
}