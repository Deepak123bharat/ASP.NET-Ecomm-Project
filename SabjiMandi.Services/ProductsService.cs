using SabjiMandi.Database;
using SabjiMandi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SabjiMandi.Services
{
    
    public class ProductsService
    {
        #region Singleton
        public static ProductsService Instance
        {
            get
            {
                if (instance == null) instance = new ProductsService();

                return instance;
            }
        }
        private static ProductsService instance { get; set; }

      

        private ProductsService()
        {
        }
        #endregion
        public int GetMaxPrice()
        {
            using (var context = new CBContext())
            {
                return  (int)context.Products.Max(x => x.Price);
            }
        }

        public List<Product> SearchProducts(string search, int? maxPrice, int? minPrice, int? categoryID, int? sortBy, int pageNo, int pageSize)
        {
            using (var context = new CBContext())
            {
                var products = context.Products.ToList();

                if(categoryID.HasValue)
                {
                    products = products.Where(x => x.Category.ID == categoryID.Value).ToList();
                }
                if(!string.IsNullOrEmpty(search))
                {
                    products = products.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                }
                if(maxPrice.HasValue)
                {
                    products = products.Where(x => x.Price <= maxPrice.Value).ToList();
                }
                if(minPrice.HasValue)
                {
                    products = products.Where(x => x.Price >= minPrice.Value).ToList();
                }

                if(sortBy.HasValue)
                {
                    switch (sortBy.Value)
                    {
                        case 2:
                            products = products.OrderByDescending(x => x.ID).ToList();
                            break;
                        case 3:
                            products = products.OrderBy(x => x.Price).ToList();
                            break;
                        case 4:
                            products = products.OrderByDescending(x => x.Price).ToList();
                            break;
                        default:
                            break;
                    }
                }

                return products.Skip((pageNo - 1)*pageSize).Take(pageSize).ToList();
            }
        }

        public int SearchProductsCount(string search, int? maxPrice, int? minPrice, int? categoryID, int? sortBy)
        {
            using (var context = new CBContext())
            {
                var products = context.Products.ToList();

                if (categoryID.HasValue)
                {
                    products = products.Where(x => x.Category.ID == categoryID.Value).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    products = products.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                }
                if (maxPrice.HasValue)
                {
                    products = products.Where(x => x.Price <= maxPrice.Value).ToList();
                }
                if (minPrice.HasValue)
                {
                    products = products.Where(x => x.Price >= minPrice.Value).ToList();
                }

                if (sortBy.HasValue)
                {
                    switch (sortBy.Value)
                    {
                        case 2:
                            products = products.OrderByDescending(x => x.ID).ToList();
                            break;
                        case 3:
                            products = products.OrderBy(x => x.Price).ToList();
                            break;
                        case 4:
                            products = products.OrderByDescending(x => x.Price).ToList();
                            break;
                        default:
                            break;
                    }
                }

                return products.Count;
            }
        }

        public Product GetProduct(int ID)
        {
            using (var context = new CBContext())
            {
                return context.Products.Where(x => x.ID == ID).Include(x => x.Category).FirstOrDefault();
            }
        }


        public List<Product> GetProducts(List<int> IDs)
        {
            using (var context = new CBContext())
            {
                return context.Products.Where(product => IDs.Contains(product.ID)).ToList();
            }
        }

        public int GetProductsCount(string search)
        {
            using (var context = new CBContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Products.Where(p => p.Name != null && p.Name.ToLower().Contains(search.ToLower())).Count();
                }
                else
                {
                    return context.Products.Count();
                }
            }
        }
        public List<Product> GetProducts(string search, int pageNo, int pageSize)
        {
            using (var context = new CBContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Products.Where(p => p.Name != null && p.Name.ToLower()
                    .Contains(search.ToLower()))
                    .OrderBy(x => x.ID)
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .Include(x => x.Category)
                    .ToList();
                }
                else
                {
                    return context.Products
                        .OrderBy(x => x.ID)
                        .Skip((pageNo - 1) * pageSize)
                        .Take(pageSize)
                        .Include(x => x.Category)
                        .ToList();
                }
            }
        }
        public List<Product> GetProducts(int pageNo)
        {
            using (var context = new CBContext())
            {
                return context.Products.Include(x => x.Category).ToList();
            }
        }

        public List<Product> GetProducts(int pageNo, int pageSize)
        {

            using (var context = new CBContext())
            {
                return context.Products.OrderByDescending(x=>x.ID).Skip((pageNo-1)* pageSize).Take(pageSize).Include(x => x.Category).ToList();
            }
        }
        public List<Product> GetLatestProducts(int numberOfProducts)
        {

            using (var context = new CBContext())
            {
                return context.Products.OrderByDescending(x => x.ID).Take(numberOfProducts).Include(x => x.Category).ToList();
            }
        }
        public List<Product> GetProductsByCategory(int CategoryID, int pageNo, int pageSize)
        {

            using (var context = new CBContext())
            {
                return context.Products.Where(x => x.Category.ID == CategoryID).OrderByDescending(x => x.ID).Take(pageSize).Include(x => x.Category).ToList();          
            }
        }

        public void SaveProduct(Product product)
        {
            using (var context = new CBContext())
            {
                context.Entry(product.Category).State = System.Data.Entity.EntityState.Unchanged;

                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var context = new CBContext())
            {
                context.Entry(product).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteProduct(int ID)
        {
            using (var context = new CBContext())
            {
                var product = context.Products.Find(ID);

                context.Products.Remove(product);
                context.SaveChanges();
            }
        }
    }
}
