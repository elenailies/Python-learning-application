using ProiectDAW.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProiectDAW2.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }

        [Required(ErrorMessage = "Valoarea rating-ului este obligatoriu")]
        [Range(0, 5, ErrorMessage = "Rating-ul trebuie sa fie cuprins intre 1 si 5!")]
        public float RatingValue { get; set; }

        [Required(ErrorMessage = "Produsul este obligatoriu")]
        public int ProductId { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Product Product { get; set; }

    }
}