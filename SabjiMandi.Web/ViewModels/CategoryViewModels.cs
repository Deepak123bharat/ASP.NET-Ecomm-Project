using SabjiMandi.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SabjiMandi.Web.ViewModels
{
    public class CategorySearchViewModel
    {
        public List<Category> Categories { get; set; }
        public string SearchTerm { get; set; }
        public Pager Pager { get; set; }
    }

    public class NewCategoryViewModel
    {
        [Required]
        [MaxLength(50), MinLength(2)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsFeatured { get; set; }
    }

    public class EditCategoryViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsFeatured { get; set; }
    }
}