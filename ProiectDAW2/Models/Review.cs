using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProiectDAW.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Continutul review-ului este obligatoriu!")]
        [DataType(DataType.MultilineText)]
        public string ReviewContent { get; set; }

        public DateTime ReviewDate { get; set; }

        [Required(ErrorMessage = "Produsul este obligatoriu")]
        public int ProductId { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Product Product { get; set; }
    }
}