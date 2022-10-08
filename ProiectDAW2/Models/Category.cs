using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProiectDAW.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Numele categoriei este obligatoriu!")]
        [StringLength(100, ErrorMessage = "Numele categoriei nu poate avea mai mult de 100 de caractere")]
        public string CategoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}