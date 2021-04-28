using SabjiMandi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SabjiMandi.Web.ViewModels
{
    public class ProductsWidgetViewModel
    {
        public List<Product> Products { get; set; }
        public bool IsLatestProduct { get; set; }
        public bool ShowCategory { get; set; }
    }
}