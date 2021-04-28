using SabjiMandi.Entities;
using SabjiMandi.Services;
using SabjiMandi.Web.ViewModels;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace SabjiMandi.Web.Controllers
{
    public class CategoryController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CategoryTable(string search, int? pageNo)
        {
            CategorySearchViewModel model = new CategorySearchViewModel();
            int pageSize = 3;
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo : 1 : 1;
            model.SearchTerm = search;
            var totalRecords = CategoriesService.Instance.GetCategoriesCount(search);
            model.Categories = CategoriesService.Instance.GetCategories(search, pageNo.Value);
            
            if(model.Categories != null)
            {
                model.Pager = new Pager(totalRecords, pageNo, pageSize);
                return PartialView("CategoryTable", model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        #region Creation

        [HttpGet]
        public ActionResult Create()
        {
            NewCategoryViewModel model = new NewCategoryViewModel();

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Create(NewCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newCategory = new Category();
                newCategory.Name = model.Name;
                newCategory.Description = model.Description;
                newCategory.ImageUrl = model.ImageUrl;
                newCategory.IsFeatured = model.IsFeatured;

                CategoriesService.Instance.SaveCategory(newCategory);

                return RedirectToAction("CategoryTable");
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        #endregion

        #region Update

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            EditCategoryViewModel model = new EditCategoryViewModel();

            var category = CategoriesService.Instance.GetCategory(ID);

            model.ID = category.ID;
            model.Name = category.Name;
            model.Description = category.Description;
            model.ImageUrl = category.ImageUrl;
            model.IsFeatured = category.IsFeatured;

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(EditCategoryViewModel model)
        {
            var existingCategory = CategoriesService.Instance.GetCategory(model.ID);
            existingCategory.Name = model.Name;
            existingCategory.Description = model.Description;
            existingCategory.ImageUrl = model.ImageUrl;
            existingCategory.IsFeatured = model.IsFeatured;

            CategoriesService.Instance.UpdateCategory(existingCategory);

            return RedirectToAction("CategoryTable");
        }

        #endregion

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            CategoriesService.Instance.DeleteCategory(ID);

            return RedirectToAction("CategoryTable");
        }
    }
}