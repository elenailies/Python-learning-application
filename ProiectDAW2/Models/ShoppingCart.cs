using ProiectDAW.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProiectDAW2.Models
{
    public class ShoppingCart
    {
        [Key]
        public int ShoppingCartId { get; set; }

        [Required(ErrorMessage = "User-ul este obligatoriu!")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public bool UsedForOrder { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Adding> Addings { get; set; }

    }
}