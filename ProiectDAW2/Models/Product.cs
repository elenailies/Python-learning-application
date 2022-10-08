using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDAW.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Descrierea este obligatorie!")]
        [StringLength(200, ErrorMessage = "Descrierea nu poate contine peste 200 de caractere!")]
        public string Title { get; set; }

       /*
        [Required(ErrorMessage = "valoarea este obligatorie!")]
        [Range(1, 1000, ErrorMessage = "valoarea trebuie sa fie cuprins intre 1 si 1000!")]
        public float Price { get; set; }
       */

        [Required(ErrorMessage = "Rating-ul este obligatoriu!")]
        [Range(0, 5, ErrorMessage = "Rating-ul trebuie sa fie cuprins intre 1 si 5!")]
        public float Rating { get; set; }
       

        [Required(ErrorMessage = "Continutul este obligatoriu!")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        //[Required(ErrorMessage = "Imaginea este obligatorie!")]
        public int ImageId { get; set; }

        [Required(ErrorMessage = "Categoria este obligatorie!")]
        public int CategoryId { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public bool Request { get; set; } // daca este True inseamna produsul a fost acceptat si poate fi afisat
        
        public virtual Category Category { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Adding> Addings { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public IEnumerable<SelectListItem> Categ { get; set; }
        public virtual Image Image { get; set; }
        public IEnumerable<SelectListItem> Imag { get; set; }
    }
}